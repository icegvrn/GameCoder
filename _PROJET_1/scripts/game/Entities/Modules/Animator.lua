-- MODULE QUI GERE LES DEPLACEMENTS "AUTOMATIQUES" DES PERSONNAGES ; TOUT LE TEMPS POUR ENNEMI ; EN CINEMATIQUE POUR JOUEUR
local mapManager = require(PATHS.MAPMANAGER)
local c_Animator = {}
local Animator_mt = {__index = c_Animator}

function c_Animator.new()
    local _Animator = {}
    return setmetatable(_Animator, Animator_mt)
end

function c_Animator:create()
    local animator = {
        velocityX = 0,
        velocityY = 0,
        speed = 0,
        initialSpeed = 0,
        boostedSpeed = 0,
        angle = 0,
        lastPositionX = 0,
        lastPositionY = 0,
        destinationX = 0,
        destinationY = 0,
        lastTurningPositionX = 0
    }

    function animator:init(player)
        self.initialSpeed = player.character.controller.speed
        self.speed = self.initialSpeed
        self.boostedSpeed = self.initialSpeed * 2
        self.lastPositionX, self.lastPositionY = player.character.transform:getPosition()
    end

    function animator:update(dt, player)
        if player.character.controller.isCinematicMode then
            self:playEntrance(dt, player)
        end
        self:setVelocity(dt)
    end

    function animator:playEntrance(dt, player)
        if player.character:getState() ~= CHARACTERS.STATE.WALKING then
            player.character:setState(CHARACTERS.STATE.WALKING)
            self:chooseARandomPoint(dt, player)
        end

        local x, y = player.character.transform:getPosition()
        local speed = player.character.controller.speed
        if player.character.controller.player then
            local velocityX = speed / 3 * dt
            x = x + velocityX
            player.character.transform:setPosition(x, y)
        else
            self:tryMove(dt, player)
        end
    end

    function animator:chooseARandomPoint(dt, player)
        self.speed = self.initialSpeed
        local x, y = player.character.transform:getPosition()
        self.destinationX, self.destinationY = mapManager:getMapDimension()
        self.destinationX = love.math.random(0, self.destinationX)
        self.destinationY = love.math.random(0, self.destinationY)
        self.angle = Utils.angle(x, y, self.destinationX, self.destinationY)
    end

    function animator:tryMove(dt, player)
        local initialPositionX, initialPositionY = player.character.transform:getPosition()
        local ennemiWidth, ennemiHeight =
            player.character.sprites:getDimension(player.character.mode, player.character.state)
        local newPositionX = initialPositionX + self.velocityX
        local newPositionY = initialPositionY + self.velocityY

        if (mapManager:isThereASolidElement(newPositionX, newPositionY, ennemiWidth, ennemiHeight)) == false then
            self:move(dt, player, newPositionX, newPositionY)
            return true
        else
            self:move(dt, player, self.lastPositionX, self.lastPositionY)
            return false
        end
    end

    function animator:move(dt, player, newPositionX, newPositionY)
        if newPositionX - self.lastTurningPositionX > 2 or newPositionX - self.lastTurningPositionX < -2 then
            animator:updateDirection(dt, player)
        end
        player.character.transform:setPosition(newPositionX, newPositionY)
        self.lastPositionX, self.lastPositionY = newPositionX, newPositionY
    end

    function animator:updateDirection(dt, player)
        if self.velocityX < 0 then
            player.character.controller:changeDirection(player.character, CONST.DIRECTION.left, dt)
        else
            player.character.controller:changeDirection(player.character, CONST.DIRECTION.right, dt)
        end
        self.lastTurningPositionX = self.lastPositionX
    end

    function animator:chase(dt, player, targetX, targetY)
        self.speed = self.boostedSpeed
        local attackRandomizer = self:randomizeAttackDirection(player)
        self:setAngle(self.lastPositionX, self.lastPositionY, targetX + attackRandomizer, targetY + attackRandomizer)
        return self:tryMove(dt, player)
    end

    function animator:randomizeAttackDirection(player)
        if (player.character.fight.weaponSlot:getWeaponRange()) then
            local randNumber =
                math.random(
                -player.character.fight.weaponSlot:getWeaponRange() + mapManager:getCurrentMap().tilewidth * 2,
                player.character.fight.weaponSlot:getWeaponRange() - mapManager:getCurrentMap().tilewidth * 2
            )
            return randNumber
        else
            local randNumber =
                math.random(-mapManager:getCurrentMap().tilewidth * 2, mapManager:getCurrentMap().tilewidth * 2)
            return randNumber
        end
    end

    function animator:setAngle(x1, y1, x2, y2)
        if x2 and y2 then
            self.angle = Utils.angle(x1, y1, x2, y2)
        else
            self.angle = 1
        end
    end

    function animator:setVelocity(dt)
        self.velocityX = self.speed * math.cos(self.angle) * (dt * 60)
        self.velocityY = self.speed * math.sin(self.angle) * (dt * 60)
        return self.velocityX, self.velocityY
    end

    function animator:setDestination(dX, dY)
        self.destinationX, self.destinationY = dX, dY
    end

    return animator
end

return c_Animator

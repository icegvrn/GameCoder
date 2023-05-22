-- MODULE QUI GERE LES ACTIONS DU PLAYER EN FONCTION DES TOUCHES UTILISEES PAR LE JOUEUR

local c_PlayerInput = {}
local PlayerInput_mt = {__index = c_PlayerInput}

function c_PlayerInput.new()
    local PlayerInput = {}
    return setmetatable(PlayerInput, PlayerInput_mt)
end

function c_PlayerInput:create()
    local playerInput = {}

    function playerInput:update(dt, player)
        if player.character.controller.canMove then
            player.lastX, player.lastY = player.character.transform:getPosition()
        else
            player.character.transform:setPosition(player.lastX, player.lastY)
        end

        if self:useAction(controller.action1) then
            self:fire(dt, player)
        end
        self:move(dt, player)
    end

    function playerInput:draw(player)
    end

    function playerInput:useAction(action)
        if action == "mouse1" then
            return love.mouse.isDown(1)
        elseif action == "mouse2" then
            return love.mouse.isDown(2)
        elseif action == "mouse3" then
            return love.mouse.isDown(3)
        else
            return love.keyboard.isDown(action)
        end
    end

    function playerInput:keypressed(key, player)
        if key == controller.action2 then
            if
                player.character:getMode() == CHARACTERS.MODE.NORMAL and
                    player.pointsCounter.points == player.pointsCounter.maxPoints
             then
                player.character:setMode(CHARACTERS.MODE.BOOSTED)
                player.playerBooster:init(player)
                player.character.sound:playBoostedSound()
            else
                player.character:setMode(CHARACTERS.MODE.NORMAL)
            end
        end
    end

    function playerInput:fire(dt, player)
        player.character:setState(CHARACTERS.STATE.FIRE)
        player.character:fire(dt)
    end

    function playerInput:move(dt, player)
        if
            love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
                love.keyboard.isDown(controller.left) or
                love.keyboard.isDown(controller.right)
         then
            if player.character:getState() ~= CHARACTERS.STATE.WALKING then
                player.character:setState(CHARACTERS.STATE.WALKING)
            end

            local x, y = player.character.transform:getPosition()
            local sX, sY = player.character.transform:getScale()
            local speed = player.character.controller.speed

            local angle = utils.angleWithMouseWorldPosition(player.character.transform:getPosition())
            local velocityX = speed * dt
            local velocityY = speed * dt

            if love.keyboard.isDown(controller.up) then
                y = y - velocityY
            elseif love.keyboard.isDown(controller.down) then
                y = y + velocityY
            elseif love.keyboard.isDown(controller.left) then
                x = x - velocityX
            elseif love.keyboard.isDown(controller.right) then
                x = x + velocityX
            end
            player.character.transform:setPosition(x, y)
        elseif player.character:getState() == CHARACTERS.STATE.WALKING or player.character:getMode() ~= lastMode then
            player.character:setState(CHARACTERS.STATE.IDLE)
            lastMode = player.character:getMode()
        end
    end

    return playerInput
end

return c_PlayerInput

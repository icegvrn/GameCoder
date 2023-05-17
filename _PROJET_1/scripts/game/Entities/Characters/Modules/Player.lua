oldPlayer = require("scripts/game/player")
camera = require("scripts/game/mainCamera")
local map = require("scripts/game/gameMap")
require("scripts/states/CHARACTERS")
GAMESTATE = require("scripts/states/GAMESTATE")
local ui = require("scripts/ui/uiGame")

local Character = require("scripts/engine/character") -- Ajout de l'exigence de Character

local Player = {}
local Player_mt = {__index = Player}

function Player.new()
    local _player = {character = Character.new()}
    return setmetatable(_player, Player_mt)
end

function Player:create()
    local player = {
        character = self.character:create(),
        points = 50,
        maxPoints = 50,
        boosterDuration = 5,
        boosterTimer = 5,
        keyDown = false,
        lastX = 0,
        lastY = 0
    }

    function player:update(dt)
        local gmap = map.getCurrentMap()
        local x, y = self.character.transform:getPosition()

        local w, h = self.character.sprites:getDimension(self.character.mode, self.character.state)

        -- Remplacer 16 par "largeur des tuiles"
        if (map.isThereASolidElement(x + 16, y + 16, w - 16, h - 16, self.character)) then
            self.character.controller.canMove = false
        else
            self.character.controller.canMove = true
        end

        if self.character.fight.currentPV <= 0 then
            soundManager:playSound("contents/sounds/game/heros_death.wav", 0.3, false)
            GAMESTATE.currentState = GAMESTATE.STATE.GAMEOVER
        end

        self.character:update(dt)

        if self.character.controller:isInCinematicMode() == false then
            self:updatePVbar(dt)
            self:updateMode(dt)
            if self.character.controller.canMove then
                self.lastX, self.lastY = self.character.transform:getPosition()
                self:updatePosition(dt)
            else
                self.character.transform:setPosition(self.lastX, self.lastY)
            end

            if self:useAction(controller.action1) then
                self:fire(dt)
            end
        end
    end

    function player:addPoints(points)
        if self.points < self.maxPoints then
            self.points = self.points + 1
        end
    end

    function player:playEntranceAnimation(dt)
        if self.character:getState() ~= CHARACTERS.STATE.WALKING then
            self.character:setState(CHARACTERS.STATE.WALKING)
        end
        local x, y = self.character.transform:getPosition()
        local speed = self.character.controller.speed
        local velocityX = speed / 3 * dt
        x = x + velocityX
        self.character.transform:setPosition(x, y)
    end

    function player:resetPoints()
        self.points = 0
    end

    function player:useAction(action)
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

    function player:keypressed(key)
        if key == controller.action2 then
            if self.character:getMode() == CHARACTERS.MODE.NORMAL and self.points == self.maxPoints then
                self:resetPoints()
                self.character:setMode(CHARACTERS.MODE.BOOSTED)
                self.character.sound:playBoostedSound()
            else
                self.character:setMode(CHARACTERS.MODE.NORMAL)
            end
        end
    end

    function player:fire(dt, character)
        if self.character:getState() ~= CHARACTERS.STATE.FIRE then
            self.character:setState(CHARACTERS.STATE.FIRE)
        end
        self.character:fire(dt)
    end

    function player:updateMode(dt)
        if self.character:getMode() == CHARACTERS.MODE.BOOSTED then
            self.boosterTimer = self.boosterTimer - 1 * dt
            if self.boosterTimer <= 0 then
                self.character:setMode(CHARACTERS.MODE.NORMAL)
                soundManager:playSound("contents/sounds/game/endPlayerBoost.wav", 0.5, false)
                self.boosterTimer = self.boosterDuration
            end
        end
    end

    function player:changeState(state)
        self.character:setState(state)
    end

    function player:draw()
        self.character:draw()
        if self.character.controller:isInCinematicMode() == false then
            self:drawUI()
        end
    end

    function player:updatePosition(dt)
        if map.isOverTheMap(self.character.transform:getPosition()) == false then
            self:move(dt)
        else
            self.character:setPosition(map.clamp(self.character.transform:getPosition()))
        end
    end

    function player:move(dt)
        if
            love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
                love.keyboard.isDown(controller.left) or
                love.keyboard.isDown(controller.right)
         then
            if self.character:getState() ~= CHARACTERS.STATE.WALKING then
                self:changeState(CHARACTERS.STATE.WALKING)
            end

            local x, y = self.character.transform:getPosition()
            local sX, sY = self.character.transform:getScale()
            local speed = self.character.controller.speed
            local angle = utils.angleWithMouseWorldPosition(self.character.transform:getPosition())
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
            self.character.transform:setPosition(x, y)
        elseif self.character:getState() ~= CHARACTERS.STATE.IDLE or self.character:getMode() ~= lastMode then
            player:changeState(CHARACTERS.STATE.IDLE)
            lastMode = self.character:getMode()
        end
    end

    function player:updatePVbar(dt)
        ui.updatePlayerLifeBar(self.character.fight.currentPV, self.character.fight.maxPV)
        ui.updatePlayerPointsBar(dt, self, self.points, self.maxPoints)
    end

    function player:drawUI()
        local x, y = self.character.transform:getPosition()
        ui.drawPlayerLifeBar(
            x,
            y,
            self.character.fight.maxPV,
            self.character.fight.currentPV,
            self.character.transform.scale.x,
            self.character.transform.scale.y
        )

        ui.drawPlayerPointBar(self, self.points, self.maxPoints)
    end
    player.character.controller.player = player
    return player
end

return Player

character = require("scripts/engine/character")
controller = require("scripts/engine/controller")
camera = require("scripts/game/mainCamera")
local map = require("scripts/game/gameMap")
require("scripts/states/CHARACTERS")
GAMESTATE = require("scripts/states/GAMESTATE")
local ui = require("scripts/ui/uiGame")

-- Création d'un player basé sur Character
player = {}
player.character = nil
player.points = 0
player.maxPoints = 50
player.boosterDuration = 5
player.boosterTimer = 5
keyDown = false

function player.setCharacter(character)
    print(map.getCurrentMap().height)
    player.character = character
    player.character:setPosition(-10, map.getCurrentMap().height * map.getCurrentMap().tileheight / 2)
    player.character:setSpeed(120)
    player.character:setMode(CHARACTERS.MODE.NORMAL)
    player.changeState(CHARACTERS.STATE.IDLE)
end

function player.setInCinematicMode(bool)
    if bool then
        player.setPosition(-10, map.getCurrentMap().height * map.getCurrentMap().tileheight / 2)
    end
    player.character:setInCinematicMode(bool)
end

function player.getCharacter()
    return player.character
end

function player.load()
    player.character:load()
    player.boosterTimer = player.boosterDuration
end

function player.update(dt)
    local gmap = map.getCurrentMap()
    x, y = player.character:getPosition()

    local w, h = player.character:getDimension()

    -- Remplacer 16 par "largeur des tuiles"
    if (map.isThereASolidElement(x + 16, y + 16, w - 16, h - 16, player.character)) then
        player.character.canMove = false
    else
        player.character.canMove = true
    end

    if player.character:getCurrentPV() <= 0 then
        soundManager:playSound("contents/sounds/game/heros_death.wav", 0.3, false)
        GAMESTATE.currentState = GAMESTATE.STATE.GAMEOVER
    end

    player.character:update(dt)

    if player.character:isInCinematicMode() == false then
        player.updatePVbar(dt)
        player.updateMode(dt)
        if player.character.canMove then
            lastX, lastY = player.character:getPosition()
            player.updatePosition(dt)
        else
            player.character:setPosition(lastX, lastY)
        end

        if player.useAction(controller.action1) then
            player.fire(dt)
        end
    end
end

function player.draw()
    player.character:draw()
    if player.character:isInCinematicMode() == false then
        player.drawUI()
    end
end

function player.updateMode(dt)
    if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
        player.boosterTimer = player.boosterTimer - 1 * dt
        if player.boosterTimer <= 0 then
            player.character:setMode(CHARACTERS.MODE.NORMAL)
            soundManager:playSound("contents/sounds/game/endPlayerBoost.wav", 0.5, false)
            player.boosterTimer = player.boosterDuration
        end
    end
end

function player.updatePosition(dt)
    if map.isOverTheMap(player.character:getPosition()) == false then
        player.move(dt)
    else
        player.character:setPosition(map.clamp(player.character:getPosition()))
    end
end

function player.fire(dt)
    if player.character:getState() ~= CHARACTERS.STATE.FIRE then
        player.changeState(CHARACTERS.STATE.FIRE)
    end
    player.character:fire(dt)
end

function player.changeState(state)
    player.character:setState(state)
end

function player.move(dt)
    if
        love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
            love.keyboard.isDown(controller.left) or
            love.keyboard.isDown(controller.right)
     then
        if player.character:getState() ~= CHARACTERS.STATE.WALKING then
            player.changeState(CHARACTERS.STATE.WALKING)
        end

        x, y = player.character:getPosition()
        local sX, sY = player.character:getScale()
        local speed = player.character:getSpeed()
        local angle = utils.angleWithMouseWorldPosition(player.character:getPosition())
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
        player.setPosition(x, y)
    elseif player.character:getState() ~= CHARACTERS.STATE.IDLE or player.character:getMode() ~= lastMode then
        player.changeState(CHARACTERS.STATE.IDLE)
        lastMode = player.character:getMode()
    end
end

function player.setPosition(x, y)
    player.character:setPosition(x, y)
end

function player.keypressed(key)
    if key == controller.action2 then
        if player.character:getMode() == CHARACTERS.MODE.NORMAL and player.points == player.maxPoints then
            player.resetPoints()
            player.character:setMode(CHARACTERS.MODE.BOOSTED)
            soundManager:playSound("contents/sounds/game/heros_transform.wav", 0.4, false)
        else
            player.character:setMode(CHARACTERS.MODE.NORMAL)
        end
    end

    if key == "up" then
        player.points = player.points + 1
    end

    if key == "down" then
        player.points = player.points - 1
    end
end

function player.updatePVbar(dt)
    ui.updatePlayerLifeBar(player.character:getCurrentPV(), player.character:getMaxPV())
    ui.updatePlayerPointsBar(dt, player.points, player.maxPoints)
end

function player.drawUI()
    local x, y = player.character:getPosition()
    ui.drawPlayerLifeBar(
        x,
        y,
        player.character:getMaxPV(),
        player.character:getCurrentPV(),
        player.scaleX,
        player.scaleY
    )

    ui.drawPlayerPointBar(player.points, player.maxPoints)
end

function player.addPoints(points)
    if player.points < player.maxPoints then
        player.points = player.points + 1
    end
end

function player.resetPoints()
    player.points = 0
end

function player.playEntranceAnimation(dt)
    if player.character:getState() ~= CHARACTERS.STATE.WALKING then
        player.changeState(CHARACTERS.STATE.WALKING)
    end

    x, y = player.character:getPosition()
    local sX, sY = player.character:getScale()
    local speed = player.character:getSpeed()
    local velocityX = speed / 3 * dt

    x = x + velocityX * sX

    player.setPosition(x, y)
end

function player.useAction(action)
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

return player

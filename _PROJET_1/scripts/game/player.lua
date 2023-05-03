-- RAJOUTER UNE VITESSE DE TIR POUR JOUER LA DESSUS DANS LE JEU
-- TENTER LE MOUVEMENT DE LA CAMERA

character = require("scripts/engine/character")
controller = require("scripts/engine/controller")
camera = require("scripts/game/mainCamera")
local map = require("scripts/game/gameMap")
require("scripts/states/PLAYER_STATE")
-- Création d'un player basé sur Character
player = {}
player.character = nil

player.weapon = love.graphics.newImage("contents/images/magicStaff.png")

keyDown = false

-- Gère les tilesheet
player.images = {}
player.lookAt = "left"
player.canMove = true
player.canFire = true
local lastPositionY = nil
local lastPositionY = nil

fireList = {}

function player.setCharacter(character)
    player.character = character
    player.character:setPosition(100, 300)
    player.character:setSpeed(120)

    player.character:setMode(CHARACTERS.MODE.NORMAL)
    player.changeState(1)
end

function player.getCharacter()
    return player.character
end

function player.load()
end

function player.update(dt)
    if map.isOverTheMap(player.character:getPosition()) == false then
        player.move(dt)
    else
        player.character:setPosition(map.clamp(player.character:getPosition()))
    end

    if love.keyboard.isDown(controller.action1) == false then
        player.canMove = true
    end

    if
        love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
            love.keyboard.isDown(controller.left) or
            love.keyboard.isDown(controller.right) or
            love.keyboard.isDown(controller.action1) or
            love.keyboard.isDown(controller.action2)
     then
        keyDown = true
    else
        if player.character:getState() ~= PLAYER_STATE[1] or player.character:getMode() ~= lastMode then
            keyDown = false
            player.changeState(1)
            lastMode = player.character:getMode()
        end
    end

    if love.keyboard.isDown(controller.action1) then
        player.character:fire(dt)
    end

    for n = #fireList, 1, -1 do
        local t = fireList[n]
        t.x = t.x + t.speed * math.cos(t.angle)
        t.y = t.y + t.speed * math.sin(t.angle)
        t.lifeTime = t.lifeTime - dt
        if t.lifeTime <= 0 then
            table.remove(fireList, n)
        end
    end
    lastPositionX, lastPositionY = player.character:getPosition()
end

function player.draw()
    for k, v in ipairs(fireList) do
        love.graphics.circle("fill", v.x, v.y, 5, 5)
    end

    x, y = player.character:getPosition()

    --- PV BAR --
    love.graphics.setColor(1, 1, 1, 0.3)
    love.graphics.rectangle("fill", x - 35, y - 35, 50, 10, player.scaleX, player.scaleY)
    love.graphics.setColor(0.06, 0.69, 0.27)
    love.graphics.rectangle("fill", x - 35, y - 35, 40, 10, player.scaleX, player.scaleY)
    love.graphics.setColor(1, 1, 1)

    -- EN PV BAR --
end

function player.changeState(nb)
    player.character:setState(PLAYER_STATE[nb])
end

function player.move(dt)
    if player.canMove then
        local speed = player.character:getSpeed()
        x, y = player.character:getPosition()
        if
            love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
                love.keyboard.isDown(controller.left) or
                love.keyboard.isDown(controller.right)
         then
            if player.character:getState() ~= PLAYER_STATE[2] then
                player.changeState(2)
            end

            if love.keyboard.isDown(controller.up) then
                local mx, my = love.mouse.getPosition()
                local cX, cY = mainCamera.camera:getPosition()
                mx = mx + cX
                my = my + cY
                local angle = math.atan2(my - y, mx - x)
                x = x + speed * dt * math.cos(angle)
                y = y + speed * dt * math.sin(angle)
            elseif love.keyboard.isDown(controller.down) then
                local mx, my = love.mouse.getPosition()
                local cX, cY = mainCamera.camera:getPosition()
                mx = mx + cX
                my = my + cY
                local angle = math.atan2(my - y, mx - x)
                x = x - speed * dt * math.cos(angle)
                y = y - speed * dt * math.sin(angle)
            elseif love.keyboard.isDown(controller.left) then
                local mx, my = love.mouse.getPosition()
                local cX, cY = mainCamera.camera:getPosition()
                local sX, sY = player.character:getScale()
                mx = mx + cX
                my = my + cY
                local angle = math.atan2(my - y, mx - x)
                x = x - (speed * dt * math.cos(angle)) * sX
                y = y - (speed * dt * math.sin(angle)) * sX
            elseif love.keyboard.isDown(controller.right) then
                local mx, my = love.mouse.getPosition()
                local cX, cY = mainCamera.camera:getPosition()
                local sX, sY = player.character:getScale()
                mx = mx + cX
                my = my + cY
                local angle = math.atan2(my - y, mx - x)
                x = x + (speed * dt * math.cos(angle)) * sX
                y = y + (speed * dt * math.sin(angle)) * sX
            end
            player.setPosition(x, y)
        end
    end
end

function player.setPosition(x, y)
    player.character:setPosition(x, y)
end

function player.keypressed(key)
    if key == controller.action1 then
        player.canFire = true
    end
    if key == controller.action2 then
        if player.character:getMode() == CHARACTERS.MODE.NORMAL then
            player.character:setMode(CHARACTERS.MODE.BOOSTED)
            player.character:changeWeapon(2)
        else
            player.character:setMode(CHARACTERS.MODE.NORMAL)
            player.character:changeWeapon(1)
        end
    end
end

function player.animFire()
    if player.character:getState() ~= PLAYER_STATE[3] then
        player.changeState(3)
        player.canMove = false
    end
end

return player

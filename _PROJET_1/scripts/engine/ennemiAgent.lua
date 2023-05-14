require("scripts/Utils/utils")
require("scripts/states/CHARACTERS")
require("scripts/states/CONST")

local map = require("scripts/game/gameMap")
ennemiAgent = {}
local ennemiAgent_mt = {__index = ennemiAgent}

function ennemiAgent.new()
    newEnnemi = {}
    return setmetatable(newEnnemi, ennemiAgent_mt)
end

function ennemiAgent:create()
    local ennemiAgent = {}
    ennemiAgent.randomNumber = 0
    ennemiAgent.velocityX = 0
    ennemiAgent.velocityY = 0
    ennemiAgent.character = nil
    ennemiAgent.angle = 0
    ennemiAgent.range = love.math.random(10,20)
    ennemiAgent.isCinematicMode = false
    ennemiAgent.timer = 0
    ennemiAgent.timerIsStarted = false
    lastPositionX = 0
    lastPositionY = 0
    ennemiAgent.isAlreadyIDLE = false
    chiffre = 0

    function ennemiAgent:init(character)
        ennemiAgent.character = character
        local x, y = ennemiAgent.character:getPosition()
        local speed = ennemiAgent.character:getSpeed()
        local mpW, mpH = map.getMapDimension()
        ennemiAgent.angle = Utils.angle(x, y, love.math.random(0, mpW), love.math.random(0, mpH))
        ennemiAgent.velocityX = speed * math.cos(ennemiAgent.angle)
        ennemiAgent.velocityY = speed * math.sin(ennemiAgent.angle)
    end

    function ennemiAgent:update(dt, positionX, positionY, currentState)
        local x, y = positionX, positionY
        local targetX, targetY = ennemiAgent.character:getTargetPos()
        local distance = Utils.distance(x, y, targetX, targetY)
        local ennemiWidth, ennemiHeight = ennemiAgent.character:getDimension()

        if currentState == CHARACTERS.STATE.IDLE then
            local speed = ennemiAgent.character:getSpeed()
            local mpW, mpH = map.getMapDimension()
            ennemiAgent.angle = Utils.angle(x, y, love.math.random(0, mpW), love.math.random(0, mpH))
            ennemiAgent.velocityX = speed * math.cos(ennemiAgent.angle)
            ennemiAgent.velocityY = speed * math.sin(ennemiAgent.angle)

            if (ennemiAgent.character:getWeaponRange()) then
                ennemiAgent.randomNumber =
                    math.random(-ennemiAgent.character:getWeaponRange() + 1, ennemiAgent.character:getWeaponRange() - 1)
            end

            ennemiAgent.character:setState(CHARACTERS.STATE.WALKING)
        elseif currentState == CHARACTERS.STATE.WALKING then
            local ennemiWidth, ennemiHeight = ennemiAgent.character:getDimension()
            local newPositionX = x + ennemiAgent.velocityX * dt
            local newPositionY = y + ennemiAgent.velocityY * dt

            if map.isOverTheMap(newPositionX, newPositionY) then
                ennemiAgent.character:setPosition(map.clamp(newPositionX, newPositionY))
                ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
            else
                local ennemiWidth, ennemiHeight = ennemiAgent.character:getDimension()
                local gmap = map.getCurrentMap()

                if (map.isThereASolidElement(newPositionX, newPositionY, ennemiWidth, ennemiHeight)) then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                else
                    ennemiAgent.character:setPosition(newPositionX, newPositionY)
                    lastPositionX, lastPositionY = ennemiAgent.character:getPosition()

                    if ennemiAgent.character:isInCinematicMode() == false then
                        ---- On cherche le joueur, qui est dans target déjà.  ----
                        if distance <= ennemiAgent.range then
                            if ennemiAgent.timerIsStarted == false then
                                ennemiAgent.timer = 0
                                ennemiAgent.timerIsStarted = true
                            else
                                ennemiAgent.timer = ennemiAgent.timer + dt
                                if ennemiAgent.timer >= 1.5 then
                                    ennemiAgent.character:setState(CHARACTERS.STATE.ALERT)
                                    ennemiAgent.timerIsStarted = false
                                end
                            end
                        end
                    end
                end
            end
            if ennemiAgent.velocityX < 0 then
                ennemiAgent.character:changeDirection(CONST.DIRECTION.left, dt)
            else
                ennemiAgent.character:changeDirection(CONST.DIRECTION.right, dt)
            end
        elseif currentState == CHARACTERS.STATE.ALERT then
            local speed = ennemiAgent.character:getSpeed() * 2
            local scaleX, scaleY = ennemiAgent.character:getScale()

            ennemiAgent.angle =
                Utils.angle(x, y, targetX + ennemiAgent.randomNumber, targetY + ennemiAgent.randomNumber)

            ennemiAgent.velocityX = (speed * math.cos(ennemiAgent.angle))
            ennemiAgent.velocityY = (speed * math.sin(ennemiAgent.angle))

            local newPositionX = x + ennemiAgent.velocityX * dt
            local newPositionY = y + ennemiAgent.velocityY * dt

            local ennemiWidth, ennemiHeight = ennemiAgent.character:getDimension()

            local gmap = map.getCurrentMap()
            if (map.isThereASolidElement(newPositionX, newPositionY, ennemiWidth, ennemiHeight)) then
                ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
            else
                ennemiAgent.character:setPosition(newPositionX, newPositionY)
                lastPositionX, lastPositionY = newPositionX, newPositionY
            end

            -- VERIFIER SI JE PERDS DE VUE LE HEROS
            if ennemiAgent.character:getWeaponRange() then
                if distance <= ennemiAgent.character:getWeaponRange() then
                    ennemiAgent.character:setState(CHARACTERS.STATE.FIRE)
                elseif distance > ennemiAgent.range then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                end
            end
            if ennemiAgent.velocityX < 0 then
                ennemiAgent.character:changeDirection(CONST.DIRECTION.left, dt)
            else
                ennemiAgent.character:changeDirection(CONST.DIRECTION.right, dt)
            end
        elseif currentState == CHARACTERS.STATE.FIRE then
            if ennemiAgent.character:getWeaponRange() then
                if distance > ennemiAgent.character:getWeaponRange() then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                else
                    ennemiAgent.character:fire(dt)
                    if ennemiAgent.character:getCurrentWeapon():getIsRangedWeapon() == false then
                        ennemiAgent.character:getTarget():hit(
                            ennemiAgent.character,
                            (ennemiAgent.character:getCurrentWeapon():getDamage() * dt) / 30
                        )
                    end
                end
            end
        end
    end

    function ennemiAgent:draw()
    end

    return ennemiAgent
end

return ennemiAgent

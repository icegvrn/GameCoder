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
    ennemiAgent.range = love.math.random(10, 20)
    ennemiAgent.isCinematicMode = false
    ennemiAgent.timer = 0
    ennemiAgent.timerIsStarted = false
    lastPositionX = 0
    lastPositionY = 0
    ennemiAgent.isAlreadyIDLE = false
    chiffre = 0

    function ennemiAgent:init(character)
        ennemiAgent.character = character
        local x, y = ennemiAgent.character.transform:getPosition()
        local speed = ennemiAgent.character.controller.speed
        local mpW, mpH = map.getMapDimension()
        ennemiAgent.angle = Utils.angle(x, y, love.math.random(0, mpW), love.math.random(0, mpH))
        ennemiAgent.velocityX = speed * math.cos(ennemiAgent.angle)
        ennemiAgent.velocityY = speed * math.sin(ennemiAgent.angle)
    end

    function ennemiAgent:update(dt, positionX, positionY, currentState)
    
        local x, y = positionX, positionY
        local targetX, targetY = ennemiAgent.character.controller.target.transform:getPosition()
        local distance = Utils.distance(x, y, targetX, targetY)
        local ennemiWidth, ennemiHeight =
            ennemiAgent.character.sprites:getDimension(ennemiAgent.character.mode, ennemiAgent.character.state)

        if currentState == CHARACTERS.STATE.IDLE then
            local speed = ennemiAgent.character.controller.speed
            local mpW, mpH = map.getMapDimension()
            ennemiAgent.angle = Utils.angle(x, y, love.math.random(0, mpW), love.math.random(0, mpH))
            ennemiAgent.velocityX = speed * math.cos(ennemiAgent.angle)
            ennemiAgent.velocityY = speed * math.sin(ennemiAgent.angle)

            if (ennemiAgent.character.fight.weaponSlot:getWeaponRange()) then
                ennemiAgent.randomNumber =
                    math.random(
                    -ennemiAgent.character.fight.weaponSlot:getWeaponRange() + 1,
                    ennemiAgent.character.fight.weaponSlot:getWeaponRange() - 1
                )
            end

            ennemiAgent.character:setState(CHARACTERS.STATE.WALKING)
        elseif currentState == CHARACTERS.STATE.WALKING then
            local ennemiWidth, ennemiHeight =
                ennemiAgent.character.sprites:getDimension(ennemiAgent.character.mode, ennemiAgent.character.state)
            local newPositionX = x + ennemiAgent.velocityX * dt
            local newPositionY = y + ennemiAgent.velocityY * dt

            if map.isOverTheMap(newPositionX, newPositionY) then
                ennemiAgent.character:setPosition(map.clamp(newPositionX, newPositionY))
                ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
            else
                local ennemiWidth, ennemiHeight =
                    ennemiAgent.character.sprites:getDimension(ennemiAgent.character.mode, ennemiAgent.character.state)
                local gmap = map.getCurrentMap()

                if (map.isThereASolidElement(newPositionX, newPositionY, ennemiWidth, ennemiHeight)) then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                else
                    ennemiAgent.character:setPosition(newPositionX, newPositionY)
                    lastPositionX, lastPositionY = ennemiAgent.character.transform:getPosition()

                    if ennemiAgent.character.controller:isInCinematicMode() == false then
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
                ennemiAgent.character.controller:changeDirection(ennemiAgent.character, CONST.DIRECTION.left, dt)
            else
                ennemiAgent.character.controller:changeDirection(ennemiAgent.character, CONST.DIRECTION.right, dt)
            end
        elseif currentState == CHARACTERS.STATE.ALERT then
            local speed = ennemiAgent.character.controller.speed * 2
            local scaleX, scaleY = ennemiAgent.character.transform:getScale()

            ennemiAgent.angle =
                Utils.angle(x, y, targetX + ennemiAgent.randomNumber, targetY + ennemiAgent.randomNumber)

            ennemiAgent.velocityX = (speed * math.cos(ennemiAgent.angle))
            ennemiAgent.velocityY = (speed * math.sin(ennemiAgent.angle))

            local newPositionX = x + ennemiAgent.velocityX * dt
            local newPositionY = y + ennemiAgent.velocityY * dt

            local ennemiWidth, ennemiHeight =
                ennemiAgent.character.sprites:getDimension(ennemiAgent.character.mode, ennemiAgent.character.state)

            local gmap = map.getCurrentMap()
            if (map.isThereASolidElement(newPositionX, newPositionY, ennemiWidth, ennemiHeight)) then
                ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
            else
                ennemiAgent.character:setPosition(newPositionX, newPositionY)
                lastPositionX, lastPositionY = newPositionX, newPositionY
            end

            -- VERIFIER SI JE PERDS DE VUE LE HEROS
            if ennemiAgent.character.fight.weaponSlot:getWeaponRange() then
                if distance <= ennemiAgent.character.fight.weaponSlot:getWeaponRange() then
                    ennemiAgent.character:setState(CHARACTERS.STATE.FIRE)
                elseif distance > ennemiAgent.range then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                end
            end
            if ennemiAgent.velocityX < 0 then
                ennemiAgent.character.controller:changeDirection(ennemiAgent.character, CONST.DIRECTION.left, dt)
            else
                ennemiAgent.character.controller:changeDirection(ennemiAgent.character, CONST.DIRECTION.right, dt)
            end
        elseif currentState == CHARACTERS.STATE.FIRE then
            if ennemiAgent.character.fight.weaponSlot:getWeaponRange() then
                if distance > ennemiAgent.character.fight.weaponSlot:getWeaponRange() then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                else
                    ennemiAgent.character:fire(dt)
                    if ennemiAgent.character:getCurrentWeapon():getIsRangedWeapon() == false then
                        ennemiAgent.character.controller.target.fight:hit(
                            ennemiAgent.character,
                            (ennemiAgent.character:getCurrentWeapon():getDamage() * dt) / 30
                        )
                    end
                end
            end
        end
    end
    return ennemiAgent
end

return ennemiAgent

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
    ennemiAgent.range = math.random(10, 30)

    function ennemiAgent:init(character)
        ennemiAgent.character = character
        local x, y = ennemiAgent.character:getPosition()
        local speed = ennemiAgent.character:getSpeed()
        ennemiAgent.angle =
            Utils.angle(x, y, love.math.random(0, Utils.screenWidth), love.math.random(0, Utils.screenHeight))
        ennemiAgent.velocityX = speed * math.cos(ennemiAgent.angle)
        ennemiAgent.velocityY = speed * math.sin(ennemiAgent.angle)
    end

    function ennemiAgent:update(dt, positionX, positionY, currentState)
        local x, y = positionX, positionY
        local targetX, targetY = ennemiAgent.character:getTargetPos()
        local agentX, agentY = ennemiAgent.character:getPosition()
        local distance = Utils.distance(agentX, agentY, targetX, targetY)

        if currentState == CHARACTERS.STATE.IDLE then
            local speed = ennemiAgent.character:getSpeed()
            ennemiAgent.angle =
                Utils.angle(x, y, love.math.random(0, Utils.screenWidth), love.math.random(0, Utils.screenHeight))
            ennemiAgent.velocityX = speed * math.cos(ennemiAgent.angle)
            ennemiAgent.velocityY = speed * math.sin(ennemiAgent.angle)
            if (ennemiAgent.character:getWeaponRange()) then
                ennemiAgent.randomNumber =
                    math.random(-ennemiAgent.character:getWeaponRange() + 1, ennemiAgent.character:getWeaponRange() - 1)
            end
            ennemiAgent.character:setState(CHARACTERS.STATE.WALKING)
        elseif currentState == CHARACTERS.STATE.WALKING then
            local newPositionX = x + ennemiAgent.velocityX * dt
            local newPositionY = y + ennemiAgent.velocityY * dt

            if map.isOverTheMap(newPositionX, newPositionY) then
                ennemiAgent.character:setPosition(map.clamp(newPositionX, newPositionY))
                ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
            else
                ennemiAgent.character:setPosition(newPositionX, newPositionY)
                if ennemiAgent.velocityX < 0 then
                    ennemiAgent.character:changeDirection(CONST.DIRECTION.left)
                else
                    ennemiAgent.character:changeDirection(CONST.DIRECTION.right)
                end
            end

            ---- On cherche le joueur, qui est dans target déjà.  ----

            if distance <= ennemiAgent.range then
                ennemiAgent.character:setState(CHARACTERS.STATE.ALERT)
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

            ennemiAgent.character:setPosition(map.clamp(newPositionX, newPositionY))

            -- VERIFIER SI JE PERDS DE VUE LE HEROS
            if ennemiAgent.character:getWeaponRange() then
                if distance <= ennemiAgent.character:getWeaponRange() then
                    ennemiAgent.character:setState(CHARACTERS.STATE.FIRE)
                elseif distance > ennemiAgent.range then
                    ennemiAgent.character:setState(CHARACTERS.STATE.IDLE)
                end
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

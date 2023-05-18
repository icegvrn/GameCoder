ennemiAgent = {}
local ennemiAgent_mt = {__index = ennemiAgent}

function ennemiAgent.new()
    newEnnemi = {}
    return setmetatable(newEnnemi, ennemiAgent_mt)
end

function ennemiAgent:create()
    local ennemiAgent = {}
    ennemiAgent.range = love.math.random(10, 20)
    ennemiAgent.timer = 0
    ennemiAgent.timerIsStarted = false

    function ennemiAgent:init(ennemi)
        ennemi.animator:init(ennemi)
    end

    function ennemiAgent:update(dt, ennemi, positionX, positionY, currentState)
        local x, y = positionX, positionY
        targetX, targetY = ennemi.character.controller.target:getPosition()
        local distance = Utils.distance(x, y, targetX, targetY)

        if currentState == CHARACTERS.STATE.IDLE then
            self:walkToRandomPoint(dt, ennemi)
        elseif currentState == CHARACTERS.STATE.WALKING then
            self:patrol(dt, ennemi, distance)
        elseif currentState == CHARACTERS.STATE.ALERT then
            self:chase(dt, ennemi, distance, targetX, targetY)
        elseif currentState == CHARACTERS.STATE.FIRE then
            self:attack(dt, ennemi, distance)
        end
    end

    function ennemiAgent:walkToRandomPoint(dt, ennemi)
        ennemi.animator:chooseARandomPoint(dt, ennemi)
        ennemi.character:setState(CHARACTERS.STATE.WALKING)
    end

    function ennemiAgent:patrol(dt, ennemi, distance)
        if ennemi.animator:tryMove(dt, ennemi) then
            if ennemi.character.controller:isInCinematicMode() == false then
                self:lookingForTarget(dt, ennemi, distance)
            end
        else
            ennemi.character:setState(CHARACTERS.STATE.IDLE)
        end
    end

    function ennemiAgent:lookingForTarget(dt, ennemi, distance)
        if distance <= ennemiAgent.range then
            if ennemiAgent.timerIsStarted == false then
                ennemiAgent.timer = 0
                ennemiAgent.timerIsStarted = true
            else
                ennemiAgent.timer = ennemiAgent.timer + dt
                if ennemiAgent.timer >= 1.5 then
                    ennemi.character:setState(CHARACTERS.STATE.ALERT)
                    ennemiAgent.timerIsStarted = false
                end
            end
        end
    end

    function ennemiAgent:chase(dt, ennemi, distance, targetX, targetY)
        -- VERIFIER SI JE PERDS DE VUE LE HEROS
        if ennemi.animator:chase(dt, ennemi, targetX, targetY) then
            if ennemi.character.fight.weaponSlot:getWeaponRange() then
                if distance <= ennemi.character.fight.weaponSlot:getWeaponRange() then
                    ennemi.character:setState(CHARACTERS.STATE.FIRE)
                elseif distance > ennemiAgent.range then
                    ennemi.character:setState(CHARACTERS.STATE.IDLE)
                end
            end
        else
            ennemi.character:setState(CHARACTERS.STATE.IDLE)
        end
    end

    function ennemiAgent:attack(dt, ennemi, distance)
        if ennemi.character.fight.weaponSlot:getWeaponRange() then
            if distance > ennemi.character.fight.weaponSlot:getWeaponRange() then
                ennemi.character:setState(CHARACTERS.STATE.IDLE)
            else
                ennemi.character:fire(dt)

                --- A METTRE DANS WEAPON
                if ennemi.character:getCurrentWeapon():getIsRangedWeapon() == false then
                    ennemi.character.controller.target.fight:hit(
                        ennemi.character,
                        (ennemi.character:getCurrentWeapon():getDamage() * dt) / 30
                    )
                end
            end
        end
    end
    return ennemiAgent
end

return ennemiAgent

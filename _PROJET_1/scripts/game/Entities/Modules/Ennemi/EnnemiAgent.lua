-- MODULE QUI EST EN QUELQUE SORTE L'IA DES ENNEMIS. ECHANGE AVEC L'ANIMATOR POUR FAIRE LES DEPLACEMENTS
-- SE BASE SUR LES STATES

ennemiAgent = {}
local ennemiAgent_mt = {__index = ennemiAgent}

function ennemiAgent.new()
    newEnnemi = {}
    return setmetatable(newEnnemi, ennemiAgent_mt)
end

function ennemiAgent:create()
    local ennemiAgent = {}
    ennemiAgent.range = love.math.random(10, 30)
    ennemiAgent.timer = 0
    ennemiAgent.timerIsStarted = false

    function ennemiAgent:init(ennemi)
        ennemi.animator:init(ennemi)
    end

    -- Update l'IA ennemi : marche si IDLE, patrouille si marche, est chase si est alerté, et attaque si a porté
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

    -- Fonction pour faire marcher l'ennemi jusqu'à un point random
    function ennemiAgent:walkToRandomPoint(dt, ennemi)
        ennemi.animator:chooseARandomPoint(dt, ennemi)
        ennemi.character:setState(CHARACTERS.STATE.WALKING)
    end

    -- Fonction pour faire patrouiller l'ennemi : s'il parvient à bouger et qu'il n'est pas en cinématique
    -- il reste en attente de trouver un joueur à attaquer, sinon c'est qu'il a heurté quelque chose et il
    -- repasse en mode IDLE pour choisir un nouvel endroit où se rendre
    function ennemiAgent:patrol(dt, ennemi, distance)
        if ennemi.animator:tryMove(dt, ennemi) then
            if ennemi.character.controller:isInCinematicMode() == false then
                self:lookingForTarget(dt, ennemi, distance)
            end
        else
            ennemi.character:setState(CHARACTERS.STATE.IDLE)
        end
    end

    -- Fonction de recherche de joueur a attaqué : on compare son range à la distance qui le sépare du joueur
    -- Si c'est plus petit pendant une certaine durée, il passe en alert
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

    -- Fonction de chase utilisée en alert. S'il réussit à poursuivre le héros, soit il atteint son objectif
    -- et se trouve à portée et passe en mode attaque (fire), soit le joueur parvient à le semer, donc il
    -- repasse en IDLE quand le joueur n'est plus à portée
    -- S'il ne parvient pas à chase, c'est qu'il a heurté quelque chose, il repasse en IDLE
    function ennemiAgent:chase(dt, ennemi, distance, targetX, targetY)
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

    -- Fonction d'attaque quand il est en mode fire : s'il a une arme et que la distance entre le joueur et le range
    -- de l'arme est suffisant, il attaque, sinon il repasse en ALERT.
    function ennemiAgent:attack(dt, ennemi, distance)
        if ennemi.character.fight.weaponSlot:getWeaponRange() then
            if distance > ennemi.character.fight.weaponSlot:getWeaponRange() then
                ennemi.character:getCurrentWeapon().attack.isFiring = false
                ennemi.character:setState(CHARACTERS.STATE.ALERT)
            else
                ennemi.character:fire(dt)
            end
        end
    end
    return ennemiAgent
end

return ennemiAgent

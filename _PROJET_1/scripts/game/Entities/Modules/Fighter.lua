-- MODULE QUI CONTIENT LES CARACTERISTIQUES DE "COMBATTANT" D'UN CHARACTER
PointsUI = require(PATHS.MODULES.POINTSUI)
Sound = require(PATHS.SOUND)
WeaponSlot = require(PATHS.MODULES.WEAPONSLOT)

local c_Fighter = {}
local Fighter_mt = {__index = c_Fighter}

function c_Fighter.new()
    local c_fighter = {}
    w_slot = WeaponSlot.new()
    p_UI = PointsUI.new()
    return setmetatable(c_fighter, Fighter_mt)
end

function c_Fighter:create()
    local fighter = {
        maxPV = 100,
        currentPV = 100,
        strenght = 10,
        weaponSlot = w_slot:create(),
        canBeHurt = true,
        canFire = true,
        isHit = false,
        alertImg = love.graphics.newImage(PATHS.IMG.CHARACTERS .. "exclamation.png"),
        timer = 0,
        dieTimer = 0,
        pointsUI = p_UI:create()
    }

    -- Update des evenements du fighter:  est-ce que le personnage est touché, est-ce qu'il est mort ?
    -- Modification de la position de l'arme
    function fighter:update(dt, character, sprites, soundModule, target)
        if self.isHit then
            self:hitEvents(dt, character, sprites, soundModule)
        end

        if self.currentPV <= 0 then
            self:dyingEvents(dt, character, soundModule)
        end

        self.weaponSlot:moveWeapon(dt, character, target)
    end

    -- Fonction fire qui appelle celle de weapon
    function fighter:fire(dt, parent)
        local weapon = self.weaponSlot.weapon[self.weaponSlot.currentWeaponId]
        weapon.attack:fire(
            dt,
            weapon,
            parent,
            parent.transform.position,
            parent.transform.scale,
            self.weaponSlot.handPosition,
            self.weaponSlot.weaponScaling,
            parent.controller.target
        )
    end

    -- Fonction qui met les dégâts : baisse les PV et si c'est le joueur l'attaquant, donne des points au joueur
    function fighter:hit(attacker, damage)
        if self.canBeHurt == true then
            self.isHit = true
            self.currentPV = self.currentPV - damage
            if attacker.controller.player then
                if self.currentPV <= 0 then
                    attacker.controller.player.pointsCounter:addPoints(attacker.controller.player, self.maxPV / 2)
                else
                    attacker.controller.player.pointsCounter:addPoints(attacker.controller.player, self.maxPV / 10)
                end
            end
        end
    end

    -- Fonction qui permet de jouer un son et coloriser un personnage blessé, avec un timer pour remettre le perso à son état initial
    -- Elle met également le personnage sur "dead" s'il n'a plus de PV
    function fighter:hitEvents(dt, parent, sprites, soundModule)
        if self.isHit then
            self:hitSound(parent, soundModule)
            sprites:changeSpriteColorTo({1, 0, 0, 1})
            self.timer = self.timer + dt
            if self.timer >= 0.4 then
                self.timer = 0
                self.isHit = false
                sprites:changeSpriteColorTo({1, 1, 1, 1})
                soundModule.playSound = false
            end

            if parent.controller.player then
                if parent.fight.currentPV <= 0 then
                    parent.controller.player.isDead = true
                    soundManager:playSound(PATHS.SOUNDS.GAME .. "/heros_death.wav", 0.3, false)
                end
            end
        end
    end

    -- Fonction qui laisse un petit délai avant de supprimer un personnage quand il meurt pour qu'on puisse voir les points gagnés et entendre le son de mort
    function fighter:dyingEvents(dt, parent, soundModule)
        parent:setState(CHARACTERS.STATE.DEAD)
        self.canBeHurt = false
        self.dieTimer = self.dieTimer + 1 * dt
        if self.dieTimer >= 0.5 then
            soundModule:dyingSound()
            self.dieTimer = 0
            parent:destroy()
        end
    end

    -- Fonction qui permet de jouer un son de blessure (blessure joueur ou blessure ennemi selon qui est le controlleur)
    function fighter:hitSound(parent, soundModule)
        if soundModule.playSound == false then
            if self.isHit then
                if parent.controller.player then
                    soundManager:playSound(PATHS.SOUNDS.GAME .. "heros_hitted.wav", 0.7, false)
                else
                    soundManager:playSound(PATHS.SOUNDS.GAME .. "ennemi_hitted.wav", 0.1, false)
                end
                soundModule.playSound = true
            end
        end
    end

    -- Fonction qui permet de changer l'ID de l'arme que doit tenir le personnage
    function fighter:changeWeapon(nb)
        self.weaponSlot.currentWeaponId = nb
    end

    -- Fonction qui permet de draw les points gagné (utilisé par le joueur)
    function fighter:drawHittingPoints(parent)
        local font = UIAll.font10
        if self.isHit then
            local points = self.maxPV / 10
            local pvColor = {1, 1, 1}

            if self.currentPV <= 0 then
                points = self.maxPV / 2
                pvColor = {1, 0.9, 0.1}
                font = UIAll.font15
            end

            self.pointsUI:showPoints(
                points,
                pvColor,
                font,
                parent.transform.position.x,
                parent.transform.position.y - 30
            )
        end
    end

    -- Fonction qui permet de droit un point d'exclamation quand le personnage a repéré un adversaire (utilisé pour ennemis)
    function fighter:drawAlertSign(parent)
        love.graphics.draw(
            self.alertImg,
            parent.transform.position.x,
            parent.transform.position.y - 30,
            0,
            parent.transform.scale.x,
            parent.transform.scale.y
        )
    end

    -- Fonction qui appelle le draw de l'arme
    function fighter:drawWeapon(state)
        if state ~= CHARACTERS.STATE.DEAD then
            if #self.weaponSlot.weapon ~= 0 then
                self.weaponSlot.weapon[self.weaponSlot.currentWeaponId]:draw()
            end
        end
    end

    -- Fonction qui permet d'upgrade le niveau de combat en augmentant la vitesse de l'arme
    function fighter:upFightLevel()
        self.weaponSlot.weapon[self.weaponSlot.currentWeaponId]:upgradeWeapon()
    end

    -- Setters
    function fighter:setMaxPV(pv)
        self.maxPV = pv
    end

    function fighter:setStrenght(strg)
        self.strenght = strg
    end
    return fighter
end

return c_Fighter

PointsUI = require("scripts/game/Entities/Characters/Modules/c_PointsUI")
Sound = require("scripts/game/Entities/Characters/Modules/c_Sound")
WeaponSlot = require("scripts/game/Entities/Characters/Modules/c_WeaponSlot")

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
        alertImg = love.graphics.newImage("contents/images/characters/exclamation.png"),
        timer = 0,
        dieTimer = 0,
        pointsUI = p_UI:create()
    }

    function fighter:fire(dt, parent)
        local weapon = self.weaponSlot.weapon[self.weaponSlot.currentWeaponId]
        weapon.attack:fire(
            dt,
            weapon,
            parent.transform.position,
            parent.transform.scale,
            self.weaponSlot.handPosition,
            self.weaponSlot.weaponScaling,
            parent.controller.target
        )
    end

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
                    soundManager:playSound("contents/sounds/game/heros_death.wav", 0.3, false)
                end
            end
        end
    end

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

    function fighter:changeWeapon(nb)
        self.weaponSlot.currentWeaponId = nb
    end

    function fighter:drawHittingPoints(parent)
        local font = self.pointsUI.font10
        if self.isHit then
            local points = self.maxPV / 10
            local pvColor = {1, 1, 1}

            if self.currentPV <= 0 then
                points = self.maxPV / 2
                pvColor = {1, 0.9, 0.1}
                font = self.pointsUI.font15
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

    function fighter:drawWeapon(state)
        if state ~= CHARACTERS.STATE.DEAD then
            if #self.weaponSlot.weapon ~= 0 then
                self.weaponSlot.weapon[self.weaponSlot.currentWeaponId]:draw()
            end
        end
    end

    function fighter:setMaxPV(pv)
        self.maxPV = pv
    end

    function fighter:setStrenght(strg)
        self.strenght = strg
    end
    return fighter
end

return c_Fighter

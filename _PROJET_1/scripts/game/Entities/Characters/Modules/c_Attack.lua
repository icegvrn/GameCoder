Bullets = require("scripts/game/Entities/Characters/Modules/c_Bullets")

local c_Attack = {}
local Attack_mt = {__index = c_Attack}

function c_Attack.new()
    local attack = {
        bullets = Bullets.new()
    }

    return setmetatable(attack, Attack_mt)
end

function c_Attack:create()
    local attack = {
        speed = 5,
        damage = 10,
        bullets = self.bullets:create(),
        hittableCharacters = {},
        isRangedWeapon = false,
        weaponRange = 10,
        isFiring = false,
        canFire = true,
        timer = 0,
        timerIsStarted = false
    }

    function attack:setDamageValue(dmg)
        self.damage = dmg
    end

    function attack:setRangedWeapon(bool)
        self.isRangedWeapon = bool
    end

    function attack:setWeaponRange(nb)
        self.weaponRange = nb
    end

    function attack:setSpeed(pSpeed)
        self.speed = pSpeed
        self.timer = pSpeed
    end

    function attack:update(dt, parent)
        self.bullets:update(dt, self, parent)
        self:updateHittableTargets(parent.owner)
    end

    function attack:draw(parent)
        self.bullets:draw(self, parent)
    end

    function attack:updateHittableTargets(owner)
        -- Défini qui est touchable par l'arme --
        if owner.controller.player then
            if #self.hittableCharacters ~= #levelManager.getListofEnnemiesCharacters() then
                self.hittableCharacters = levelManager.getListofEnnemiesCharacters()
            end
        else
            self.hittableCharacters[1] = owner.controller.target
        end
    end

    function attack:fire(dt, parent, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
        self.isFiring = true

        self:checkIfWeaponCanFire(dt)

        if self.canFire then
            -- Lecture entre deux sons, pour avoir des sons plus naturels
            local nb = love.math.random(1, #parent.sounds.tracks)
            parent.sounds:play(parent.sounds.tracks[nb], parent.sounds.talkVolume, false)

            if self.isRangedWeapon then
                self.bullets:fire(
                    dt,
                    parent,
                    ownerPosition,
                    ownerScale,
                    ownerHandPosition,
                    ownerWeaponScaling,
                    ownerTarget
                )
            end
            self.canFire = false
        end
        if self.isRangedWeapon == false then
            parent.animator:playattackAnimation(dt, parent)
            self.isFiring = false
        end
    end

    function attack:boostedAttack(dt, owner)
        local x, y = owner.transform:getPosition()
        for c = #self.hittableCharacters, 1, -1 do
            if self:isCollide(x, y, owner.sprites.height, owner.sprites.width, self.hittableCharacters[c]) then
                self.hittableCharacters[c].fight:hit(owner, self.damage)
            end
        end
    end

    function attack:isCollide(weaponX, weaponY, weaponWidth, weaponHeight, p_character)
        return p_character.collider:isCharacterCollidedBy(p_character, weaponX, weaponY, weaponWidth, weaponHeight)
    end

    function attack:checkIfWeaponCanFire(dt)
        if self.timerIsStarted == false then
            self.timerIsStarted = true
        end

        if self.timerIsStarted then
            self.timer = self.timer + dt
            if self.timer >= self.speed then
                self.timerIsStarted = false
                self.isFiring = false
                self.canFire = true
                self.timer = 0
            end
        end
    end

    return attack
end

return c_Attack

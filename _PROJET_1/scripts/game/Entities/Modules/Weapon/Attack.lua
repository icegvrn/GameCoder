-- MODULE QUI GERE L'ATTAQUE FAITE PAR QUELQU'UN EN TANT QUE TELLE : DAMAGE, SPEED, QUI EST TOUCHABLE OU PAS...
Bullets = require(PATHS.MODULES.BULLETS)

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

    -- Update de l'attack en appelant la mise à jour les bullets éventuelles et la liste des cibles touchables
    function attack:update(dt, parent)
        self.bullets:update(dt, self, parent)
        self:updateHittableTargets(parent.owner)
    end

    -- Appel le draw des éventuels bullets
    function attack:draw(parent)
        self.bullets:draw(self, parent)
    end

    -- Update la liste des cible que l'attaque peut toucher (nécessaire car liste update avec suppression lors de mort des ennemis)
    function attack:updateHittableTargets(owner)
        if owner.controller.player then
            if #self.hittableCharacters ~= #levelManager.ennemiManager:getEnnemiesCharacters() then
                self.hittableCharacters = levelManager.ennemiManager:getEnnemiesCharacters()
            end
        else
            self.hittableCharacters[1] = owner.controller.target
        end
    end

    -- Function d'attaque : si c'est une arme à distance et qu'elle peut tirer (vérifier avec checkIfWeaponCanFire),
    -- on "fire" une bullet à partir du module bullets, sinon c'est une arme au corps à corps : animation attaque et dégâts
    function attack:fire(
        dt,
        parent,
        owner,
        ownerPosition,
        ownerScale,
        ownerHandPosition,
        ownerWeaponScaling,
        ownerTarget)
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
            else
                ownerTarget.fight:hit(owner, (parent:getDamage() / 30))
            end
            self.canFire = false
        end
        if self.isRangedWeapon == false then
            parent.animator:playattackAnimation(dt, parent)
            self.isFiring = false
        end
    end

    -- Fonction qui permet de mettre des dégâts direct s'il y a collision entre un personnage armée d'un boost
    -- et un personnage hittable
    function attack:boostedAttack(dt, owner)
        local x, y = owner.transform:getPosition()
        for c = #self.hittableCharacters, 1, -1 do
            if self:isCollide(x, y, owner.sprites.height, owner.sprites.width, self.hittableCharacters[c]) then
                self.hittableCharacters[c].fight:hit(owner, self.damage)
            end
        end
    end

    -- Fonction qui vérifie pour le corps à corps s'il y a collision entre un personnage et une arme
    function attack:isCollide(weaponX, weaponY, weaponWidth, weaponHeight, p_character)
        return p_character.collider:isCharacterCollidedBy(p_character, weaponX, weaponY, weaponWidth, weaponHeight)
    end

    -- Fonction qui vérifie si l'arme peut attaquer à l'aide d'un timer
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

    -- SETTERS ---
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

    return attack
end

return c_Attack

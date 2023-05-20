require("scripts/states/CONST")
utils = require("scripts/Utils/utils")
local map = require("scripts/game/gameMap")
Transform = require("scripts/engine/transform")
Sound = require("scripts/game/Entities/Characters/Modules/c_Sound")
HeldSlot = require("scripts/game/Entities/Characters/Modules/c_HeldSlot")
Hitbox = require("scripts/game/Entities/Characters/Modules/c_Hitbox")
Attack = require("scripts/game/Entities/Characters/Modules/c_Attack")
Sprites = require("scripts/game/Entities/Characters/Modules/c_Sprites")
WeaponAnimator = require("scripts/game/Entities/Characters/Modules/c_WeaponAnimator")

local Weapon = {}
local weapons_mt = {__index = Weapon}

function Weapon.new()
    weapon = {
        transform = Transform.new(),
        sound = Sound.new(),
        hitBox = Hitbox.new(),
        heldSlot = HeldSlot.new(),
        attack = Attack.new(),
        sprites = Sprites.new(),
        weaponAnimator = WeaponAnimator.new()
    }
    return setmetatable(weapon, weapons_mt)
end

function Weapon:create()
    local weapon = {}
    weapon.transform = self.transform:create()
    weapon.sounds = self.sound:create()
    weapon.hitBox = self.hitBox:create()
    weapon.heldSlot = self.heldSlot:create()
    weapon.attack = self.attack:create()
    weapon.sprites = self.sprites:create()
    weapon.animator = self.weaponAnimator:create()
    weapon.name = "inconnu"
    weapon.owner = self
    weapon.lookAt = CONST.DIRECTION.left

    function weapon:init()
    end

    function weapon:update(dt)
        self.attack:update(dt, self)
        if self.owner:getMode() == CHARACTERS.MODE.BOOSTED then
            self.attack:boostedAttack(dt, self.owner)
        end
        self.animator:update(dt, self)
    end

    function weapon:draw()
        self:drawWeapon()
        self.attack:draw(self)
        -- self.hitBox:draw()
    end

    function weapon:drawWeapon()
        if (self.sprites.spritesList[1][self.sprites.currentSpriteId] ~= nil) then
            love.graphics.draw(
                self.sprites.spritestileSheets[1],
                self.sprites.spritesList[1][self.sprites.currentSpriteId],
                self.transform.position.x,
                self.transform.position.y,
                self.sprites.currentAngle + math.sin(self.sprites.rotationAngle) * 0.5,
                self.transform.scale.x,
                self.transform.scale.y,
                (self.sprites.spritestileSheets[1]:getWidth() / 2) + self.heldSlot.holdingOffset.x,
                (self.sprites.spritestileSheets[1]:getHeight() / 2) + self.heldSlot.holdingOffset.y
            )
        end
    end

    function weapon:move(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
        self.animator:updateInformations(
            dt,
            self,
            ownerPosition,
            ownerScale,
            ownerHandPosition,
            ownerWeaponScaling,
            ownerTarget
        )
    end

    -- GETTERS/SETTERS------------------------------
    function weapon:getName()
        return self.name
    end

    function weapon:getDamage()
        return self.attack.damage
    end

    function weapon:getIsRangedWeapon()
        return self.attack.isRangedWeapon
    end

    function weapon:getWeaponRange()
        return self.attack.weaponRange
    end

    function weapon:setName(name)
        self.name = name
    end

    function weapon:setOwner(owner)
        self.owner = owner
    end

    function weapon:setState(state)
        self.state = state
    end

    function weapon:changeDirection(direction)
        self.lookAt = direction
    end

    function weapon:getDirection()
        return self.lookAt
    end

    return weapon
end

return Weapon

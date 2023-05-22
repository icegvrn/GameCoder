
-- ENTITE ARME, COMPREND LES COMPOSANTS QUI FONT UNE ARME : SPRITES, ATTACK... 
Transform = require(PATHS.TRANSFORM)
Sound = require(PATHS.SOUND)
HeldSlot = require(PATHS.MODULES.HELDSLOT)
Hitbox = require(PATHS.MODULES.HITBOX)
Attack = require(PATHS.MODULES.ATTACK)
Sprites = require(PATHS.SPRITES)
WeaponAnimator = require(PATHS.MODULES.WEAPONANIMATOR)

local Weapon = {}
local weapons_mt = {__index = Weapon}

-- Création des instances des composants de l'arme
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

-- Création d'une nouvelle arme locale
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

    -- Mise à jour du composant "attack" et de l'animator en permanence. L'attaque est en mode "boosted"
    -- si le héros est en mode boosted.
    function weapon:update(dt)
        self.attack:update(dt, self)
        if self.owner:getMode() == CHARACTERS.MODE.BOOSTED then
            self.attack:boostedAttack(dt, self.owner)
        end
        self.animator:update(dt, self)
    end

    -- Appel le draw de l'arme et le draw de l'attack (qui draw les bullets éventuelles)(debug possible avec le draw de la hitbox, désactivé par défaut)
    function weapon:draw()
        self:drawWeapon()
        self.attack:draw(self)
        -- self.hitBox:draw()
    end

    -- Draw du sprite de l'arme a proprement parlé en fonction de l'arme actuelle 
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

    -- Function qui envoie les informations de mouvement à l'animator de l'arme pour qu'il puisse la déplacer
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

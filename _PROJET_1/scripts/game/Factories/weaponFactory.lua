-- FACTORY PERMETTANT DE GENERER UNE ARME A LA DEMANDE
WEAPONS_CONST = require(PATHS.CONFIGS.WEAPONS)
Weapon = require(PATHS.WEAPON)

weaponFactory = {}
local weapon = Weapon.new()

function weaponFactory.createWeapon(p_type)
    local weapon = weapon:create()
    weaponFactory.setCharacteristics(weapon, p_type)
    weaponFactory.createSprites(weapon, p_type)
    return weapon
end

-- Va chercher les caractéristiques de l'arme dans un fichier portant le nom de son type ("sword", "magic_staff"...)
function weaponFactory.setCharacteristics(w, p_type)
    local weaponData = require(PATHS.CONFIGS.WEAPONSFOLDER .. p_type)
    w:setName(weaponData.name)
    w.attack:setDamageValue(weaponData.damageValue)
    w.attack:setSpeed(weaponData.speed)
    w.attack:setRangedWeapon(weaponData.isRangedWeapon)
    w.attack:setWeaponRange(weaponData.range)
    w.heldSlot:setHoldingOffset(weaponData.holdingOffset)
    w.sounds:setSounds(weaponData.sounds)
    w.sounds:setTalkingVolume(weaponData.soundsVolume)
end

-- Va chercher le sprite de l'arme dans le dossier approprié
function weaponFactory.createSprites(p_weapon, type)
    local spriteList = {}
    local imagePath = PATHS.IMG.WEAPONS .. type .. ".png"
    if love.filesystem.getInfo(imagePath) == false then
        print("WARNING WEAPONS FACTORY : " .. type .. " needs images.")
    end
    spriteList[1] = love.graphics.newImage(imagePath)
    p_weapon.sprites:setSpritesList(spriteList, 1, 1)
end

return weaponFactory

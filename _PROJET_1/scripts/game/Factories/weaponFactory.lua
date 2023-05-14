-- Créé une arme en fonction de son type
WEAPONS_CONST = require("scripts/states/WEAPONS")
PATHS = require("scripts/states/PATHS")
Weapon = require("scripts/engine/weapon")

weaponFactory = {}

function weaponFactory.createWeapon(p_type)
    local weapon = weaponFactory.createNewWeapon()
    weaponFactory.setCharacteristics(weapon, p_type)
    weaponFactory.createSprites(weapon, p_type)
    return weapon
end

function weaponFactory.createNewWeapon()
    return Weapon.new()
end

-- Va chercher les caractéristiques de l'arme dans un fichier portant le nom de son type ("sword", "magic_staff"...)
function weaponFactory.setCharacteristics(w, p_type)
    local weaponData = require(PATHS.ENTITIES.WEAPONS .. p_type)
    w:setName(weaponData.name)
    w:setDamageValue(weaponData.damageValue)
    w:setSpeed(weaponData.speed)
    w:setRangedWeapon(weaponData.isRangedWeapon)
    w:setHoldingOffset(weaponData.holdingOffset)
    w:setSounds(weaponData.sounds)
    w:setSoundsVolume(weaponData.soundsVolume)
end

-- Va chercher le sprite de l'arme dans le dossier approprié
function weaponFactory.createSprites(p_weapon, type)
    local spriteList = {}
    local imagePath = PATHS.IMG.WEAPONS .. type .. ".png"
    if love.filesystem.getInfo(imagePath) == false then
        print("WARNING WEAPONS FACTORY : " .. type .. " needs images.")
    end
    spriteList[1] = love.graphics.newImage(imagePath)
    p_weapon:setSprites(spriteList)
end

return weaponFactory

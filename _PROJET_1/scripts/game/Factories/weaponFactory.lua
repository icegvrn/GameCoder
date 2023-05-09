-- Créé un personnage en fonction de sa catégorie et de son type
require("scripts/states/WEAPONS")
Weapon = require("scripts/engine/weapon")

weaponFactory = {}

function weaponFactory.createWeapon(type)
    local weapon = Weapon.new()

    weaponFactory.createSprites(weapon, type)

    weapon:setState(WEAPONS.STATE.IDLE)
    weapon:setName(tostring(type))
    if type == WEAPONS.TYPE.NONE then
        weapon:setDamageValue(0)
        weapon:setSpeed(0)
        weapon:setRangedWeapon(false)
elseif type == WEAPONS.TYPE.HERO_MAGIC_STAFF then
        weapon:setDamageValue(20)
        weapon:setSpeed(0.2)
        weapon:setRangedWeapon(true)
        weapon:setHoldingOffset(-10, 0)
    elseif type == WEAPONS.TYPE.BITE then
        weapon:setDamageValue(1000)
        weapon:setRangedWeapon(false)
        weapon:setHoldingOffset(0, 0)
    elseif type == WEAPONS.TYPE.MAGIC_STAFF then
        weapon:setDamageValue(10)
        weapon:setRangedWeapon(true)
        weapon:setHoldingOffset(-5, 0)
        weapon:setWeaponRange(15)
    elseif type == WEAPONS.TYPE.SWORD then
        weapon:setHoldingOffset(-20, 0)
        weapon:setDamageValue(15)
        weapon:setRangedWeapon(false)
    elseif type == WEAPONS.TYPE.FLOWER then
        weapon:setDamageValue(5)
        weapon:setRangedWeapon(false)
        weapon:setHoldingOffset(-15, 0)
    elseif type == WEAPONS.TYPE.AXE then
        weapon:setDamageValue(10)
        weapon:setRangedWeapon(false)
        weapon:setHoldingOffset(-10, 0)
    elseif type == WEAPONS.TYPE.DOUBLE_AXE then
        weapon:setDamageValue(20)
        weapon:setRangedWeapon(false)
        weapon:setHoldingOffset(-10, 0)
    end

    return weapon
end

function weaponFactory.createSprites(p_weapon, type)
    local spriteList = {}
    local imagePath = WEAPONS.IMGPATH .. "/" .. type .. ".png"
    if love.filesystem.getInfo(imagePath) == false then
        print("WARNING WEAPONS FACTORY : " .. type .. " needs images.")
    end
    spriteList[1] = love.graphics.newImage(imagePath)
    p_weapon:setSprites(spriteList)
end

return weaponFactory

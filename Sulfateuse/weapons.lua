local fire = require("fire")

weapons = {}
weapons.GUN = 0
weapons.AUTO = 1
weapons.RAFALE = 2

function weapons.Init(self)
    weapons.createNewWeapon(weapons.GUN, love.graphics.newImage("images/gun.png"), 160, 10, 0)
    weapons.createNewWeapon(weapons.AUTO, love.graphics.newImage("images/auto.png"), 230, 38, 0)
    weapons.createNewWeapon(weapons.RAFALE, love.graphics.newImage("images/rafale.png"), 105, 23, 0)
end

function weapons.createNewWeapon(pid, pimage, pfireOriginX, pfireoriginY, pfireTimer, pfireFunction)
    local weapon = {}
    weapon.id = pid
    weapon.image = pimage
    weapon.fireOriginX = pfireOriginX
    weapon.fireOriginY = pfireoriginY
    weapon.fireTimer = pfireTimer
    weapon.fire = function()
        fire:fireFromOrigin(weapon.fireOriginX, weapon.fireOriginY)
    end
    weapon.getID = function()
        return weapon.id
    end

    weapon.getImage = function()
        return weapon.image
    end

    weapon.getFireTimer = function()
        return weapon.fireTimer
    end

    weapon.setFireTimer = function(timer)
        weapon.fireTimer = timer
    end

    table.insert(weapons, weapon)
end

-- Cacul l'origine du tire selon la position de l'image dans le gameManager
function weapons.setWeaponsOrigin(ox, oy)
    for k, v in ipairs(weapons) do
        v.fireOriginX = v.fireOriginX + ox
        v.fireOriginY = v.fireOriginY + oy
    end
end
return weapons

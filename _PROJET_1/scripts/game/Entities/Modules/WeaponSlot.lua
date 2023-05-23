-- MODULE QUI PERMET DE DONNER A UN CHARACTER UN "SLOT", CAD UNE POSITION, A PARTIR DE LAQUELLE TENIR L'ARME
local c_WeaponSlot = {}
local WeaponSlot_mt = {__index = c_WeaponSlot}

function c_WeaponSlot.new()
    local w_slot = {}
    return setmetatable(w_slot, WeaponSlot_mt)
end

function c_WeaponSlot:create()
    local weaponSlot = {
        weapon = {},
        currentWeaponId = 1,
        handPosition = {x = 0, y = 0},
        handOffset = {x = 0, y = 0},
        weaponScaling = 1
    }

    -- Fonction qui va appeler le move de l'arme en appliquant une transformation donnée par le personnage
    -- Appelle l'update de Weapon
    function weaponSlot:moveWeapon(dt, parent, target)
        self.handPosition.x = parent.transform.position.x + (self.handOffset.x * parent.transform.scale.x)
        self.handPosition.y = parent.transform.position.y + self.handOffset.y
        if #self.weapon ~= 0 then
            self.weapon[self.currentWeaponId]:move(
                dt,
                parent.transform.position,
                parent.transform.scale,
                self.handPosition,
                self.weaponScaling,
                target
            )
            self.weapon[self.currentWeaponId]:update(dt)
        end
    end

    -- FOnction qui appelle le clear des bullets, utilisée quand on change d'arme notamment
    function weaponSlot:clearFiringElements()
        self.weapon[self.currentWeaponId].attack.bullets:clear()
    end

    -- Setters
    function weaponSlot:setHandOffset(array)
        self.handOffset.x = array[1]
        self.handOffset.y = array[2]
    end

    function weaponSlot:setWeaponScaling(sc)
        self.weaponScaling = sc
    end

    -- Getters
    function weaponSlot:getWeaponRange()
        if #self.weapon ~= 0 then
            return self.weapon[self.currentWeaponId]:getWeaponRange()
        end
    end
    return weaponSlot
end

return c_WeaponSlot

local c_WeaponSlot = {}
local WeaponSlot_mt = {__index = c_WeaponSlot}

function c_WeaponSlot.new()
    local weaponSlot = {
       handPosition = {x = 0, y = 0},
       handOffset = {x = 20, y = 25}
    }
    return setmetatable(weaponSlot, WeaponSlot_mt)
end

return c_WeaponSlot

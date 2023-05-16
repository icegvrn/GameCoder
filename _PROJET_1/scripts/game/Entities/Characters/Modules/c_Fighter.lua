local c_Fighter = {}
local Fighter_mt = {__index = c_Fighter}

function c_Fighter.new()
    local fighter = {
        maxPV = 100,
        currentPV = 100,
        strenght = 10,
        weapon = {},
        currentWeaponId = 1,
        canBeHurt = true,
        canFire = true,
        isHit = false,
        timer = 0,
        dieTimer = 0
    }
    return setmetatable(fighter, Fighter_mt)
end

return c_Fighter

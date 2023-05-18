local c_AutoAttack = {}
local AutoAttack_mt = {__index = c_AutoAttack}

function c_AutoAttack.new()
    local autoAttack = {}
    return setmetatable(autoAttack, AutoAttack_mt)
end

function c_AutoAttack:create()
    local autoAttack = {
        speed = 5,
        damage = 10,
        isRangedWeapon = false,
        weaponRange = 10,
        isFiring = false,
        canFire = true,
        FireList = {},
        lifeFactor = 0.7,
        timer = 0,
        timerIsStarted = false
    }

    return autoAttack
end

return c_AutoAttack

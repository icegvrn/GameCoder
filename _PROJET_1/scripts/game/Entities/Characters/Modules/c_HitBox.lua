local c_Hitbox = {}
local Hitbox_mt = {__index = c_Hitbox}

function c_Hitbox.new()
    local hitbox = {}
    return setmetatable(hitbox, Hitbox_mt)
end

function c_Hitbox:create()
    local hitBox = {
        offset = {x = 0, y = 0},
        position = {x = 0, y = 0},
        size = {x = 10, y = 10}
    }

    return hitBox
end

return c_Hitbox

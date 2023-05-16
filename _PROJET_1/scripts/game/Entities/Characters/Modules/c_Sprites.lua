local c_Sprites = {}
local Sprites_mt = {__index = c_Sprites}

function c_Sprites.new()
    local sprites = {
        height = 0,
        width = 0,
        initialScale = 1,
        color = {1, 1, 1, 1},
        currentSpriteId = 1,
        spritestileSheets = {},
        spritesList = {}
    }
    return setmetatable(sprites, Sprites_mt)
end

return c_Sprites

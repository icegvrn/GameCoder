-- MODULE QUI PERMET DE DETERMINER A TOUT MOMENT LE "POINT CHAUD" D'UNE ARME, L'ENDROIT D'OU ELLE PEUT TIRER

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

    function hitBox:setHitBoxSize(sizeX, sizeY)
        self.size.x = sizeX
        self.size.y = sizeY
    end

    function hitBox:setFireOffset(x, y)
        self.offset.x = x
        self.offset.y = y
    end

    function hitBox:getFireOffset()
        return self.position.x, self.position.y
    end

    function hitBox:draw()
        -- DEBUG : Dessin de l'image de tir
        love.graphics.circle("fill", self.position.x, self.position.y, 6, 6)
    end

    return hitBox
end

return c_Hitbox

local c_Sprites = {}
local Sprites_mt = {__index = c_Sprites}

function c_Sprites.new()
    local sprites = {}
    return setmetatable(sprites, Sprites_mt)
end

function c_Sprites:create()
    local c_sprite = {
        height = 0,
        width = 0,
        initialScale = 1,
        color = {1, 1, 1, 1},
        currentSpriteId = 1,
        spritestileSheets = {},
        spritesList = {}
    }
    function c_sprite:drawSprite(parent, direction, mode, state)
        self:updateSpriteDirection(parent, direction)

        local c_state = mode .. "_" .. state
        local c_spriteID = math.floor(self.currentSpriteId)
        local px, py = parent.transform.position.x, parent.transform.position.y

        love.graphics.setColor(self.color)

        if self.spritesList[c_state][c_spriteID] then
            love.graphics.draw(
                self.spritestileSheets[c_state],
                self.spritesList[c_state][c_spriteID],
                parent.transform.position.x,
                parent.transform.position.y,
                parent.transform.rotation.y,
                parent.transform.scale.x,
                parent.transform.scale.y,
                self.spritestileSheets[c_state]:getHeight() / 2,
                self.spritestileSheets[c_state]:getHeight() / 2
            )
        end
    end

    function c_sprite:updateSpriteDirection(parent, direction)
        if direction == CONST.DIRECTION.left then
            parent.transform.scale.x = -self.initialScale
        elseif direction == CONST.DIRECTION.right then
            parent.transform.scale.x = self.initialScale
        end
    end

    function c_sprite:changeSpriteColorTo(color)
        self.color = color
    end

    function c_sprite:animate(dt, mode, state)
        local c_state = mode .. "_" .. state
        self.currentSpriteId = self.currentSpriteId + 5 * dt
        if self.currentSpriteId >= #self.spritesList[c_state] + 0.99 then
            self.currentSpriteId = 1
        end
    end

    function c_sprite:getDimension(mode, state)
        local c_state = mode .. "_" .. state
        local w = self.spritestileSheets[c_state]:getWidth() / 4
        local h = self.spritestileSheets[c_state]:getHeight()
        return w, h
    end

    function c_sprite:setSprites(p_table)
        local sprites = {}
        for k, sprite in pairs(p_table) do
            self.spritestileSheets[k] = sprite
            self.height = sprite:getHeight()
            self.width = sprite:getWidth() / 4
            local nbColumns = 4
            local nbLine = sprite:getHeight() / self.height
            local id = 1

            local spriteTable = {}
            for c = 1, nbColumns do
                for l = 1, nbLine do
                    spriteTable[id] =
                        love.graphics.newQuad(
                        (c - 1) * sprite:getWidth() / 4,
                        (l - 1) * sprite:getHeight(),
                        sprite:getWidth() / 4,
                        sprite:getHeight(),
                        sprite:getWidth(),
                        sprite:getHeight()
                    )
                    id = id + 1
                end
            end
            sprites[k] = spriteTable
        end

        self.spritesList = sprites
    end

    return c_sprite
end

return c_Sprites

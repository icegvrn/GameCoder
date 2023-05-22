-- MODULE QUI PERMET LA CREATION/GESTION DE SPRITES EN GENERANT DES QUADS
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
        spritesList = {},
        currentAngle = 0,
        rotationAngle = 0
    }

    function c_sprite:drawSprite(parent, direction, index, index2)
        if index2 then
            index = index .. "_" .. index2
        end

        self:updateSpriteDirection(parent, direction)
        local c_spriteID = math.floor(self.currentSpriteId)

        love.graphics.setColor(self.color)

        if self.spritesList[index][c_spriteID] then
            love.graphics.draw(
                self.spritestileSheets[index],
                self.spritesList[index][c_spriteID],
                parent.transform.position.x,
                parent.transform.position.y,
                parent.transform.rotation.y,
                parent.transform.scale.x,
                parent.transform.scale.y,
                self.spritestileSheets[index]:getHeight() / 2,
                self.spritestileSheets[index]:getHeight() / 2
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

    function c_sprite:animate(dt, index, index2)
        if index2 then
            index = index .. "_" .. index2
        end
        self.currentSpriteId = self.currentSpriteId + 5 * dt
        if self.currentSpriteId >= #self.spritesList[index] + 0.99 then
            self.currentSpriteId = 1
        end
    end

    function c_sprite:getDimension(index, index2)
        if index2 then
            index = index .. "_" .. index2
        end
        local w = self.spritestileSheets[index]:getWidth() / 4
        local h = self.spritestileSheets[index]:getHeight()
        return w, h
    end

    function c_sprite:setSpritesList(p_table, p_widthNb, p_heightNb)
        local sprites = {}
        for k, sprite in pairs(p_table) do
            self.spritestileSheets[k] = sprite
            self.height = sprite:getHeight() / p_heightNb
            self.width = sprite:getWidth() / p_widthNb
            local nbColumns = p_widthNb
            local nbLine = p_heightNb
            local id = 1

            local spriteTable = {}
            for c = 1, nbColumns do
                for l = 1, nbLine do
                    spriteTable[id] =
                        love.graphics.newQuad(
                        (c - 1) * sprite:getWidth() / p_widthNb,
                        (l - 1) * sprite:getHeight() / p_heightNb,
                        sprite:getWidth() / p_widthNb,
                        sprite:getHeight() / p_heightNb,
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

    function c_sprite:setCurrentSpriteId(nb)
        self.currentSpriteId = nb
    end

    function c_sprite:setAngle(angle)
        self.currentAngle = angle
    end

    return c_sprite
end

return c_Sprites

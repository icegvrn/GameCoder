local map = require("scripts/game/gameMap")

local c_Collider = {}
local Collider_mt = {__index = c_Collider}

function c_Collider.new()
    local collider = {}
    return setmetatable(collider, Collider_mt)
end

function c_Collider:create()
    local collider = {}

    function collider:isCharacterCollidedBy(parent, weaponX, weaponY, weaponWidth, weaponHeight)
        local weaponTopRight = weaponX + weaponWidth
        local weaponBottomLeft = weaponY + weaponHeight

        local c_state = parent.mode .. "_" .. parent.state

        local spriteWidth = parent.sprites.spritestileSheets[c_state]:getWidth() / #parent.sprites.spritesList[c_state]
        local spriteHeight = parent.sprites.spritestileSheets[c_state]:getHeight()

        if
            weaponX > parent.transform.position.x + spriteWidth / 2 or weaponTopRight < parent.transform.position.x or
                weaponY > parent.transform.position.y + spriteHeight / 2 or
                weaponBottomLeft < parent.transform.position.y
         then
            return false
        else
            return true
        end
        return false
    end

    function collider:checkCollisions(character)
        if character.controller.player then
            self:checkPlayerCollisions(character)
        end
    end

    function collider:update(dt, character)
        self:checkCollisions(character)
    end

    function collider:checkPlayerCollisions(character)
        local gmap = map.getCurrentMap()
        local x, y = character.transform:getPosition()
        local w, h = character.sprites:getDimension(character.mode, character.state)
        if (map.isThereASolidElement(x, y, w, h, character)) then
            character.controller.canMove = false
        else
            character.controller.canMove = true
        end
    end

    return collider
end

return c_Collider

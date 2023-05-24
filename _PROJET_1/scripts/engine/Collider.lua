-- MODULE QUI PERMET DE CREER UN COLLIDER DE CHARACTER QUI VA VERIFIER SI COLLISION AVEC ARME OU CARTE
local mapManager = require(PATHS.MAPMANAGER)

local c_Collider = {}
local Collider_mt = {__index = c_Collider}

function c_Collider.new()
    local collider = {}
    return setmetatable(collider, Collider_mt)
end

function c_Collider:create()
    local collider = {
        nextMoveX = 0,
        nextMoveY = 0,
        lastMoveKnowX = 0,
        lastMoveKnowY = 0
    }

    -- Fonction raccourcie pour les collisions avec les personnages
    function collider:isCharacterCollidedBy(character, elemX, elemY, elemWidth, elemHeight)
        local weaponTopRight = elemX + elemWidth
        local weaponBottomLeft = elemY + elemHeight
        local c_state = character.mode .. "_" .. character.state
        local spriteWidth =
            character.sprites.spritestileSheets[c_state]:getWidth() / #character.sprites.spritesList[c_state]
        local spriteHeight = character.sprites.spritestileSheets[c_state]:getHeight()

        return Utils.isCollision(
            elemX,
            elemY,
            elemWidth,
            elemHeight,
            character.transform.position.x,
            character.transform.position.y,
            spriteWidth,
            spriteHeight
        )
    end

    -- Vérifie s'il y a collision avec la carte
    function collider:checkCollisions(character)
        self:checkCollisionWithMap(character)
    end

    -- Fonction qui vérifie si y'a collision entre un personnage et la carte
    function collider:checkCollisionWithMap(character)
        if self.nextMoveX ~= self.lastMoveKnowX or self.nextMoveY ~= self.lastMoveKnowY then
            local x, y = character.transform:getPosition()
            local w, h = character.sprites:getDimension(character.mode, character.state)
            if (mapManager:isThereASolidElement(self.nextMoveX, self.nextMoveY, w, h, character)) then
                character.controller.canMove = false
            else
                character.controller.canMove = true
            end
            lastMoveKnowX = self.nextMoveX
            lastMoveKnowY = self.nextMoveY
        end
    end

    function collider:setNextMove(character, x, y)
        self.nextMoveX = x
        self.nextMoveY = y
        self:checkCollisionWithMap(character)
    end

    return collider
end

return c_Collider

-- MODULE QUI PERMET DE CREER UN COLLIDER DE CHARACTER QUI VA VERIFIER SI COLLISION AVEC ARME OU CARTE
local mapManager = require(PATHS.MAPMANAGER)

local c_Collider = {}
local Collider_mt = {__index = c_Collider}

function c_Collider.new()
    local collider = {}
    return setmetatable(collider, Collider_mt)
end

function c_Collider:create()
    local collider = {}

    -- Vérifie sur le personnage collide avec un élément (ici weapon)
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

    -- Vérifie les collision en permanence pour le joueur et la carte en utilisant une fonction spécifique
    function collider:update(dt, character)
        self:checkCollisions(character)
    end

    -- Vérifie s'il y a collision avec la carte si c'est un joueur
    function collider:checkCollisions(character)
        if character.controller.player then
            self:checkPlayerCollisions(character)
        end
    end

    -- Fonction qui vérifie si y'a collision entre le joueur et la carte
    function collider:checkPlayerCollisions(character)
        local x, y = character.transform:getPosition()
        local w, h = character.sprites:getDimension(character.mode, character.state)
        if (mapManager:isThereASolidElement(x, y, w, h, character)) then
            character.controller.canMove = false
        else
            character.controller.canMove = true
        end
    end

    return collider
end

return c_Collider

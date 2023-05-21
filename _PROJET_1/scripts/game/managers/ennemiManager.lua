local mapManager = require(PATHS.MAPMANAGER)
local c_factory = require(PATHS.CHARACTERFACTORY)
local w_factory = require(PATHS.WEAPONFACTORY)
local levelsConfig = require(PATHS.LEVELCONFIGURATION)

local EnnemiManager = {}
local Ennemi_mt = {__index = EnnemiManager}

function EnnemiManager.new()
    local ennemiManager = {}
    return setmetatable(ennemiManager, Ennemi_mt)
end

function EnnemiManager:create()
    local ennemiManager = {ennemiesList = {}}

    function ennemiManager:update(dt)
        --  self:checkCharactersCollisions(dt)
        for n = #self.ennemiesList, 1, -1 do
            self.ennemiesList[n]:update(dt)
        end
    end

    -- function ennemiManager:checkCharactersCollisions(dt)
    --     for n = #self.ennemiesList, 1, -1 do
    --         if self.ennemiesList[n].character.collider then
    --             self.ennemiesList[n].character.collider:update(dt, self.ennemiesList[n].character)
    --         end
    --     end
    -- end

    function ennemiManager:spawnEnnemies(levelManager, player)
        local currentLvlList = levelsConfig.getEnnemiesByLvl(levelManager.currentLevel)
        if currentLvlList then
            for n = 1, #currentLvlList do
                local type = currentLvlList[n][1]
                local weapon = currentLvlList[n][2]
                local number = currentLvlList[n][3]
                for i = 1, number do
                    local ennemi = self:addNewEnnemi(type, weapon, number, player)
                    self:spawnToAvailableLocation(ennemi)
                    table.insert(self.ennemiesList, ennemi)
                    table.insert(levelManager.charactersList, ennemi)
                end
            end
        end
    end

    function ennemiManager:addNewEnnemi(type, weapon, number, player)
        local ennemi = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, type, false, player.character)
        local ennemiWeapon = w_factory.createWeapon(weapon)
        ennemi.character:equip(ennemiWeapon)
        return ennemi
    end

    function ennemiManager:spawnToAvailableLocation(ennemi)
        local mapWidth, mapHeight = mapManager:getMapDimension()
        c_w, c_h = ennemi.character.sprites:getDimension(ennemi.character.mode, ennemi.character.state)
        positionX, positionY = self:findSpawnPoint(mapManager, mapWidth, mapHeight, c_w, c_h)
        ennemi.character.transform:setPosition(positionX, positionY)
    end

    function ennemiManager:findSpawnPoint(mapManager, mapWidth, mapHeight, c_w, c_h)
        local pX = love.math.random(0, mapWidth)
        local pY = love.math.random(0, mapHeight)
        local spawnFounded = false
        while spawnFounded == false do
            pX = love.math.random(0, mapWidth)
            pY = love.math.random(0, mapHeight)

            if
                (mapManager:isThereASolidElement(pX, pY, c_w, c_h)) == false and
                    (mapManager:isThereAFloor(pX, pY, c_w, c_h))
             then
                spawnFounded = true
                return pX, pY
            end
        end
    end

    function ennemiManager:nextLevel(levelManager, player)
        self:clear()
        self:spawnEnnemies(levelManager, player)
    end

    function ennemiManager:clear()
        self.ennemiesList = {}
    end

    function ennemiManager:getEnnemiesList()
        return self.ennemiesList
    end

    function ennemiManager:getEnnemiesCharacters()
        local charactersList = {}
        for n = 1, #self:getEnnemiesList() do
            charactersList[n] = self:getEnnemiesList()[n].character
        end
        return charactersList
    end

    return ennemiManager
end

return EnnemiManager

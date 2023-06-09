-- SPAWNER D'ENNEMIS PILOTE PAR LE LEVELMANAGER

-- Chargement des modules
local mapManager = require(PATHS.MAPMANAGER)
local c_factory = require(PATHS.CHARACTERFACTORY)
local w_factory = require(PATHS.WEAPONFACTORY)
local levelsConfig = require(PATHS.LEVELCONFIGURATION)

local EnnemiManager = {}
local Ennemi_mt = {__index = EnnemiManager}

-- Création de l'instance
function EnnemiManager.new()
    local ennemiManager = {}
    return setmetatable(ennemiManager, Ennemi_mt)
end

-- Création d'un ennemiManager local qui contient une liste d'ennemis
function EnnemiManager:create()
    local ennemiManager = {ennemiesList = {}}

    function ennemiManager:update(dt)
        for n = #self.ennemiesList, 1, -1 do
            self.ennemiesList[n]:update(dt)
        end
    end

    function ennemiManager:spawnEnnemies(levelManager, player)
        self:clear()
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

    -- Fonction qui appelle la character factory pour créer un ennemi
    function ennemiManager:addNewEnnemi(type, weapon, number, player)
        local ennemi = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, type, false, player.character)
        local ennemiWeapon = w_factory.createWeapon(weapon)
        ennemi.character:equip(ennemiWeapon)
        return ennemi
    end

    -- Fonction pour faire spawn les ennemis : on choppe leur taille actuelle pour vérifier la collision
    -- On choppe la taille de la carte pour tenter un spawn dans ce cadre-là et quand c'est bon on met la position
    function ennemiManager:spawnToAvailableLocation(ennemi)
        local mapWidth, mapHeight = mapManager:getMapDimension()
        c_w, c_h = ennemi.character.sprites:getDimension(ennemi.character.mode, ennemi.character.state)
        positionX, positionY = self:findSpawnPoint(mapManager, mapWidth, mapHeight, c_w, c_h)
        ennemi.character.transform:setPosition(positionX, positionY)
    end

    -- Fonction permettant de trouver un point de spawn valide
    function ennemiManager:findSpawnPoint(mapManager, mapWidth, mapHeight, c_w, c_h)
        local spawnFounded = false

        -- Une boucle qui cherche un point de spawn sur la taille de la carte où il n'y a ni collision,
        -- ni absence de sol elle n'a de return true que si elle en trouve un
        while spawnFounded == false do
            pX = love.math.random(200, mapWidth)
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

    -- Action à effectuer au niveau des ennemis lorsqu'on change de level : on nettoie l'ancienne liste
    -- au cas où et on spawn des nouveaux
    function ennemiManager:nextLevel(levelManager, player)
        self:clear()
        self:spawnEnnemies(levelManager, player)
    end

    -- Retourne la liste d'ennemis sous type "ennemi"
    function ennemiManager:getEnnemiesList()
        return self.ennemiesList
    end

    -- Retourne une liste des Characters des ennemis
    function ennemiManager:getEnnemiesCharacters()
        local charactersList = {}
        for n = 1, #self:getEnnemiesList() do
            charactersList[n] = self:getEnnemiesList()[n].character
        end
        return charactersList
    end

    -- Reset la liste d'ennemis
    function ennemiManager:clear()
        self.ennemiesList = {}
    end

    return ennemiManager
end

return EnnemiManager

-- LE LEVEL MANAGER GERE TOUT CE QUI EST PROPRE AU NIVEAU : LE NOMBRE D'ENNEMI, LEUR TYPE, LEURS ARMES, LA MAP A AFFICHER

-- Chargement des modules
local levelsConfig = require(PATHS.LEVELCONFIGURATION)
local soundManager = require(PATHS.SOUNDMANAGER)
local mapManager = require(PATHS.MAPMANAGER)
local CinematicManager = require(PATHS.CINEMATICMANAGER)
local PlayerManager = require(PATHS.PLAYERMANAGER)
local EnnemiManager = require(PATHS.ENNEMIMANAGER)

require(PATHS.CONFIGS.CHARACTERS)
require(PATHS.CONFIGS.WEAPONS)
local ui = require(PATHS.UIGAME)

local m_LevelManager = {}
local LevelManager_mt = {__index = m_LevelManager}

function m_LevelManager.new()
    local levelManager = {
        cinematicManager = CinematicManager.new(),
        playerManager = PlayerManager.new(),
        ennemiManager = EnnemiManager.new()
    }
    return setmetatable(levelManager, LevelManager_mt)
end

function m_LevelManager:create()
    local levelManager = {
        playerManager = self.playerManager:create(),
        ennemiManager = self.ennemiManager:create(),
        cinematicManager = self.cinematicManager:create(),
        charactersList = {},
        exitDoor = nil,
        LEVELSTATE = {start = "start", game = "game", win = "win", currentState = "start"},
        currentLevel = 1,
        isTheGameFinish = false,
        player = nil
    }

    function levelManager:load(nb)
        self:createMap(nb)
        self:spawnCharacters()
        self.player = self:getPlayer()
    end

    function levelManager:update(dt)
        self:updateMap(dt)

        if self.player.isDead then
            GAMESTATE.currentState = GAMESTATE.STATE.GAMEOVER
        elseif self.LEVELSTATE.currentState == self.LEVELSTATE.start then
            self.cinematicManager:playLevelCinematic(dt, self)
        elseif self.LEVELSTATE.currentState == self.LEVELSTATE.game then
            if #self.ennemiManager:getEnnemiesList() <= 0 then
                levelManager:endTheLevel()
                self.LEVELSTATE.currentState = self.LEVELSTATE.win
            else
                self:updateCharacters(dt)
            end
        elseif self.LEVELSTATE.currentState == self.LEVELSTATE.win then
            if mapManager:afterlevelWinAction(self.player) then
                levelManager:nextLevel()
            end
        end
        
        self.player:update(dt)
    end

    function levelManager:draw()
        mapManager:draw()
        for n = #levelManager.ennemiManager:getEnnemiesList(), 1, -1 do
            levelManager.ennemiManager:getEnnemiesList()[n]:draw()
        end
        levelManager.playerManager:getPlayer():draw()
    end

    function levelManager:createMap(nb)
        self.currentLevel = nb
        mapManager:initMap(self.currentLevel)
        self.exitDoor = mapManager:getDoor()
    end

    function levelManager:spawnCharacters()
        self.playerManager:spawnPlayer(self)
        self.ennemiManager:spawnEnnemies(self, self.playerManager:getPlayer())
    end

    function levelManager:updateMap(dt)
        mapManager:update(dt)
    end

    function levelManager:updateCharacters(dt)
        self.playerManager:update(dt)
        self.ennemiManager:update(dt)
    end

    function levelManager:endTheLevel()
        mapManager:endTheLevel()
        ui:endTheLevel()
    end

    function levelManager.setCurrentLevel(nb)
        levelManager.currentLevel = nb
    end

    function levelManager.getCurrentLevel()
        return levelManager.currentLevel
    end

    function levelManager.keypressed(key)
        if key == "m" then
            levelManager:nextLevel()
        end
        for n = 1, #levelManager.charactersList do
            levelManager.charactersList[n]:keypressed(key)
        end
    end

    function levelManager.destroyCharacter(character, weapon)
        for n = #levelManager.charactersList, 1, -1 do
            if levelManager.charactersList[n].character == character then
                table.remove(levelManager.charactersList, n)
            end
        end
        for n = #levelManager.ennemiManager:getEnnemiesList(), 1, -1 do
            if levelManager.ennemiManager:getEnnemiesList()[n].character == character then
                table.remove(levelManager.ennemiManager:getEnnemiesList(), n)
            end
        end
    end

    function levelManager:nextLevel()
        if levelManager.currentLevel < #levelsConfig then
            levelManager.currentLevel = levelManager.currentLevel + 1
            levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.start
            mapManager:loadMap(levelManager.currentLevel)
            self.ennemiManager:nextLevel(self, self.player)
            ui:nextLevel()
            soundManager:endOfLevel()
        end
    end

    function levelManager.clearEnnemies()
        self.ennemiManager:clear()
    end

    function levelManager.playerWinGame()
        return levelManager.isTheGameFinish
    end

    function levelManager:getPlayer()
        return self.playerManager:getPlayer()
    end

    return levelManager
end

return m_LevelManager

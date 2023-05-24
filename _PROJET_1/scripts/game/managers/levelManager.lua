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
        LEVELSTATE = {start = "start", game = "game", win = "win", currentState = "start"},
        currentLevel = 1,
        isTheGameFinish = false,
        player = nil
    }

    -- Au load, appelle de la fonction d'initiation d'un premier level et enregistrement de la référence joueur
    function levelManager:load(nb)
        mapManager:initMap()
        self:initLevel(nb)
        self.player = self:getPlayer()
        ui:load()
    end

    -- Fonction update : change le LEVELSTATE selon situation, voire le GAMESTATE dans le cas de la mort du joueur.
    -- Si joueur mort, game over. Si state du niveau "start" on joue une cinématique, si level game, on met fin au niveau quand les ennemis sont à 0 et on passe en win
    -- Sinon on update les ennemis. En level win, après avoir réalisé l'action pour finir le niveau (ici passer une porte), on joue le nextLevel.
    -- Update de la map et du player dans tous les cas (sauf mort du joueur), affichage du compte d'ennemis
    function levelManager:update(dt)
        if self.player.isDead then
            GAMESTATE.currentState = GAMESTATE.STATE.GAMEOVER
        elseif self.LEVELSTATE.currentState == self.LEVELSTATE.start then
            self.cinematicManager:playLevelCinematic(dt, self)
        elseif self.LEVELSTATE.currentState == self.LEVELSTATE.game then
            if #self.ennemiManager:getEnnemiesList() <= 0 then
                if self.player.character:getMode() ~= CHARACTERS.MODE.BOOSTED then
                    levelManager:endTheLevel()
                    self.LEVELSTATE.currentState = self.LEVELSTATE.win
                end
            else
                self:updateCharacters(dt)
            end
        elseif self.LEVELSTATE.currentState == self.LEVELSTATE.win then
            if mapManager:afterlevelWinAction(self.player) then
                levelManager:nextLevel()
            end
        end
        ui:setEnnemiesCount(#self.ennemiManager:getEnnemiesList())
        self.player:update(dt)
    end

    -- Fonction qui appelle le draw de mapManager, mais aussi de chaque ennemi et du player via leur manager
    function levelManager:draw()
        mapManager:draw()
        for n = #levelManager.ennemiManager:getEnnemiesList(), 1, -1 do
            levelManager.ennemiManager:getEnnemiesList()[n]:draw()
        end
        levelManager.playerManager:getPlayer():draw()
    end

    -- Fonction pour initier un nouveau niveau : on appelle la fonction qui initialise une nouvelle map et on fait spawn les personnages dessus
    function levelManager:initLevel(nb)
        self:createMap(nb)
        self:spawnCharacters()
    end

    -- Fonction qui initialise une nouvelle carte avec un ID.
    function levelManager:createMap(nb)
        self.currentLevel = nb
        mapManager:loadMap(self.currentLevel)
    end

    -- Fonction qui fait spawn les personnages : le joueur et les ennemis
    function levelManager:spawnCharacters()
        self.playerManager:spawnPlayer(self)
        self.ennemiManager:spawnEnnemies(self, self.playerManager:getPlayer())
    end

    -- Fonction qui appelle les updates des personnages
    function levelManager:updateCharacters(dt)
        --self.playerManager:update(dt)
        self.ennemiManager:update(dt)
    end

    -- Fonction qui appelle les actions à faire en fin de niveau sur la map et l'ui (ouvrir une porte)
    function levelManager:endTheLevel()
        mapManager:endTheLevel()
        ui:endTheLevel()
    end

    -- Fonction qui appelle les keypressed sur tous les personnages.
    -- Raccourci de debug pour passer de niveau plus rapidement.
    function levelManager.keypressed(key)
        if key == "m" then
            levelManager:nextLevel()
        end
        for n = 1, #levelManager.charactersList do
            levelManager.charactersList[n]:keypressed(key)
        end
    end

    function levelManager.setCurrentLevel(nb)
        levelManager.currentLevel = nb
    end

    function levelManager.getCurrentLevel()
        return levelManager.currentLevel
    end

    -- Fonction qui permet de détruire un personnage et une arme de la liste des personnages dans le niveau (et de la liste des ennemis)
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

    -- Fonction qui permet de passer au niveau suivant : s'il y a un niveau suivant, charge une nouvelle carte, charge de nouveaux ennemis et augmente le niveau joueur
    function levelManager:nextLevel()
        if levelManager.currentLevel < #levelsConfig then
            levelManager.currentLevel = levelManager.currentLevel + 1
            ui:nextLevel()
            soundManager:endOfLevel()
            levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.start
            self.player:upPlayerLevel()
            ui:updatePlayerLevel()
            self:initLevel(levelManager.currentLevel)
            if levelManager.currentLevel == #levelsConfig then
                ui:setPlayInformations(false)
            end
        end
    end

    -- Fonction qui permet de demander à clear la liste des ennemis
    function levelManager.clearEnnemies()
        self.ennemiManager:clear()
    end

    -- Fonction qui permet de savoir si le jeu est terminé ou non, si le joueur a gagné
    function levelManager.playerWinGame()
        return levelManager.isTheGameFinish
    end

    function levelManager:getPlayer()
        return self.playerManager:getPlayer()
    end

    return levelManager
end

return m_LevelManager

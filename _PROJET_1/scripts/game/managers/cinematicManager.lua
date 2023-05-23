-- MANAGER PERMETTANT DE GERER LES CINEMATIQUES DE CHAQUE

-- Chargement des modules
local mainCamera = require(PATHS.MAINCAMERA)
local levelsConfig = require(PATHS.LEVELCONFIGURATION)
local uiGame = require(PATHS.UIGAME)

local CinematicManager = {}
local Cinematic_mt = {__index = CinematicManager}

-- Instanciation de ma "factory" cinematicManager
function CinematicManager.new()
    local cinematicManager = {}
    return setmetatable(cinematicManager, Cinematic_mt)
end

-- Création d'un cinematicManager avec initialisation des paramètres : la longueur voulue de la cinématique et le timer qui va avec
function CinematicManager:create()
    local cinematicManager = {
        timer = 0,
        IsStarted = false,
        lenght = 3
    }

    -- Fonction qui est appelée pour jouer une cinématique de début de level : personnage qui s'avance
    function cinematicManager:playLevelCinematic(dt, levelManager)
        -- Si la cinématique n'est pas encore démarrée, je mets tous les personnages sur un mode cinématique (annule l'alert par exemple)
        -- puis je lock la caméra et je dis qu'elle a démarré
        if self.IsStarted == false then
            -- Si la cinématique est déjà démarré, mon timer est lancée (petite différence si cinématique finale sur la longueur)
            if levelManager.charactersList then
                for n = 1, #levelManager.charactersList do
                    levelManager.charactersList[n].character.controller:setInCinematicMode(
                        levelManager.charactersList[n].character,
                        true
                    )
                end
            end
            mainCamera.lock(true)
            self.IsStarted = true
        elseif self.IsStarted then
            if levelManager.currentLevel == #levelsConfig then
                self.timer = self.timer + dt * 0.25
            else
                self.timer = self.timer + dt
            end

            for n = 1, #levelManager.charactersList do
                if levelManager.charactersList[n].agent then
                    levelManager.charactersList[n]:update(dt)
                end
            end
            -- Affichage d'un texte "victoire" si c'est la cinématique finale, à environ la moitié de la cinématique
            if self.timer >= self.lenght / 2 then
                if levelManager.currentLevel == #levelsConfig then
                    uiGame.drawVictory()
                end
            end
            -- Si le timer arrive à la durée voulu, soit c'était la cinématique finale et donc on transmet au LVLmanager fin du jeu
            -- Sinon on délock la caméra, on passe les persos en mode normal et non pu cinématique
            -- Et on indique que le level est en mode "game" au lieu de "start"
            if self.timer >= self.lenght then
                if levelManager.currentLevel == #levelsConfig then
                    self.timer = 0
                    self.IsStarted = false
                    levelManager.isTheGameFinish = true
                else
                    mainCamera.lock(false)
                    for n = 1, #levelManager.charactersList do
                        levelManager.charactersList[n].character.controller:setInCinematicMode(
                            levelManager.charactersList[n],
                            false
                        )
                    end

                    levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.game
                    self.timer = 0
                    self.IsStarted = false
                end
            end
        end
    end

    return cinematicManager
end

return CinematicManager

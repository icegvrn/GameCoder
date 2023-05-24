-- MODULE APPELE PAR GAMEMANAGER LORSQUE LE GAMESTATE EST SUR GAME
GAMESTATE = require(PATHS.GAMESTATE)
camera = require(PATHS.MAINCAMERA)
local LevelManager = require(PATHS.LEVELMANAGER)
local ui = require(PATHS.UIGAME)

local game = {}
local LvlManager = LevelManager.new()
levelManager = LvlManager:create()

local isLoaded = false

-- Au load, on charge le levelManager en lui demandant la carte 1. On indique à la caméra qu'elle doit suivre le personnage joueur, puis que cette scène a été chargée une fois.
function game:load()
    ui:load()
    levelManager:load(1)
    camera.follow(levelManager:getPlayer().character)
    isLoaded = true
end

-- Durant le jeu, on update le levelManager et la caméra. Si le levelManager a le boolean indiquant que le joueur a gagné, game passe le gamestate à win.
function game:update(dt)
    levelManager:update(dt)
    camera.update(dt)
    if levelManager.playerWinGame() then
        GAMESTATE.currentState = GAMESTATE.STATE.WIN
    end
end

-- Appelle le draw de la camera et du levelMaanger
function game:draw()
    camera.draw()
    levelManager:draw()
end

-- Appelle le keypressed du levelManager et permet l'affichage du menu si escape en modifiant l'état du gameState
function game:keypressed(key)
    levelManager.keypressed(key)

    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.MENU
    end
end

-- Vérifie si cette scène a déjà été chargée pour ne pas répéter ce qui ne doit pas être répété
function game:isAlreadyLoaded()
    return isLoaded
end

return game

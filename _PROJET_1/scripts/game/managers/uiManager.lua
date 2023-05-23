-- MANAGER QUI CHOISIT QUEL UI AFFICHER EN FONCTION DE L'ETAT DU JEU

-- Chargement des modules
GAMESTATE = require(PATHS.GAMESTATE)

uiManager = {}
uiManager.ui = {}
uiManager.ui.all = require(PATHS.UIALL)
uiManager.ui.menus = require(PATHS.UIMENUS)
uiManager.ui.game = require(PATHS.UIGAME)
uiManager.currentState = GAMESTATE.STATE.START

-- Au load, charge l'UI commun et l'UI spécifiques aux menus (menus = start, narrative, menu, win... tous les écrans sauf jeu)
function uiManager.load()
    uiManager.ui.all:load()
    uiManager.ui.menus:load()
end

-- L'Update update l'ui commun et l'ui du gamestate en cours
function uiManager.update(dt)
    uiManager.ui.all:update(dt)
    if GAMESTATE.currentState == GAMESTATE.STATE.START then
        uiManager.ui.menus:update(dt)
    elseif
        GAMESTATE.currentState == GAMESTATE.STATE.MENU or GAMESTATE.currentState == GAMESTATE.STATE.GAMEOVER or
            GAMESTATE.currentState == GAMESTATE.STATE.WIN
     then
        uiManager.ui.menus:update(dt)
    elseif GAMESTATE.currentState == GAMESTATE.STATE.GAME then
        uiManager.ui.game:update(dt)
    end
end

-- Le draw draw l'ui commun et l'ui du gamestate en cours
function uiManager.draw()
    uiManager.ui.all:draw()
    if GAMESTATE.currentState == GAMESTATE.STATE.START then
        uiManager.ui.menus:draw()
    elseif
        GAMESTATE.currentState == GAMESTATE.STATE.MENU or GAMESTATE.currentState == GAMESTATE.STATE.GAMEOVER or
            GAMESTATE.currentState == GAMESTATE.STATE.WIN
     then
        uiManager.ui.menus:draw()
    elseif GAMESTATE.currentState == GAMESTATE.STATE.GAME then
        uiManager.ui.game:draw()
    end
end

-- Le keypressed appelle le keypressed de l'ui commun et de l'ui du gamestate en cours
function uiManager.keypressed(key)
    uiManager.ui.all:keypressed(key)
    if GAMESTATE.currentState == GAMESTATE.STATE.START then
        uiManager.ui.menus:keypressed(key)
    elseif
        GAMESTATE.currentState == GAMESTATE.STATE.MENU or GAMESTATE.currentState == GAMESTATE.STATE.GAMEOVER or
            GAMESTATE.currentState == GAMESTATE.STATE.WIN
     then
        uiManager.ui.game:keypressed(key)
    elseif GAMESTATE.currentState == GAMESTATE.STATE.GAME then
        uiManager.ui.game:keypressed(key)
    end
end

return uiManager

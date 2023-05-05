GAMESTATE = require("scripts/states/GAMESTATE")

uiManager = {}

uiManager.ui = {}

uiManager.ui.all = require("scripts/ui/uiAll")
uiManager.ui.menus = require("scripts/ui/uiMenus")
uiManager.ui.game = require("scripts/ui/uiGame")
uiManager.currentState = GAMESTATE.STATE.START

function uiManager.load()
    uiManager.ui.all:load()
    uiManager.ui.menus:load()
end

function uiManager.update(dt)
    if GAMESTATE.currentState == GAMESTATE.STATE.START then
        uiManager.ui.all:update(dt)
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

function uiManager.draw()
    if GAMESTATE.currentState == GAMESTATE.STATE.START then
        uiManager.ui.all:draw()
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

function uiManager.keypressed(key)
    if GAMESTATE.currentState == GAMESTATE.STATE.START then
        uiManager.ui.all:keypressed(key)
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

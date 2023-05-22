-- Chargement des modules
soundManager = require(PATHS.SOUNDMANAGER)
uiManager = require(PATHS.UIMANAGER)
GAMESTATE = require(PATHS.GAMESTATE)
debug = require(PATHS.DEBUG)
require(PATHS.GAMEMAP)

gameManager = {}

gameManager.scenes = {
    start = require(PATHS.START),
    narrative = require(PATHS.NARRATIVE),
    menu = require(PATHS.MENU),
    game = require(PATHS.GAME),
    gameOver = require(PATHS.GAMEOVER),
    win = require(PATHS.WIN)
}

gameManager.currentState = gameManager.scenes.start

function gameManager.debug(string)
    print("GameManager : " .. string .. "")
end

function gameManager.load()
    gameManager.scenes.start:load()
    uiManager.load()
    soundManager:load()
end

function gameManager.update(dt)
    uiManager.update(dt)
    soundManager.update(dt)

    for sceneName, scene in pairs(gameManager.scenes) do
        if sceneName == GAMESTATE.currentState then
            if gameManager.currentState ~= scene then
                gameManager.currentState = scene
                if gameManager.currentState:isAlreadyLoaded() == false then
                    gameManager.currentState:load()
                end
            end
        end
    end

    gameManager.currentState:update(dt)
end

function gameManager.draw()
    gameManager.currentState:draw()
    uiManager.draw()
end

function gameManager.keypressed(key)
    gameManager.currentState:keypressed(key)
    uiManager.keypressed(key)
end

return gameManager

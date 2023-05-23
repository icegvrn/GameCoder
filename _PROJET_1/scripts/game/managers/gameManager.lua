-- Chargement des modules
GAMESTATE = require(PATHS.GAMESTATE)
debug = require(PATHS.DEBUG)
require(PATHS.GAMEMAP)

gameManager = {
    currentState = nil
}
gameManager.scenes = {
    start = require(PATHS.START),
    narrative = require(PATHS.NARRATIVE),
    menu = require(PATHS.MENU),
    game = require(PATHS.GAME),
    gameOver = require(PATHS.GAMEOVER),
    win = require(PATHS.WIN)
}

--
function gameManager.load()
    gameManager.currentState = gameManager.scenes.start
    gameManager.currentState:load()
end

-- Le GameManager charge le bon fichier .lua de en fonction de la scène
function gameManager.update(dt)
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

-- Draw du bon fichier scene LUA
function gameManager.draw()
    gameManager.currentState:draw()
end

-- Keypressed du bon fichier scene LUA
function gameManager.keypressed(key)
    gameManager.currentState:keypressed(key)
end

return gameManager

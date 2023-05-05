require("scripts/game/gameMap")
soundManager = require("scripts/game/managers/soundManager")
uiManager = require("scripts/game/managers/uiManager")
GAMESTATE = require("scripts/states/GAMESTATE")

gameManager = {}

gameManager.scenes = {}

gameManager.scenes.start = require("scripts/game/scenes/start")
gameManager.scenes.narrative = require("scripts/game/scenes/narrative")
gameManager.scenes.menu = require("scripts/game/scenes/menu")
gameManager.scenes.game = require("scripts/game/scenes/game")
gameManager.scenes.gameOver = require("scripts/game/scenes/gameOver")
gameManager.scenes.win = require("scripts/game/scenes/win")

debug = require("scripts/Utils/debug")

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

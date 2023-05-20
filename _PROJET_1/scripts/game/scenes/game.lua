GAMESTATE = require("scripts/states/GAMESTATE")
local scene = require("scripts/engine/scene")
camera = require("scripts/game/mainCamera")
levelManager = require("scripts/game/managers/levelManager")
local ui = require("scripts/ui/uiGame")
local isLoaded = false

local game = scene.new()

function game:load()
    ui.load()
    gameMap.initMap(1)
    levelManager.load()
    camera.follow(levelManager.player.character)
    isLoaded = true
end

function game:update(dt)
    levelManager.update(dt)
    camera.update(dt)
    if levelManager.playerWinGame() then
        GAMESTATE.currentState = GAMESTATE.STATE.WIN
    end
end

function game:draw()
    camera.draw()
    gameMap.draw()
    levelManager.draw()
end

function game:keypressed(key)
    levelManager.keypressed(key)

    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.MENU
    end
end

function game:isAlreadyLoaded()
    return isLoaded
end

return game

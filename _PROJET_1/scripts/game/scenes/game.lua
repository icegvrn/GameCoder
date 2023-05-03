GAMESTATE = require("scripts/states/GAMESTATE")
local scene = require("scripts/engine/scene")
player = require("scripts/game/player")
camera = require("scripts/game/mainCamera")
levelManager = require("scripts/game/managers/levelManager")
local isLoaded = false
local game = scene.new()

function game:load()
    isLoaded = true
    print("---GAME")
    levelManager.load()
    gameMap.load()
    camera.follow(player.character)
end

function game:update(dt)
    levelManager.update(dt)
    camera.update(dt)
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

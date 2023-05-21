GAMESTATE = require(PATHS.GAMESTATE)

camera = require(PATHS.MAINCAMERA)
local LevelManager = require(PATHS.LEVELMANAGER)
local ui = require(PATHS.UIGAME)
local isLoaded = false

local game = {}
local LvlManager = LevelManager.new()
levelManager = LvlManager:create()

function game:load()
    ui.load()
    levelManager:load(1)
    camera.follow(levelManager:getPlayer().character)
    isLoaded = true
end

function game:update(dt)
    levelManager:update(dt)
    camera.update(dt)
    if levelManager.playerWinGame() then
        GAMESTATE.currentState = GAMESTATE.STATE.WIN
    end
end

function game:draw()
    camera.draw()
    levelManager:draw()
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

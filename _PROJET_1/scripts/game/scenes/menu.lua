GAMESTATE = require("scripts/states/GAMESTATE")
local scene = require("scripts/engine/scene")
local isLoaded = false
local menu = scene.new()

function menu:load()
    isLoaded = true
end

function menu:update(dt)
 
end

function menu:draw()
end

function menu:keypressed(key)
    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.GAME
    end
end

function menu:isAlreadyLoaded()
    return isLoaded
end

return menu

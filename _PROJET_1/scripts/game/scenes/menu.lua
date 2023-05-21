GAMESTATE = require(PATHS.GAMESTATE)

local isLoaded = false
local menu = {}

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

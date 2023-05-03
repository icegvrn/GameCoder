GAMESTATE = require("scripts/states/GAMESTATE")
local scene = require("scripts/engine/scene")

local win = scene.new()
local isLoaded = false

function win:load()
    isLoaded = true
end

function win:update(dt)
end

function win:draw()
end

function win:keypressed(key)
end

function win:isAlreadyLoaded()
    return isLoaded
end

return win

GAMESTATE = require("scripts/states/GAMESTATE")

local win = {}
local isLoaded = false
local background = love.graphics.newImage("contents/images/end_background.png")

function win:load()
    isLoaded = true
end

function win:update(dt)
end

function win:draw()
    love.graphics.draw(background, 0, 0)
end

function win:keypressed(key)
end

function win:isAlreadyLoaded()
    return isLoaded
end

return win

GAMESTATE = require("scripts/states/GAMESTATE")
local scene = require("scripts/engine/scene")
local isLoaded = false
local gameOver = scene.new()

function gameOver:load()
    isLoaded = true
end

function gameOver:update(dt)
end

function gameOver:draw()
    love.graphics.print("Game Over", Utils.screenWidth/2, Utils.screenHeight/2)
end

function gameOver:keypressed(key)
end

function gameOver:isAlreadyLoaded()
    return isLoaded
end

return gameOver

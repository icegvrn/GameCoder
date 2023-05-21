GAMESTATE = require(PATHS.GAMESTATE)

local isLoaded = false
local gameOver = {}
local background = love.graphics.newImage(PATHS.IMG.ROOT .. "gameOver_background.png")

function gameOver:load()
    isLoaded = true
end

function gameOver:update(dt)
end

function gameOver:draw()
    love.graphics.draw(background, 0, 0)
end

function gameOver:keypressed(key)
    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.START
        love.event.quit("restart")
    end
end

function gameOver:isAlreadyLoaded()
    return isLoaded
end

return gameOver

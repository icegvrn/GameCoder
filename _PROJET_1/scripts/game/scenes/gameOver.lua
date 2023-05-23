-- MODULE APPELE PAR GAMEMANAGER LORSQUE LE GAMESTATE EST SUR GAMEOVER
local isLoaded = false
local gameOver = {}
local background = love.graphics.newImage(PATHS.IMG.ROOT .. "gameOver_background.png")

function gameOver:load()
    isLoaded = true
end

-- Draw le fond d'écran indiquant que le joueur est game over
function gameOver:draw()
    love.graphics.draw(background, 0, 0)
end

-- Permet avec escape de resart le jeu.
function gameOver:keypressed(key)
    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.START
        love.event.quit("restart")
    end
end

-- Vérifie si cette scène a déjà été chargée pour ne pas répéter ce qui ne doit pas être répété
function gameOver:isAlreadyLoaded()
    return isLoaded
end

function gameOver:update(dt)
    --
end

return gameOver

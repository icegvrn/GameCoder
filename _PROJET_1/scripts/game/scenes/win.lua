-- MODULE APPELE PAR GAMEMANAGER LORSQUE LE GAMESTATE EST SUR WIN
GAMESTATE = require(PATHS.GAMESTATE)

local win = {}
local isLoaded = false
local background = love.graphics.newImage(PATHS.IMG.ROOT .. "end_background.png")

function win:load()
    isLoaded = true
end

-- Affiche l'écran de win
function win:draw()
    love.graphics.draw(background, 0, 0)
end

-- Vérifie si cette scène a déjà été chargée pour ne pas répéter ce qui ne doit pas être répété
function win:isAlreadyLoaded()
    return isLoaded
end

function win:update(dt)
    --
end

return win

-- MODULE APPELE PAR GAMEMANAGER LORSQUE LE GAMESTATE EST SUR MENU
local isLoaded = false
local menu = {}

function menu:load()
    isLoaded = true
end

-- Permet avec escape de reprendre ou commencer le jeu
function menu:keypressed(key)
    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.GAME
    end
end

-- Vérifie si cette scène a déjà été chargée pour ne pas répéter ce qui ne doit pas être répété
function menu:isAlreadyLoaded()
    return isLoaded
end

function menu:update(dt)
    --
end

function menu:draw()
    --
end

return menu

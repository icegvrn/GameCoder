-- MODULE APPELE PAR GAMEMANAGER LORSQUE LE GAMESTATE EST SUR START
soundManager = require(PATHS.SOUNDMANAGER)

local start = {}
local showImage = true
local timer = 0
local interval = 0.5
local isLoaded = false

function start:load()
    isLoaded = true
end

-- Fait blink l'image indiquant de press space pour démarrer
function start:update(dt)
    timer = timer + dt
    if timer >= interval then
        showImage = not showImage
        timer = 0
    end
end

-- Draw un background, ainsi qu'une image qui blink indiquant au joueur comment démarrer le jeu
-- Draw aussi du texte en bas de l'écran
function start:draw()
    local startButton = love.graphics.newImage(PATHS.IMG.ROOT .. "startButton.png")
    local background = love.graphics.newImage(PATHS.IMG.ROOT .. "background.png")
    love.graphics.draw(background, 0, 0)
    if showImage then
        love.graphics.draw(
            startButton,
            (Utils.screenWidth / 2 - startButton:getWidth() / 2 * 0.7),
            Utils.screenHeight - 100,
            0,
            0.7
        )
    end
    love.graphics.setFont(UIAll.font9)
    love.graphics.print(
        "Simon Foucher - First LUA project 2023 - Illustration generate by Midjourney",
        5,
        Utils.screenHeight - 10
    )
    love.graphics.setFont(UIAll.defaultFont)
end

-- Fonction qui permet de passer à l'introduction quand le joueur a validé qu'il souhaitait commencer
function start:keypressed(key)
    if key == "space" or key == "escape" then
        soundManager:playSound(PATHS.SOUNDS.ROOT .. "button.mp3", 0.5, false)
        GAMESTATE.currentState = GAMESTATE.STATE.NARRATIVE
    end
end

-- Vérifie si cette scène a déjà été chargée pour ne pas répéter ce qui ne doit pas être répété
function start:isAlreadyLoaded()
    return isLoaded
end

return start

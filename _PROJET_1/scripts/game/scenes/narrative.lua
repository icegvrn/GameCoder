-- MODULE APPELE PAR GAMEMANAGER LORSQUE LE GAMESTATE EST SUR NARRATIVE
local narrative = {}
local isLoaded = false
local textPositionY = 0
local timer = 0
local timerEnd = 45

function narrative:load()
    isLoaded = true
end

-- Fonction qui permet de faire un effet de défilement du texte d'introduction à l'aide d'un timer.
function narrative:update(dt)
    timer = timer + dt
    if textPositionY >= -550 then
        textPositionY = textPositionY - 15 * dt
    end
    if timer >= timerEnd then
        timer = 0
        GAMESTATE.currentState = GAMESTATE.STATE.GAME
    end
end

-- Draw le texte d'introduction du jeu à une position qui évolue au fil du temps grâce à l'update
function narrative:draw()
    love.graphics.setFont(UIAll.font30)
    love.graphics.setColor(0, 1, 0)
    love.graphics.print(
        "            Il était une fois, le monde fantastique de Herodom. \n Dans cet univers dominé par les héros, Gromokgg, un paisible orc, a été capturé, \n entrainé dans un sombre donjon en raison de son apparence jugée très très vilaine.\n Gromokgg y a subi les expériences impies du Mage-en-chef qui souhaitait le rendre… \n                        plus comme eux ? \n \n Mais Gromokgg a su saisir l’opportunité d’assommer son tortionnaire. \n Armé du bâton magique qu’il lui a volé, Gromokgg compte bien retrouver sa liberté \n et affronter quiconque lui barrera la route. \n                   Saura-t-il braver tous les dangers ? \n Et surtout, quelle est cette étonnante créature qui, désormais, sommeille en lui ?",
        10,
        Utils.screenHeight + textPositionY
    )

    love.graphics.setFont(UIAll.font50)
    love.graphics.print(
        "    La destinée de Gromokgg est désormais \n             entre vos mains…",
        10,
        Utils.screenHeight + 400 + textPositionY
    )
    love.graphics.setColor(1, 1, 1)
end

-- Fonction qui permet de skiper et passer directement au jeu si on fait échap
function narrative:keypressed(key)
    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.GAME
    end
end

-- Vérifie si cette scène a déjà été chargée pour ne pas répéter ce qui ne doit pas être répété
function narrative:isAlreadyLoaded()
    return isLoaded
end

return narrative

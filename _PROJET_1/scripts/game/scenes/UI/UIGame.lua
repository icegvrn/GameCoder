-- MODULE QUI COMPORTE L'UI DE LA STATE "GAME", HORS UI CONCERNANT LE JOUEUR (VOIR PLAYERUI)

UIAll = require(PATHS.UIALL)
require(PATHS.UTILS)
controller = require(PATHS.CONFIGS.CONTROLLER)

local uiGame = {}
uiGame.buttons = {}
uiGame.buttons.door = love.graphics.newImage(PATHS.IMG.UI .. "doorOpen.png")
uiGame.buttons.doorOpened = false
uiGame.victoryText = love.graphics.newText(UIAll.font100, "VICTORY!")
uiGame.victory = false

-- Modifie la couleur de fond lorsqu'on est en mode jeu
function uiGame:load()
    love.graphics.setBackgroundColor(0.08, 0.06, 0.05)
end

-- Draw une icone de porte ouverte lorsque la porte est ouverte ou affiche un texte "Victory" sur l'écran pendant la cinématique de fin (fonctionne par boolean)
function uiGame:draw()
    if uiGame.buttons.doorOpened then
        love.graphics.draw(
            uiGame.buttons.door,
            Utils.screenCoordinates(Utils.screenWidth - 60, Utils.screenHeight - 60)
        )
    end

    if uiGame.victory then
        love.graphics.draw(
            uiGame.victoryText,
            Utils.screenCoordinates(-uiGame.victoryText:getWidth() / 2, -uiGame.victoryText:getHeight() / 2)
        )
    end
end

-- Fonction qui permet de modifier le boléan permettant d'afficher ou non l'icone de la porte ouverte.
function uiGame:doorIsOpen(bool)
    if bool then
        uiGame.buttons.doorOpened = true
    else
        uiGame.buttons.doorOpened = false
    end
end

-- Fonction permettant d'effectuer les actions UI lors d'un nouveau niveau. Ici modifier la booléan de l'icone de la porte ouverte.
function uiGame:nextLevel()
    self:doorIsOpen(false)
end

-- Fonction permettant d'effectuer les actions UI lorsqu'un niveau est terminé. Ici modifier la booléan de l'icone de la porte ouverte.
function uiGame:endTheLevel()
    self:doorIsOpen(true)
end

-- Fonction qui modifie le boléan permettant de draw le texte victory
function uiGame.drawVictory()
    uiGame.victory = true
end

function uiGame:update(dt)
    --
end

function uiGame.keypressed(key)
    --
end

return uiGame

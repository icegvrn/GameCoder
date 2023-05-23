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
uiGame.timerMinutes = 0
uiGame.timerSecondes = 0
uiGame.ennemiesCount = 0
uiGame.weaponLevel = 1
uiGame.playInformations = true

-- Modifie la couleur de fond lorsqu'on est en mode jeu
function uiGame:load()
    love.graphics.setBackgroundColor(0.08, 0.06, 0.05)
end

-- Draw d'un timer sur le jeu et du nombre d'ennemis restants dans chaque niveau.
-- Draw d'une icone de porte ouverte lorsque la porte est ouverte ou affiche un texte "Victory" sur l'écran pendant la cinématique de fin (fonctionne par boolean)
function uiGame:draw()
    if self.playInformations then
        uiGame:drawTimer()
        uiGame:drawEnnemiCount()
        uiGame:drawWeaponLevel()
    end

    if uiGame.buttons.doorOpened then
        love.graphics.draw(
            uiGame.buttons.door,
            Utils.screenCoordinates(Utils.screenWidth - 60, Utils.screenHeight - 60)
        )
    end

    if uiGame.victory then
        love.graphics.draw(
            uiGame.victoryText,
            Utils.screenCoordinates(
                Utils.screenWidth / 2 - uiGame.victoryText:getWidth() / 2,
                Utils.screenHeight / 2 - uiGame.victoryText:getHeight() / 2
            )
        )
    end
end

-- Update du timer de temps restant
function uiGame:updateTimer(dt)
    self.timerSecondes = self.timerSecondes + dt
    if self.timerSecondes >= 60 then
        self.timerSecondes = 0
        self.timerMinutes = self.timerMinutes + 1
    end
end

-- Permet le draw du timer
function uiGame:drawTimer()
    love.graphics.setFont(UIAll.font20stylized)
    love.graphics.print(
        string.format("%02d:%02d", self.timerMinutes, self.timerSecondes),
        Utils.screenCoordinates(Utils.screenWidth - 70, 10)
    )
    love.graphics.setFont(UIAll.defaultFont)
end

--Permet le draw du compte d'ennemi si le booléan le permettant est ok (actuellement, pas dans la cinématique final)
function uiGame:drawEnnemiCount()
    love.graphics.setFont(UIAll.font20stylized)
    love.graphics.setColor(0.60, 0.1, 0.82)
    love.graphics.print("Ennemies : " .. self.ennemiesCount, Utils.screenCoordinates(Utils.screenWidth - 170, 10))
    love.graphics.setFont(UIAll.defaultFont)
    love.graphics.setColor(1, 1, 1)
end

--Permet le draw du compte d'ennemi si le booléan le permettant est ok (actuellement, pas dans la cinématique final)
function uiGame:drawWeaponLevel()
    love.graphics.setFont(UIAll.font20stylized)
    love.graphics.print(
        "Your Weapon Level : " .. self.weaponLevel,
        Utils.screenCoordinates(Utils.screenWidth - 330, 10)
    )
    love.graphics.setFont(UIAll.defaultFont)
end

-- Permet de modifier le nombre d'ennemi, info mise à jour par le levelManager
function uiGame:setEnnemiesCount(ennemiCount)
    self.ennemiesCount = ennemiCount
end

function uiGame:updatePlayerLevel()
    self.weaponLevel = self.weaponLevel + 1
end

-- Permet de modifier le booléan permettant l'affichage ou non du compte d'ennemis restants
function uiGame:setPlayInformations(bool)
    self.playInformations = bool
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
    uiGame:updateTimer(dt)
end

function uiGame.keypressed(key)
    --
end

return uiGame

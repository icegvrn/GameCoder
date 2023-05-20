ui = require("scripts/engine/ui")
require("scripts/Utils/utils")
controller = require("scripts/engine/controller")

local uiGame = ui.new()

uiGame.buttons = {}
uiGame.buttons.timer = 0
defaultFont = love.graphics.newFont()
font9 = love.graphics.newFont(9)
font100 = love.graphics.newFont(100)
uiGame.buttons.weaponText = love.graphics.newText(font9, controller.action1)
uiGame.buttons.boosterText = love.graphics.newText(font9, controller.action2)

uiGame.buttons.door = love.graphics.newImage("contents/images/ui/doorOpen.png")
uiGame.buttons.doorOpened = false
uiGame.victory = false

function uiGame.load()
end

function uiGame:initPlayerBar()
end

function uiGame:update(dt)
end

function uiGame:draw()
    if uiGame.buttons.doorOpened then
        love.graphics.draw(
            uiGame.buttons.door,
            Utils.screenCoordinates(Utils.screenWidth - 60, Utils.screenHeight - 60)
        )
    end
    if uiGame.victory then
        love.graphics.setFont(font100)
        love.graphics.print(
            "VICTORY !",
            Utils.screenCoordinates((Utils.screenWidth / 2) - 250, (Utils.screenHeight / 2) - 50)
        )
    end
end

function ui.drawPlayerPointBar(player, points, maxPoints)
    local x, y = Utils.screenCoordinates(10, (Utils.screenHeight - 40))

    if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
        love.graphics.setFont(defaultFont)
        love.graphics.print(math.floor(uiGame.buttons.timer), x, y)
    end
end

function ui.doorIsOpen(bool)
    if bool then
        uiGame.buttons.doorOpened = true
        soundManager:playSound("contents/sounds/game/door_signal.wav", 0.5, false)
        soundManager:playSound("contents/sounds/game/doorOpen.wav", 0.1, false)
    else
        uiGame.buttons.doorOpened = false
    end
end

function ui.drawVictory()
    uiGame.victory = true
end

return uiGame

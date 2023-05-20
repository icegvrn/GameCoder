UITools = require("scripts/ui/uiTools")
require("scripts/Utils/utils")
controller = require("scripts/engine/controller")

local uiGame = {}

uiGame.buttons = {}
uiGame.buttons.door = love.graphics.newImage("contents/images/ui/doorOpen.png")
uiGame.buttons.doorOpened = false
uiGame.victory = false

function uiGame:load()
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
        love.graphics.setFont(UITools.font100)
        love.graphics.print(
            "VICTORY !",
            Utils.screenCoordinates((Utils.screenWidth / 2) - 250, (Utils.screenHeight / 2) - 50)
        )
    end
end

function uiGame:nextLevel()
    self:doorIsOpen(false)
end

function uiGame:endTheLevel()
    self:doorIsOpen(true)
end

function uiGame:doorIsOpen(bool)
    if bool then
        uiGame.buttons.doorOpened = true
    else
        uiGame.buttons.doorOpened = false
    end
end

function uiGame.drawVictory()
    uiGame.victory = true
end

function uiGame.keypressed(key)
end

return uiGame

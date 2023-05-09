ui = require("scripts/engine/ui")
require("scripts/Utils/utils")
controller = require("scripts/engine/controller")

local uiGame = ui.new()

uiGame.player = {}
uiGame.player.maxPVBarSize = 70
uiGame.player.currentPVBarSize = 0
uiGame.player.lifeColor = {}
uiGame.player.lifeColor[1] = {0.06, 0.69, 0.27}
uiGame.player.lifeColor[2] = {1, 1, 0}
uiGame.player.lifeColor[3] = {1, 0, 0}
uiGame.player.currentLifeColor = uiGame.player.lifeColor[1]

uiGame.player.pointsBarWidth = 200
uiGame.player.pointsBarHeight = 15
uiGame.player.currentPointsBarWidth = 0
uiGame.player.pointsColor = {}
uiGame.player.pointsColor[1] = {0.8, 0.5, 0.2}
uiGame.player.pointsColor[2] = {0.8, 0.4, 0.8}
uiGame.player.pointsColor[3] = {0.8, 0.1, 0.6}
uiGame.player.pointsColor[4] = {0.8, 0, 0.2}
uiGame.player.pointsColor[5] = {1, 1, 1}
uiGame.player.currentPointsColor = uiGame.player.pointsColor[1]
uiGame.player.timer = 0

uiGame.buttons = {}
uiGame.buttons.timer = 0
defaultFont = love.graphics.newFont()
font9 = love.graphics.newFont(9)
uiGame.buttons.weaponText = love.graphics.newText(font9, controller.action1)
uiGame.buttons.boosterText = love.graphics.newText(font9, controller.action2)

function uiGame.load()
    uiGame.buttons.timer = player.boosterDuration
    uiGame.buttons.boostedSheet = love.graphics.newImage("contents/images/ui/boostedButtons.png")
    uiGame.buttons.weaponSheet = love.graphics.newImage("contents/images/ui/weaponButtons.png")
    uiGame.buttons.boostedImg =
        love.graphics.newQuad(
        0,
        0,
        (uiGame.buttons.boostedSheet:getWidth() / uiGame.buttons.boostedSheet:getHeight()),
        uiGame.buttons.boostedSheet:getHeight(),
        uiGame.buttons.boostedSheet:getDimensions()
    )
    uiGame.buttons.weaponImg =
        love.graphics.newQuad(
        0,
        0,
        (uiGame.buttons.weaponSheet:getWidth() / uiGame.buttons.weaponSheet:getHeight()),
        uiGame.buttons.weaponSheet:getHeight(),
        uiGame.buttons.weaponSheet:getDimensions()
    )
end

function uiGame.update(dt)
end

function uiGame.draw()
end

function uiGame.updatePlayerPointsBar(dt, points, maxPoints)
    uiGame.player.currentPointsBarWidth = (points / maxPoints) * uiGame.player.pointsBarWidth

    if points < maxPoints / 4 then
        uiGame.player.currentPointsColor = uiGame.player.pointsColor[1]
    elseif points < maxPoints / 2 then
        uiGame.player.currentPointsColor = uiGame.player.pointsColor[2]
    elseif points < maxPoints / 1.3 then
        uiGame.player.currentPointsColor = uiGame.player.pointsColor[3]
    elseif points == maxPoints then
        uiGame.player.timer = uiGame.player.timer + dt
        if uiGame.player.timer % 2 > 1 then
            uiGame.player.currentPointsColor = uiGame.player.pointsColor[4]
        else
            uiGame.player.currentPointsColor = uiGame.player.pointsColor[5]
        end
    end

    if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
        if uiGame.buttons.timer > 0 then
            uiGame.buttons.timer = uiGame.buttons.timer - 1 * dt
        else
            uiGame.buttons.timer = player.boosterDuration
        end
    end
end

function uiGame.updatePlayerLifeBar(currentPV, maxPV)
    -- A replacer dans player
    if currentPV < 0 then
        ---
        uiGame.player.currentPVBarSize = 0
    else
        uiGame.player.currentPVBarSize = (currentPV / maxPV) * uiGame.player.maxPVBarSize
    end

    if currentPV > maxPV / 2 then
        uiGame.player.currentLifeColor = uiGame.player.lifeColor[1]
    elseif currentPV < maxPV / 2 and currentPV > maxPV / 4 then
        uiGame.player.currentLifeColor = uiGame.player.lifeColor[2]
    elseif currentPV <= currentPV / 4 then
        uiGame.player.currentLifeColor = uiGame.player.lifeColor[3]
    end
end

function uiGame.drawPlayerLifeBar(position_x, position_y, maxPV, currentPV, scale_x, scale_y)
    love.graphics.setColor(1, 1, 1, 0.3)
    love.graphics.rectangle("fill", position_x - 35, position_y - 35, uiGame.player.maxPVBarSize, 10, scale_x, scale_y)
    love.graphics.setColor(uiGame.player.currentLifeColor)
    love.graphics.rectangle(
        "fill",
        position_x - 35,
        position_y - 35,
        uiGame.player.currentPVBarSize,
        10,
        scale_x,
        scale_y
    )
    love.graphics.setColor(1, 1, 1)
end

function ui.drawPlayerPointBar(points, maxPoints)
    local x, y = Utils.screenCoordinates(10, (Utils.screenHeight - 40))
    love.graphics.setColor(1, 1, 1, 0.3)
    love.graphics.rectangle("fill", x + 45, y + 5, uiGame.player.pointsBarWidth, uiGame.player.pointsBarHeight)
    love.graphics.setColor(uiGame.player.currentPointsColor)
    love.graphics.rectangle("fill", x + 45, y + 5, uiGame.player.currentPointsBarWidth, uiGame.player.pointsBarHeight)
    love.graphics.setColor(1, 1, 1)

    uiGame.buttons.boostedSheet = love.graphics.newImage("contents/images/ui/boostedButtons.png")
    uiGame.buttons.weaponSheet = love.graphics.newImage("contents/images/ui/weaponButtons.png")

    if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
        uiGame.buttons.boostedImg =
            love.graphics.newQuad(
            uiGame.buttons.boostedSheet:getHeight(),
            0,
            (uiGame.buttons.boostedSheet:getWidth() /
                (uiGame.buttons.boostedSheet:getWidth() / uiGame.buttons.boostedSheet:getHeight())),
            uiGame.buttons.boostedSheet:getHeight(),
            uiGame.buttons.boostedSheet:getDimensions()
        )
    elseif player.points == player.maxPoints then
        uiGame.buttons.boostedImg =
            love.graphics.newQuad(
            uiGame.buttons.boostedSheet:getHeight() * 2,
            0,
            (uiGame.buttons.boostedSheet:getWidth() /
                (uiGame.buttons.boostedSheet:getWidth() / uiGame.buttons.boostedSheet:getHeight())),
            uiGame.buttons.boostedSheet:getHeight(),
            uiGame.buttons.boostedSheet:getDimensions()
        )
    else
        uiGame.buttons.boostedImg =
            love.graphics.newQuad(
            0,
            0,
            (uiGame.buttons.boostedSheet:getWidth() /
                (uiGame.buttons.boostedSheet:getWidth() / uiGame.buttons.boostedSheet:getHeight())),
            uiGame.buttons.boostedSheet:getHeight(),
            uiGame.buttons.boostedSheet:getDimensions()
        )
    end

    if player.character:getState() == CHARACTERS.STATE.FIRE then
        uiGame.buttons.weaponImg =
            love.graphics.newQuad(
            0,
            0,
            (uiGame.buttons.weaponSheet:getWidth() /
                (uiGame.buttons.weaponSheet:getWidth() / uiGame.buttons.weaponSheet:getHeight())),
            uiGame.buttons.weaponSheet:getHeight(),
            uiGame.buttons.weaponSheet:getDimensions()
        )
    else
        uiGame.buttons.weaponImg =
            love.graphics.newQuad(
            uiGame.buttons.weaponSheet:getHeight(),
            0,
            (uiGame.buttons.weaponSheet:getWidth() /
                (uiGame.buttons.weaponSheet:getWidth() / uiGame.buttons.weaponSheet:getHeight())),
            uiGame.buttons.weaponSheet:getHeight(),
            uiGame.buttons.weaponSheet:getDimensions()
        )
    end

    love.graphics.draw(uiGame.buttons.boostedSheet, uiGame.buttons.boostedImg, x, y - 5)
    love.graphics.draw(uiGame.buttons.weaponSheet, uiGame.buttons.weaponImg, x, y - 50)

    love.graphics.setColor(1, 0.7, 0)
    love.graphics.rectangle(
        "fill",
        x,
        y - 65 + uiGame.buttons.weaponSheet:getHeight(),
        uiGame.buttons.weaponText:getWidth() + 2,
        uiGame.buttons.weaponText:getHeight() + 2
    )
    love.graphics.rectangle(
        "fill",
        x - 1,
        y - 20 + uiGame.buttons.boostedSheet:getHeight(),
        uiGame.buttons.boosterText:getWidth() + 2,
        uiGame.buttons.boosterText:getHeight() + 2
    )

    love.graphics.setColor(0, 0, 0)
    love.graphics.draw(uiGame.buttons.weaponText, x, y - 65 + uiGame.buttons.weaponSheet:getHeight())
    love.graphics.draw(uiGame.buttons.boosterText, x, y - 20 + uiGame.buttons.weaponSheet:getHeight())
    love.graphics.setColor(1, 1, 1)
    if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
        love.graphics.setFont(defaultFont)
        love.graphics.print(math.floor(uiGame.buttons.timer), x, y)
    end
end

return uiGame

GAMESTATE = require("scripts/states/GAMESTATE")
require("scripts/Utils/utils")
local scene = require("scripts/engine/scene")

local narrative = scene.new()
local isLoaded = false

local textPositionY = 0
local timer = 0
local timerEnd = 45

local textFont = love.graphics.newFont("contents/fonts/pixelfont.ttf", 30)
local textFont50 = love.graphics.newFont("contents/fonts/pixelfont.ttf", 50)

function narrative:load()
    isLoaded = true
end

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

function narrative:draw()
    love.graphics.setFont(textFont)
    love.graphics.setColor(0, 1, 0)
    love.graphics.print(
        "            Il était une fois, le monde fantastique de Herodom. \n Dans cet univers dominé par les héros, Gromokgg, un paisible orc, a été capturé, \n entrainé dans un sombre donjon en raison de son apparence jugée très très vilaine.\n Gromokgg y a subi les expériences impies du Mage-en-chef qui souhaitait le rendre… \n                        plus comme eux ? \n \n Mais Gromokgg a su saisir l’opportunité d’assommer son tortionnaire. \n Armé du bâton magique qu’il lui a volé, Gromokgg compte bien retrouver sa liberté \n et affronter quiconque lui barrera la route. \n                   Saura-t-il braver tous les dangers ? \n Et surtout, quelle est cette étonnante créature qui, désormais, sommeille en lui ?",
        10,
        utils.screenHeight + textPositionY
    )

    love.graphics.setFont(textFont50)
    love.graphics.print(
        "    La destinée de Gromokgg est désormais \n             entre vos mains…",
        10,
        utils.screenHeight + 400 + textPositionY
    )
    love.graphics.setColor(1, 1, 1)
end

function narrative:keypressed(key)
    if key == "escape" then
        GAMESTATE.currentState = GAMESTATE.STATE.GAME
    end
end

function narrative:isAlreadyLoaded()
    return isLoaded
end

return narrative

GAMESTATE = require("scripts/states/GAMESTATE")
require("scripts/Utils/utils")
local scene = require("scripts/engine/scene")
soundManager = require("scripts/game/managers/soundManager")

local start = scene.new()

local showImage = true
local timer = 0
local interval = 0.5
local isLoaded = false

function start:load()
    isLoaded = true
    print("Je suis chargé")
end

function start:update(dt)
    timer = timer + dt

    if timer >= interval then
        showImage = not showImage -- Inverse l'état de l'image
        timer = 0
    end
end

function start:draw()
    local startButton = love.graphics.newImage("contents/images/startButton.png")
    local background = love.graphics.newImage("contents/images/background.png")
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
    local font = love.graphics.newFont(9)
    love.graphics.setFont(font)
    love.graphics.print(
        "Simon Foucher - First LUA project 2023 - Illustration generate by Midjourney",
        5,
        Utils.screenHeight - 10
    )
end

function start:keypressed(key)
    if key == "space" then
        soundManager:playSound("contents/sounds/button.mp3", 0.5, false)
        GAMESTATE.currentState = GAMESTATE.STATE.NARRATIVE
    end
end

function start:isAlreadyLoaded()
    return isLoaded
end

return start

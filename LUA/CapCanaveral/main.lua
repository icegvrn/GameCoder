-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

gameManager = require("gameManager")
countdown = require("countdown")
timeManager = require("timeManager")
animationManager = require("animationManager")
fusee = require("fusee")
timerStart = false
timer = 0

function love.load()
end

function love.update(dt)
    if timerStart then
        timeManager.startTimer(dt)
    end
    timeManager.setTime(timer)
    animationManager.animate(dt)
end

function love.draw()
    font = love.graphics.newFont(24)
    love.graphics.setFont(font)
    love.graphics.print(countdown.getTime(), love.graphics.getWidth() / 3, 10)
    love.graphics.print(instructionsManager.getText(), love.graphics.getWidth() / 3, 34)
    love.graphics.draw(fusee.img, fusee.posX, fusee.posY)
end

function love.keypressed(key)
    if key == "space" then
        timerStart = true
    end
end

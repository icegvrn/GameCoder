-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")
love.graphics.setDefaultFilter("nearest")
local game = require("game")
local cursor = love.mouse.newCursor("images/shovel.png", 0, 0)
function love.load()
    screenWidth = love.graphics.getWidth()
    screenHeight = love.graphics.getHeight()
    game.load()
    love.mouse.setCursor(cursor)
end

function love.update(dt)
    game.checkMouse()
end

function love.draw()
    game.draw()
end

function love.wheelmoved(x, y)
    game.changeTexture()
end

function love.keypressed(key)
    if key == "mousewheel" then
        game.changeTexture()
    end
end

PATHS = require("scripts/configs/PATHS")
gameManager = require(PATHS.GAMEMANAGER)
UITools = require(PATHS.UITOOLS)
debugManager = require(PATHS.DEBUGMANAGER)

-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

function love.load()
    gameManager.load()
    UITools.load()
end

function love.update(dt)
    dt = math.min(dt, 1 / 60)
    gameManager.update(dt)
    debugManager.update(dt)
end

function love.draw()
    gameManager.draw()
    debugManager.draw()
end

function love.keypressed(key)
    gameManager.keypressed(key)
    debugManager.keypressed(key)
end

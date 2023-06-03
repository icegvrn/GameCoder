PATHS = require("scripts/Configs/PATHS")
CONST = require(PATHS.CONST)
UIAll = require(PATHS.UIALL)
Utils = require(PATHS.UTILS)
GAMESTATE = require(PATHS.GAMESTATE)
gameManager = require(PATHS.GAMEMANAGER)
debugManager = require(PATHS.DEBUGMANAGER)
soundManager = require(PATHS.SOUNDMANAGER)
UIManager = require(PATHS.UIMANAGER)

-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

function love.load()
    gameManager.load()
    soundManager.load()
    UIManager.load()
end

function love.update(dt)
    dt = math.min(dt, 1 / 60) -- Permet d'éviter les bugs de calcul lors d'un changement de frame.
    gameManager.update(dt)
    UIManager.update(dt)
    soundManager.update(dt)
    debugManager.update(dt)
end

function love.draw()
    gameManager.draw()
    UIManager.draw()
    debugManager.draw()
end

function love.keypressed(key)
    gameManager.keypressed(key)
    UIManager.keypressed(key)
    debugManager.keypressed(key)
end

-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

gameManager = require("scripts/game/managers/gameManager")
ui = require("scripts/ui/ui")
debugManager = require("scripts/game/managers/debugManager")
require("scripts/states/PATHS")

function love.load()
    gameManager.load()
    ui.load()
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

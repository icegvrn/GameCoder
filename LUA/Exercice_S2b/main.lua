-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")
local etoiles = require("stars")
local bubble = require("bubble")

function love.load()
    love.window.setMode(1080, 720)
    love.window.setTitle("BUBBLE SHIELD")
    etoiles:Create(1000)
    bubble:load()
end

function love.update(dt)
    dt = math.min(dt, 1 / 60)
    etoiles:move(dt)
    bubble:update(dt)
end

function love.draw()
    etoiles:draw()
    bubble:draw()
end

-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

imageTank = love.graphics.newImage("images/tank.png")

print("je vais charger le module tank")
local tank = require("tank")
print("l'adresse du moduleTank dans main est " .. tostring(moduleTank))
print("je vais charger le module tirs")
local moduleTirs = require("tirs")

local tourelle = require("tourelle")

function love.load()
end

function love.update(dt)
    if love.keyboard.isDown("right") then
        tank:tourne(2 * dt)
    elseif love.keyboard.isDown("left") then
        tank:tourne(-2 * dt)
    elseif love.keyboard.isDown("up") then
        tank:mouvement(100 * dt)
    elseif love.keyboard.isDown("down") then
        tank:mouvement(-100 * dt)
    end
    moduleTirs.Update(dt)
    tank.update(dt)
    tourelle:update(dt)
end

function love.draw()
    moduleTirs.Draw()
    tank.draw()
    tourelle:draw()
    love.graphics.print(moduleTirs.GetNumberOfTirs())
end

function love.keypressed(key)
    if key == "space" then
        moduleTirs.Tire(tank.x, tank.y, tank.angle, 300)
    end
end

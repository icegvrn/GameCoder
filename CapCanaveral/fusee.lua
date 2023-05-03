-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

fusee = {}
-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

fusee.img = love.graphics.newImage("images/rocket.png")
fusee.posX = love.graphics.getWidth() / 2 - fusee.img:getWidth() / 2
fusee.posY = love.graphics.getHeight() / 2 - fusee.img:getHeight() / 2

function love.load()
end

function love.update(dt)
end

function love.draw()
end

function love.keypressed(key)
end

function fusee.setAnimation(state)
    if state == 5 then
        fusee.launch()
    end
end

function fusee.launch()
end

return fusee

-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

animationManager = {}
launch = false
fusee = require("fusee")
-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

function love.load()
end

function love.update(dt)
end

function love.draw()
end

function love.keypressed(key)
end

function animationManager.setAnimation(state)
    if state == 6 then
        launch = true
    end
end

function animationManager.animate(dt)
    if launch == true then
        fusee.posY = fusee.posY - 40 * dt
    end
end

return animationManager

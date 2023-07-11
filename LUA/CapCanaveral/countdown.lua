-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

countdown = {}
-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

startingTime = 40
time = 0

function love.load()
end

function love.update(dt)
end

function love.draw()
end

function love.keypressed(key)
end

function countdown.setTime(timer)
    if (timer) then
        time = startingTime - math.floor(timer)
    end
end

function countdown.getTime()
    return time
end

return countdown

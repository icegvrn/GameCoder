timeManager = {}
gameManager = require("gameManager")

function timeManager.startTimer(dt)
    timer = timer + dt
end

function timeManager.setTime(timer)
    if timer <= 0 then
        gameManager.setStateAt(1)
    elseif timer <= 10 then
        gameManager.setStateAt(2)
    elseif timer <= 20 then
        gameManager.setStateAt(3)
    elseif timer <= 30 then
        gameManager.setStateAt(4)
    elseif timer <= 40 then
        gameManager.setStateAt(5)
    end
    countdown.setTime(timer)
end

return timeManager

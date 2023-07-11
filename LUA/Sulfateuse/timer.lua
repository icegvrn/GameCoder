timer = {}
currentValue = 0
start = false

function timer.newTimer(self, p_duration)
    if start == false then
        currentValue = p_duration
        start = true
    end
end

function timer.runTimer(self, dt)
    if start == true then
        if currentValue <= 0 then
            start = false
        end
        currentValue = currentValue - dt
    end
end

function timer.isStarted(self)
    return start
end

function timer.getCurrentValue(self)
    return currentValue
end
return timer

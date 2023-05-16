local c_Controller = {}
local Controller_mt = {__index = c_Controller}

function c_Controller.new()
    local controller = {
        isPlayer = false,
        ennemiAgent = nil,
        canMove = true,
        speed = 0.3,
        target = love.mouse,
        isCinematicMode = false,
        directionTimer = 0.01,
        directionTimerIsStarted = false,
        directionCanBeChange = false, 
        lookAt = CONST.DIRECTION.left
    }
    return setmetatable(controller, Controller_mt)
end

return c_Controller

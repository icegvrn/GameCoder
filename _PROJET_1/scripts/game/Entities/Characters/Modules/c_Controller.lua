local mapManager = require("scripts/game/managers/mapManager")
local c_Controller = {}
local Controller_mt = {__index = c_Controller}

function c_Controller.new()
    local controller = {}
    return setmetatable(controller, Controller_mt)
end

function c_Controller:create()
    local controller = {
        ennemiAgent = nil,
        player = nil,
        canMove = true,
        speed = 0.3,
        target = love.mouse,
        isCinematicMode = false,
        directionTimer = 0.01,
        directionTimerIsStarted = false,
        directionCanBeChange = false,
        lookAt = CONST.DIRECTION.left
    }

    function controller:lookAtRightDirection(dt, parent)
        local x, y = self.target:getPosition()
        local px, py = parent.transform.position.x, parent.transform.position.y

        if (self.target == love.mouse) then
            x, y = Utils.mouseToWorldCoordinates(x, y)
        end

        if self.player or parent.state == CHARACTERS.STATE.ALERT or parent.state == CHARACTERS.STATE.FIRE then
            if x > px then
                self:changeDirection(parent, CONST.DIRECTION.right, dt)
            end

            if x < px then
                self:changeDirection(parent, CONST.DIRECTION.left, dt)
            end
        end
    end

    function controller:changeDirection(parent, direction, dt)
        if self.lookAt ~= direction then
            self.lookAt = direction
        end
    end

    function controller:setTarget(target)
        self.target = target
    end

    function controller:setSpeed(speed)
        self.speed = speed
    end

    function controller:setInCinematicMode(parent, bool)
        self.isCinematicMode = bool
        if self.player and bool then
            parent.transform:setPosition(
                -10,
                (mapManager:getCurrentMap().height * mapManager:getCurrentMap().tileheight / 2)
            )
        end
    end

    function controller:isInCinematicMode()
        return self.isCinematicMode
    end

    return controller
end

return c_Controller

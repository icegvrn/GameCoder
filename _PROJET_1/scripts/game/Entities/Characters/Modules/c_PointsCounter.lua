local c_PointsCounter = {}
local PointsCounter_mt = {__index = c_PointsCounter}

function c_PointsCounter.new()
    local _PointsCounter = {}
    return setmetatable(_PointsCounter, PointsCounter_mt)
end

function c_PointsCounter:create()
    local pointsCounter = {
        points = 0,
        maxPoints = 50
    }

    function pointsCounter:update(dt, player)
        if player.character:getMode() == CHARACTERS.MODE.BOOSTED and self.points == self.maxPoints then
            self:resetPoints()
        end
    end

    function pointsCounter:addPoints(points)
        if self.points < self.maxPoints then
            self.points = self.points + 1
        end
    end

    function pointsCounter:resetPoints()
        self.points = 0
    end

    return pointsCounter
end

return c_PointsCounter

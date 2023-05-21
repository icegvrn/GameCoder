local c_PointsUI = {}
local PointsUI_mt = {__index = c_PointsUI}

function c_PointsUI.new()
    local _pointsUI = {
        font10 = love.graphics.newFont(10),
        font15 = love.graphics.newFont(15),
        defaultFont = love.graphics.newFont()
    }
    return setmetatable(_pointsUI, PointsUI_mt)
end

function c_PointsUI:create()
    local pointsUI = {font10 = self.font10, font15 = self.font15, defaultFont = self.defaultFont}

    function pointsUI:showPoints(points, color, font, x, y)
        love.graphics.setFont(font)
        love.graphics.setColor(color)
        points = "+" .. points .. " points"
        love.graphics.print(points, x, y)
        love.graphics.setFont(self.defaultFont)
        love.graphics.setColor(1, 1, 1)
    end

    return pointsUI
end

return c_PointsUI

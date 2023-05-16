local c_PointsUI = {}
local PointsUI_mt = {__index = c_PointsUI}

function c_PointsUI.new()
    local pointsUI = {
        font10 = love.graphics.newFont(10),
        font15 = love.graphics.newFont(15),
        defaultFont = love.graphics.newFont(),
        alertImg = love.graphics.newImage("contents/images/characters/exclamation.png")
    }
    return setmetatable(pointsUI, PointsUI_mt)
end

return c_PointsUI

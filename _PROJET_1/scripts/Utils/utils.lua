camera = require("scripts/game/mainCamera")
Utils = {}
Utils.screenWidth = love.graphics.getWidth()
Utils.screenHeight = love.graphics.getHeight()

function Utils.screenCoordinates(x, y)
    local cX, cY = mainCamera.camera:getPosition()
    x = x + cX
    y = y + cY
    return x, y
end

function Utils.mouseToWorldCoordinates(x, y)
    return Utils.screenCoordinates(x, y)
end

function Utils.distance(xA, yA, xB, yB)
    return math.sqrt((xB - xA) ^ 2 + (yB - yA) ^ 2) ^ 0.5
end

function Utils.angle(x1, y1, x2, y2)
    return math.atan2(y2 - y1, x2 - x1)
end

return Utils

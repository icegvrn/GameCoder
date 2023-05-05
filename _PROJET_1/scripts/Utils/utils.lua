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

function Utils.angleWithMouse(x1, y1)
    local x2, y2 = love.mouse.getPosition()
    return math.atan2(y2 - y1, x2 - x1)
end

function Utils.angleWithMouseWorldPosition(x1, y1)
    local mx, my = love.mouse.getPosition()
    local cX, cY = mainCamera.camera:getPosition()
    local x2 = mx + cX
    local y2 = my + cY

    return math.atan2(y2 - y1, x2 - x1)
end

function Utils.isCollision(x1, y1, w1, h1, x2, y2, w2, h2)
    local topRight1 = x1 + w1
    local bottomLeft1 = y1 + h1
    local topRight2 = x2 + w2
    local bottomLeft2 = y2 + h2

    if x1 > topRight2 or topRight1 < x2 or y1 > bottomLeft2 or bottomLeft1 < y2 then
        return false
    else
        return true
    end
end

return Utils

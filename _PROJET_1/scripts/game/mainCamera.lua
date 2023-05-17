camera = require("scripts/engine/camera")
map = require("scripts/game/gameMap")
--player = require("scripts/game/player")
mainCamera = {}
mainCamera.camera = camera.new()
mainCamera.originalTarget = nil
mainCamera.isLocked = false
destX = 0
destY = 0

function mainCamera.follow(target)
    mainCamera.camera:setTarget(target)
    mainCamera.originalTarget = target
end

function mainCamera.update(dt)
    local targetPosition = mainCamera.camera:getTarget():getPosition()
    mainCamera.camera:setTargetPosition(targetPosition)
    destX, destY = mainCamera.camera:calcSmoothDestination(dt)

end

function mainCamera.draw()
    love.graphics.translate(-destX, -destY)
end

function mainCamera.getPosition()
    if mainCamera.isLocked then
        local mapWidth, mapHeight = map.getMapDimension()
        return Utils.screenWidth / 2, mapHeight / 2
    else
        return mainCamera.camera:getPosition()
    end
end

function mainCamera.lock(bool)
    if bool then
        mainCamera.camera:setTarget(mainCamera)
        mainCamera.isLocked = true
    else
        mainCamera.camera:setTarget(mainCamera.originalTarget)
        mainCamera.isLocked = false
    end
end

return mainCamera

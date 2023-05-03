camera = require("scripts/engine/camera")
--player = require("scripts/game/player")
mainCamera = {}
mainCamera.camera = camera.new()
destX = 0
destY = 0

function mainCamera.follow(target)
    mainCamera.camera:setTarget(target)
end

function mainCamera.update(dt)
    local targetPosition = mainCamera.camera:getTarget():getPosition()
    mainCamera.camera:setTargetPosition(mainCamera.camera:getTarget():getPosition())
    destX, destY = mainCamera.camera:calcSmoothDestination(dt)
end

function mainCamera.draw()
    love.graphics.translate(-destX, -destY)
end

return mainCamera

local mapManager = require("scripts/game/managers/mapManager")

mainCamera = {}
mainCamera.originalTarget = nil
mainCamera.isLocked = false
mainCamera.x = 0
mainCamera.y = 0
mainCamera.smooth = 0.7
mainCamera.target = nil
mainCamera.targetX = 0
mainCamera.targetY = 0
destX = 0
destY = 0

function mainCamera.follow(target)
    mainCamera.originalTarget = target
    mainCamera.target = target
end

function mainCamera.update(dt)
    mainCamera.targetX, mainCamera.targetY = mainCamera.target:getPosition()
    destX, destY = mainCamera:calcSmoothDestination(dt)
end

function mainCamera.draw()
    love.graphics.translate(-destX, -destY)
end

function mainCamera:getPosition()
    -- Pour qu'elle s'appuie sur-elle même quand elle est en mode lock
    if mainCamera.isLocked then
        local mapWidth, mapHeight = mapManager:getMapDimension()
        return Utils.screenWidth / 2, mapHeight / 2
    else
        -- Pour renvoyer le chiffre normal quand elle est pas lock
        return mainCamera.x, mainCamera.y
    end
end

function mainCamera.lock(bool)
    print(bool)
    if bool then
        mainCamera.target = mainCamera
        mainCamera.isLocked = true
    else
        mainCamera.target = mainCamera.originalTarget
        mainCamera.isLocked = false
    end
end

function mainCamera:calcSmoothDestination(dt)
    -- Si j'ai attribué une cible à ma caméra et que la fonction getPosition() existe sur celle-ci
    if self.target ~= nil then
        -- Calcul de là où la caméra doit aller ; elle arrive à zéro quand elle a parcouru la distance
        destX = self.targetX - (love.graphics.getWidth() / 2) - self.x
        destY = self.targetY - (love.graphics.getHeight() / 2) - self.y

        -- La caméra y est emmené selon la vitesse et le dt
        self.x = self.x + (destX / self.smooth) * dt
        self.y = self.y + (destY / self.smooth) * dt

        return self.x, self.y
    end
end

return mainCamera

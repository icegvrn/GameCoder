-- MODULE PERMETTANT D'AVOIR UNE CAMERA QUI FOLLOW LE JOUEUR OU EST FIXE EN MODE CINEMATIQUE, AVEC UN PEU DE SMOOTHNESS
local mapManager = require(PATHS.MAPMANAGER)

mainCamera = {
    originalTarget = nil,
    isLocked = false,
    x = 0,
    y = 0,
    smooth = 0.1,
    target = nil,
    targetX = 0,
    targetY = 0
}

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
        mainCamera.smooth = 0.7
        local mapWidth, mapHeight = mapManager:getMapDimension()
        return Utils.screenWidth / 2, mapHeight / 2
    else
        mainCamera.smooth = 0.05
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
    if self.target ~= nil then
        local destX = self.targetX - (love.graphics.getWidth() / 2) - self.x
        local destY = self.targetY - (love.graphics.getHeight() / 2) - self.y

        self.x = self.x + (destX / self.smooth) * dt
        self.y = self.y + (destY / self.smooth) * dt

        return math.ceil(self.x), math.ceil(self.y)
    end
end

return mainCamera

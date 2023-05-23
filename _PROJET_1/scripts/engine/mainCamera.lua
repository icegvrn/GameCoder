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
    targetY = 0,
    destX = 0,
    destY = 0
}

-- Intialisation de la cible que la caméra va devoir suivre
function mainCamera.follow(target)
    mainCamera.originalTarget = target
    mainCamera.target = target
end

-- Récupération de la position actuelle de la cible et calcul du translate à effectuer
function mainCamera.update(dt)
    mainCamera.targetX, mainCamera.targetY = mainCamera.target:getPosition()
    mainCamera.destX, mainCamera.destY = mainCamera:calcSmoothDestination(dt)
end

-- Draw du translate une fois calculé 
function mainCamera.draw()
    love.graphics.translate(-mainCamera.destX, -mainCamera.destY)
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

-- Fonction permettant de lock la caméra en changeant sa cible : soit la cible originale (ici le joueur)
-- soit elle est sur elle-même, donc elle ne bouge pas
function mainCamera.lock(bool)
    if bool then
        mainCamera.target = mainCamera
        mainCamera.isLocked = true
    else
        mainCamera.target = mainCamera.originalTarget
        mainCamera.isLocked = false
    end
end

-- Calcul la destination de la caméra à chaque frame de façon smooth
function mainCamera:calcSmoothDestination(dt)
    if self.target ~= nil then
        -- On soustrait la cible à la position actuelle, ainsi que le centre de l'écran pour voir la cible au milieu
        local destX = self.targetX - (Utils.screenWidth / 2) - self.x
        local destY = self.targetY - (Utils.screenHeight / 2) - self.y

        -- On calcul la position suivante via un calcul de vélocité
        self.x = self.x + (destX / self.smooth) * dt
        self.y = self.y + (destY / self.smooth) * dt

        return math.floor(self.x), math.floor(self.y)
    end
end

return mainCamera

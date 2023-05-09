local Camera = {}
local camera_mt = {__index = Camera}

function Camera.new()
    local newCamera = {}
    newCamera.x = 0
    newCamera.y = 0
    newCamera.smooth = 0.7
    newCamera.target = nil
    newCamera.destinationX = 0
    newCamera.destinationY = 0

    return setmetatable(newCamera, camera_mt)
end

function Camera:CreateCamera()
end

function Camera:getPosition()
    return self.x, self.y
end

function Camera:getSmooth()
    return self.smooth
end

function Camera:getTarget()
    return self.target
end

function Camera:setPosition(p_x, p_y)
    self.x = p_x
    self.y = p_y
end
function Camera:setSmooth(p_smooth)
    self.smooth = p_smooth
end

function Camera:setTarget(p_target)
    self.target = p_target
end

function Camera:setTargetPosition(p_targetX, p_targetY)
    self.targetPosX = p_targetX
    self.targetPosY = p_targetY
end

function Camera:calcSmoothDestination(dt)
    -- Si j'ai attribué une cible à ma caméra et que la fonction getPosition() existe sur celle-ci
    if self.target ~= nil then
        if self.target:getPosition() then
            local targetPosX, targetPosY = self.target:getPosition()

            -- Calcul de là où la caméra doit aller
            destX = targetPosX - love.graphics.getWidth() / 2 - self.x
            destY = targetPosY - love.graphics.getHeight() / 2 - self.y

            -- La caméra y est emmené selon la vitesse et le dt
            self.x = self.x + (destX / self.smooth) * dt
            self.y = self.y + (destY / self.smooth) * dt

            return self.x, self.y
        end
    end
end

return Camera

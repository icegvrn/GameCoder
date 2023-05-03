require("scripts/states/CONST")
utils = require("scripts/Utils/utils")

local weapon = {}
local weapons_mt = {__index = weapon}

function weapon.new()
    newWeapon = {}
    newWeapon.name = "inconnu"
    newWeapon.damage = 10
    newWeapon.owner = self
    newWeapon.isRangedWeapon = false
    newWeapon.weaponRange = 7
    newWeapon.isVisible = true
    newWeapon.isFiring = false
    newWeapon.speed = 5
    newWeapon.transform = {}
    newWeapon.transform.position = {}
    newWeapon.transform.position.x = 100
    newWeapon.transform.position.y = 100

    newWeapon.hitBox = {}
    newWeapon.hitBox.offset = {}
    newWeapon.hitBox.offset.x = 0
    newWeapon.hitBox.offset.y = 0

    newWeapon.hitBox.position = {}
    newWeapon.hitBox.position.x = 0
    newWeapon.hitBox.position.y = 0

    newWeapon.hitBox.size = {}
    newWeapon.hitBox.size.x = 10
    newWeapon.hitBox.size.y = 10

    newWeapon.transform.rotation = {}
    newWeapon.transform.rotation.x = 0
    newWeapon.transform.rotation.y = 0
    newWeapon.transform.scale = {}
    newWeapon.transform.scale.x = 1
    newWeapon.transform.scale.y = 1
    newWeapon.initialScale = 1

    newWeapon.holdingOffset = {}
    newWeapon.holdingOffset.x = 0
    newWeapon.holdingOffset.y = 0

    newWeapon.idleAngle = 45
    newWeapon.currentAngle = 0
    newWeapon.canFire = true
    newWeapon.lookAt = CONST.DIRECTION.left
    newWeapon.sprites = {}

    newWeapon.currentSpriteId = 1

    newWeaponFireList = {}

    newWeapon.rotationAngle = 0
    newWeapon.timer = 0

    return setmetatable(newWeapon, weapons_mt)
end

function weapon:setWeaponVisible(bool)
    self.isVisible = bool
end

function weapon:isWeaponVisible()
    return self.isVisible
end

function weapon:setDamageValue(dmg)
    self.damage = dmg
end

function weapon:getDamage()
    return self.damage
end

function weapon:setHitBoxSize(sizeX, sizeY)
    self.hitBox.size.x = sizeX
    self.hitBox.size.y = sizeY
end

function weapon:getHitBoxSize()
    return self.hitBox.size.x, self.hitBox.size.y
end

function weapon:setRangedWeapon(bool)
    self.isRangedWeapon = bool
end

function weapon:isRangedWeapon()
    return self.isRangedWeapon
end

function weapon:setWeaponRange(nb)
    self.weaponRange = nb
end

function weapon:getWeaponRange()
    return self.weaponRange
end

function weapon:setSprites(p_table)
    self.sprites = p_table
end

function weapon:setCurrentSpriteId(nb)
    self.currentSpriteId = nb
end

function weapon:getCurrentSpriteId()
    return self.currentSpriteId
end

function weapon:setAngle(angle)
    self.currentAngle = angle
end

function weapon:getAngle()
    return self.currentAngle
end

function weapon:setName(name)
    self.name = name
end

function weapon:getName()
    return self.name
end

function weapon:getSprite(id)
    return self.sprites[id]
end

function weapon:getSprites()
    return self.sprites
end

function weapon:getPosition()
    return self.transform.position.x, self.transform.position.y
end

function weapon:setPosition(x, y)
    self.transform.position.x, self.transform.position.y = x, y
end

function weapon:setSpeed(pSpeed)
    self.speed = pSpeed
end

function weapon:getSpeed()
    return self.speed
end

function weapon:setState(state)
    self.state = state
end

function weapon:getState()
    return self.state
end

function weapon:changeDirection(direction)
    self.lookAt = direction
end

function weapon:getDirection()
    return self.lookAt
end

function weapon:setStrenght(nb)
    self.strenght = nb
end

function weapon:getDirection()
    return self.strenght
end

function weapon:setHoldingOffset(x, y)
    self.holdingOffset.x = x
    self.holdingOffset.y = y
end

function weapon:getHoldingOffset()
    return self.holdingOffset.x, self.holdingOffset.y
end

function weapon:draw()
    if self.isVisible then
        self:drawWeapon()
        self:drawFiredElements()
    end
end

function weapon:drawFiredElements()
    if (self.isRangedWeapon) then
        if newWeaponFireList then
            for k, v in ipairs(newWeaponFireList) do
                love.graphics.circle("fill", v.x, v.y, 5, 5)
            end
        end
    end
end

function weapon:setOwner(owner)
    self.owner = owner
end

function weapon:setFireOffset(x, y)
    self.hitBox.offset.x = x
    self.hitBox.offset.y = y
end

function weapon:getFireOffset()
    return self.hitBox.position.x, self.hitBox.position.y
end

function weapon:update(dt)
    self:updateFiredElements(dt)
end

function weapon:fire(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    self.isFiring = true

    if self.isRangedWeapon then
        self.timer = self.timer + dt
        if self.timer >= self.speed then
            self.canFire = true
            self.timer = 0
        end

        local pX, pY = ownerPosition.x, ownerPosition.y
        local mX, mY = ownerTarget:getPosition()

        if ownerTarget == love.mouse then
            mX, mY = utils.mouseToWorldCoordinates(love.mouse.getPosition())
        end

        local angle = math.atan2(mY - pY, mX - pX)
        self.currentAngle = angle
        self:move(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)

        if self.canFire == true then
            local fire = {}
            fire.x = self.hitBox.position.x
            fire.y = self.hitBox.position.y
            fire.angle = angle
            fire.speed = 180 * dt
            fire.lifeTime = 2
            fire.distance = utils.distance(pX, pY, mX, mY)

            table.insert(newWeaponFireList, fire)
            self.canFire = false
        end
    else
        self.rotationAngle = self.rotationAngle + self.speed * dt
    end

    self.isFiring = false
end

function weapon:updateFiredElements(dt)
    if (self.isRangedWeapon) then
        if newWeaponFireList then
            for n = #newWeaponFireList, 1, -1 do
                local t = newWeaponFireList[n]
                t.x = t.x + t.speed * math.cos(t.angle)
                t.y = t.y + t.speed * math.sin(t.angle)
                t.lifeTime = t.lifeTime - dt
                if t.lifeTime <= 0 then
                    table.remove(newWeaponFireList, n)
                end
            end
        end
    end
end

function weapon:drawWeapon()
    if self.sprites[self.currentSpriteId] ~= nil then
        love.graphics.draw(
            self.sprites[self.currentSpriteId],
            self.transform.position.x,
            self.transform.position.y,
            self.currentAngle + math.sin(self.rotationAngle) * 0.5,
            self.transform.scale.x,
            self.transform.scale.y,
            (self.sprites[self.currentSpriteId]:getWidth() / 2) + self.holdingOffset.x,
            (self.sprites[self.currentSpriteId]:getHeight() / 2) + self.holdingOffset.y
        )

    -- DEBUG (NE PAS DELETE) : Dessin de l'image de tir
    --love.graphics.circle("fill", self.hitBox.position.x, self.hitBox.position.y, 6, 6)
    end
end

function weapon:move(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    self:calcWeaponPosition(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    self:calcWeaponAngle(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    self:calcWeaponScale(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    self:calcWeaponHitZone(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
end

function weapon:calcWeaponPosition(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    if self.owner then
        self.transform.position.x = ownerHandPosition.x
        self.transform.position.y = ownerHandPosition.y
    end
end

function weapon:calcWeaponAngle(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    local ownerDirectionX, ownerDirectionY = ownerScale.x, ownerScale.y
    local weaponAngle = 0

    if self.isFiring == false then
        weaponAngle = math.rad(-self.idleAngle * ownerDirectionX)
    else
        local pX, pY = self.hitBox.position.x, self.hitBox.position.y
        local mX, mY = ownerTarget:getPosition()
        weaponAngle = math.atan2((mY - pY) * ownerDirectionX, (mX - pX) * ownerDirectionX)
    end

    if self.owner:isThePlayer() then
        -- Permet de "tracker" la souris par le calcul atan2 qui calcul l'angle entre deux vecteurs pour modifier l'angle si le personnage est joueur
        local mouseWorldPositionX, mouseWorldPositionY = utils.mouseToWorldCoordinates(love.mouse.getPosition())
        weaponAngle =
            math.atan2(
            (mouseWorldPositionY - ownerPosition.y) * ownerDirectionX,
            (mouseWorldPositionX - ownerPosition.x) * ownerDirectionX
        )
    end
    self.currentAngle = weaponAngle
end

function weapon:calcWeaponScale(ddt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    local ownerPositionX, ownerPositionY = ownerPosition.x, ownerPosition.y
    local ownerDirectionX, ownerDirectionY = ownerScale.x, ownerScale.y
    self.transform.scale.x = ownerWeaponScaling * ownerDirectionX
    self.transform.scale.y = ownerWeaponScaling * ownerDirectionY
end

function weapon:calcWeaponHitZone(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    local ownerHandPositionX, ownerHandPositionY = ownerHandPosition.x, ownerHandPosition.y
    local ownerDirectionX, ownerDirectionY = ownerScale.x, ownerScale.y
    local handX, handY = self.transform.position.x, self.transform.position.y

    local weaponLength =
        (((self.sprites[self.currentSpriteId]:getWidth() / 2) - self.holdingOffset.x) * ownerWeaponScaling) *
        ownerDirectionX

    local weaponHitPositionX = handX + weaponLength * math.sin(math.cos(self.currentAngle)) * ownerDirectionX
    local weaponHitPositionY = handY + weaponLength * math.sin(math.sin(self.currentAngle)) * ownerDirectionY

    if ownerDirectionX < 0 then
        weaponHitPositionX = handX - weaponLength * math.cos(self.currentAngle) * ownerDirectionX
    end
    if ownerDirectionY < 0 then
        weaponHitPositionY = handY - weaponLength * math.sin(self.currentAngle) * ownerDirectionY
    end

    self.hitBox.position.x = weaponHitPositionX
    self.hitBox.position.y = weaponHitPositionY
end

return weapon

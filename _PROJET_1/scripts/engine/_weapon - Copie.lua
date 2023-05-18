require("scripts/states/CONST")
utils = require("scripts/Utils/utils")
local map = require("scripts/game/gameMap")
Transform = require("scripts/engine/transform")

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
    newWeapon.transform = Transform.new()
    newWeapon.transform = newWeapon.transform:create()
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

    newWeapon.FireList = {}

    newWeapon.rotationAngle = 0
    newWeapon.timer = 0
    newWeapon.timerIsStarted = false

    newWeapon.hittableCharacters = {}
    lifeFactor = 0.7

    newWeapon.sounds = {}
    newWeapon.sounds[1] = PATHS.SOUNDS.GAME .. "heros_hitted.wav"
    newWeapon.sounds[2] = PATHS.SOUNDS.GAME .. "heros_hitted.wav"
    newWeapon.soundVolume = 0.3
    newWeapon.playSound = false

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

function weapon:setSounds(array)
    for i = 1, #array do
        self.sounds[i] = array[i]
    end
end

function weapon:setSoundsVolume(nb)
    self.soundVolume = nb
end

function weapon:getSound()
    return self.sounds[1], self.sounds[2]
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

function weapon:getIsRangedWeapon()
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
    self.timer = pSpeed
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

function weapon:setHoldingOffset(array)
    self.holdingOffset.x = array[1]
    self.holdingOffset.y = array[2]
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
        if self.FireList then
            for k, v in ipairs(self.FireList) do
                if self.owner.controller.player then
                    love.graphics.setColor(1, 0.1, 0, 1)
                    love.graphics.circle("fill", v.x, v.y, 6)
                    for n = 1, #v.list_trail do
                        local t = v.list_trail[n]
                        love.graphics.setColor(t.color, 0.1, 0.8, t.vie)
                        love.graphics.circle("fill", t.x, t.y, 6)
                    end
                else
                    love.graphics.setColor(0, 1, 0.1, 0.8)
                    love.graphics.circle("fill", v.x, v.y, 4)
                    for n = 1, #v.list_trail do
                        local t = v.list_trail[n]
                        love.graphics.setColor(t.color, 0.8, 0.1, t.vie)
                        love.graphics.circle("fill", t.x, t.y, 4)
                    end
                end
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
    if self.owner.controller.player then
        self.hittableCharacters = levelManager.getListofEnnemiesCharacters()
        if self.owner:getMode() == CHARACTERS.MODE.BOOSTED then
            self:boostOwner(dt)
        end
    else
        if self.hittableCharacters[1] ~= self.owner.controller.target then
            self.hittableCharacters[1] = self.owner.controller.target
        end
    end

    self:updateFiredElements(dt)
    self:checkIfWeaponCanFire(dt)
end

function weapon:checkIfWeaponCanFire(dt)
    if self.timerIsStarted then
        self.timer = self.timer + dt

        if self.timer >= self.speed then
            self.canFire = true
            self.timer = 0
        end
    end
end

function weapon:boostOwner(dt)
    local x, y = self.owner.transform:getPosition()
    for c = #self.hittableCharacters, 1, -1 do
        if self:isCollide(x, y, self.owner.sprites.height, self.owner.sprites.width, self.hittableCharacters[c]) then
            self:fire(dt)
            self.hittableCharacters[c].fight:hit(self.owner, self.damage)
        end
    end
end

function weapon:fire(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
    self.isFiring = true

    if self.timerIsStarted == false then
        self.timerIsStarted = true
    end

    if self.canFire then
        -- Lecture entre deux sons, pour avoir des sons plus naturels
        local nb = love.math.random(1, #self.sounds)
        soundManager:playSound(self.sounds[nb], self.soundVolume, false)

        if self.isRangedWeapon then
            local pX, pY = self.hitBox.position.x, self.hitBox.position.y
            local mX, mY = ownerTarget:getPosition()
            local angle = math.atan2(mY - pY, mX - pX)

            if ownerTarget == love.mouse then
                angle = Utils.angleWithMouseWorldPosition(pX, pY)
            end

            self.currentAngle = angle
            self:move(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)

            local fire = {}
            fire.x = self.hitBox.position.x
            fire.y = self.hitBox.position.y
            fire.angle = angle
            fire.speed = 180 * dt
            fire.lifeTime = 2
            fire.size = 5
            fire.list_trail = {}
            table.insert(self.FireList, fire)
        end
        self.canFire = false
    end
    if self.isRangedWeapon == false then
        self:playAttackAnimation(dt)
    end
    self.isFiring = false
end

function weapon:AddTrailToBall(dt, fireList)
    for i = #fireList, 1, -1 do
        for n = #fireList[i].list_trail, 1, -1 do
            local t = fireList[i].list_trail[n]
            t.vie = t.vie - dt + lifeFactor * dt
            t.color = t.color - 0.7 * dt
            t.x = t.x + t.vx
            t.y = t.y + t.vy
            if t.vie <= 0 then
                table.remove(fireList[i].list_trail, n)
            end
        end

        local maTrainee = {}
        maTrainee.vx = math.random(-0.1, 0.1)
        maTrainee.vy = math.random(-0.02, 0.02)
        maTrainee.x = fireList[i].x
        maTrainee.y = fireList[i].y
        maTrainee.color = 1
        maTrainee.vie = 0.4

        table.insert(fireList[i].list_trail, maTrainee)
    end
end
function weapon:updateFiredElements(dt)
    if (self.isRangedWeapon) then
        if self.FireList then
            for n = #self.FireList, 1, -1 do
                local t = self.FireList[n]
                t.x = t.x + t.speed * math.cos(t.angle)
                t.y = t.y + t.speed * math.sin(t.angle)
                t.lifeTime = t.lifeTime - dt

                if t.lifeTime <= 0 then
                    table.remove(self.FireList, n)
                elseif map.isThereASolidElement(t.x, t.y, t.size, t.size, c) then
                    table.remove(self.FireList, n)
                else
                    for c = #self.hittableCharacters, 1, -1 do
                        if self:isCollide(t.x, t.y, t.size, t.size, self.hittableCharacters[c]) then
                            self.hittableCharacters[c].fight:hit(self.owner, self.damage)
                            table.remove(self.FireList, n)
                        end
                    end
                end
            end
            weapon:AddTrailToBall(dt, self.FireList)
        end
    end
end

function weapon:isCollide(weaponX, weaponY, weaponWidth, weaponHeight, p_character)
    return p_character.collider:isCharacterCollidedBy(p_character, weaponX, weaponY, weaponWidth, weaponHeight)
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

    if self.owner.controller.player then
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

function weapon:playAttackAnimation(dt)
    self.rotationAngle = self.rotationAngle + self.speed * 10 * dt
end

function weapon:clear()
    for n = #self.FireList, 1, -1 do
        table.remove(self.FireList, n)
    end
end

return weapon

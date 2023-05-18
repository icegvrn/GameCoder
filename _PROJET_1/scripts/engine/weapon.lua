require("scripts/states/CONST")
utils = require("scripts/Utils/utils")
local map = require("scripts/game/gameMap")
Transform = require("scripts/engine/transform")
Sound = require("scripts/game/Entities/Characters/Modules/c_Sound")
HeldSlot = require("scripts/game/Entities/Characters/Modules/c_HeldSlot")
Hitbox = require("scripts/game/Entities/Characters/Modules/c_Hitbox")
AutoAttack = require("scripts/game/Entities/Characters/Modules/c_AutoAttack")
Sprites = require("scripts/game/Entities/Characters/Modules/c_Sprites")

local Weapon = {}
local weapons_mt = {__index = Weapon}

function Weapon.new()
    weapon = {
        transform = Transform.new(),
        sound = Sound.new(),
        hitBox = Hitbox.new(),
        heldSlot = HeldSlot.new(),
        autoAttack = AutoAttack.new(),
        sprites = Sprites.new()
    }
    return setmetatable(weapon, weapons_mt)
end

function Weapon:create()
    local weapon = {}
    weapon.transform = self.transform:create()
    weapon.sounds = self.sound:create()
    weapon.hitBox = self.hitBox:create()
    weapon.heldSlot = self.heldSlot:create()
    weapon.autoAttack = self.autoAttack.create()
    weapon.sprites = self.sprites.create()
    weapon.name = "inconnu"
    weapon.owner = self
    weapon.lookAt = CONST.DIRECTION.left
    weapon.hittableCharacters = {}

    function weapon:setWeaponVisible(bool)
        self.isVisible = bool
    end

    function weapon:isWeaponVisible()
        return self.isVisible
    end

    function weapon:setDamageValue(dmg)
        self.autoAttack.damage = dmg
    end

    function weapon:getDamage()
        return self.autoAttack.damage
    end

    function weapon:setHitBoxSize(sizeX, sizeY)
        self.hitBox.size.x = sizeX
        self.hitBox.size.y = sizeY
    end

    function weapon:getHitBoxSize()
        return self.hitBox.size.x, self.hitBox.size.y
    end

    function weapon:setRangedWeapon(bool)
        self.autoAttack.isRangedWeapon = bool
    end

    function weapon:getIsRangedWeapon()
        return self.autoAttack.isRangedWeapon
    end

    function weapon:setWeaponRange(nb)
        self.autoAttack.weaponRange = nb
    end

    function weapon:getWeaponRange()
        if self.autoAttack.weaponRange > 0 then
            return self.autoAttack.weaponRange
        else
            return false
        end
    end

    function weapon:setSprites(p_table)
        self.sprites = p_table
    end

    function weapon:setCurrentSpriteId(nb)
        self.sprites.currentSpriteId = nb
    end

    function weapon:getCurrentSpriteId()
        return self.sprites.currentSpriteId
    end

    function weapon:setAngle(angle)
        self.sprites.currentAngle = angle
    end

    function weapon:getAngle()
        return self.sprites.currentAngle
    end

    function weapon:setName(name)
        self.name = name
    end

    function weapon:getName()
        return self.name
    end

    function weapon:getSprite(id)
        return self.sprites.spritesList[id]
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
        self.autoAttack.speed = pSpeed
        self.autoAttack.timer = pSpeed
    end

    function weapon:getSpeed()
        return self.autoAttack.speed
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
        self.heldSlot.holdingOffset.x = array[1]
        self.heldSlot.holdingOffset.y = array[2]
    end

    function weapon:getHoldingOffset()
        return self.heldSlot.holdingOffset.x, self.heldSlot.holdingOffset.y
    end

    function weapon:draw()
        self:drawWeapon()
        self:drawFiredElements()
    end

    function weapon:drawFiredElements()
        if (self.autoAttack.isRangedWeapon) then
            if self.autoAttack.FireList then
                for k, v in ipairs(self.autoAttack.FireList) do
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
        if self.autoAttack.timerIsStarted then
            self.autoAttack.timer = self.autoAttack.timer + dt

            if self.autoAttack.timer >= self.autoAttack.speed then
                self.autoAttack.canFire = true
                self.autoAttack.timer = 0
            end
        end
    end

    function weapon:boostOwner(dt)
        local x, y = self.owner.transform:getPosition()
        for c = #self.hittableCharacters, 1, -1 do
            if self:isCollide(x, y, self.owner.sprites.height, self.owner.sprites.width, self.hittableCharacters[c]) then
                self:fire(dt)
                self.hittableCharacters[c].fight:hit(self.owner, self.autoAttack.damage)
            end
        end
    end

    function weapon:fire(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
        self.autoAttack.isFiring = true

        if self.autoAttack.timerIsStarted == false then
            self.autoAttack.timerIsStarted = true
        end

        if self.autoAttack.canFire then
            -- Lecture entre deux sons, pour avoir des sons plus naturels
            local nb = love.math.random(1, #self.sounds.tracks)
            soundManager:playSound(self.sounds.tracks[nb], self.sounds.talkVolume, false)

            if self.autoAttack.isRangedWeapon then
                local pX, pY = self.hitBox.position.x, self.hitBox.position.y
                local mX, mY = ownerTarget:getPosition()
                local angle = math.atan2(mY - pY, mX - pX)

                if ownerTarget == love.mouse then
                    angle = Utils.angleWithMouseWorldPosition(pX, pY)
                end

                self.sprites.currentAngle = angle
                self:move(dt, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)

                local fire = {}
                fire.x = self.hitBox.position.x
                fire.y = self.hitBox.position.y
                fire.angle = angle
                fire.speed = 180 * dt
                fire.lifeTime = 2
                fire.size = 5
                fire.list_trail = {}
                table.insert(self.autoAttack.FireList, fire)
            end
            self.autoAttack.canFire = false
        end
        if self.autoAttack.isRangedWeapon == false then
            self:playAttackAnimation(dt)
        end
        self.autoAttack.isFiring = false
    end

    function weapon:AddTrailToBall(dt, fireList)
        for i = #fireList, 1, -1 do
            for n = #fireList[i].list_trail, 1, -1 do
                local t = fireList[i].list_trail[n]
                t.vie = t.vie - dt + self.autoAttack.lifeFactor * dt
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
        if (self.autoAttack.isRangedWeapon) then
            if self.autoAttack.FireList then
                for n = #self.autoAttack.FireList, 1, -1 do
                    local t = self.autoAttack.FireList[n]
                    t.x = t.x + t.speed * math.cos(t.angle)
                    t.y = t.y + t.speed * math.sin(t.angle)
                    t.lifeTime = t.lifeTime - dt

                    if t.lifeTime <= 0 then
                        table.remove(self.autoAttack.FireList, n)
                    elseif map.isThereASolidElement(t.x, t.y, t.size, t.size, c) then
                        table.remove(self.autoAttack.FireList, n)
                    else
                        for c = #self.hittableCharacters, 1, -1 do
                            if self:isCollide(t.x, t.y, t.size, t.size, self.hittableCharacters[c]) then
                                self.hittableCharacters[c].fight:hit(self.owner, self.autoAttack.damage)
                                table.remove(self.autoAttack.FireList, n)
                            end
                        end
                    end
                end
                weapon:AddTrailToBall(dt, self.autoAttack.FireList)
            end
        end
    end

    function weapon:isCollide(weaponX, weaponY, weaponWidth, weaponHeight, p_character)
        return p_character.collider:isCharacterCollidedBy(p_character, weaponX, weaponY, weaponWidth, weaponHeight)
    end

    function weapon:drawWeapon()
        if (self.sprites.spritesList[1][self.sprites.currentSpriteId] ~= nil) then
            love.graphics.draw(
                self.sprites.spritestileSheets[1],
                self.sprites.spritesList[1][self.sprites.currentSpriteId],
                self.transform.position.x,
                self.transform.position.y,
                self.sprites.currentAngle + math.sin(self.sprites.rotationAngle) * 0.5,
                self.transform.scale.x,
                self.transform.scale.y,
                (self.sprites.spritestileSheets[1]:getWidth() / 2) + self.heldSlot.holdingOffset.x,
                (self.sprites.spritestileSheets[1]:getHeight() / 2) + self.heldSlot.holdingOffset.y
            )

        -- DEBUG (NE PAS DELETE) : Dessin de l'image de tir
        -- love.graphics.circle("fill", self.hitBox.position.x, self.hitBox.position.y, 6, 6)
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

        if self.autoAttack.isFiring == false then
            weaponAngle = math.rad(-self.heldSlot.idleAngle * ownerDirectionX)
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
        self.sprites.currentAngle = weaponAngle
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
            (((self.sprites.spritestileSheets[1]:getWidth() / 2) - self.heldSlot.holdingOffset.x) * ownerWeaponScaling) *
            ownerDirectionX
        local weaponHitPositionX =
            handX + weaponLength * math.sin(math.cos(self.sprites.currentAngle)) * ownerDirectionX
        local weaponHitPositionY =
            handY + weaponLength * math.sin(math.sin(self.sprites.currentAngle)) * ownerDirectionY
        if ownerDirectionX < 0 then
            weaponHitPositionX = handX - weaponLength * math.cos(self.sprites.currentAngle) * ownerDirectionX
        end
        if ownerDirectionY < 0 then
            weaponHitPositionY = handY - weaponLength * math.sin(self.sprites.currentAngle) * ownerDirectionY
        end
        self.hitBox.position.x = weaponHitPositionX
        self.hitBox.position.y = weaponHitPositionY
    end

    function weapon:playAttackAnimation(dt)
        self.sprites.rotationAngle = self.sprites.rotationAngle + self.autoAttack.speed * 10 * dt
    end

    function weapon:clear()
        for n = #self.autoAttack.FireList, 1, -1 do
            table.remove(self.autoAttack.FireList, n)
        end
    end

    return weapon
end

return Weapon

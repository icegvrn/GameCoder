require("scripts/states/CONST")
ennemiAgent = require("scripts/engine/ennemiAgent")
utils = require("scripts/Utils/utils")

local Character = {}
local characters_mt = {__index = Character}

function Character.new()
    newCharacter = {}
    newCharacter.isPlayer = false
    newCharacter.name = "inconnu"
    newCharacter.height = 0
    newCharacter.width = 0
    newCharacter.maxPV = 50
    newCharacter.currentPV = 100
    newCharacter.strenght = 10
    newCharacter.canMove = true
    newCharacter.canFire = true
    newCharacter.isCinematicMode = false
    newCharacter.ennemiAgent = nil
    newCharacter.target = love.mouse
    newCharacter.lookAt = CONST.DIRECTION.left
    newCharacter.initialScale = 1
    newCharacter.speed = 0
    newCharacter.mode = CHARACTERS.MODE.NORMAL
    newCharacter.state = CHARACTERS.STATE.IDLE

    newCharacter.transform = {}
    newCharacter.transform.position = {}
    newCharacter.transform.position.x = 100
    newCharacter.transform.position.y = 100
    newCharacter.transform.rotation = {}
    newCharacter.transform.rotation.x = 0
    newCharacter.transform.rotation.y = 0
    newCharacter.transform.scale = {}
    newCharacter.transform.scale.x = 1
    newCharacter.transform.scale.y = 1

    newCharacter.weapon = {}
    newCharacter.currentWeaponId = 1
    newCharacter.weaponScaling = 1

    newCharacter.color = {1, 1, 1, 1}
    newCharacter.isHit = false
    newCharacter.timer = 0
    newCharacter.dieTimer = 0
    newCharacter.canBeHurt = true
    newCharacter.handPosition = {}
    newCharacter.handPosition.x = 0
    newCharacter.handPosition.y = 0
    newCharacter.handOffset = {}
    newCharacter.handOffset.x = 20
    newCharacter.handOffset.y = 15

    newCharacter.spritestileSheets = {}
    newCharacter.spritesList = {}
    newCharacter.currentSpriteId = 1
    newCharacter.font10 = love.graphics.newFont(10)
    newCharacter.font15 = love.graphics.newFont(15)
    newCharacter.defaultFont = love.graphics.newFont()
    newCharacter.alertImg = love.graphics.newImage("contents/images/characters/exclamation.png")

    return setmetatable(newCharacter, characters_mt)
end
function Character:setWeaponScaling(nb)
    self.weaponScaling = nb
end

function Character:getCurrentPV()
    return self.currentPV
end

function Character:setCurrentPV(pv)
    self.currentPV = pv
end

function Character:getHandOffset()
    return self.handOffset.x, self.handOffset.y
end

function Character:setHandOffset(x, y)
    self.handOffset.x = x
    self.handOffset.y = y
end

function Character:getHandPosition()
    return self.handPosition.x, self.handPosition.yA
end

function Character:setPlayer()
    self.isPlayer = true
end

function Character:isThePlayer()
    return self.isPlayer
end

function Character:getHeight()
    return self.height
end

function Character:getDimension()
    local c_state = self.mode .. "_" .. self.state
    local w = self.spritestileSheets[c_state]:getWidth() / 4
    local h = self.spritestileSheets[c_state]:getHeight()
    return w, h
end

function Character:setDimension(h, w)
    self.height = h
    self.width = w
end

function Character:getWeaponScaling()
    return self.weaponScaling
end
function Character:setSprites(p_table)
    local sprites = {}
    for k, sprite in pairs(p_table) do
        self.spritestileSheets[k] = sprite
        self.height = sprite:getHeight()
        self.width = sprite:getWidth() / 4
        local nbColumns = sprite:getWidth() / sprite:getHeight()
        local nbLine = sprite:getHeight() / self.height
        local id = 1

        local spriteTable = {}
        for c = 1, nbColumns do
            for l = 1, nbLine do
                spriteTable[id] =
                    love.graphics.newQuad(
                    (c - 1) * sprite:getWidth() / 4,
                    (l - 1) * sprite:getHeight(),
                    sprite:getWidth() / 4,
                    sprite:getHeight(),
                    sprite:getWidth(),
                    sprite:getHeight()
                )
                id = id + 1
            end
        end
        sprites[k] = spriteTable
    end

    self.spritesList = sprites
end

function Character:setCurrentSpriteId(nb)
    self.currentSpriteId = nb
end

function Character:getCurrentSpriteId()
    return self.currentSpriteId
end

function Character:getCurrentWeapon()
    return self.weapon[self.currentWeaponId]
end

function Character:setNB(nb)
    self.nb = nb
end

function Character:getNB()
    return self.nb
end

function Character:getScale()
    return self.transform.scale.x, self.transform.scale.y
end

function Character:setName(name)
    self.name = name
end

function Character:getName()
    return self.name
end

function Character:getSprite(id)
    return self.sprites[id]
end

function Character:getSprites()
    return self.sprites
end

function Character:getPosition()
    return self.transform.position.x, self.transform.position.y
end

function Character:setPosition(x, y)
    self.transform.position.x, self.transform.position.y = x, y
end

function Character:setSpeed(pSpeed)
    self.speed = pSpeed
end

function Character:getSpeed()
    return self.speed
end

function Character:setState(state)
    self.state = state
end

function Character:getState()
    return self.state
end

function Character:setMode(mode)
    self.mode = mode
    if mode == CHARACTERS.MODE.BOOSTED then
        self:changeWeapon(2)
    else
        self:changeWeapon(1)
    end
end

function Character:getMode()
    return self.mode
end

function Character:getCurrentTileSheet()
    local c_state = c
    return self.spritestileSheets[c_state]
end

function Character:setCurrentTileSheet(ts)
    self.currentTilesheet = ts
end

function Character:equip(p_weapon)
    p_weapon:setOwner(self)
    table.insert(self.weapon, p_weapon)
end

function Character:setMaxPV(pv)
    self.maxPV = pv
end

function Character:getMaxPV()
    return self.maxPV
end

function Character:changeDirection(direction)
    if direction == CONST.DIRECTION.left then
        if (self.transform.scale.x == self.initialScale) then
            self.transform.scale.x = -self.initialScale
            self.lookAt = direction
        end
    end

    if direction == CONST.DIRECTION.right then
        if (self.transform.scale.x ~= self.initialScale) then
            self.transform.scale.x = self.initialScale
            self.lookAt = direction
        end
    end
end

function Character:getDirection()
    return self.lookAt
end

function Character:setStrenght(nb)
    self.strenght = nb
end

function Character:getStrenght()
    return self.strenght
end

function Character:load()
end

function Character:setTarget(target)
    self.target = target
end

function Character:moveWeapon(angle)
    self.weapon:setAngle(angle)
end

function Character:draw()
    if self.ennemiAgent then
        self.ennemiAgent:draw()
    end
    if #self.weapon ~= 0 then
        self.weapon[self.currentWeaponId]:draw()
    end

    local c_state = self.mode .. "_" .. self.state
    local c_spriteID = math.floor(self.currentSpriteId)
    local px, py = self.transform.position.x, self.transform.position.y
    local x, y = Utils.screenCoordinates(self.target:getPosition())

    if (self.target ~= love.mouse) then
        px, py = Utils.screenCoordinates(px, py)
    end

    if self.isPlayer == true or self.state == CHARACTERS.STATE.ALERT or self.state == CHARACTERS.STATE.FIRE then
        if x > px then
            self:changeDirection(CONST.DIRECTION.right)
        end

        if x < px then
            self:changeDirection(CONST.DIRECTION.left)
        end
    end

    love.graphics.setColor(self.color)

    love.graphics.draw(
        self.spritestileSheets[c_state],
        self.spritesList[c_state][c_spriteID],
        self.transform.position.x,
        self.transform.position.y,
        self.transform.rotation.y,
        self.transform.scale.x,
        self.transform.scale.y,
        self.spritestileSheets[c_state]:getHeight() / 2,
        self.spritestileSheets[c_state]:getHeight() / 2
    )

    if self.isPlayer == false then
        if self.isHit then
            local points = ""
            if self.currentPV > 0 then
                love.graphics.setFont(self.font10)
                love.graphics.setColor(1, 1, 1)
                points = "+" .. (self.maxPV / 10) .. " points"
            else
                love.graphics.setFont(self.font15)
                love.graphics.setColor(1, 0.9, 0.1)
                points = "+" .. (self.maxPV / 2) .. " points"
            end
            love.graphics.print(points, self.transform.position.x, self.transform.position.y - 30)
            love.graphics.setFont(self.defaultFont)
        end
    end

    if self.isPlayer == false and self.state == CHARACTERS.STATE.ALERT then
        love.graphics.draw(
            self.alertImg,
            self.transform.position.x,
            self.transform.position.y - 30,
            0,
            self.transform.scale.x,
            self.transform.scale.y
        )
    end
end

function Character:update(dt)
    if self.isHit then
        self:ChangeColorRed(true)
        self.timer = self.timer + dt
        if self.timer >= 0.4 then
            self.timer = 0
            self.isHit = false
            self:ChangeColorRed(false)
        end
    end

    local c_state = self.mode .. "_" .. self.state
    self.currentSpriteId = self.currentSpriteId + 5 * dt
    if self.currentSpriteId >= #self.spritesList[c_state] + 0.99 then
        self.currentSpriteId = 1
    end

    self.handPosition.x = self.transform.position.x + (self.handOffset.x * self.transform.scale.x)
    self.handPosition.y = self.transform.position.y + self.handOffset.y

    self:moveWeapon(dt)

    if self.ennemiAgent then
        self.ennemiAgent:update(dt, self.transform.position.x, self.transform.position.y, self.state)
    end

    if self.currentPV <= 0 then
        self.canBeHurt = false
        self.dieTimer = self.dieTimer + 1 * dt
        if self.dieTimer >= 0.5 then
            levelManager.destroyCharacter(self, weapon)
            self.dieTimer = 0
        end
    end
end

function Character:getHandPosition()
    return self.handPosition.x, self.handPosition.y
end

function Character:getTargetPos()
    if self.target:getPosition() then
        return self.target:getPosition()
    end
end

function Character:getTarget()
    return self.target
end

function Character:changeWeapon(nb)
    self.currentWeaponId = nb
end

function Character:fire(dt)
    self.weapon[self.currentWeaponId]:fire(
        dt,
        self.transform.position,
        self.transform.scale,
        self.handPosition,
        self.weaponScaling,
        self.target
    )
end

function Character:moveWeapon(dt)
    if #self.weapon ~= 0 then
        self.weapon[self.currentWeaponId]:move(
            dt,
            self.transform.position,
            self.transform.scale,
            self.handPosition,
            self.weaponScaling,
            self.target
        )
        self.weapon[self.currentWeaponId]:update(dt)
    end
end

function Character:getWeaponRange()
    if #self.weapon ~= 0 then
        return self.weapon[self.currentWeaponId]:getWeaponRange()
    end
end

function Character:keypressed(key)
end

function Character:addEnnemiAgent()
    local localEnnemiAgent = ennemiAgent.new()
    self.ennemiAgent = localEnnemiAgent.create()
    self.ennemiAgent:init(self)
end

function Character:hit(attacker, damage)
    if self.canBeHurt == true then
        self.isHit = true
        self.currentPV = self.currentPV - damage
        if attacker:isThePlayer() then
            if self.currentPV <= 0 then
                player.addPoints(self.maxPV / 2)
                print("Tu gagne : " .. self.maxPV / 2)
            else
                player.addPoints(self.maxPV / 10)
                print("Tu gagne : " .. self.maxPV / 10)
            end
        end
    end
end

function Character:isCollidedBy(weaponX, weaponY, weaponWidth, weaponHeight)
    local weaponTopRight = weaponX + weaponWidth
    local weaponBottomLeft = weaponY + weaponHeight

    local c_state = self.mode .. "_" .. self.state
    local spriteWidth = self.spritestileSheets[c_state]:getWidth() / #self.spritesList[c_state]
    local spriteHeight = self.spritestileSheets[c_state]:getHeight()

    if
        weaponX > self.transform.position.x + spriteWidth / 2 or weaponTopRight < self.transform.position.x or
            weaponY > self.transform.position.y + spriteHeight / 2 or
            weaponBottomLeft < self.transform.position.y
     then
        return false
    else
        return true
    end

    return false
end

function Character:ChangeColorRed(bool)
    if bool then
        self.color = {1, 0, 0, 1}
    else
        self.color = {1, 1, 1, 1}
    end
end

function Character:setInCinematicMode(bool)
    if bool then
        self.isCinematicMode = true
    else
        self.isCinematicMode = false
    end
end

function Character:isInCinematicMode()
    return self.isCinematicMode
end

return Character

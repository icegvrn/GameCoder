require("scripts/states/CONST")

ennemiAgent = require("scripts/engine/ennemiAgent")
utils = require("scripts/Utils/utils")
Transform = require("scripts/engine/transform")
Fighter = require("scripts/game/Entities/Characters/Modules/c_Fighter")
Controller = require("scripts/game/Entities/Characters/Modules/c_Controller")
Sprites = require("scripts/game/Entities/Characters/Modules/c_Sprites")
WeaponSlot = require("scripts/game/Entities/Characters/Modules/c_WeaponSlot")
Sound = require("scripts/game/Entities/Characters/Modules/c_Sound")
PointsUI = require("scripts/game/Entities/Characters/Modules/c_PointsUI")

local Character = {}
local characters_mt = {__index = Character}

function Character.new()
    newCharacter = {}

    newCharacter.name = "inconnu"
    newCharacter.mode = CHARACTERS.MODE.NORMAL
    newCharacter.state = CHARACTERS.STATE.IDLE
    newCharacter.transform = Transform.new()
    newCharacter.fight = Fighter.new()
    newCharacter.controller = Controller.new()
    newCharacter.sprites = Sprites.new()
    newCharacter.weaponSlot = WeaponSlot.new()
    newCharacter.sound = Sound.new()
    newCharacter.pointsUI = PointsUI.new()

    return setmetatable(newCharacter, characters_mt)
end

function Character:setSounds(array)
    for i = 1, #array do
        self.sound.tracks[i] = array[i]
    end
end

function Character:getSound()
    return self.sound.tracks[1], self.sound.tracks[2], self.sound.tracks[3]
end

function Character:setSilenceIntervalBetweenTalk(nb)
    self.sound.silenceBetweenTalk = nb
end

function Character:setWeaponScaling(nb)
    self.fight.weapon.weaponScaling = nb
end

function Character:setTalkingVolume(nb)
    self.sound.talkVolume = nb
end
function Character:getCurrentPV()
    return self.fight.currentPV
end

function Character:setCurrentPV(pv)
    self.fight.currentPV = pv
end

function Character:getHandOffset()
    return self.weaponSlot.handOffset.x, self.weaponSlot.handOffset.y
end

function Character:setHandOffset(array)
    self.weaponSlot.handOffset.x = array[1]
    self.weaponSlot.handOffset.y = array[2]
end

function Character:getHandPosition()
    return self.weaponSlot.handPosition.x, self.weaponSlot.handPosition.yA
end

function Character:setPlayer()
    self.controller.isPlayer = true
end

function Character:isThePlayer()
    return self.controller.isPlayer
end

function Character:getHeight()
    return self.sprites.height
end

function Character:getPosition()
    return self.transform.position.x, self.transform.position.y
end

function Character:getDimension()
    local c_state = self.mode .. "_" .. self.state
    local w = self.sprites.spritestileSheets[c_state]:getWidth() / 4
    local h = self.sprites.spritestileSheets[c_state]:getHeight()
    return w, h
end

function Character:setDimension(h, w)
    self.sprites.height = h
    self.sprites.width = w
end

function Character:getWeaponScaling()
    return self.fight.weapon.weaponScaling
end
function Character:setSprites(p_table)
    local sprites = {}
    for k, sprite in pairs(p_table) do
        self.sprites.spritestileSheets[k] = sprite
        self.sprites.height = sprite:getHeight()
        self.sprites.width = sprite:getWidth() / 4
        local nbColumns = 4
        local nbLine = sprite:getHeight() / self.sprites.height
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

    self.sprites.spritesList = sprites
end

function Character:setCurrentSpriteId(nb)
    self.sprites.currentSpriteId = nb
end

function Character:getCurrentSpriteId()
    return self.sprites.currentSpriteId
end

function Character:getCurrentWeapon()
    return self.fight.weapon[self.fight.currentWeaponId]
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

function Character:setPosition(x, y)
    self.transform.position.x, self.transform.position.y = x, y
end

function Character:setSpeed(pSpeed)
    self.controller.speed = pSpeed
end

function Character:getSpeed()
    return self.controller.speed
end

function Character:setState(state)
    self.state = state

    if state == CHARACTERS.STATE.ALERT then
        local randNb = love.math.random(1, self.sound.silenceBetweenTalk * 100)
        if randNb == self.sound.silenceBetweenTalk * 100 then
            soundManager:playSound(PATHS.SOUNDS.CHARACTERS .. "alert.wav", 0.2, false)
        end
    end
end

function Character:getState()
    return self.state
end

function Character:setMode(mode)
    self.mode = mode
    if mode == CHARACTERS.MODE.BOOSTED then
        self:clearFiringElements()
        self:changeWeapon(2)
    else
        self:changeWeapon(1)
    end
end

function Character:clearFiringElements()
    self.fight.weapon[self.fight.currentWeaponId]:clear()
end

function Character:getMode()
    return self.mode
end

function Character:getCurrentTileSheet()
    local c_state = c
    return self.sprites.spritestileSheets[c_state]
end

function Character:setCurrentTileSheet(ts)
    self.currentTilesheet = ts
end

function Character:equip(p_weapon)
    p_weapon:setOwner(self)
    table.insert(self.fight.weapon, p_weapon)
end

function Character:setMaxPV(pv)
    self.fight.maxPV = pv
end

function Character:getMaxPV()
    return self.fight.maxPV
end

function Character:changeDirection(direction, dt)
    if self.controller.lookAt ~= direction then
        self.controller.lookAt = direction
    else
        if self.controller.lookAt == CONST.DIRECTION.left then
            self.transform.scale.x = -self.sprites.initialScale
        elseif self.controller.lookAt == CONST.DIRECTION.right then
            self.transform.scale.x = self.sprites.initialScale
        end
    end
end

function Character:getDirection()
    return self.controller.lookAt
end

function Character:setStrenght(nb)
    self.fight.strenght = nb
end

function Character:getStrenght()
    return self.fight.strenght
end

function Character:load()
end

function Character:setTarget(target)
    self.controller.target = target
end

function Character:getTarget()
    return self.controller.target
end

function Character:moveWeapon(angle)
    self.fight.weapon:setAngle(angle)
end

function Character:draw()
    print(self.fight.maxPV)
    if self.controller.ennemiAgent then
        self.controller.ennemiAgent:draw()
    end
    if self.state ~= CHARACTERS.STATE.DEAD then
        if #self.fight.weapon ~= 0 then
            self.fight.weapon[self.fight.currentWeaponId]:draw()
        end
    end

    local c_state = self.mode .. "_" .. self.state
    local c_spriteID = math.floor(self.sprites.currentSpriteId)
    local px, py = self.transform.position.x, self.transform.position.y

    if (self.controller.target ~= love.mouse) then
        px, py = Utils.screenCoordinates(px, py)
        local x, y = Utils.screenCoordinates(self.controller.target.transform:getPosition())
    else
        local x, y = Utils.screenCoordinates(self.controller.target:getPosition())
    end

    if self.controller.isPlayer == true or self.state == CHARACTERS.STATE.ALERT or self.state == CHARACTERS.STATE.FIRE then
        if x > px then
            self:changeDirection(CONST.DIRECTION.right, dt)
        end

        if x < px then
            self:changeDirection(CONST.DIRECTION.left, dt)
        end
    end

    love.graphics.setColor(self.sprites.color)

    if self.sprites.spritesList[c_state][c_spriteID] then
        love.graphics.draw(
            self.sprites.spritestileSheets[c_state],
            self.sprites.spritesList[c_state][c_spriteID],
            self.transform.position.x,
            self.transform.position.y,
            self.transform.rotation.y,
            self.transform.scale.x,
            self.transform.scale.y,
            self.sprites.spritestileSheets[c_state]:getHeight() / 2,
            self.sprites.spritestileSheets[c_state]:getHeight() / 2
        )
    end
    if self.controller.isPlayer == false then
        if self.fight.isHit then
            local points = ""
            if self.fight.currentPV > 0 then
                love.graphics.setFont(self.pointsUI.font10)
                love.graphics.setColor(1, 1, 1)
                points = "+" .. (self.fight.maxPV / 10) .. " points"
            else
                love.graphics.setFont(self.pointsUI.font15)
                love.graphics.setColor(1, 0.9, 0.1)
                points = "+" .. (self.fight.maxPV / 2) .. " points"
            end
            love.graphics.print(points, self.transform.position.x, self.transform.position.y - 30)
            love.graphics.setFont(self.pointsUI.defaultFont)
            love.graphics.setColor(1, 1, 1)
        end
    end

    if self.controller.isPlayer == false and self.state == CHARACTERS.STATE.ALERT then
        love.graphics.draw(
            self.pointsUI.alertImg,
            self.transform.position.x,
            self.transform.position.y - 30,
            0,
            self.transform.scale.x,
            self.transform.scale.y
        )
    end
end

function Character:playSounds()
    if self.sound.playSound == false then
        if self.fight.isHit then
            if self.controller.isPlayer then
                soundManager:playSound(PATHS.SOUNDS.GAME .. "heros_hitted.wav", 0.7, false)
            else
                soundManager:playSound(PATHS.SOUNDS.GAME .. "ennemi_hitted.wav", 0.1, false)
            end
            self.sound.playSound = true
        end
    end
end

function Character:update(dt)
    if self.fight.isHit then
        self:ChangeColorRed(true)
        self.fight.timer = self.fight.timer + dt
        if self.fight.timer >= 0.4 then
            self.fight.timer = 0
            self.fight.isHit = false
            self.sound.playSound = false
            self:ChangeColorRed(false)
        end
        self:playSounds()
    end

    local c_state = self.mode .. "_" .. self.state
    self.sprites.currentSpriteId = self.sprites.currentSpriteId + 5 * dt
    if self.sprites.currentSpriteId >= #self.sprites.spritesList[c_state] + 0.99 then
        self.sprites.currentSpriteId = 1
    end

    self.weaponSlot.handPosition.x = self.transform.position.x + (self.weaponSlot.handOffset.x * self.transform.scale.x)
    self.weaponSlot.handPosition.y = self.transform.position.y + self.weaponSlot.handOffset.y

    self:moveWeapon(dt)

    if self.controller.ennemiAgent then
        self.controller.ennemiAgent:update(dt, self.transform.position.x, self.transform.position.y, self.state)
    end

    if self.fight.currentPV <= 0 then
        self:setState(CHARACTERS.STATE.DEAD)
        self.fight.canBeHurt = false
        self.fight.dieTimer = self.fight.dieTimer + 1 * dt
        if self.fight.dieTimer >= 0.5 then
            levelManager.destroyCharacter(self, weapon)
            self.fight.dieTimer = 0
            soundManager:playSound(PATHS.SOUNDS.GAME .. "win_point.wav", 0.2, false)
        end
    end
end

function Character:getHandPosition()
    return self.weaponSlot.handPosition.x, self.weaponSlot.handPosition.y
end

function Character:getTarget()
    return self.controller.target
end

function Character:changeWeapon(nb)
    self.fight.currentWeaponId = nb
end

function Character:fire(dt)
    self.fight.weapon[self.fight.currentWeaponId]:fire(
        dt,
        self.transform.position,
        self.transform.scale,
        self.weaponSlot.handPosition,
        self.fight.weapon.weaponScaling,
        self.controller.target
    )
    local randNb = love.math.random(1, self.sound.silenceBetweenTalk * 1000)
    if randNb == self.sound.silenceBetweenTalk * 1000 then
        local nb = love.math.random(1, #self.sound.tracks)
        soundManager:playSound(self.sound.tracks[nb], self.sound.talkVolume, false)
    end
end

function Character:moveWeapon(dt)
    if #self.fight.weapon ~= 0 then
        self.fight.weapon[self.fight.currentWeaponId]:move(
            dt,
            self.transform.position,
            self.transform.scale,
            self.weaponSlot.handPosition,
            self.fight.weapon.weaponScaling,
            self.controller.target
        )
        self.fight.weapon[self.fight.currentWeaponId]:update(dt)
    end
end

function Character:getWeaponRange()
    if #self.fight.weapon ~= 0 then
        return self.fight.weapon[self.fight.currentWeaponId]:getWeaponRange()
    end
end

function Character:keypressed(key)
end

function Character:addEnnemiAgent()
    local localEnnemiAgent = ennemiAgent.new()
    self.controller.ennemiAgent = localEnnemiAgent.create()
    self.controller.ennemiAgent:init(self)
end

function Character:hit(attacker, damage)
    if self.fight.canBeHurt == true then
        self.fight.isHit = true
        self.fight.currentPV = self.fight.currentPV - damage
        if attacker:isThePlayer() then
            if self.fight.currentPV <= 0 then
                player.addPoints(self.fight.maxPV / 2)
            else
                player.addPoints(self.fight.maxPV / 10)
            end
        end
    end
end

function Character:isCollidedBy(weaponX, weaponY, weaponWidth, weaponHeight)
    local weaponTopRight = weaponX + weaponWidth
    local weaponBottomLeft = weaponY + weaponHeight

    local c_state = self.mode .. "_" .. self.state

    local spriteWidth = self.sprites.spritestileSheets[c_state]:getWidth() / #self.sprites.spritesList[c_state]
    local spriteHeight = self.sprites.spritestileSheets[c_state]:getHeight()

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
        self.sprites.color = {1, 0, 0, 1}
    else
        self.sprites.color = {1, 1, 1, 1}
    end
end

function Character:setInCinematicMode(bool)
    if bool then
        self.controller.isCinematicMode = true
    else
        self.controller.isCinematicMode = false
    end
end

function Character:isInCinematicMode()
    return self.controller.isCinematicMode
end

return Character

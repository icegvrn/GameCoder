require("scripts/states/CONST")
utils = require("scripts/Utils/utils")
Transform = require("scripts/engine/transform")
ennemiAgent = require("scripts/engine/ennemiAgent")
Fighter = require("scripts/game/Entities/Characters/Modules/c_Fighter")
Controller = require("scripts/game/Entities/Characters/Modules/c_Controller")
Sprites = require("scripts/game/Entities/Characters/Modules/c_Sprites")
WeaponSlot = require("scripts/game/Entities/Characters/Modules/c_WeaponSlot")
Sound = require("scripts/game/Entities/Characters/Modules/c_Sound")
Collider = require("scripts/game/Entities/Characters/Modules/c_Collider")

local Character = {}
local characters_mt = {__index = Character}

function Character.new()
    newCharacter = {}
    newCharacter.transform = Transform.new()
    newCharacter.collider = Collider.new()
    newCharacter.fight = Fighter.new()
    newCharacter.controller = Controller.new()
    newCharacter.sprites = Sprites.new()
    newCharacter.sound = Sound.new()
    return setmetatable(newCharacter, characters_mt)
end

function Character:create()
    local character = {
        name = "character unknown",
        mode = CHARACTERS.MODE.NORMAL,
        state = CHARACTERS.STATE.IDLE,
        transform = self.transform:create(),
        collider = self.collider:create(),
        fight = self.fight:create(),
        controller = self.controller:create(),
        sprites = self.sprites:create(),
        sound = self.sound:create(),
        i = 0
    }

    function character:update(dt)
        self.controller:lookAtRightDirection(dt, self)
        self.sprites:animate(dt, self.mode, self.state)
        self.fight.weaponSlot:moveWeapon(dt, self, self.controller.target)
        if self.fight.isHit then
            self.fight:hitEvents(dt, self, self.sprites, self.sound)
        end
        if self.fight.currentPV <= 0 then
            self.fight:dyingEvents(dt, self, self.sound)
        end
        self.collider:update(dt, self)
    end

    function character:draw()
        self.fight:drawWeapon(self.state)
        self.sprites:drawSprite(self, self.controller.lookAt, self.mode, self.state)

        if self.controller.ennemiAgent then
            if self.state == CHARACTERS.STATE.ALERT then
                self.fight:drawAlertSign(self)
            end
            self.fight:drawHittingPoints(self)
        end
    end

    function character:fire(dt)
        self.fight:fire(dt, self)
        self.sound:randomSpeak()
    end

    ---- A REVOIR A PARTIR DE LA ---
    function character:addPoints(points)
        if self.controller.isThePlayer then
            player.addPoints(points)
        end
    end

    function character:destroy()
        levelManager.destroyCharacter(self, self.fight.weaponSlot.weapon)
    end

    function character:equip(p_weapon)
        p_weapon:setOwner(self)
        table.insert(self.fight.weaponSlot.weapon, p_weapon)
    end

    function character:setName(name)
        self.name = name
    end

    function character:setPosition(x, y)
        self.transform.position.x, self.transform.position.y = x, y
    end

    function character:setSpeed(pSpeed)
        self.controller.speed = pSpeed
    end

    function character:setState(state)
        self.state = state
        if state == CHARACTERS.STATE.ALERT then
            self.sound:alertedSound()
        end
    end

    function character:setMode(mode)
        self.mode = mode
        if mode == CHARACTERS.MODE.BOOSTED then
            self.fight.weaponSlot:clearFiringElements()
            self.fight:changeWeapon(2)
        else
            self.fight:changeWeapon(1)
        end
    end

    function character:getName()
        return self.name
    end

    function character:getPosition()
        return self.transform.position.x, self.transform.position.y
    end

    function character:getCurrentWeapon()
        return self.fight.weaponSlot.weapon[self.fight.weaponSlot.currentWeaponId]
    end

    function character:getState()
        return self.state
    end

    function character:getMode()
        return self.mode
    end

    return character
end

return Character

local PlayerUI = require("scripts/game/Entities/Characters/Modules/c_PlayerUI")
local PlayerInput = require("scripts/game/Entities/Characters/Modules/c_PlayerInput")
local PointsCounter = require("scripts/game/Entities/Characters/Modules/c_PointsCounter")
local Booster = require("scripts/game/Entities/Characters/Modules/c_Booster")
local Animator = require("scripts/game/Entities/Characters/Modules/c_Animator")

local Player = {}
local Player_mt = {__index = Player}

function Player.new()
    local _player = {
        character = Character.new(),
        playerUI = PlayerUI.new(),
        pointsCounter = PointsCounter.new(),
        playerInput = PlayerInput.new(),
        booster = Booster.new(),
        animator = Animator.new()
    }
    return setmetatable(_player, Player_mt)
end

function Player:create()
    local player = {
        character = self.character:create(),
        playerUI = self.playerUI.create(),
        playerInput = self.playerInput.create(),
        playerBooster = self.booster.create(),
        pointsCounter = self.pointsCounter.create(),
        animator = self.animator.create(),
        isPlayer = true,
        isDead = false
    }

    function player:update(dt)
        self.character:update(dt)
        self.animator:update(dt, self)

        if self.character.controller:isInCinematicMode() == false then
            self:updatePlayables(dt, self)
        end
    end

    function player:updatePlayables(dt, self)
        self.playerUI:update(dt, self)
        self.playerInput:update(dt, self)
        self.playerBooster:update(dt, self)
        self.pointsCounter:update(dt, self)
    end

    function player:keypressed(key)
        self.playerInput:keypressed(key, self)
    end

    function player:draw()
        self.character:draw()
        if self.character.controller:isInCinematicMode() == false then
            self.playerUI:draw(self)
        end
    end

    player.character.controller.player = player

    return player
end

return Player

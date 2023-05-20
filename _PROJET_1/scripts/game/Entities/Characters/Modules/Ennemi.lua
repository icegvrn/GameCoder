Character = require("scripts/engine/character")
EnnemiAgent = require("scripts/engine/ennemiAgent")
local Animator = require("scripts/game/Entities/Characters/Modules/c_Animator")

local Ennemi = {}
local Ennemi_mt = {__index = Ennemi}

function Ennemi.new()
    local ennemi = {
        ennemiCharacter = Character.new(),
        ennemiAgent = EnnemiAgent.new(),
        animator = Animator.new()
    }
    return setmetatable(ennemi, Ennemi_mt)
end

function Ennemi:create()
    local ennemi = {
        character = self.ennemiCharacter:create(),
        agent = self.ennemiAgent.create(),
        animator = self.animator.create()
    }

    function ennemi:init()
        ennemi.agent:init(ennemi)
        ennemi.character.controller.ennemiAgent = ennemi.agent
    end

    function ennemi:draw()
        self.character:draw()
    end

    function ennemi:update(dt)
        self.character:update(dt)
        x, y = self.character:getPosition()
        self.character.controller.ennemiAgent:update(dt, self, x, y, self.character.state)
        self.animator:update(dt, self)
    end

    function ennemi:keypressed(key)
        -- nothing
    end

    return ennemi
end

return Ennemi

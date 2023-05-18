Character = require("scripts/engine/character")
EnnemiAgent = require("scripts/engine/ennemiAgent")

local Ennemi = {}
local Ennemi_mt = {__index = Ennemi}

function Ennemi.new()
    local ennemi = {
        ennemiCharacter = Character.new(),
        ennemiAgent = EnnemiAgent.new()
    }
    return setmetatable(ennemi, Ennemi_mt)
end

function Ennemi:create()
    local ennemi = {
        character = self.ennemiCharacter:create(),
        agent = self.ennemiAgent.create()
    }

    ennemi.agent:init(ennemi.character)
    ennemi.character.controller.ennemiAgent = ennemi.agent

    function ennemi:draw()
        self.character:draw()
    end

    function ennemi:update(dt)
        self.character:update(dt)
        x, y = self.character:getPosition()
        self.character.controller.ennemiAgent:update(dt, x, y, self.character.state)
    end

    function ennemi:keypressed(key)
        -- nothing
    end

    return ennemi
end

return Ennemi

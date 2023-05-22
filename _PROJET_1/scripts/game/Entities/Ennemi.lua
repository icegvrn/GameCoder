-- ENTITE ENNEMI, CONTIENT LES COMPOSANTS D'UN ENNEMI : CHARACTER, AGENT ENNEMI, ANIMATOR

Character = require(PATHS.MODULES.CHARACTER)
EnnemiAgent = require(PATHS.MODULES.ENNEMIAGENT)
local Animator = require(PATHS.MODULES.ANIMATOR)

local Ennemi = {}
local Ennemi_mt = {__index = Ennemi}

-- Création des différents composants nécessaire à un ennemi
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

    -- Intiation de l'ia ennemi
    function ennemi:init()
        self.agent:init(self)
        self.character:setAgentEnnemi(self.agent)
    end

    -- Draw du character de l'ennemi
    function ennemi:draw()
        self.character:draw()
    end

    -- Update du character, de l'animator et de l'IA de l'ennemi
    function ennemi:update(dt)
        self.character:update(dt, self)
        self.animator:update(dt, self)
    end

    -- Transmission éventuelle du keypressed
    function ennemi:keypressed(key)
        -- nothing
    end

    return ennemi
end

return Ennemi

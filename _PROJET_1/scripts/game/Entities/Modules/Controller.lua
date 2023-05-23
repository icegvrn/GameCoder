-- MODULE QUI GERE LE CONTROLE DU PERSONNAGE : EST-IL CONTROLLE PAR IA OU INPUT, EST-IL EN CINEMATIQUE ? 
-- DANS QUEL DIRECTION VA-T-IL ALLER ?

local mapManager = require(PATHS.MAPMANAGER)
local c_Controller = {}
local Controller_mt = {__index = c_Controller}

function c_Controller.new()
    local controller = {}
    return setmetatable(controller, Controller_mt)
end

function c_Controller:create()
    local controller = {
        ennemiAgent = nil,
        player = nil,
        canMove = true,
        speed = nil,
        target = love.mouse,
        isCinematicMode = false,
        directionTimer = 0.01,
        directionTimerIsStarted = false,
        directionCanBeChange = false,
        lookAt = CONST.DIRECTION.left
    }

    -- Fonction permettant d'appeler les fonction de changemetn de direction si sa cible X passe dans son dos.
    -- Si joueur, la cible est la souris

    function controller:lookAtRightDirection(dt, character)
        local x, y = self.target:getPosition()
        local px, py = character.transform.position.x, character.transform.position.y

        if (self.target == love.mouse) then
            x, y = Utils.mouseToWorldCoordinates(x, y)
        end

        if self.player or character.state == CHARACTERS.STATE.ALERT or character.state == CHARACTERS.STATE.FIRE then
            if x > px then
                self:changeDirection(character, CONST.DIRECTION.right, dt)
            end

            if x < px then
                self:changeDirection(character, CONST.DIRECTION.left, dt)
            end
        end
    end

    -- Update l'IA ennemi si le controller a un ia ennemi, update la direction dans laquelle il doit regarder 
    function controller:update(dt, entity, character, characterState)
        if self.ennemiAgent then
            self.ennemiAgent:update(
                dt,
                entity,
                character.transform.position.x,
                character.transform.position.y,
                characterState
            )
        end
        self:lookAtRightDirection(dt, character)
    end

    -- Fonction permettant de changer la variable "lookAt" qui sera utilisé pour draw le sprite
    function controller:changeDirection(parent, direction, dt)
        if self.lookAt ~= direction then
            self.lookAt = direction
        end
    end

       -- Fonction qui permet de modifier le bool cinématique et qui replace le character un peu hors de l'écran près de la porte pour la cinématique si joueur
       function controller:setInCinematicMode(parent, bool)
        self.isCinematicMode = bool
        if self.player and bool then
            parent.transform:setPosition(
                -10,
                (mapManager:getCurrentMap().height * mapManager:getCurrentMap().tileheight / 2)
            )
        end
    end

    -- Setters
    function controller:setAgentEnnemi(ennemi)
        self.ennemiAgent = ennemi
    end

    function controller:setTarget(target)
        self.target = target
    end

    function controller:setSpeed(speed, category)
        if category == CHARACTERS.CATEGORY.PLAYER then
            self.speed = 120
        else
            self.speed = speed
        end
    end

    -- Getters
    function controller:getSpeed()
        return self.speed
    end


    function controller:isInCinematicMode()
        return self.isCinematicMode
    end

    return controller
end

return c_Controller

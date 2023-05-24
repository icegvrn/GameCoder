-- L'ENTITE PLAYER, COMPREND LES COMPOSANTS D'UN PLAYER, CHARACTER, PLAYERINPUT, BOOSTER, ANIMATOR ETC
local PlayerUI = require(PATHS.MODULES.PLAYERUI)
local PlayerInput = require(PATHS.MODULES.PLAYERINPUT)
local PointsCounter = require(PATHS.MODULES.POINTSCOUNTER)
local Booster = require(PATHS.MODULES.BOOSTER)
local Animator = require(PATHS.MODULES.ANIMATOR)

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

-- Création des différents composants nécessaire à un joueur
function Player:create()
    local player = {
        character = self.character:create(),
        playerUI = self.playerUI.create(),
        playerInput = self.playerInput.create(),
        playerBooster = self.booster.create(player),
        pointsCounter = self.pointsCounter.create(),
        animator = self.animator.create(),
        isPlayer = true,
        isDead = false
    }

    -- Initialisation du playerUI (permet de créer les barres et les boutons de capacités)
    function player:init()
        self.playerUI:load(self)
    end

    -- Update des éléments du joueur avec séparation des éléments qui doivent être update ou non durant une cinématique
    function player:update(dt)
        self.character:update(dt, self)
        self.animator:update(dt, self)
        if self.character.controller:isInCinematicMode() == false then
        self:updatePlayables(dt, self)
        end
    end

    -- Update des éléments utilisables qu'en mode jeu et pas en mode cinématique
    function player:updatePlayables(dt, self)
        self.playerUI:update(dt, self)
        self.playerBooster:update(dt, self)
        self.pointsCounter:update(dt, self)
        self.playerInput:update(dt, self)
    end

    -- Fonction qui permet d'upgrade le niveau du joueur lorsqu'il change de map, appelé par levelManager
    function player:upPlayerLevel()
        self.character:upCharacterLevel()
    end

    -- Transmission des keyPressed au playerInput
    function player:keypressed(key)
        self.playerInput:keypressed(key, self)
    end

    -- Draw des éléments du player ; pas l'UI en mode cinématique
    function player:draw()
        self.character:draw()
        if self.character.controller:isInCinematicMode() == false then
            self.playerUI:draw(self)
        end
    end

    -- Renvoi comme référence au controller
    player.character.controller.player = player

    return player
end

return Player

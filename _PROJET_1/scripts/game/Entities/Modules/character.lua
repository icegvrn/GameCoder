-- MODULE PERSONNAGE QUI RASSEMBLE TOUS LES COMPOSANTS D'UN PERSONNAGE
Transform = require(PATHS.TRANSFORM)
ennemiAgent = require(PATHS.MODULES.ENNEMIAGENT)
Fighter = require(PATHS.MODULES.FIGTHER)
Controller = require(PATHS.MODULES.CONTROLLER)
Sprites = require(PATHS.SPRITES)
WeaponSlot = require(PATHS.MODULES.WEAPONSLOT)
Sound = require(PATHS.SOUND)
Collider = require(PATHS.COLLIDER)

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
        sound = self.sound:create()
    }

    -- Fonction d'update des composant du character
    function character:update(dt, parent)
        self.fight:update(dt, self, self.sprites, self.sound, self.controller.target)
        self.controller:update(dt, parent, self, self.state)
        self.sprites:animate(dt, self.mode, self.state)
    end

    -- Draw du personnage : arme, personnage et appel au fight pour draw points ou signe d'alerte selon l'état du controller
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

    -- Function Fire du character qui appelle le fire de Fight + parole aléatoire sur le son
    function character:fire(dt)
        self.fight:fire(dt, self)
        self.sound:randomSpeak()
    end

    -- Fonction pour "équiper" le personnage d'une arme : attribue un weapon à la liste des armes dans le fighter
    function character:equip(p_weapon)
        p_weapon:setOwner(self)
        p_weapon:init()
        table.insert(self.fight.weaponSlot.weapon, p_weapon)
    end

    -- Fonction qui permet d'upgrade le niveau du personnage en augmentant la vitesse de son arme
    function character:upCharacterLevel()
        self.fight:upFightLevel()
    end

    -- Fonction pour changer le "mode" du personnage et indiquer au fight ce qu'il doit faire en conséquence
    -- (changer d'arme et clear les tirs en cours)
    function character:setMode(mode)
        self.mode = mode
        if mode == CHARACTERS.MODE.BOOSTED then
            self.fight.weaponSlot:clearFiringElements()
            self.fight:changeWeapon(2)
        else
            self.fight:changeWeapon(1)
        end
    end

    -- Fonction pour attribuer un Agent au controller
    function character:setAgentEnnemi(agent)
        self.controller:setAgentEnnemi(agent)
    end

    -- Function pour supprimer le personnage et son arme de la liste des personnages
    function character:destroy()
        levelManager.destroyCharacter(self, self.fight.weaponSlot.weapon)
    end

    -- Setters principalement utilisés par la CharacterFactory
    function character:setName(name)
        self.name = name
    end

    function character:setSpeed(pSpeed)
        self.controller.speed = pSpeed
    end

    -- Fonction setters fréquemment utilisées
    function character:setPosition(x, y)
        self.transform.position.x, self.transform.position.y = x, y
    end

    function character:setState(state)
        self.state = state
        if state == CHARACTERS.STATE.ALERT then
            self.sound:alertedSound()
        end
    end

    -- Getters
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

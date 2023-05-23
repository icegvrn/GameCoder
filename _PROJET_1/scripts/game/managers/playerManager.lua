-- SPAWNER DU PLAYER, PILOTE PAR LE LEVELMANAGER

-- Chargement des modules
local c_factory = require(PATHS.CHARACTERFACTORY)
local w_factory = require(PATHS.WEAPONFACTORY)

local PlayerManager = {}
local Player_mt = {__index = PlayerManager}

function PlayerManager.new()
    local playerManager = {}
    return setmetatable(playerManager, Player_mt)
end

function PlayerManager:create()
    local playerManager = {playersList = {}}

    -- Fonction qui permet de créer l'entité joueur en lui attribuant une arme et le retourne. 
    -- Existe actuellement sous forme de liste pour avoir un fonctionnement similaire à ennemi et pouvoir éventuellement créer plusieurs joueurs
    function playerManager:spawnPlayer(levelManager)
        if #self.playersList == 0 then
            myHeroWeapon = w_factory.createWeapon(WEAPONS.TYPE.HERO_MAGIC_STAFF)
            myHeroWeapon_bite = w_factory.createWeapon(WEAPONS.TYPE.BITE)
            myCharacter = c_factory.createCharacter(CHARACTERS.CATEGORY.PLAYER, CHARACTERS.TYPE.ORC, true, love.mouse)
            myCharacter.character:equip(myHeroWeapon)
            myCharacter.character:equip(myHeroWeapon_bite)
            table.insert(self.playersList, myCharacter)
            table.insert(levelManager.charactersList, myCharacter)
            return myCharacter
        else
            return self.playersList[1]
        end
    end

    -- Fonction qui permet de clear la liste de player
    function playerManager:clear()
        self.playersList = {}
    end

    -- Fonction qui renvoie le player en cours
    function playerManager:getPlayer()
        return self.playersList[1]
    end

    return playerManager
end

return PlayerManager

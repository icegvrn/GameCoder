local c_factory = require("scripts/game/factories/characterFactory")
local w_factory = require("scripts/game/factories/weaponFactory")

local PlayerManager = {}
local Player_mt = {__index = PlayerManager}

function PlayerManager.new()
    local playerManager = {}
    return setmetatable(playerManager, Player_mt)
end

function PlayerManager:create()
    local playerManager = {playersList = {}}

    function playerManager:update(dt, mapManager)
        --   self:checkCharactersCollisions(dt, mapManager)
    end

    -- function playerManager:checkCharactersCollisions(dt, mapManager)
    --     for n = #self.playersList, 1, -1 do
    --         if self.playersList[n].character.collider then
    --             self.playersList[n].character.collider:update(dt, self.playersList[n].character)
    --         end
    --     end
    -- end

    function playerManager:spawnPlayer(levelManager)
        -- Fais spawn le player à un endroit précis du jeu
        myHeroWeapon = w_factory.createWeapon(WEAPONS.TYPE.HERO_MAGIC_STAFF)
        myWeapon4 = w_factory.createWeapon(WEAPONS.TYPE.BITE)
        myCharacter = c_factory.createCharacter(CHARACTERS.CATEGORY.PLAYER, CHARACTERS.TYPE.ORC, true, love.mouse)
        myCharacter.character:equip(myHeroWeapon)
        myCharacter.character:equip(myWeapon4)
        table.insert(self.playersList, myCharacter)
        table.insert(levelManager.charactersList, myCharacter)
        return myCharacter
    end

    function playerManager:clear()
        self.playersList = {}
    end

    function playerManager:getPlayer()
        return self.playersList[1]
    end

    return playerManager
end

return PlayerManager

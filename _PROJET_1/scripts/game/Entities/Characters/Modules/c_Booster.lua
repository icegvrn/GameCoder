local c_Booster = {}
local Booster_mt = {__index = c_Booster}

function c_Booster.new()
    local _Booster = {}
    return setmetatable(_Booster, Booster_mt)
end

function c_Booster:create()
    local booster = {
        boosterDuration = 5,
        boosterTimer = 5,
        initialSpeed = 0,
        boostedSpeed = 0
    }

    function booster:init(player)
        self.initialSpeed = player.character.controller.speed
        self.boostedSpeed = player.character.controller.speed * 2
    end
    function booster:update(dt, player)
        if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
            self.boosterTimer = self.boosterTimer - 1 * dt
            player.character.controller.speed = self.boostedSpeed
            if self.boosterTimer <= 0 then
                player.character.controller.speed = self.initialSpeed
                player.character:setMode(CHARACTERS.MODE.NORMAL)
                soundManager:playSound(PATHS.SOUNDS.GAME .. "endPlayerBoost.wav", 0.5, false)
                self.boosterTimer = self.boosterDuration
            end
        end
    end

    return booster
end

return c_Booster

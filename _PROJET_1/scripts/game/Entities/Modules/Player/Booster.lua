-- MODULE PERMETTANT DE GERER LE TEMPS DU BOOST POUR LE PLAYER
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

    -- Fonction qui initialise les vitesses voulues pour le mode normal et le mode boost
    function booster:init(character)
        self.initialSpeed = character.controller.speed
        self.boostedSpeed = character.controller.speed * 2
    end

    -- Update du boost : quand il est déclanché, on a un timer du temps de boost, puis on repasse le personnage en mode normal
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

    -- Fonction de boost : si le perso est en mode normal et que ses points sont au max, il passe en state boost,
    -- sinon il repasse en normal
    function booster:boost(character, pointsCounter)
        if character:getMode() == CHARACTERS.MODE.NORMAL and pointsCounter.points == pointsCounter.maxPoints then
            character:setMode(CHARACTERS.MODE.BOOSTED)
            self:init(character)
            character.sound:playBoostedSound()
        end
    end

    return booster
end

return c_Booster

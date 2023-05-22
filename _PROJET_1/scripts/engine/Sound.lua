-- MODULE QUI PERMET D'AJOUTER DU SON EN COMMUNIQUANT AVEC LE SOUNDMANAGER
local c_Sound = {}
local Sound_mt = {__index = c_Sound}

function c_Sound.new()
    local sound = {}
    return setmetatable(sound, Sound_mt)
end

function c_Sound:create()
    local sound = {
        tracks = {
            PATHS.SOUNDS.CHARACTERS .. "default.wav",
            PATHS.SOUNDS.CHARACTERS .. "default.wav",
            PATHS.SOUNDS.CHARACTERS .. "default.wav"
        },
        talkVolume = 0.3,
        silenceBetweenTalk = 1,
        playSound = false
    }

    function sound:randomSpeak()
        local nb = love.math.random(1, #self.tracks)
        self:play(self.tracks[nb], self.talkVolume, false, 1000)
    end

    function sound:alertedSound()
        self:play(PATHS.SOUNDS.CHARACTERS .. "alert.wav", 0.2, false, 100)
    end

    function sound:dyingSound()
        soundManager:playSound(PATHS.SOUNDS.GAME .. "win_point.wav", 0.2, false)
    end

    function sound:setSounds(array)
        self.tracks = {}
        for i = 1, #array do
            self.tracks[i] = array[i]
        end
    end

    function sound:setSilenceIntervalBetweenTalk(nb)
        self.silenceBetweenTalk = nb
    end

    function sound:setTalkingVolume(nb)
        self.talkVolume = nb
    end

    function sound:playBoostedSound()
        soundManager:playSound(PATHS.SOUNDS.GAME .. "heros_transform.wav", 0.4, false)
    end

    function sound:play(sound, volume, loop, interval)
        if sound ~= nil then
            if interval then
                local randNb = love.math.random(1, self.silenceBetweenTalk * interval)
                if randNb == self.silenceBetweenTalk * interval then
                    soundManager:playSound(sound, volume, loop)
                end
            else
                soundManager:playSound(sound, volume, loop)
            end
        end
    end

    return sound
end

return c_Sound

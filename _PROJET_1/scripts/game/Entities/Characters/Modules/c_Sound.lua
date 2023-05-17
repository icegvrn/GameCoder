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
        local randNb = love.math.random(1, self.silenceBetweenTalk * 1000)
        if randNb == self.silenceBetweenTalk * 1000 then
            local nb = love.math.random(1, #self.tracks)
            soundManager:playSound(self.tracks[nb], self.talkVolume, false)
        end
    end

    function sound:alertedSound()
        local randNb = love.math.random(1, self.silenceBetweenTalk * 100)
        if randNb == self.silenceBetweenTalk * 100 then
            soundManager:playSound(PATHS.SOUNDS.CHARACTERS .. "alert.wav", 0.2, false)
        end
    end

    function sound:dyingSound()
        soundManager:playSound(PATHS.SOUNDS.GAME .. "win_point.wav", 0.2, false)
    end

    function sound:setSounds(array)
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
    return sound
end

return c_Sound

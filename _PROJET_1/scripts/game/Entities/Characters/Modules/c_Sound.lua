local c_Sound = {}
local Sound_mt = {__index = c_Sound}

function c_Sound.new()
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
    return setmetatable(sound, Sound_mt)
end

return c_Sound

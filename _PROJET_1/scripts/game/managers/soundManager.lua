local soundManager = {}

GAMESTATE = require("scripts/states/GAMESTATE")
soundManager.currentGameState = GAMESTATE.STATE.START
local startMusic = "contents/sounds/musics/start.mp3"
local narrativeMusic = "contents/sounds/musics/narrative.wav"
soundManager.currentMusic = love.audio.newSource(startMusic, "static")

function soundManager.load()
    soundManager:loadMusic(GAMESTATE.currentState)
end

function soundManager.update(dt)
    if soundManager.currentGameState ~= GAMESTATE.currentState then
        soundManager.currentGameState = GAMESTATE.currentState
        soundManager:loadMusic(soundManager.currentGameState)
    end
end

function soundManager.draw()
end

function soundManager:loadMusic(state)
    if state == GAMESTATE.STATE.START then
        soundManager:playBackgroundMusic(startMusic, 0.6, true)
    elseif state == GAMESTATE.STATE.NARRATIVE then
        soundManager:playBackgroundMusic(narrativeMusic, 0.1, true)
    elseif state == GAMESTATE.STATE.GAME then
        soundManager:playBackgroundMusic(nil, 0.1, true)
    elseif state == GAMESTATE.STATE.MENU then
    elseif state == GAMESTATE.STATE.GAMEOVER then
    elseif state == GAMESTATE.STATE.WIN then
    end
end

function soundManager:playSound(sound, volume, loop)
    local audio = love.audio.newSource(sound, "static")
    audio:setVolume(volume)
    audio:setLooping(loop)
    audio:play()
end

function soundManager:playBackgroundMusic(sound, volume, loop)
    if soundManager.currentMusic then
        soundManager.currentMusic:stop()
    end

    if sound then
        local audio = love.audio.newSource(sound, "static")
        soundManager.currentMusic = audio
        soundManager.currentMusic:setVolume(volume)
        soundManager.currentMusic:setLooping(loop)
        soundManager.currentMusic:play()
    end
end

return soundManager

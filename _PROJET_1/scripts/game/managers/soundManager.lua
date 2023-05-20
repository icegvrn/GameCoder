local soundManager = {}

GAMESTATE = require("scripts/states/GAMESTATE")
soundManager.currentGameState = GAMESTATE.STATE.START
local startMusic = "contents/sounds/musics/start.mp3"
local narrativeMusic = "contents/sounds/musics/narrative.wav"
local gameOverMusic = "contents/sounds/musics/gameOver.mp3"
local winMusic = "contents/sounds/musics/win.mp3"
local gameMusics = {}

gameMusics[1] = "contents/sounds/musics/background_1.mp3"
gameMusics[2] = "contents/sounds/musics/background_2.wav"
gameMusics[3] = "contents/sounds/musics/background_3.wav"

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
        soundManager:playBackgroundMusic(gameMusics[3], 0.3, true)
    elseif state == GAMESTATE.STATE.MENU then
        soundManager:playBackgroundMusic(startMusic, 0.3, true)
    elseif state == GAMESTATE.STATE.GAMEOVER then
        soundManager:playBackgroundMusic(gameOverMusic, 0.5, true)
    elseif state == GAMESTATE.STATE.WIN then
        soundManager:playBackgroundMusic(winMusic, 0.4, true)
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

function soundManager:changeSoundForLevel()
    local random = love.math.random(1, #gameMusics)
    local vol = 0.3
    if random == 1 then
        vol = 0.1
    elseif random == 2 then
        vol = 0.2
    elseif random == 3 then
        vol = 0.3
    end
    soundManager:playBackgroundMusic(gameMusics[random], vol, true)
end

function soundManager:endOfLevel()
    self:playSound("contents/sounds/game/playerInDoor.wav", 0.5, false)
    self:changeSoundForLevel()
end

return soundManager

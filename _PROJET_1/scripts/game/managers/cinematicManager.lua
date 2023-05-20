local mainCamera = require("scripts/game/mainCamera")
local levelsConfig = require("scripts/game/levelsConfiguration")
local uiGame = require("scripts/ui/uiGame")

local CinematicManager = {}
local Cinematic_mt = {__index = CinematicManager}

function CinematicManager.new()
    local cinematicManager = {}
    return setmetatable(cinematicManager, Cinematic_mt)
end

function CinematicManager:create()
    local cinematicManager = {
        timer = 0,
        IsStarted = false,
        lenght = 3
    }

    function cinematicManager:playLevelCinematic(dt, levelManager)
        if self.IsStarted == false then
            if levelManager.charactersList then
                for n = 1, #levelManager.charactersList do
                    levelManager.charactersList[n].character.controller:setInCinematicMode(
                        levelManager.charactersList[n].character,
                        true
                    )
                end
            end
            mainCamera.lock(true)
            self.IsStarted = true
        elseif self.IsStarted then
            if levelManager.currentLevel == #levelsConfig then
                self.timer = self.timer + dt * 0.25
            else
                self.timer = self.timer + dt
            end

            for n = 1, #levelManager.charactersList do
                if levelManager.charactersList[n].agent then
                    levelManager.charactersList[n]:update(dt)
                end
            end

            if self.timer >= self.lenght / 2 then
                if levelManager.currentLevel == #levelsConfig then
                    uiGame.drawVictory()
                end
            end

            if self.timer >= self.lenght then
                if levelManager.currentLevel == #levelsConfig then
                    self.timer = 0
                    self.IsStarted = false
                    levelManager.isTheGameFinish = true
                else
                    mainCamera.lock(false)
                    for n = 1, #levelManager.charactersList do
                        levelManager.charactersList[n].character.controller:setInCinematicMode(
                            levelManager.charactersList[n],
                            false
                        )
                    end

                    levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.game
                    self.timer = 0
                    self.IsStarted = false
                end
            end
        end
    end

    return cinematicManager
end

return CinematicManager

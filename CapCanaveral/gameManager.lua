instructionsManager = require("instructionsManager")
animationManager = require("animationManager")
countdown = require("countdown")

gameManager = {}
state = {"Begin", "Tower", "Fire", "Cryo", "Motors", "Launch", "Success"}
currentState = state[1]

function gameManager.setStateAt(state)
    if currentState ~= state then
        gameManager.setCurrentState(state)
    end
end

function gameManager.setCurrentState(p_state)
    currentState = state[p_state]
    instructionsManager.setText(p_state)
    animationManager.setAnimation(p_state)
end

return gameManager

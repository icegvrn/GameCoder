local controller = {}

controller.mode = "AZERTY"
controller.up = "z"
controller.down = "s"
controller.left = "q"
controller.right = "d"
controller.action1 = "space"
controller.action2 = "lalt"
controller.bend = "lshift"

function controller.changeMode()
    if controller.mode == "AZERTY" then
        controller.mode = "QWERTY"
        controller.up = "w"
        controller.down = "s"
        controller.left = "a"
        controller.right = "d"
    elseif controller.mode == "QWERTY" then
        controller.mode = "AZERTY"
        controller.up = "z"
        controller.down = "s"
        controller.left = "q"
        controller.right = "d"
    end
end

return controller

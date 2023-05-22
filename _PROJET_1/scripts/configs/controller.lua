-- FICHIER DE CONFIGURATION QUI DETERMINE LES CONTROLS QUI SERONT UTILISES DANS LE JEU
local controller = {}

controller.mode = "AZERTY"
controller.up = "z"
controller.down = "s"
controller.left = "q"
controller.right = "d"
controller.action1 = "mouse1"
controller.action2 = "space"
controller.bend = "lshift"

-- Permet de switcher d'un mode azerty à un mode qwerty
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

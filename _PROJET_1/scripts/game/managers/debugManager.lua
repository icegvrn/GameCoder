debug = require("scripts/Utils/debug")

debugManager = {}
debugManager.debugger = true

function debugManager.load()
end

function debugManager.update(dt)
    debug.update(dt)
end

function debugManager.keypressed(key)
    if debugManager.debugger then
        if key == "1" then
            if debug.isPlayerCoordinatesTrue() then
                debug.setPlayerCoordinates(false)
            else
                debug.setPlayerCoordinates(true)
            end
        end

        if key == "2" then
            if debug.isCameraOverlayTrue() then
                debug.setCameraOverlay(false)
            else
                debug.setCameraOverlay(true)
            end
        end

        if key == "3" then
            if debug.isMapEnable() then
                debug.setMapEnable(false)
            else
                debug.setMapEnable(true)
            end
        end
    end
end

function debugManager.draw()
    debug.draw()
end

return debugManager

d_mainCamera = require("scripts/game/mainCamera")
d_player = require("scripts/game/player")
d_Utils = require("scripts/Utils/utils")
d_map = require("scripts/game/gameMap")
debug = {}
debug.cameraOverlay = false
debug.playerCoordinates = false
debug.player = {}
debug.player.position = {}
debug.player.position.x = 0
debug.player.position.y = 0
debug.mouse = {}
debug.mouse.position = {}
debug.mouse.position.x = 0
debug.mouse.position.y = 0

function debug.load()
end

function debug.update()
    if debug.playerCoordinates then
        debug.player.position.x, debug.player.position.y = d_player.character:getPosition()
        debug.mouse.position.x, debug.mouse.position.y = d_Utils.mouseToWorldCoordinates(love.mouse.getPosition())
    end
end

function debug.draw()
    debug.drawCameraDebugOverlay()
    debug.drawPlayerCoordinates()
end

function debug.drawCameraDebugOverlay()
    if debug.cameraOverlay == true then
        local x, y = d_mainCamera.camera:getPosition()
        love.graphics.setColor(1, 0, 0)
        love.graphics.rectangle("line", x, y, love.graphics.getWidth() - 1, love.graphics.getHeight() - 1)
        love.graphics.circle("line", x + love.graphics.getWidth() / 2, y + love.graphics.getHeight() / 2, 10)
        love.graphics.setColor(1, 1, 1)
    end
end

function debug.drawPlayerCoordinates()
    local cx, cy = d_mainCamera.camera:getPosition()
    local debugBarSize = 30
    if debug.playerCoordinates then
        love.graphics.setColor(0.2, 0.2, 0.2)
        love.graphics.rectangle("fill", 0 + cx, cy, love.graphics.getWidth(), debugBarSize)

        love.graphics.setColor(1, 1, 1)

        love.graphics.print("Mouse on screen : " .. love.mouse.getPosition(), Utils.screenCoordinates(5, 10))
        love.graphics.print(
            "Mouse in world : " .. string.format("%.3f", debug.mouse.position.x),
            Utils.screenCoordinates(150, 10)
        )
        love.graphics.print(
            "Player in world : " .. string.format("%.3f", debug.player.position.x),
            Utils.screenCoordinates(310, 10)
        )
    end
end

function debug.setCameraOverlay(bool)
    debug.cameraOverlay = bool
end

function debug.setPlayerCoordinates(bool)
    debug.playerCoordinates = bool
end

function debug.isPlayerCoordinatesTrue()
    return debug.playerCoordinates
end

function debug.isCameraOverlayTrue()
    return debug.cameraOverlay
end

function debug.isMapEnable()
    return d_map.mapEnable
end

function debug.setMapEnable(bool)
    d_map.mapEnable = bool
end

return debug

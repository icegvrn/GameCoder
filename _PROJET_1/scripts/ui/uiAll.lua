ui = require("scripts/engine/ui")

local uiAll = ui.new()

function uiAll.load()
    love.window.setMode(800, 610)
    love.window.setTitle("GROMOKGG ESCAPE")
    local cursor = love.mouse.newCursor("contents/images/cursor.png")
    --   love.mouse.setCursor(cursor)
end

function uiAll.update(dt)
end

function uiAll.draw()
end

function uiAll.keypressed(key)
end

return uiAll

local ui = {}

function ui.load()
    love.window.setMode(800, 610)
    love.window.setTitle("GROMOKGG ESCAPE")
    local cursor = love.mouse.newCursor("contents/images/cursor.png")
    love.mouse.setCursor(cursor)
end

return ui

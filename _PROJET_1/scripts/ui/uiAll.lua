local uiAll = {}

function uiAll.load()
    love.window.setMode(800, 610)
    love.window.setTitle("GROMOKGG ESCAPE")
    local cursor = love.mouse.newCursor(PATHS.IMG.ROOT .. "cursor.png", 48, 48)
    love.mouse.setCursor(cursor)
end

function uiAll.update(dt)
end

function uiAll.draw()
end

function uiAll.keypressed(key)
end

return uiAll

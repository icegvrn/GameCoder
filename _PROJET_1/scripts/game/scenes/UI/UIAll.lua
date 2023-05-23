local UIALL = {
    -- Initialisation de fonts utilisées partout
    defaultFont = love.graphics.newFont(),
    font100 = love.graphics.newFont(100),
    font50 = love.graphics.newFont(PATHS.FONTS .. "pixelfont.ttf", 50),
    font30 = love.graphics.newFont(PATHS.FONTS .. "pixelfont.ttf", 30),
    font15 = love.graphics.newFont(15),
    font12 = love.graphics.newFont(12),
    font10 = love.graphics.newFont(10),
    font9 = love.graphics.newFont(9)
}

-- Setup les paramètres par défauts d'affichage du jeu
function UIALL.load()
    love.window.setMode(800, 610)
    love.window.setTitle("GROMOKGG ESCAPE")
    local cursor = love.mouse.newCursor(PATHS.IMG.ROOT .. "cursor.png", 48, 48)
    love.mouse.setCursor(cursor)
end

function UIALL.update(dt)
    --
end

function UIALL.draw()
    --
end

function UIALL.keypressed(key)
    --
end

return UIALL

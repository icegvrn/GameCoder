-- MODULE QUI PERMET D'AFFICHER LES POINTS GAGNES PAR UN CHARACTER, EN L'OCCURRENCE LE JOUEUR
local c_PointsUI = {}
local PointsUI_mt = {__index = c_PointsUI}

function c_PointsUI.new()
    local _pointsUI = {}
    return setmetatable(_pointsUI, PointsUI_mt)
end

function c_PointsUI:create()
    local pointsUI = {}

    -- Fonction permettant d'afficher les points selon une font, une couleur et un transform donné
    function pointsUI:showPoints(points, color, font, x, y)
        love.graphics.setFont(font)
        love.graphics.setColor(color)
        points = "+" .. points .. " points"
        love.graphics.print(points, x, y)
        love.graphics.setFont(UIAll.defaultFont)
        love.graphics.setColor(1, 1, 1)
    end

    return pointsUI
end

return c_PointsUI

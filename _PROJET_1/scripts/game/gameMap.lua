local map = require("scripts/engine/map")
mapList = require("maps/mapList")

gameMap = {}
currentMap = {}
tileSheet = love.graphics.newImage("contents/images/dungeontileset-extended.png")
tilesTexture = {}
gameMap.mapEnable = true

function gameMap.load()
    -- Choisi la première carte à afficher
    gameMap.initMap()
end

function gameMap.draw()
    if gameMap.mapEnable then
        for i = 1, #currentMap.data do
            for l = 1, currentMap.map.height do
                for c = 1, currentMap.map.width do
                    local index = (l - 1) * currentMap.map.width + c
                    local tid = currentMap.data[i][index]
                    if tid ~= 0 then
                        local x = (c - 1) * currentMap.map.tilewidth
                        local y = (l - 1) * currentMap.map.tileheight
                        if tilesTexture[tid] then
                            love.graphics.draw(tileSheet, tilesTexture[tid], x, y)
                        end
                    end
                end
            end
        end
    end
end

-- Initialise le jeu sur la première map
function gameMap.initMap()
    currentMap = map.new()
    currentMap:setMapTo(mapList, 1)
    gameMap.loadMap()
end

function gameMap.loadMap()
    local nbColumns = tileSheet:getWidth() / currentMap.map.tilewidth
    local nbLine = tileSheet:getHeight() / currentMap.map.height
    local l, c
    local id = 1
    for l = 1, nbLine do
        for c = 1, nbColumns do
            tilesTexture[id] =
                love.graphics.newQuad(
                (c - 1) * currentMap.map.tilewidth,
                (l - 1) * currentMap.map.tileheight,
                currentMap.map.tilewidth,
                currentMap.map.tileheight,
                tileSheet:getWidth(),
                tileSheet:getHeight()
            )
            id = id + 1
        end
    end
end

function gameMap.isOverTheMap(elementPosX, elementPosY)
    local mapSizeHeight = currentMap.map.height * currentMap.map.tileheight
    local mapSizeWidth = currentMap.map.width * currentMap.map.tilewidth

    if elementPosX <= 0 or elementPosX >= mapSizeWidth or elementPosY <= 0 or elementPosY >= mapSizeHeight then
        return true
    else
        return false
    end
end

function gameMap.clamp(elementPosX, elementPosY)
    local mapSizeHeight = currentMap.map.height * currentMap.map.tileheight
    local mapSizeWidth = currentMap.map.width * currentMap.map.tilewidth

    if elementPosX <= 0 then
        elementPosX = 1
    elseif elementPosX >= mapSizeWidth then
        elementPosX = mapSizeWidth - 1
    end

    if elementPosY <= 0 then
        elementPosY = 1
    elseif elementPosY >= mapSizeHeight then
        elementPosY = mapSizeHeight - 1
    end
    return elementPosX, elementPosY
end

return gameMap

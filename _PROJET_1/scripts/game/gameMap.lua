local map = require("scripts/engine/map")
mapList = require("maps/mapList")

gameMap = {}
currentMap = {}
tileSheet = love.graphics.newImage("contents/images/dungeontileset-extended.png")
tilesTexture = {}
gameMap.mapEnable = true
gameMap.doorOpen = false
local testFont = love.graphics.newFont(9)
function gameMap.load()
    gameMap.initMap()
end

function gameMap.draw()
    if gameMap.mapEnable then
        for i = 1, #currentMap.data do
            gameMap.drawTiles(i)
        end
        posX = currentMap.map.width * currentMap.map.tilewidth
        posY = (currentMap.map.height * currentMap.map.tileheight) / 2

        local door = currentMap.door
        local exitDoorImg = currentMap.closeDoorImg

        if gameMap.doorOpen then
            exitDoorImg = currentMap.openDoorImg
        end
        love.graphics.draw(
            exitDoorImg,
            door.positionX + currentMap.openDoorImg:getWidth() / 2,
            door.positionY + 5,
            math.rad(90),
            1,
            1,
            currentMap.openDoorImg:getWidth() / 2,
            currentMap.openDoorImg:getHeight() / 2
        )

        love.graphics.draw(
            currentMap.closeDoorImg,
            10 - currentMap.openDoorImg:getWidth() / 2,
            door.positionY + 5,
            math.rad(-90),
            1,
            1,
            currentMap.openDoorImg:getWidth() / 2,
            currentMap.openDoorImg:getHeight() / 2
        )
    --   love.graphics.draw(tileSheet, tilesTexture[1366], x, y)
    --  love.graphics.rectangle("fill", door.positionX, door.positionY, door.width, door.height)
    end
    -- love.graphics.clear()
end

function gameMap.drawTiles(i)
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
            -- if i == 3 then
            --     local x = (c - 1) * currentMap.map.tilewidth
            --     local y = (l - 1) * currentMap.map.tileheight
            --     love.graphics.setFont(testFont)
            --     love.graphics.print(currentMap.data[i][index], x, y)
            -- end
        end
    end
end

-- Initialise le jeu sur la première map
function gameMap.initMap(nb)
    currentMap = map.new()
    currentMap:setMapTo(mapList, nb)
    gameMap.loadMap()
end

function gameMap.loadMap()
    local nbColumns = tileSheet:getWidth() / currentMap.map.tilewidth
    local nbLine = tileSheet:getHeight() / currentMap.map.tileheight
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

function gameMap.getMapDimension()
    if currentMap.map then
        local width = currentMap.map.width * currentMap.map.tilewidth
        local height = currentMap.map.height * currentMap.map.tileheight
        return width, height
    end
end

function gameMap.getDoor()
    return currentMap.door
end

function gameMap.closeDoor()
    gameMap.doorOpen = false
end

function gameMap.openDoor()
    gameMap.doorOpen = true
end

function gameMap.getCurrentMap()
    return currentMap.map
end

function gameMap.isThereASolidElement(p_left, p_top, p_width, p_height, character)
    -- Créé une tolérance pour ne pas rendre la collision trop abrupte en ignorant quelques tiles par rapport à la taille du personnage
    if character then
        if character.controller.player then
            if character.mode == CHARACTERS.MODE.BOOSTED then
                p_width = p_width - currentMap.map.tilewidth * 4
                p_height = p_height - currentMap.map.tileheight * 4
            else
                p_width = p_width - currentMap.map.tilewidth * 2
                p_height = p_height - currentMap.map.tileheight * 2
            end
        end
    end

    local touch = false
    local col = math.floor(p_left / currentMap.map.tilewidth)
    local lin = math.floor(p_top / currentMap.map.tileheight)

    -- calcul de la colonne et de la ligne de la dernière case couverte par le joueur
    local col2 = col + math.floor((p_width) / currentMap.map.tilewidth)
    local lin2 = lin + math.floor((p_height) / currentMap.map.tileheight)

    -- parcours de toutes les cases couvertes par le joueur
    for c = col, col2 do
        for r = lin, lin2 do
            local index = r * currentMap.map.width + c
            if currentMap.data[3] then
                if currentMap.data[3][index] then
                    if index > 0 then
                        if (currentMap.data[3][index] ~= 0) then
                            touch = true
                        end
                    end
                end
            end
        end
    end

    return touch
end

function gameMap.isThereAFloor(p_left, p_top, p_width, p_height, character)
    local floor = false
    local col = math.floor(p_left / currentMap.map.tilewidth)
    local lin = math.floor(p_top / currentMap.map.tileheight)

    -- calcul de la colonne et de la ligne de la dernière case couverte par le joueur
    local col2 = col + math.floor((p_width) / currentMap.map.tilewidth)
    local lin2 = lin + math.floor((p_height) / currentMap.map.tileheight)

    -- parcours de toutes les cases couvertes par le joueur
    for c = col, col2 do
        for r = lin, lin2 do
            local index = r * currentMap.map.width + c
            if currentMap.data[1] then
                if currentMap.data[1][index] then
                    if index > 0 then
                        if (currentMap.data[1][index] ~= 0) then
                            floor = true
                        else
                            floor = false
                        end
                    end
                end
            end
        end
    end

    return floor
end

function gameMap.getName()
    return currentMap.map.layers[3].name
end

return gameMap

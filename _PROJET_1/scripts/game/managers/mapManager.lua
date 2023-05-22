-- Chargement des modules
local Map = require(PATHS.GAMEMAP)
Door = require(PATHS.DOOR)
mapList = require(PATHS.MAPLIST)

local mapManager = {
    currentMap = nil,
    currentMapTotalHeight = 0,
    currentMapTotalWidth = 0,
    map = Map.new(),
    door = Door.new()
}

function mapManager:initMap(nb)
    self.currentMap = Map.create()
    mapManager.door = self.door:create()
    self:loadMap(nb)
end

function mapManager:loadMap(nb)
    self.currentMap:setMapTo(mapList, nb)
    self.currentMap:loadMap()
    self.currentMapTotalHeight = self.currentMap.map.height * self.currentMap.map.tileheight
    self.currentMapTotalWidth = self.currentMap.map.width * self.currentMap.map.tilewidth
    self.door:init(self.currentMap)
end

function mapManager:draw()
    self.currentMap:draw()
    self.door:draw()
end

function mapManager:update(dt)
    self.door:update(dt)
end

function mapManager:endTheLevel()
    self.door:openDoor()
end

function mapManager:afterlevelWinAction(player)
    if self.door:checkTakeDoor(player) then
        return true
    else
        return false
    end
end

function mapManager:getDoor()
    return self.door
end

function mapManager:getCurrentMap()
    return self.currentMap.map
end

function mapManager:isOverTheMap(elementPosX, elementPosY)
    if
        elementPosX <= 0 or elementPosX >= self.currentMapTotalWidth or elementPosY <= 0 or
            elementPosY >= self.currentMapTotalHeight
     then
        return true
    else
        return false
    end
end

function mapManager:clamp(elementPosX, elementPosY)
    if elementPosX <= 0 then
        elementPosX = 1
    elseif elementPosX >= self.currentMapTotalWidth then
        elementPosX = self.currentMapTotalWidth - 1
    end

    if elementPosY <= 0 then
        elementPosY = 1
    elseif elementPosY >= self.currentMapTotalHeight then
        elementPosY = self.currentMapTotalHeight - 1
    end
    return elementPosX, elementPosY
end

function mapManager:getMapDimension()
    return self.currentMapTotalWidth, self.currentMapTotalHeight
end

function mapManager:isThereASolidElement(p_left, p_top, p_width, p_height, character)
    -- Créé une tolérance pour ne pas rendre la collision trop abrupte en ignorant quelques tiles par rapport à la taille du personnage
    if character then
        if character.controller.player then
            if character.mode == CHARACTERS.MODE.BOOSTED then
                p_width = p_width - self.currentMap.map.tilewidth * 4
                p_height = p_height - self.currentMap.map.tileheight * 4
            else
                p_width = p_width - self.currentMap.map.tilewidth * 2
                p_height = p_height - self.currentMap.map.tileheight * 2
            end
        end
    end

    local touch = false
    local col = math.floor(p_left / self.currentMap.map.tilewidth)
    local lin = math.floor(p_top / self.currentMap.map.tileheight)

    -- calcul de la colonne et de la ligne de la dernière case couverte par le joueur
    local col2 = col + math.floor((p_width) / self.currentMap.map.tilewidth)
    local lin2 = lin + math.floor((p_height) / self.currentMap.map.tileheight)

    -- parcours de toutes les cases couvertes par le joueur
    for c = col, col2 do
        for r = lin, lin2 do
            local index = r * self.currentMap.map.width + c
            if self.currentMap.data[3] then
                if self.currentMap.data[3][index] then
                    if index > 0 then
                        if (self.currentMap.data[3][index] ~= 0) then
                            touch = true
                        end
                    end
                end
            end
        end
    end

    return touch
end

function mapManager:isThereAFloor(p_left, p_top, p_width, p_height, character)
    local floor = false
    local col = math.floor(p_left / self.currentMap.map.tilewidth)
    local lin = math.floor(p_top / self.currentMap.map.tileheight)

    -- calcul de la colonne et de la ligne de la dernière case couverte par le joueur
    local col2 = col + math.floor((p_width) / self.currentMap.map.tilewidth)
    local lin2 = lin + math.floor((p_height) / self.currentMap.map.tileheight)

    -- parcours de toutes les cases couvertes par le joueur
    for c = col, col2 do
        for r = lin, lin2 do
            local index = r * self.currentMap.map.width + c
            if self.currentMap.data[1] then
                if self.currentMap.data[1][index] then
                    if index > 0 then
                        if (self.currentMap.data[1][index] ~= 0) then
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

return mapManager

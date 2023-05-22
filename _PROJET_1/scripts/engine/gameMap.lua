-- MODULE QUI PERMET L'AFFICHAGE D'UNE CARTE A PARTIR D'UN INDEX DONNE ET COMPARE A MAPLIST
local m_gameMap = {}
local GameMap_mt = {__index = m_gameMap}

function m_gameMap.new()
    local gameMap = {}
    return setmetatable(gameMap, GameMap_mt)
end

function m_gameMap:create()
    local gameMap = {
        currentMap = {},
        tileSheet = love.graphics.newImage(PATHS.IMG.ROOT .. "dungeontileset-extended.png"),
        tilesTexture = {},
        mapEnable = true
    }

    function gameMap:setMapTo(list, mapNb)
        self.map = require(list[mapNb])
        self.data = {}
        if self.map.layers[1] then
            self.data[1] = self.map.layers[1].data
        end
        if self.map.layers[2] then
            self.data[2] = self.map.layers[2].data
        end
        if self.map.layers[3] then
            self.data[3] = self.map.layers[3].data
        end
        if self.map.layers[4] then
            self.data[4] = self.map.layers[4].data
        end
        if self.map.layers[5] then
            self.data[5] = self.map.layers[5].data
        end
    end

    function gameMap:loadMap()
        local nbColumns = self.tileSheet:getWidth() / self.map.tilewidth
        local nbLine = self.tileSheet:getHeight() / self.map.tileheight
        local l, c
        local id = 1
        for l = 1, nbLine do
            for c = 1, nbColumns do
                self.tilesTexture[id] =
                    love.graphics.newQuad(
                    (c - 1) * self.map.tilewidth,
                    (l - 1) * self.map.tileheight,
                    self.map.tilewidth,
                    self.map.tileheight,
                    self.tileSheet:getWidth(),
                    self.tileSheet:getHeight()
                )
                id = id + 1
            end
        end
    end

    function gameMap:draw()
        if gameMap.mapEnable then
            for i = 1, #self.data do
                gameMap:drawTiles(i)
            end
            posX = self.map.width * self.map.tilewidth
            posY = (self.map.height * self.map.tileheight) / 2
        end
    end

    function gameMap:drawTiles(i)
        for l = 1, self.map.height do
            for c = 1, self.map.width do
                local index = (l - 1) * self.map.width + c
                local tid = self.data[i][index]

                if tid ~= 0 then
                    local x = (c - 1) * self.map.tilewidth
                    local y = (l - 1) * self.map.tileheight

                    if self.tilesTexture[tid] then
                        love.graphics.draw(self.tileSheet, self.tilesTexture[tid], x, y)
                    end
                end
                -- gameMap:debug(i, 3)
            end
        end
    end

    function gameMap:getName()
        return self.map.layers[3].name
    end

    function gameMap:debug(nb)
        if i == nb then
            local x = (c - 1) * self.map.tilewidth
            local y = (l - 1) * self.map.tileheight
            love.graphics.print(self.data[i][index], x, y)
        end
    end

    return gameMap
end

return m_gameMap

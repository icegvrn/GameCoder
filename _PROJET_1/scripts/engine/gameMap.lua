-- MODULE QUI PERMET L'AFFICHAGE D'UNE CARTE A PARTIR D'UN INDEX DONNE ET COMPARE A MAPLIST
local m_gameMap = {}
local GameMap_mt = {__index = m_gameMap}

-- Création de l'instance gameMap
function m_gameMap.new()
    local gameMap = {}
    return setmetatable(gameMap, GameMap_mt)
end

-- Création d'une nouvelle carte
function m_gameMap:create()
    local gameMap = {
        currentMap = {},
        tileSheet = love.graphics.newImage(PATHS.IMG.ROOT .. "dungeontileset-extended.png"),
        tilesTexture = {},
        mapEnable = true
    }

    -- Fonction pour récupérer la bonne carte et ses données via un index et un fichier de configuration
    -- de map (mapList)
    function gameMap:setMapTo(list, mapNb)
        self.map = require(list[mapNb])
        self.data = {}
        for n = 1, 5 do
            if self.map.layers[n] then
                self.data[n] = self.map.layers[n].data
            end
        end
    end

    -- Fonction qui créée les quads de tiles en fonction des données récupérées
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

    -- Draw des tiles de la carte : pour chaque layer, on draw les tiles
    function gameMap:draw()
        if gameMap.mapEnable then
            for i = 1, #self.data do
                gameMap:drawTiles(i)
            end
            posX = self.map.width * self.map.tilewidth
            posY = (self.map.height * self.map.tileheight) / 2
        end
    end

    -- Draw des tiles de la carte sur un layer (i)
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

    -- Pour le debug : affiche visuellement le contenu des cellules d'un layer en particulier
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

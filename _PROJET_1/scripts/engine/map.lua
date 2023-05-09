local Map = {}
local maps_mt = {__index = Map}

function Map.new()
    local newMap = {}
    newMap.door = {}
    newMap.door.height = 50
    newMap.door.width = 10
    return setmetatable(newMap, maps_mt)
end

function Map:setMapTo(list, mapNb)
    self.map = require(list[mapNb])
    self.data = {}
    self.data[1] = self.map.layers[1].data
    if self.map.layers[2] then
        self.data[2] = self.map.layers[2].data
    end
    if self.map.layers[3] then
        self.data[3] = self.map.layers[3].data
    end

    self:initDoor()
end

function Map:getName()
    return self.name
end

function Map:setName(pName)
    self.name = pName
end

function Map:getData()
    return self.data
end

function Map:getTileWidth()
    return self.tilewidth
end

function Map:initDoor()
    self.width = self.map.width * self.map.tilewidth
    self.height = self.map.height * self.map.tileheight
    self.door.positionX = self.width - self.door.width
    self.door.positionY = self.height / 2
end

return Map

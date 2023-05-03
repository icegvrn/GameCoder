local Map = {}
local maps_mt = {__index = Map}

function Map.new()
    local newMap = {}
    return setmetatable(newMap, maps_mt)
end

function Map:setMapTo(list, mapNb)
    self.map = require(list[mapNb])
    self.data = {}
    self.data[1] = self.map.layers[1].data
    self.data[2] = self.map.layers[2].data
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

return Map

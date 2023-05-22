-- MODULE QUI PERMET D'AJOUTER UN TRANSFORM A N'IMPORTE QUEL OBJET
local Transform = {}
local transform_mt = {__index = Transform}

function Transform.new()
    local _transform = {}
    return setmetatable(_transform, transform_mt)
end

function Transform:create()
    local transform = {
        position = {x = 0, y = 0},
        rotation = {x = 0, y = 0},
        scale = {x = 1, y = 1}
    }

    function transform:getTransform()
        return self.position.x, self.position.y, self.rotation.x, self.rotation.y, self.scale.x, self.scale.y
    end

    function transform:getPosition()
        return self.position.x, self.position.y
    end

    function transform:getRotation()
        return self.rotation.x, self.rotation.y
    end

    function transform:getScale()
        return self.scale.x, self.scale.y
    end

    function transform:setPosition(x, y)
        self.position.x, self.position.y = x, y
    end

    function transform:setRotation(x, y)
        self.rotation.x, self.rotation.y = x, y
    end

    function transform:setScale(x, y)
        self.scale.x, self.scale.y = x, y
    end

    return transform
end

return Transform

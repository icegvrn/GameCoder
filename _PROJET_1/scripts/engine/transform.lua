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
        scale = {x = 1, y = 1}}

    -- function transform:setTransform(p_x, p_y, r_x, r_y, s_x, s_y)
    --     self.position.x, self.position.y = p_x, p_y
    --     if r_x and r_y then
    --         self.rotation.x, self.rotation.y = r_x, r_y
    --         if s_x and sy then
    --             self.scale.x, self.scale.y = s_x, s_y
    --         end
    --     end
    -- end

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

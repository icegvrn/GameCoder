local Transform = {}
local transform_mt = {__index = Transform}

function Transform.new()
    local transform = {position = {x = 0, y = 0}, rotation = {x = 0, y = 0}, scale = {x = 1, y = 1}}
    return setmetatable(transform, transform_mt)
end

function Transform:setTransform(p_x, p_y, r_x, r_y, s_x, s_y)
    self.position.x, self.position.y = p_x, p_y
    if r_x and r_y then
        self.rotation.x, self.rotation.y = r_x, r_y
        if s_x and sy then
            self.scale.x, self.scale.y = s_x, s_y
        end
    end
end

function Transform:getTransform()
    return self.position.x, self.position.y, self.rotation.x, self.rotation.y, self.scale.x, self.scale.y
end

function Transform:getPosition()
    return self.position.x, self.position.y
end

function Transform:getRotation()
    return self.rotation.x, self.rotation.y
end

function Transform:getScale()
    return self.scale.x, self.scale.y
end

function Transform:setPosition(x, y)
    self.position.x, self.position.y = x, y
end

function Transform:setRotation(x, y)
    self.rotation.x, self.rotation.y = x, y
end

function Transform:setScale(x, y)
    self.scale.x, self.scale.y = x, y
end

return Transform

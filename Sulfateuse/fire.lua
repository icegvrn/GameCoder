fire = {}
fire.origins = {}
fire.origins.x = 0
fire.origins.y = 0

function fire.drawOrigins(self)
    love.graphics.setColor(1, 0, 0)
    for k, v in ipairs(fire) do
        love.graphics.circle("fill", v.x, v.y, 2)
    end
end

function fire.getFireOrigins()
    return fire.origins.x, fire.origins.y
end

function fire.fireFromOrigin(self, x, y)
    fire.origins.x, fire.origins.y = x, y
    ball = {}
    ball.x = x
    ball.y = y
    table.insert(fire, ball)
end

function fire.move(self, dt)
    for k, v in ipairs(fire) do
        v.x = v.x + 200 * dt
    end
end

return fire

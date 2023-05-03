local stars = {}
stars.nb = 0
stars.scrollSpeed = 100
stars.directionX = 1
stars.directionY = 0

function stars.Create(self, nb)
    self.nb = nb
    for i = 1, self.nb do
        local randomX = love.math.random(0, love.graphics.getWidth())
        local randomY = love.math.random(0, love.graphics.getHeight())
        local etoile = {}
        etoile.x = randomX
        etoile.y = randomY
        table.insert(self, etoile)
    end
end

function stars.draw(self)
    love.graphics.setColor(1, 1, 1)
    for key, value in ipairs(self) do
        love.graphics.circle("fill", value.x, value.y, 2)
    end
end

function stars.move(self, dt)
    for key, value in ipairs(self) do
        if stars.directionX == -1 then
            value.x = value.x - stars.scrollSpeed * dt
        end
        if stars.directionX == 1 then
            value.x = value.x + stars.scrollSpeed * dt
        end
        if stars.directionY == 1 then
            value.y = value.y - stars.scrollSpeed * dt
        end
        if stars.directionY == -1 then
            value.y = value.y + stars.scrollSpeed * dt
        end
        stars.updateStarsSpawner(value)
    end
end

function stars.updateStarsSpawner(value)
    if value.x < -1 then
        value.x = love.graphics.getWidth()
    elseif value.x > love.graphics.getWidth() + 1 then
        value.x = 0
    end
    if value.y < -1 then
        value.y = love.graphics.getHeight()
    elseif value.y > love.graphics.getHeight() + 1 then
        value.y = 0
    end
end

function stars.setStarsDirection(self, x, y)
    stars.directionX = x
    stars.directionY = y
end

function stars.GetStarsNb(self)
    return self.nb
end

return stars

require("const")
local etoile = require("stars") -- Faire un gameManager pour gérer la ball et l'étoile

local bubble = {}
bubble.speed = 100
bubble.size = 15
bubble.shieldSize = 20
bubble.color = CONST.BUBBLECOLOR.WHITE
bubble.shield = CONST.BUBBLESHIELD.NOSHIELD
bubble.x = love.graphics.getWidth() / 2 - bubble.size / 2
bubble.y = love.graphics.getHeight() / 2 - bubble.size / 2
bubble.directionX = love.math.random(-1, 1)
bubble.directionY = love.math.random(-1, 1)

counterStarted = false
counter = 0

function bubble.load()
    if bubble.directionX == 0 then
        bubble.directionX = love.math.random(-1, 1)
    end

    if bubble.directionY == 0 then
        bubble.directionY = love.math.random(-1, 1)
    end
end

function bubble.update(self, dt)
    self:move(dt)

    if bubble.shield == CONST.BUBBLESHIELD.SHIELD then
        bubble.giveThisBubbleAShield(5, dt)
    end
end

function bubble.draw(self)
    if bubble.color == CONST.BUBBLECOLOR.WHITE then
        love.graphics.setColor(1, 1, 1)
    elseif bubble.color == CONST.BUBBLECOLOR.GREEN then
        love.graphics.setColor(0, 1, 0)
    end

    if bubble.shield == CONST.BUBBLESHIELD.SHIELD then
        love.graphics.setLineWidth(5)
        love.graphics.circle("line", bubble.x, bubble.y, bubble.shieldSize)
        love.graphics.rectangle("fill", 10, love.graphics.getHeight() - 60, 50, 50)
        love.graphics.setColor(0, 0, 0)
        font = love.graphics.newFont(45)
        love.graphics.setFont(font)
        love.graphics.print(math.ceil(counter), 20, love.graphics.getHeight() - 60)
        love.graphics.setColor(1, 1, 1)
    end

    love.graphics.circle("fill", bubble.x, bubble.y, bubble.size)
end

function bubble.move(self, dt)
    local size = bubble.size
    if bubble.shield == CONST.BUBBLESHIELD.SHIELD then
        size = bubble.shieldSize
    end

    self:updateBallInformation(size)

    bubble.directionX, bubble.directionY = self:calcDirection(size)

    bubble.x = bubble.x + bubble.directionX * bubble.speed * dt
    bubble.y = bubble.y + bubble.directionY * bubble.speed * dt

    etoile:setStarsDirection(-bubble.directionX, bubble.directionY)
end

function bubble.updateBallInformation(self, size)
    bubble.bottom = bubble.y + size
    bubble.right = bubble.x + size
    bubble.left = bubble.x - size
    bubble.top = bubble.y - size
end

function bubble.calcDirection(self, size)
    if bubble.left <= 0 then
        -- self:changeColor()
        bubble.x = size
        bubble.directionX = -bubble.directionX
    elseif bubble.right >= love.graphics.getWidth() then
        -- self:changeColor()
        bubble.x = love.graphics.getWidth() - size
        bubble.directionX = -bubble.directionX
    end

    if bubble.top <= 0 then
        -- self:changeColor()
        bubble.y = size
        bubble.directionY = -bubble.directionY
    elseif bubble.bottom >= love.graphics.getHeight() then
        bubble.y = love.graphics.getHeight() - size
        bubble.directionY = -bubble.directionY
    -- self:changeColor()
    end

    return bubble.directionX, bubble.directionY
end

function love.keypressed(key)
    if key == "escape" then
        bubble.shield = CONST.BUBBLESHIELD.NOSHIELD
        bubble.color = CONST.BUBBLECOLOR.WHITE
    end

    if key == "space" then
        bubble.shield = CONST.BUBBLESHIELD.SHIELD
        bubble.color = CONST.BUBBLECOLOR.GREEN
    end
end

function bubble.giveThisBubbleAShield(nb, dt)
    if counterStarted == false then
        counter = nb
        counterStarted = true
    end

    counter = counter - dt

    if counter <= 0 then
        bubble.shield = CONST.BUBBLESHIELD.NOSHIELD
        bubble.color = CONST.BUBBLECOLOR.WHITE
        counterStarted = false
    end
end

-- function bubble.changeColor(self)
--     local r = love.math.random(0.3, 1)
--     local g = love.math.random(0.3, 1)
--     local b = love.math.random(0.3, 1)
--     self.color = {r, g, b}
-- end

return bubble

UITools = require(PATHS.UITOOLS)

local m_capacityButton = {}
local capacityButton_mt = {__index = m_capacityButton}

function m_capacityButton.new()
    local capacityButton = {}
    return setmetatable(capacityButton, capacityButton_mt)
end

function m_capacityButton:create(p_sheet, p_positionX, p_positionY, p_quadNB)
    local button = {
        sheet = love.graphics.newImage(p_sheet),
        currentQuad = nil,
        position = {x = p_positionX, y = p_positionY},
        initialPosition = {x = p_positionX, y = p_positionY},
        quadNB = p_quadNB,
        quadIDLE = nil,
        quadUse = nil,
        quadPower = nil
    }

    function button:init()
        local quadWidth = self.sheet:getWidth() / self.quadNB
        self.quadIDLE = love.graphics.newQuad(0, 0, quadWidth, self.sheet:getHeight(), self.sheet:getDimensions())
        self.quadUse =
            love.graphics.newQuad(quadWidth, 0, quadWidth, self.sheet:getHeight(), self.sheet:getDimensions())
        if self.quadNB == 3 then
            self.quadPower =
                love.graphics.newQuad((quadWidth * 2), 0, quadWidth, self.sheet:getHeight(), self.sheet:getDimensions())
        end
        self.currentQuad = self.quadIDLE
    end

    function button:update(dt)
    end

    function button:draw()
        self.position.x, self.position.y =
            Utils.screenCoordinates(self.initialPosition.x, Utils.screenHeight + self.initialPosition.y)
        love.graphics.draw(self.sheet, self.currentQuad, self.position.x, self.position.y)

        if self.text and self.text.isVisible then
            button:drawText()
        elseif self.timer then
            button:drawTimer()
        end
    end

    function button:drawText()
        love.graphics.setColor(self.text.bgColors)
        love.graphics.rectangle(
            "fill",
            self.position.x,
            self.position.y + self.sheet:getHeight() - self.text.content:getHeight() - 5,
            self.text.content:getWidth() + 2,
            self.text.content:getHeight() + 2
        )

        love.graphics.setColor(self.text.color)

        love.graphics.draw(
            self.text.content,
            self.position.x,
            self.position.y + self.sheet:getHeight() - self.text.content:getHeight() - 5
        )
        love.graphics.setColor(1, 1, 1)
    end

    function button:addText(string, color, bgColors)
        self.text = {}
        self.text.isVisible = true
        self.text.color = color
        self.text.bgColors = bgColors
        self.text.content = love.graphics.newText(UITools.font9, string)
    end

    function button:addTimer(duration, positionX, positionY)
        self.timer = {}
        self.timer.count = duration
        self.timer.isStarted = false
        self.timer.position = {x = positionX, y = positionY}
    end

    function button:drawTimer()
        love.graphics.setFont(UITools.defaultFont)
        love.graphics.print(
            math.floor(self.timer.count),
            self.position.x + self.timer.position.x,
            self.position.y + self.timer.position.y
        )
    end

    return button
end

return m_capacityButton

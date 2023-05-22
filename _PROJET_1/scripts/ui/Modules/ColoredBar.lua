-- MODULE QUI PERMET DE CREER UNE NOUVELLE BARRE COLOREE, QUI PEUT ETRE BLINKABLE ET SOUNDABLE AU BESOIN
local m_coloredBar = {}
local coloredBar_mt = {__index = m_coloredBar}

function m_coloredBar.new()
    local coloredBar = {}
    return setmetatable(coloredBar, coloredBar_mt)
end

function m_coloredBar:create(
    p_colors,
    p_thresholds,
    p_maxSize,
    p_barHeight,
    p_maxData,
    p_positionX,
    p_positionY,
    p_attachToWorld,
    p_soundable,
    p_blinkable)
    local coloredBar = {
        maxSize = p_maxSize,
        currentSize = p_maxSize,
        currentData = p_maxData,
        maxData = p_maxData,
        height = p_barHeight,
        colors = p_colors,
        currentColor = p_colors[1],
        thresholds = p_thresholds,
        position = {x = p_positionX, y = p_positionY},
        initialPosition = {x = p_positionX, y = p_positionY},
        isAttachedToWorld = p_attachToWorld,
        canPlaySoundNotification = true,
        isSoundable = p_soundable,
        isBlinkable = p_blinkable,
        timer = 0
    }

    function coloredBar:update(dt, positionX, positionY)
        coloredBar:updateColor()

        if self.isAttachedToWorld then
            coloredBar:updatePosition(positionX, positionY)
        end

        if self.isBlinkable then
            coloredBar:blink(dt)
        end
    end

    function coloredBar:updatePosition(positionX, positionY)
        self.position.x = positionX
        self.position.y = positionY
    end

    function coloredBar:updateColor()
        for n = 1, #self.thresholds do
            if self.currentData <= self.thresholds[n] then
                self.currentColor = self.colors[n]
                break
            end
        end
    end

    function coloredBar:updateData(data, maxData)
        self.currentData = data
        self.maxData = maxData
    end

    function coloredBar:draw()
        local x, y = self.position.x + self.initialPosition.x, self.position.y + self.initialPosition.y
        if not self.isAttachedToWorld then
            x, y = Utils.screenCoordinates(self.initialPosition.x, self.initialPosition.y)
        end

        love.graphics.setColor(1, 1, 1, 0.3)
        love.graphics.rectangle("fill", x, y, self.maxSize, self.height)
        love.graphics.setColor(self.currentColor)
        local size = (self.currentData / self.maxData) * self.maxSize
        love.graphics.rectangle("fill", x, y, size, self.height)
        love.graphics.setColor(1, 1, 1)
    end

    function coloredBar:blink(dt)
        if self.currentData == self.maxData then
            if self.isSoundable then
                self:Soundnotification()
            end

            self.timer = self.timer + dt

            if self.timer % 2 > 1 then
                self.currentColor = self.colors[#self.colors - 1]
            else
                self.currentColor = self.colors[#self.colors]
            end
        elseif self.currentData == 0 then
            self:resetBar()
        end
    end

    function coloredBar:Soundnotification()
        if self.canPlaySoundNotification then
            soundManager:playSound(PATHS.SOUNDS.GAME .. "boostCharge.wav", 0.5, false)
            self.canPlaySoundNotification = false
        end
    end

    function coloredBar:resetBar()
        self.canPlaySoundNotification = true
    end

    return coloredBar
end

return m_coloredBar

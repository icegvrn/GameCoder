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

    -- La fonction update mets à jour à la fois la couleur et la position de la barre, tenant compte du fait qu'elle est attachée au monde ou à l'écran
    -- ou si elle est mise comme étant "blinkable".
    function coloredBar:update(dt, positionX, positionY)
        coloredBar:updateColor()

        if self.isAttachedToWorld then
            coloredBar:updatePosition(positionX, positionY)
        end

        if self.isBlinkable then
            coloredBar:blink(dt)
        end
    end

    -- Fonction qui met à jour la position.
    function coloredBar:updatePosition(positionX, positionY)
        self.position.x = positionX
        self.position.y = positionY
    end

    -- Fonction qui update la couleur de la barre : fonctionne par seuil. Si la barre passe en dessous/dessus un certain seuil elle change de couleur.
    function coloredBar:updateColor()
        for n = 1, #self.thresholds do
            if self.currentData <= self.thresholds[n] then
                self.currentColor = self.colors[n]
                break
            end
        end
    end

    -- Fonction qui permet de faire passer les infos de data et maxData à mettre à jour dans la barre (maxData = barre à 100% et data = données qui évoluent)
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

    -- Fonction en update qui vérifie lorsqu'une barre est blinkable si elle doit blinker. Elle le fait lorsqu'elle atteint 100%.
    -- Si elle est "soundable", elle fait une notification sonore en plus une seule fois.
    -- Lorsque le barre retombe à zéro, on reset la possibilité de jouer une notification via resetBar()
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

    -- Fonction qui appelle le soundManager et permet de jouer une notification sonore lorsque la barre est pleine.
    function coloredBar:Soundnotification()
        if self.canPlaySoundNotification then
            soundManager:playSound(PATHS.SOUNDS.GAME .. "boostCharge.wav", 0.5, false)
            self.canPlaySoundNotification = false
        end
    end

    -- Permet de réinitialiser le booléen permettant de lire un son lorsqu'elle atteint pour la première fois 100%
    function coloredBar:resetBar()
        self.canPlaySoundNotification = true
    end

    return coloredBar
end

return m_coloredBar

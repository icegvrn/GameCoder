-- MODULE QUI PERMET DE CREER FACILEMENT UN BOUTON INTERACTIF POUR LE MENU
local m_uiButton = {}
local uiButton_mt = {__index = m_uiButton}

function m_uiButton.new()
    local uiButton = {}
    return setmetatable(uiButton, uiButton_mt)
end

function m_uiButton:create(sheetUrl, positionY, state, p_function)
    local button = {
        sheet = nil,
        current = nil,
        position = {x = 0, y = 0},
        width = 0,
        height = 0,
        isHover = false,
        state = nil,
        quadIdle = nil,
        quadHover = nil,
        action = p_function
    }

    button.sheet = love.graphics.newImage(sheetUrl)
    button.position.x = Utils.screenWidth / 2 - button.sheet:getWidth() / 4
    button.position.y = positionY
    button.width = button.sheet:getWidth() / 2
    button.height = button.sheet:getHeight()
    button.isHover = false
    button.state = state
    button.quadIdle =
        love.graphics.newQuad(0, 0, button.width, button.height, button.sheet:getWidth(), button.sheet:getHeight())
    button.quadHover =
        love.graphics.newQuad(
        button.width,
        0,
        button.width,
        button.height,
        button.sheet:getWidth(),
        button.sheet:getHeight()
    )
    button.current = button.quadIdle

    function button:hover()
        self.current = self.quadHover
        self.isHover = true
    end

    function button:leave()
        self.current = self.quadIdle
        self.isHover = false
    end

    function button:interaction(parent)
        parent.action(self.action)
    end

    function button:draw()
        love.graphics.draw(self.sheet, self.current, self.position.x, self.position.y)
    end

    return button
end

return m_uiButton

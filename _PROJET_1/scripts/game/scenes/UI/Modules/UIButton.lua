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

    -- Initialise le bouton à partir des données renseignées dans le create.
    function button:init()
        self.sheet = love.graphics.newImage(sheetUrl)
        self.position.x = Utils.screenWidth / 2 - self.sheet:getWidth() / 4
        self.position.y = positionY
        self.width = self.sheet:getWidth() / 2
        self.height = self.sheet:getHeight()
        self.isHover = false
        self.state = state
        self.quadIdle =
            love.graphics.newQuad(0, 0, self.width, self.height, self.sheet:getWidth(), self.sheet:getHeight())
        self.quadHover =
            love.graphics.newQuad(self.width, 0, self.width, self.height, self.sheet:getWidth(), self.sheet:getHeight())
        self.current = self.quadIdle
    end

    -- Permet d'ajouter un hover au bouton, modifie le quad.
    function button:hover()
        self.current = self.quadHover
        self.isHover = true
    end

    -- Permet d'ajouter un leave au bouton, modifie le quad.
    function button:leave()
        self.current = self.quadIdle
        self.isHover = false
    end

    -- Permet d'ajouter une interaction au bouton. Fonctionne grâce à une string self.action qu'il renvoie au parent qui l'appelle.
    function button:interaction(parent)
        parent.action(self.action)
    end

    -- Fonction draw du bouton
    function button:draw()
        love.graphics.draw(self.sheet, self.current, self.position.x, self.position.y)
    end

    return button
end

return m_uiButton

UIColoredBar = require(PATHS.UICOLOREDBAR)
UICapacityButton = require(PATHS.UICAPACITYBUTTON)

local c_PlayerUI = {}
local PlayerUI_mt = {__index = c_PlayerUI}

function c_PlayerUI.new()
    local PlayerUI = {}
    local UIColoredBar = UIColoredBar.new()
    local UICapacityButton = UICapacityButton.new()
    return setmetatable(PlayerUI, PlayerUI_mt)
end

function c_PlayerUI:create()
    local playerUI = {}

    function playerUI:load(player)
        self:createPlayerBars(player)
        self:createPlayerCapacityButtons(player)
    end

    function playerUI:update(dt, player)
        self:updateBars(dt, player)
        self:updatePlayerCapacityButtons(dt, player)
        self:updateTimers(player)
    end

    function playerUI:draw()
        self:drawPlayerBars()
        self:drawPlayerCapacityButtons()
    end

    function playerUI:createPlayerCapacityButtons(player)
        local x, y = Utils.screenCoordinates(10, (Utils.screenHeight - 40))
        self.weaponButton = UICapacityButton:create(PATHS.IMG.UI .. "weaponButtons.png", 10, -90, 2)
        self.weaponButton:init()
        self.weaponButton:addText(controller.action1, {0, 0, 0}, {1, 0.7, 0})
        self.boosterButton = UICapacityButton:create(PATHS.IMG.UI .. "boostedButtons.png", 10, -45, 3)
        self.boosterButton:init()
        self.boosterButton:addText(controller.action2, {0, 0, 0}, {1, 0.7, 0})
        self.boosterButton:addTimer(player.playerBooster.boosterDuration, 2, 2)
    end

    function playerUI:createPlayerBars(player)
        self:createPlayerLifeBar(player)
        self:createPlayerPointsBar(player)
    end

    function playerUI:updateBars(dt, player)
        self.lifeBar:update(dt, player.character.transform.position.x, player.character.transform.position.y)
        self.lifeBar:updateData(player.character.fight.currentPV, player.character.fight.maxPV)
        self.pointsBar:update(dt, player.character.transform.position.x, player.character.transform.position.y)
        self.pointsBar:updateData(player.pointsCounter.points, player.pointsCounter.maxPoints)
    end

    function playerUI:drawPlayerBars()
        self.lifeBar:draw()
        self.pointsBar:draw()
    end

    function playerUI:drawPlayerCapacityButtons()
        self.weaponButton:draw()
        self.boosterButton:draw()
    end

    function playerUI:updatePlayerCapacityButtons(dt, player)
        if player.pointsCounter.points == player.pointsCounter.maxPoints then
            self.boosterButton.currentQuad = self.boosterButton.quadPower
            if player.character:getState() == CHARACTERS.STATE.FIRE then
                self.weaponButton.currentQuad = self.weaponButton.quadIDLE
            else
                self.weaponButton.currentQuad = self.weaponButton.quadUse
            end
        elseif player.character:getMode() == CHARACTERS.MODE.BOOSTED then
            self.boosterButton.currentQuad = self.boosterButton.quadUse
            self.weaponButton.currentQuad = self.weaponButton.quadIDLE
        elseif player.character:getState() == CHARACTERS.STATE.FIRE then
            self.weaponButton.currentQuad = self.weaponButton.quadIDLE
            self.boosterButton.currentQuad = self.boosterButton.quadIDLE
        else
            self.weaponButton.currentQuad = self.weaponButton.quadUse
            self.boosterButton.currentQuad = self.boosterButton.quadIDLE
        end
    end

    function playerUI:updateTimers(player)
        if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
            self.boosterButton.text.isVisible = false
            self.weaponButton.text.isVisible = false
            self.boosterButton.timer.count = player.playerBooster.boosterTimer + 1
        else
            self.boosterButton.text.isVisible = true
            self.weaponButton.text.isVisible = true
        end
    end

    function playerUI:createPlayerLifeBar(player)
        local colors = {{1, 0, 0}, {1, 1, 0}, {0.06, 0.69, 0.27}}
        local thresholds = {
            player.character.fight.maxPV / 4,
            player.character.fight.maxPV / 2,
            player.character.fight.maxPV
        }
        local barSize = 70
        local barHeight = 10
        local positionX, positionY = -35, -35
        self.lifeBar =
            UIColoredBar:create(
            colors,
            thresholds,
            barSize,
            barHeight,
            player.character.fight.maxPV,
            positionX,
            positionY,
            true,
            false,
            false
        )
        return playerUI.lifeBar
    end

    function playerUI:createPlayerPointsBar(player)
        local colors = {{0.8, 0.5, 0.2}, {0.8, 0.4, 0.8}, {0.8, 0.1, 0.6}, {0.8, 0, 0.2}, {1, 1, 1}}
        local thresholds = {
            (player.pointsCounter.maxPoints / 4),
            (player.pointsCounter.maxPoints / 2),
            (player.pointsCounter.maxPoints / 1.3),
            (player.pointsCounter.maxPoints)
        }
        local barSize = 200
        local barHeight = 15
        local positionX, positionY = 55, (Utils.screenHeight - 35)

        self.pointsBar =
            UIColoredBar:create(
            colors,
            thresholds,
            barSize,
            barHeight,
            player.pointsCounter.maxPoints,
            positionX,
            positionY,
            false,
            true,
            true
        )
        return playerUI.pointsBar
    end

    return playerUI
end

return c_PlayerUI

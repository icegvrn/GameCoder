local ui = require("scripts/ui/uiGame")

local c_PlayerUI = {}
local PlayerUI_mt = {__index = c_PlayerUI}

function c_PlayerUI.new()
    local PlayerUI = {}
    return setmetatable(PlayerUI, PlayerUI_mt)
end

function c_PlayerUI:create()
    local playerUI = {}

    function playerUI:update(dt, player)
        self:updatePVbar(dt, player)
    end

    function playerUI:updatePVbar(dt, player)
        ui.updatePlayerLifeBar(player.character.fight.currentPV, player.character.fight.maxPV)
        ui.updatePlayerPointsBar(dt, player, player.pointsCounter.points, player.pointsCounter.maxPoints)
    end

    function playerUI:draw(player)
        local x, y = player.character.transform:getPosition()
        ui.drawPlayerLifeBar(
            x,
            y,
            player.character.fight.maxPV,
            player.character.fight.currentPV,
            player.character.transform.scale.x,
            player.character.transform.scale.y
        )

        ui.drawPlayerPointBar(player, player.pointsCounter.points, player.pointsCounter.maxPoints)
    end

    return playerUI
end

return c_PlayerUI

-- Contient tout l'UI du jeu

ui = {}

ui.player = {}
ui.player.maxPVBarSize = 70
ui.player.currentPVBarSize = 0
ui.player.lifeColor = {}
ui.player.lifeColor[1] = {0.06, 0.69, 0.27}
ui.player.lifeColor[2] = {1, 1, 0}
ui.player.lifeColor[3] = {1, 0, 0}
ui.player.currentLifeColor = ui.player.lifeColor[1]

function ui.load()
end

function ui.update(dt)
end

function ui.draw()
end

function ui.updatePlayerLifeBar(currentPV, maxPV)
    if currentPV < 0 then
        ui.player.currentPVBarSize = 0
    else
        ui.player.currentPVBarSize = (currentPV / maxPV) * ui.player.maxPVBarSize
    end

    if currentPV > maxPV / 2 then
        ui.player.currentLifeColor = ui.player.lifeColor[1]
    elseif currentPV < maxPV / 2 and currentPV > maxPV / 4 then
        ui.player.currentLifeColor = ui.player.lifeColor[2]
    elseif currentPV <= currentPV / 4 then
        ui.player.currentLifeColor = ui.player.lifeColor[3]
    end
end

function ui.drawPlayerLifeBar(position_x, position_y, maxPV, currentPV, scale_x, scale_y)
    love.graphics.setColor(1, 1, 1, 0.3)
    love.graphics.rectangle("fill", position_x - 35, position_y - 35, ui.player.maxPVBarSize, 10, scale_x, scale_y)
    love.graphics.setColor(ui.player.currentLifeColor)
    love.graphics.rectangle("fill", position_x - 35, position_y - 35, ui.player.currentPVBarSize, 10, scale_x, scale_y)
    love.graphics.setColor(1, 1, 1)
end

return ui

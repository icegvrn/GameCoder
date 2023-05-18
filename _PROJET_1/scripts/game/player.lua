character = require("scripts/engine/character")
controller = require("scripts/engine/controller")
camera = require("scripts/game/mainCamera")
local map = require("scripts/game/gameMap")
require("scripts/states/CHARACTERS")
GAMESTATE = require("scripts/states/GAMESTATE")
--local ui = require("scripts/ui/uiGame")

-- Création d'un player basé sur Character
player = {}
player.character = nil
player.points = 50
player.maxPoints = 50
player.boosterDuration = 5
player.boosterTimer = 5
keyDown = false

-- function player.update(dt)
--     -- local gmap = map.getCurrentMap()
--     -- x, y = player.character.transform:getPosition()

--     -- local w, h = player.character.sprites:getDimension(player.character.mode, player.character.state)

--     -- -- Remplacer 16 par "largeur des tuiles"
--     -- if (map.isThereASolidElement(x + 16, y + 16, w - 16, h - 16, player.character)) then
--     --     player.character.canMove = false
--     -- else
--     --     player.character.canMove = true
--     -- end

--     -- if player.character.fight.currentPV <= 0 then
--     --     soundManager:playSound("contents/sounds/game/heros_death.wav", 0.3, false)
--     --     GAMESTATE.currentState = GAMESTATE.STATE.GAMEOVER
--     -- end

--     -- player.character:update(dt)

--     -- if player.character.controller:isInCinematicMode() == false then
--     --     player.updatePVbar(dt)
--     --     player.updateMode(dt)
--     --     if player.character.canMove then
--     --         lastX, lastY = player.character.transform:getPosition()
--     --         player.updatePosition(dt)
--     --     else
--     --         player.character:setPosition(lastX, lastY)
--     --     end

--     --     if player.useAction(controller.action1) then
--     --         player.fire(dt)
--     --     end
--     -- end
-- end

-- function player.fire(dt)
--     player.character.controller.player:fire(dt, player.character)
-- end

-- function player.changeState(state)
--     player.character:setState(state)
-- end

-- function player.draw()
--     player.character:draw()
--     if player.character.controller:isInCinematicMode() == false then
--         player.drawUI()
--     end
-- end

-- function player.updatePosition(dt)
--     if map.isOverTheMap(player.character.transform:getPosition()) == false then
--         player.move(dt)
--     else
--         player.character:setPosition(map.clamp(player.character.transform:getPosition()))
--     end
-- end

-- function player.move(dt)
--     if
--         love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
--             love.keyboard.isDown(controller.left) or
--             love.keyboard.isDown(controller.right)
--      then
--         if player.character:getState() ~= CHARACTERS.STATE.WALKING then
--             player.changeState(CHARACTERS.STATE.WALKING)
--         end

--         x, y = player.character.transform:getPosition()
--         local sX, sY = player.character.transform:getScale()
--         local speed = player.character.controller.speed
--         local angle = utils.angleWithMouseWorldPosition(player.character.transform:getPosition())
--         local velocityX = speed * dt
--         local velocityY = speed * dt

--         if love.keyboard.isDown(controller.up) then
--             y = y - velocityY
--         elseif love.keyboard.isDown(controller.down) then
--             y = y + velocityY
--         elseif love.keyboard.isDown(controller.left) then
--             x = x - velocityX
--         elseif love.keyboard.isDown(controller.right) then
--             x = x + velocityX
--         end
--         player.setPosition(x, y)
--     elseif player.character:getState() ~= CHARACTERS.STATE.IDLE or player.character:getMode() ~= lastMode then
--         player.changeState(CHARACTERS.STATE.IDLE)
--         lastMode = player.character:getMode()
--     end
-- end

-- function player.updatePVbar(dt)
--     ui.updatePlayerLifeBar(player.character.fight.currentPV, player.character.fight.maxPV)
--     ui.updatePlayerPointsBar(dt, player.points, player.maxPoints)
-- end

-- function player.drawUI()
--     local x, y = player.character.transform:getPosition()
--     ui.drawPlayerLifeBar(
--         x,
--         y,
--         player.character.fight.maxPV,
--         player.character.fight.currentPV,
--         player.scaleX,
--         player.scaleY
--     )

--     ui.drawPlayerPointBar(player.points, player.maxPoints)
-- end

-- -- A METTRE DANS CHARACTER ?
-- -- function player.updateMode(dt)
-- --     if player.character:getMode() == CHARACTERS.MODE.BOOSTED then
-- --         player.boosterTimer = player.boosterTimer - 1 * dt
-- --         if player.boosterTimer <= 0 then
-- --             player.character:setMode(CHARACTERS.MODE.NORMAL)
-- --             soundManager:playSound("contents/sounds/game/endPlayerBoost.wav", 0.5, false)
-- --             player.boosterTimer = player.boosterDuration
-- --         end
-- --     end
-- -- end
-- -------------------------------------

-- function player.addPoints(points)
--     player.character.controller.player:addPoints(points)
-- end

-- function player.resetPoints()
--     player.character.controller.player:resetPoints()
-- end

-- function player.playEntranceAnimation(dt)
--     player.character.controller.player:playEntranceAnimation(dt)
-- end

-- function player.useAction(action)
--     return player.character.controller.player:useAction(action)
-- end

-- function player.setPosition(x, y)
--     player.character:setPosition(x, y)
-- end

-- function player.keypressed(key)
--     player.character.controller.player:keypressed(player.character, key)
-- end

-- function player.setCharacter(character)
--     player.character = character
-- end

-- function player.setInCinematicMode(bool)
--     player.character.controller:setInCinematicMode(player.character, bool)
-- end

return player

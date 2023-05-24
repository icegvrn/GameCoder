-- MODULE QUI GERE LES ACTIONS DU PLAYER EN FONCTION DES TOUCHES UTILISEES PAR LE JOUEUR
local c_PlayerInput = {}
local PlayerInput_mt = {__index = c_PlayerInput}

function c_PlayerInput.new()
    local PlayerInput = {}
    return setmetatable(PlayerInput, PlayerInput_mt)
end

function c_PlayerInput:create()
    local playerInput = {}

    -- Update du playerInput : si le personnage a pu bouger on enregistre sa dernière position (utilisé pour collision)
    -- sinon on le met à sa dernière position connue, puis on bouge le perso.
    -- Si le joueur utilise "action1" dans ses contrôles, on appelle la fonction Fire du personnage.
    function playerInput:update(dt, player)
        if player.character.controller.canMove then
            player.lastX, player.lastY = player.character.transform:getPosition()
        else
            player.character.transform:setPosition(player.lastX, player.lastY)
        end

        self:move(dt, player)

        if self:useAction(controller.action1) then
            self:Action1(dt, player)
        end
    end

    -- Fonction permettant de vérifier si le joueur utilise une action de souris ou un keyboardDown
    -- Utilisé pour pouvoir utiliser des constants type "mouse1" dans le fichier de conf CONTROLLER
    function playerInput:useAction(action)
        if action == "mouse1" then
            return love.mouse.isDown(1)
        elseif action == "mouse2" then
            return love.mouse.isDown(2)
        elseif action == "mouse3" then
            return love.mouse.isDown(3)
        else
            return love.keyboard.isDown(action)
        end
    end

    -- Si le joueur utilise la touche espace, il essaie d'appeler la fonction boost du booster
    function playerInput:keypressed(key, player)
        if key == controller.action2 then
            player.playerBooster:boost(player.character, player.pointsCounter)
        end
    end

    -- Fonction qui appelle les bonnes fonctions quand la touche action principale a été utilisée : appelle fire et update le state en conséquence
    function playerInput:Action1(dt, player)
        player.character:setState(CHARACTERS.STATE.FIRE)
        player.character:fire(dt)
    end

    -- Fonction qui permet de faire bouger le personnage. Modifie également le state en conséquence : walking ou idle
    function playerInput:move(dt, player)
        if
            love.keyboard.isDown(controller.up) or love.keyboard.isDown(controller.down) or
                love.keyboard.isDown(controller.left) or
                love.keyboard.isDown(controller.right)
         then
            if player.character:getState() ~= CHARACTERS.STATE.WALKING then
                player.character:setState(CHARACTERS.STATE.WALKING)
            end

            local x, y = player.character.transform:getPosition()
            local speed = player.character.controller.speed
            local velocityX = speed * dt
            local velocityY = speed * dt

            -- Modification de la vélocité en fonction de la touche sur laquelle on appuie
            if love.keyboard.isDown(controller.up) then
                y = y - velocityY
            elseif love.keyboard.isDown(controller.down) then
                y = y + velocityY
            elseif love.keyboard.isDown(controller.left) then
                x = x - velocityX
            elseif love.keyboard.isDown(controller.right) then
                x = x + velocityX
            end

            player.character.collider:setNextMove(player.character, x, y)

            if player.character.controller.canMove then
                player.character.transform:setPosition(x, y)
            end
        elseif player.character:getState() == CHARACTERS.STATE.WALKING or player.character:getMode() ~= lastMode then
            player.character:setState(CHARACTERS.STATE.IDLE)
            lastMode = player.character:getMode()
        end
    end

    function playerInput:draw(player)
    end

    return playerInput
end

return c_PlayerInput

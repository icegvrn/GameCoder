-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

local gameManager = require("gameManager")
local weapons = require("weapons")
local timer = require("timer")

canFire = false

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

function love.load()
    -- Initialisation du GM pour faire le lien GM > Player, et on set le GUN comme arme au démarrage
    weapons:Init()
    gameManager:Init()
    gameManager:setPlayerCurrentWeapon(weapons.GUN)
end

function love.update(dt)
    -- Tirer en continue pour AUTO et RAFALE, avec le bon comportement selon arme
    if love.keyboard.isDown("space") then
        if gameManager.getPlayerCurrentWeapon().getID() == weapons.AUTO then
            gameManager.getPlayerCurrentWeapon().setFireTimer(gameManager.getPlayerCurrentWeapon().getFireTimer() + dt)

            if gameManager.getPlayerCurrentWeapon().getFireTimer() >= 0.6 then
                gameManager.getPlayerCurrentWeapon().fire()
                gameManager.getPlayerCurrentWeapon().setFireTimer(0)
            end
        elseif gameManager.getPlayerCurrentWeapon().getID() == weapons.RAFALE then
            if canFire == true then
                gameManager.getPlayerCurrentWeapon().setFireTimer(
                    gameManager.getPlayerCurrentWeapon().getFireTimer() + dt
                )
                if gameManager.getPlayerCurrentWeapon().getFireTimer() >= 0.2 then
                    gameManager.getPlayerCurrentWeapon().fire()
                    gameManager.getPlayerCurrentWeapon().setFireTimer(0)
                end
            end

            if timer:isStarted() == true then
                timer:runTimer(dt)
            else
                canFire = false
            end
        end
    end
    -- Mouvement des éventuelles balles qui existent
    fire:move(dt)
end

function love.draw()
    gameManager:drawPlayerWeapon()
end

function love.keypressed(key)
    -- Changement de l'arme sur touche numérique 1, 2, 3
    if key == "kp1" then
        gameManager:setPlayerCurrentWeapon(weapons.GUN)
    elseif key == "kp2" then
        gameManager:setPlayerCurrentWeapon(weapons.AUTO)
    elseif key == "kp3" then
        gameManager:setPlayerCurrentWeapon(weapons.RAFALE)
    end

    -- Le comportement si l'arme est le GUN
    if key == "space" then
        if gameManager.getPlayerCurrentWeapon().getID() == weapons.GUN then
            gameManager.getPlayerCurrentWeapon().fire()
        end

        -- Redonner la possibilité de tirer avec le RAFALE si on appuie de nouveau sur SPACE
        if gameManager.getPlayerCurrentWeapon().getID() == weapons.RAFALE then
            if canFire == false then
                timer:newTimer(2)
                canFire = true
            end
        end
    end
end

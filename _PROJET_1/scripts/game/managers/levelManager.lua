-- LE LEVEL MANAGER GERE TOUT CE QUI EST PROPRE AU NIVEAU : LE NOMBRE D'ENNEMI, LEUR TYPE, LEURS ARMES, LA MAP A AFFICHER
local c_factory = require("scripts/game/factories/characterFactory")
local w_factory = require("scripts/game/factories/weaponFactory")
local levelsConfig = require("scripts/game/levelsConfiguration")
local soundManager = require("scripts/game/managers/soundManager")
local camera = require("scripts/game/mainCamera")
local map = require("scripts/game/gameMap")
require("scripts/states/CHARACTERS")
require("scripts/states/WEAPONS")
local ui = require("scripts/ui/uiGame")

levelManager = {}
levelManager.currentLevel = 1
levelManager.charactersList = {}
levelManager.ennemiesList = {}
levelManager.exitDoor = nil

levelManager.LEVELSTATE = {}
levelManager.LEVELSTATE.start = "start"
levelManager.LEVELSTATE.game = "game"
levelManager.LEVELSTATE.win = "win"
levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.start

levelManager.cinematic = {}
levelManager.cinematic.timer = 0
levelManager.cinematic.IsStarted = false
levelManager.cinematic.lenght = 3

levelManager.isTheGameFinish = false
levelManager.doorOpened = false

levelManager.player = nil

function levelManager.load()
    levelManager.spawnPlayer()
    levelManager.player = c_factory.getPlayer()
    levelManager.spawnEnnemies()
    levelManager.exitDoor = map.getDoor()
    gameMap.initMap(levelManager.currentLevel)
end

function levelManager.update(dt)
    if levelManager.LEVELSTATE.currentState == levelManager.LEVELSTATE.start then
        levelManager.playLevelCinematic(dt)
    elseif levelManager.LEVELSTATE.currentState == levelManager.LEVELSTATE.game then
        levelManager.updateGame(dt)

        if #levelManager.ennemiesList <= 0 then
            levelManager.openDoor()
            levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.win
        end
    elseif levelManager.LEVELSTATE.currentState == levelManager.LEVELSTATE.win then
        levelManager.checkDoor(levelManager.exitDoor)
    end
    levelManager.player:update(dt)
end

function levelManager.draw()
    for n = #levelManager.ennemiesList, 1, -1 do
        levelManager.ennemiesList[n]:draw()
    end
    levelManager.player:draw()
end

function levelManager.updateGame(dt)
    for n = #levelManager.ennemiesList, 1, -1 do
        levelManager.ennemiesList[n]:update(dt)
    end
end

function levelManager.openDoor()
    map.openDoor()
    ui.doorIsOpen(true)
end

function levelManager.checkDoor(exitDoor)
    playerX, playerY = levelManager.player.character:getPosition()
    playerH, playerW =
        levelManager.player.character.sprites:getDimension(
        levelManager.player.character.mode,
        levelManager.player.character.state
    )

    if
        Utils.isCollision(
            exitDoor.positionX,
            exitDoor.positionY,
            exitDoor.width,
            exitDoor.height,
            playerX,
            playerY,
            playerH,
            playerW
        )
     then
        levelManager.nextLevel()
    end
end

function levelManager.setCurrentLevel(nb)
    levelManager.currentLevel = nb
end

function levelManager.getCurrentLevel()
    return levelManager.currentLevel
end

-- Fonction pour afficher la bonne map au bon level
function levelManager.getMap(lvl)
end

-- Fonction pour faire spawn les ennemis au bon endroit

function levelManager.spawnEnnemies()
    local map = require("scripts/game/gameMap")
    local mapWidth, mapHeight = map.getMapDimension()
    local currentLvlList = levelsConfig.getEnnemiesByLvl(levelManager.currentLevel)
    if currentLvlList then
        for n = 1, #currentLvlList do
            local type = currentLvlList[n][1]
            local weapon = currentLvlList[n][2]
            local number = currentLvlList[n][3]
            for i = 1, number do
                local ennemi = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, type, false, myCharacter.character)
                local ennemiWeapon = w_factory.createWeapon(weapon)
                ennemi.character:equip(ennemiWeapon)
                c_w, c_h = ennemi.character.sprites:getDimension(ennemi.character.mode, ennemi.character.state)
                positionX, positionY = levelManager.findSpawnPoint(mapWidth, mapHeight, c_w, c_h)
                ennemi.character.transform:setPosition(positionX, positionY)
                mapWidth, mapHeight = map.getMapDimension()
                -- ennemi:setPosition(mapWidth / 2, mapHeight / 2)

                table.insert(levelManager.ennemiesList, ennemi)
                table.insert(levelManager.charactersList, ennemi)
            end
        end
    end
end

function levelManager.findSpawnPoint(mapWidth, mapHeight, c_w, c_h)
    local pX = love.math.random(0, mapWidth)
    local pY = love.math.random(0, mapHeight)
    local spawnFounded = false
    while spawnFounded == false do
        pX = love.math.random(0, mapWidth)
        pY = love.math.random(0, mapHeight)

        if (map.isThereASolidElement(pX, pY, c_w, c_h)) == false and (map.isThereAFloor(pX, pY, c_w, c_h)) then
            spawnFounded = true
            return pX, pY
        end
    end
end

function levelManager.spawnPlayer()
    -- Fais spawn le player à un endroit précis du jeu
    myHeroWeapon = w_factory.createWeapon(WEAPONS.TYPE.HERO_MAGIC_STAFF)
    myWeapon4 = w_factory.createWeapon(WEAPONS.TYPE.BITE)
    myCharacter = c_factory.createCharacter(CHARACTERS.CATEGORY.PLAYER, CHARACTERS.TYPE.ORC, true, love.mouse)
    myCharacter.character:equip(myHeroWeapon)
    myCharacter.character:equip(myWeapon4)
    table.insert(levelManager.charactersList, myCharacter)
    return myCharacter
end

function levelManager.keypressed(key)


    if key == "m" then
            levelManager.nextLevel()
    end

    for n = 1, #levelManager.charactersList do
        levelManager.charactersList[n]:keypressed(key)
    end

    
end

function levelManager.getListofEnnemiesCharacters()
    local charactersList = {}
    for n = 1, #levelManager.ennemiesList do
        charactersList[n] = levelManager.ennemiesList[n].character
    end
    return charactersList
end

function levelManager.destroyCharacter(character, weapon)
    for n = #levelManager.charactersList, 1, -1 do
        if levelManager.charactersList[n].character == character then
            table.remove(levelManager.charactersList, n)
        end
    end
    for n = #levelManager.ennemiesList, 1, -1 do
        if levelManager.ennemiesList[n].character == character then
            table.remove(levelManager.ennemiesList, n)
        end
    end
end

function levelManager.playLevelCinematic(dt)
    if levelManager.cinematic.IsStarted == false then
        for n = 1, #levelManager.charactersList do
            levelManager.charactersList[n].character.controller:setInCinematicMode(
                levelManager.charactersList[n].character,
                true
            )
        end

        camera.lock(true)
        levelManager.cinematic.IsStarted = true
    elseif levelManager.cinematic.IsStarted then
        if levelManager.currentLevel == #levelsConfig then
            levelManager.cinematic.timer = levelManager.cinematic.timer + dt * 0.25
        else
            levelManager.cinematic.timer = levelManager.cinematic.timer + dt
        end
        for n = 1, #levelManager.charactersList do
            if levelManager.charactersList[n].character.controller.player then
                levelManager.charactersList[n].character.controller.player:playEntranceAnimation(dt)
            else
                levelManager.charactersList[n]:update(dt)
            end
        end

        if levelManager.cinematic.timer >= levelManager.cinematic.lenght / 2 then
            if levelManager.currentLevel == #levelsConfig then
                ui.drawVictory()
            end
        end

        if levelManager.cinematic.timer >= levelManager.cinematic.lenght then
            if levelManager.currentLevel == #levelsConfig then
                levelManager.cinematic.timer = 0
                levelManager.cinematic.IsStarted = false
                levelManager.isTheGameFinish = true
            else
                camera.lock(false)

                for n = 1, #levelManager.charactersList do
                    levelManager.charactersList[n].character.controller:setInCinematicMode(
                        levelManager.charactersList[n],
                        false
                    )
                end

                levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.game
                levelManager.cinematic.timer = 0
                levelManager.cinematic.IsStarted = false
            end
        end
    end
end

function levelManager.nextLevel()
    if levelManager.currentLevel < #levelsConfig then
        levelManager.currentLevel = levelManager.currentLevel + 1
        gameMap.initMap(levelManager.currentLevel)
        levelManager.LEVELSTATE.currentState = levelManager.LEVELSTATE.start
        levelManager.clearEnnemies()
        levelManager.spawnEnnemies()
        ui.doorIsOpen(false)
        map.closeDoor()
        levelManager.exitDoor = map.getDoor()
        soundManager:playSound("contents/sounds/game/playerInDoor.wav", 0.5, false)
        soundManager:changeSoundForLevel()
    end
end

function levelManager.clearEnnemies()
    levelManager.ennemiesList = {}
end

function levelManager.playerWinGame()
    return levelManager.isTheGameFinish
end

return levelManager

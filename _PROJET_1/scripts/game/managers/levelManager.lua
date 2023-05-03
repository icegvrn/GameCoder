-- LE LEVEL MANAGER GERE TOUT CE QUI EST PROPRE AU NIVEAU : LE NOMBRE D'ENNEMI, LEUR TYPE, LEURS ARMES, LA MAP A AFFICHER
c_factory = require("scripts/game/factories/characterFactory")
w_factory = require("scripts/game/factories/weaponFactory")
player = require("scripts/game/player")
require("scripts/states/CHARACTERS")
require("scripts/states/WEAPONS")

levelManager = {}
levelManager.currentLevel = 1
levelManager.charactersList = {}
-- REGARDE A QUEL NIVEAU ON EST
-- CHANGE LE NIVEAU AUQUEL ON EST

-- SI LEVEL 1 --

-- SI LEVEL 2 --

-- SI LEVEL 3 --

-- SI LEVEL 4 --

function levelManager.load()
    myHeroWeapon = w_factory.createWeapon(WEAPONS.TYPE.HERO_MAGIC_STAFF)
    myWeapon = w_factory.createWeapon(WEAPONS.TYPE.SWORD)
    myWeapon2 = w_factory.createWeapon(WEAPONS.TYPE.DOUBLE_AXE)
    myWeapon3 = w_factory.createWeapon(WEAPONS.TYPE.AXE)
    myWeapon4 = w_factory.createWeapon(WEAPONS.TYPE.FLOWER)
    myWeapon5 = w_factory.createWeapon(WEAPONS.TYPE.MAGIC_STAFF)
    myCharacter = c_factory.createCharacter(CHARACTERS.CATEGORY.PLAYER, CHARACTERS.TYPE.ORC, true, love.mouse)

    player.setCharacter(myCharacter)
    myCharacter2 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.PRINCESS, false, myCharacter)

    myCharacter4 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter5 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.MAGE, false, myCharacter)

    myCharacter3 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.DWARF, false, myCharacter)

    myCharacter6 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter7 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter8 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter9 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter10 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter11 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter12 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter13 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter14 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)
    myCharacter15 = c_factory.createCharacter(CHARACTERS.CATEGORY.ENNEMY, CHARACTERS.TYPE.KNIGHT, false, myCharacter)

    myCharacter2:setPosition(200, 300)
    myCharacter5:setPosition(600, 260)
    myCharacter4:setWeaponScaling(0.7)
    myCharacter3:setPosition(500, 200)
    myCharacter4:setPosition(400, 200)

    myCharacter5:equip(myWeapon5)
    myCharacter2:equip(myWeapon2)
    myCharacter3:equip(myWeapon3)
    myCharacter4:equip(myWeapon)
    myCharacter:equip(myHeroWeapon)
    myCharacter:equip(myWeapon4)

    myWeapon6 = w_factory.createWeapon(WEAPONS.TYPE.SWORD)
    myWeapon7 = w_factory.createWeapon(WEAPONS.TYPE.DOUBLE_AXE)
    myWeapon8 = w_factory.createWeapon(WEAPONS.TYPE.AXE)
    myWeapon9 = w_factory.createWeapon(WEAPONS.TYPE.FLOWER)
    myWeapon10 = w_factory.createWeapon(WEAPONS.TYPE.MAGIC_STAFF)
    myWeapon11 = w_factory.createWeapon(WEAPONS.TYPE.SWORD)
    myWeapon12 = w_factory.createWeapon(WEAPONS.TYPE.DOUBLE_AXE)
    myWeapon13 = w_factory.createWeapon(WEAPONS.TYPE.AXE)
    myWeapon14 = w_factory.createWeapon(WEAPONS.TYPE.FLOWER)
    myWeapon15 = w_factory.createWeapon(WEAPONS.TYPE.MAGIC_STAFF)
    myCharacter6:equip(myWeapon6)
    myCharacter7:equip(myWeapon7)
    myCharacter8:equip(myWeapon8)
    myCharacter9:equip(myWeapon9)
    myCharacter10:equip(myWeapon10)
    myCharacter11:equip(myWeapon11)
    myCharacter12:equip(myWeapon12)
    myCharacter13:equip(myWeapon13)
    myCharacter14:equip(myWeapon14)
    myCharacter15:equip(myWeapon15)

    table.insert(levelManager.charactersList, myCharacter)
    table.insert(levelManager.charactersList, myCharacter2)
    table.insert(levelManager.charactersList, myCharacter3)
    table.insert(levelManager.charactersList, myCharacter4)
    table.insert(levelManager.charactersList, myCharacter5)

    table.insert(levelManager.charactersList, myCharacter6)
    table.insert(levelManager.charactersList, myCharacter7)
    table.insert(levelManager.charactersList, myCharacter8)
    table.insert(levelManager.charactersList, myCharacter9)
    table.insert(levelManager.charactersList, myCharacter10)
    table.insert(levelManager.charactersList, myCharacter11)
    table.insert(levelManager.charactersList, myCharacter12)
    table.insert(levelManager.charactersList, myCharacter13)
    table.insert(levelManager.charactersList, myCharacter14)
    table.insert(levelManager.charactersList, myCharacter15)
end

function levelManager.update(dt)
    player.update(dt)

    for n = 1, #levelManager.charactersList do
        levelManager.charactersList[n]:update(dt)
    end
end

function levelManager.draw()
    for n = 1, #levelManager.charactersList do
        if levelManager.charactersList[n]:isThePlayer() == false then
            levelManager.charactersList[n]:draw()
        end
    end

    for n = 1, #levelManager.charactersList do
        if levelManager.charactersList[n]:isThePlayer() then
            levelManager.charactersList[n]:draw()
        end
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
    -- Fais spawn les différents ennemis à des points précis
end

function levelManager.spawnPlayer()
    -- Fais spawn le player à un endroit précis du jeu
end

-- Fonction pour créer un nouveau personnage
function levelManager.createCharacter()
    -- Appelle la factory character pour créer un nouveau personnage
    -- Appelle la factory Weapon pour créer une nouvelle arme
    -- Appelle une fonction qui crée un ennemi en associant le personnage à l'arme
    -- Ajoute l'ennemi à une liste d'ennemis
end

function levelManager.keypressed(key)
    for n = 1, #levelManager.charactersList do
        if levelManager.charactersList[n]:isThePlayer() == false then
            levelManager.charactersList[n]:keypressed(key)
        end
    end
    player.keypressed(key)
end
return levelManager

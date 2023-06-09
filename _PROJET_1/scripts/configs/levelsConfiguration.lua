-- FICHIER DE CONFIGURATION QUI PERMET DE DEFINIR LA NATURE DES ENNEMIS ET DE LEUR ARME A CHAQUE NIVEAU
local CHARACTERS = require(PATHS.CONFIGS.CHARACTERS)
local WEAPONS = require(PATHS.CONFIGS.WEAPONS)

local levelsConfiguration = {}

levelsConfiguration[1] = {}
levelsConfiguration[2] = {}
levelsConfiguration[3] = {}
levelsConfiguration[4] = {}
levelsConfiguration[5] = {}
levelsConfiguration[6] = {}
levelsConfiguration[7] = {}
levelsConfiguration[8] = {}

levelsConfiguration[1].ennemies = {
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.NONE, 1}
}

levelsConfiguration[2].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 10}
}

levelsConfiguration[3].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 23},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.AXE, 23},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 2}
}

levelsConfiguration[4].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 80},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.SWORD, 25},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.AXE, 25}
}

levelsConfiguration[5].ennemies = {
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.DOUBLE_AXE, 100},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 20},
    {CHARACTERS.TYPE.PRINCESS, WEAPONS.TYPE.FLOWER, 3}
}

levelsConfiguration[6].ennemies = {
    {CHARACTERS.TYPE.PRINCESS, WEAPONS.TYPE.FLOWER, 100},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 10}
}

levelsConfiguration[7].ennemies = {
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.MAGIC_STAFF, 7},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.AXE, 20},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.SWORD, 60},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.DOUBLE_AXE, 40},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 90},
    {CHARACTERS.TYPE.PRINCESS, WEAPONS.TYPE.FLOWER, 20},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 10}
}

levelsConfiguration[8].ennemies = {{CHARACTERS.TYPE.ORC, WEAPONS.TYPE.NONE, 3}}

-- Retourne la liste de tous les ennemis du niveau en cours (utilisé pour définir la victoire du level par exemple)
function levelsConfiguration.getEnnemiesByLvl(lvl)
    return levelsConfiguration[lvl].ennemies
end

return levelsConfiguration

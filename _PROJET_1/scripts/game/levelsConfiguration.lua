-- Permet de définir les ennemis qu'il y a dans chaque niveau
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
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 10},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 3}
}

levelsConfiguration[3].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 25},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.AXE, 25}
}

levelsConfiguration[4].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 100},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.SWORD, 15},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.AXE, 15}
}

levelsConfiguration[5].ennemies = {
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.DOUBLE_AXE, 100},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 20},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 3}
}

levelsConfiguration[6].ennemies = {
    {CHARACTERS.TYPE.PRINCESS, WEAPONS.TYPE.FLOWER, 100},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 10}
}

levelsConfiguration[7].ennemies = {
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.MAGIC_STAFF, 10},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.SWORD, 30},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.DOUBLE_AXE, 20},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 80},
    {CHARACTERS.TYPE.PRINCESS, WEAPONS.TYPE.FLOWER, 20},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 5}
}

levelsConfiguration[8].ennemies = {{CHARACTERS.TYPE.ORC, WEAPONS.TYPE.NONE, 3}}

-- Retourne la liste de tous les ennemis du niveau en cours (utilisé pour définir la victoire du level par exemple)
function levelsConfiguration.getEnnemiesByLvl(lvl)
    return levelsConfiguration[lvl].ennemies
end

return levelsConfiguration

local levelsConfiguration = {}

levelsConfiguration[1] = {}
levelsConfiguration[2] = {}
levelsConfiguration[3] = {}
levelsConfiguration[4] = {}
levelsConfiguration[5] = {}
levelsConfiguration[6] = {}
levelsConfiguration[7] = {}

levelsConfiguration[1].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 70},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 25}
}

levelsConfiguration[2].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 20}
}

levelsConfiguration[3].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 25},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 25}
}

levelsConfiguration[4].ennemies = {
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 100},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.SWORD, 30}
}

levelsConfiguration[5].ennemies = {
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.DOUBLE_AXE, 100},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 30}
}

levelsConfiguration[6].ennemies = {
    {CHARACTERS.TYPE.PRINCESS, WEAPONS.TYPE.FLOWER, 100},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 10}
}

levelsConfiguration[7].ennemies = {
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.MAGIC_STAFF, 3},
    {CHARACTERS.TYPE.DWARF, WEAPONS.TYPE.SWORD, 3},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.DOUBLE_AXE, 5},
    {CHARACTERS.TYPE.KNIGHT, WEAPONS.TYPE.SWORD, 10},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.FLOWER, 10},
    {CHARACTERS.TYPE.MAGE, WEAPONS.TYPE.MAGIC_STAFF, 5}
}

function levelsConfiguration.getEnnemiesByLvl(lvl)
    return levelsConfiguration[lvl].ennemies
end

return levelsConfiguration

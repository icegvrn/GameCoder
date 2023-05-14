local dwarf = {}

-- Caractéristiques du personnage
dwarf.name = "dwarf"
dwarf.type = CHARACTERS.TYPE.DWARF
dwarf.pv = 80
dwarf.speed = love.math.random(40, 55)

-- Position de l'arme, taille de l'arme et facteur de dégâts
dwarf.handOffset = {10, 16}
dwarf.weaponScaling = 0.7
dwarf.strenght = 3

-- Sons du personnage, joué à interval régulier --
dwarf.talkingInterval = 0.75
dwarf.talkingVolume = 0.3
dwarf.talkingSound = {PATHS.SOUNDS.CHARACTERS .. "dwarf.wav"}

return dwarf

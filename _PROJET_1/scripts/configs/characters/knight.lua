local knight = {}

-- Caractéristiques du personnage
knight.name = "knight"
knight.type = CHARACTERS.TYPE.KNIGHT
knight.pv = 200
knight.speed = 0.3

-- Position de l'arme, taille de l'arme et facteur de dégâts
knight.handOffset = {2, 16}
knight.weaponScaling = 0.7
knight.strenght = 2

-- Sons du personnage, joué à interval régulier --
knight.talkingInterval = 0.6
knight.talkingVolume = 0.3
knight.talkingSound = {PATHS.SOUNDS.CHARACTERS .. "knight.wav"}

return knight

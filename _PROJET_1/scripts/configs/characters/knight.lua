local knight = {}

-- Caractéristiques du personnage
knight.name = "knight"
knight.type = CHARACTERS.TYPE.KNIGHT
knight.pv = 200
knight.speed = love.math.random(15, 35)

-- Position de l'arme, taille de l'arme et facteur de dégâts
knight.handOffset = {10, 16}
knight.weaponScaling = 0.8
knight.strenght = 2

-- Sons du personnage, joué à interval régulier --
knight.talkingInterval = 0.6
knight.talkingVolume = 0.3
knight.talkingSound = {PATHS.SOUNDS.CHARACTERS .. "knight.wav"}

return knight

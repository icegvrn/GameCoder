local princess = {}

-- Caractéristiques du personnage
princess.name = "princess"
princess.type = CHARACTERS.TYPE.PRINCESS
princess.pv = 60
princess.speed = love.math.random(20,35)

-- Position de l'arme, taille de l'arme et facteur de dégâts
princess.handOffset = {10, 22}
princess.weaponScaling = 0.7
princess.strenght = 0.5

-- Sons du personnage, joué à interval régulier --
princess.talkingInterval = 0.75
princess.talkingVolume = 0.3
princess.talkingSound = {PATHS.SOUNDS.CHARACTERS .. "princess.wav"}

return princess

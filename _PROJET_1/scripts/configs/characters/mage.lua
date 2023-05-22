local mage = {}

-- Caractéristiques du personnage
mage.name = "mage"
mage.type = CHARACTERS.TYPE.MAGE
mage.pv = 10
mage.speed = 0.2
-- Position de l'arme, taille de l'arme et facteur de dégâts
mage.handOffset = {10, 5}
mage.weaponScaling = 0.7
mage.strenght = 0.5

-- Sons du personnage, joué à interval régulier --
mage.talkingInterval = 0.75
mage.talkingVolume = 0.3
mage.talkingSound = {PATHS.SOUNDS.CHARACTERS .. "mage.wav"}

return mage

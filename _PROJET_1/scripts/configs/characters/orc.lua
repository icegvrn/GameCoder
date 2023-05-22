local orc = {}

-- Caractéristiques du personnage
orc.name = "orc"
orc.type = CHARACTERS.TYPE.ORC
orc.pv = 100
orc.speed = 0.3

-- Position de l'arme, taille de l'arme et facteur de dégâts
orc.handOffset = {10, 5}
orc.weaponScaling = 1
orc.strenght = 1

-- Sons du personnage, joué à interval régulier --
orc.talkingInterval = 0.09
orc.talkingVolume = 0.8
orc.talkingSound = {
    PATHS.SOUNDS.CHARACTERS .. "orc.wav",
    PATHS.SOUNDS.CHARACTERS .. "orc_2.wav",
    PATHS.SOUNDS.CHARACTERS .. "orc_3.wav"
}

return orc

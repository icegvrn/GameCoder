-- Chemin d'accès aux éléments, pour la maintenance. Global au projet.--
PATHS = {}

-- SONS --
PATHS.SOUNDS = {}
PATHS.SOUNDS.ROOT = "contents/sounds/"
PATHS.SOUNDS.MUSICS = PATHS.SOUNDS.ROOT .. "musics/"
PATHS.SOUNDS.GAME = PATHS.SOUNDS.ROOT .. "game/"
PATHS.SOUNDS.CHARACTERS = PATHS.SOUNDS.GAME .. "characters/"
PATHS.SOUNDS.WEAPONS = PATHS.SOUNDS.GAME .. "weapons/"

PATHS.ENTITIES = {}
PATHS.ENTITIES.ROOT = "scripts/game/Entities/"
PATHS.ENTITIES.CHARACTERS = PATHS.ENTITIES.ROOT .. "Characters/"
PATHS.ENTITIES.WEAPONS = PATHS.ENTITIES.ROOT .. "Weapons/"

PATHS.IMG = {}
PATHS.IMG.ROOT = "contents/images/"
PATHS.IMG.CHARACTERS = PATHS.IMG.ROOT .. "characters/"
PATHS.IMG.WEAPONS = PATHS.IMG.ROOT .. "weapons/"
PATHS.IMG.UI = PATHS.IMG.ROOT .. "ui/"

return PATHS

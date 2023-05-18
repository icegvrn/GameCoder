-- Créé un personnage en fonction de sa catégorie et de son type
PATHS = require("scripts/states/PATHS")
CHARACTERS_STATE = require("scripts/states/CHARACTERS")
Character = require("scripts/engine/character")
debug = require("scripts/Utils/debug")
Player = require("scripts/game/Entities/Characters/Modules/Player")
Ennemi = require("scripts/game/Entities/Characters/Modules/Ennemi")

characterFactory = {}
local player = Player.new()
local ennemi = Ennemi.new()
local createdPlayer = nil

-- Crée un nouveau personnage à la demande
function characterFactory.createCharacter(p_category, p_type, p_boostable, p_target)
    local c = characterFactory.createCharacterByRole(character, p_category)
    -- local character = characterFactory.createNewCharacter()
    characterFactory.setTarget(c.character, p_target)
    characterFactory.setCharacteristics(c.character, p_category, p_type, p_boostable)
    characterFactory.createSprites(c.character, p_category, p_type, p_boostable)
    characterFactory.initCharacter(c.character)
    return c
end

-- Ajoute un agent si c'est un ennemi, ou passe le personnage en mode joueur si c'est le joueur
function characterFactory.createCharacterByRole(c, category)
    if category == CHARACTERS_STATE.CATEGORY.PLAYER then
        createdPlayer = player:create()
        return createdPlayer
    else
        local e = ennemi:create()
        return e
    end
end

function characterFactory:getPlayer()
    return createdPlayer
end

-- Va chercher les caractéristiques du personnage dans un fichier portant le nom de son type ("knight", "princess"...)
function characterFactory.setCharacteristics(c, category, p_type, boostable)
    local characterData = require(PATHS.ENTITIES.CHARACTERS .. p_type)
    c:setName(characterData.name)
    c.controller:setSpeed(characterData.speed)
    c.fight:setMaxPV(characterData.pv)
    c.fight:setStrenght(characterData.strenght)
    c.fight.weaponSlot:setHandOffset(characterData.handOffset)
    c.fight.weaponSlot:setWeaponScaling(characterData.weaponScaling)
    c.sound:setSounds(characterData.talkingSound)
    c.sound:setSilenceIntervalBetweenTalk(characterData.talkingInterval)
    c.sound:setTalkingVolume(characterData.talkingVolume)
end

function characterFactory.setTarget(c, target)
    if (target) then
        c.controller:setTarget(target)
    else
        print("CHARACTER FACTORY : LA CIBLE EST INCORRECTE.")
    end
end

-- -- Génère les sprites associés à ce type de personnage pour chaque "state" possible du personnage.
-- State"Dead" traité différemment car même image pour tous. Boostable true uniquement pour le joueur ;
function characterFactory.createSprites(character, category, type, boostable)
    local spriteList = {}

    for k, v in pairs(CHARACTERS_STATE.STATE) do
        --
        if v ~= CHARACTERS_STATE.STATE.DEAD then
            -- Personnage mode normal
            local state = CHARACTERS_STATE.MODE.NORMAL .. "_" .. v
            local imagePath = PATHS.IMG.CHARACTERS .. category .. "/" .. type .. "_" .. state .. ".png"
            if love.filesystem.getInfo(imagePath) then
                local sprite = love.graphics.newImage(imagePath)
                spriteList[state] = sprite

                -- Si le personnage a mode boostable
                if (boostable) then
                    local state = CHARACTERS_STATE.MODE.BOOSTED .. "_" .. v
                    local imagePath = PATHS.IMG.CHARACTERS .. category .. "/" .. type .. "_" .. state .. ".png"
                    if love.filesystem.getInfo(imagePath) then
                        local sprite = love.graphics.newImage(imagePath)
                        spriteList[state] = sprite
                    else
                        print(
                            "WARNING CHARACTER FACTORY : " ..
                                type .. " is set on boostable and needs images for boosted " .. state .. "."
                        )
                    end
                end
            else
                if v == CHARACTERS_STATE.STATE.ALERT and character.controller.ennemiAgent then
                    print("WARNING CHARACTER FACTORY : " .. type .. " needs images for " .. state .. ".")
                end
            end
        else
            c_state = CHARACTERS_STATE.MODE.NORMAL .. "_" .. CHARACTERS_STATE.STATE.DEAD
            spriteList[c_state] = love.graphics.newImage(PATHS.IMG.CHARACTERS .. "explosion_sprite.png")
        end
    end

    -- Retourne la liste finale au character
    character.sprites:setSpritesList(spriteList, 4, 1)
end

function characterFactory.initCharacter(c)
    c:setMode(CHARACTERS_STATE.MODE.NORMAL)
    c:setState(CHARACTERS_STATE.STATE.IDLE)
end

return characterFactory

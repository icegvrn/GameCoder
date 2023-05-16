-- Créé un personnage en fonction de sa catégorie et de son type
PATHS = require("scripts/states/PATHS")
CHARACTERS_STATE = require("scripts/states/CHARACTERS")
Character = require("scripts/engine/character")

characterFactory = {}

-- Crée un nouveau personnage à la demande
function characterFactory.createCharacter(p_category, p_type, p_boostable, p_target)
    local character = characterFactory.createNewCharacter()
    characterFactory.defineRole(character, p_category)
    characterFactory.setTarget(character, p_target)
    characterFactory.setCharacteristics(character, p_category, p_type, p_boostable)
    characterFactory.createSprites(character, p_category, p_type, p_boostable)
    characterFactory.initCharacter(character)
    return character
end

function characterFactory.createNewCharacter()
    return Character.new()
end

-- Ajoute un agent si c'est un ennemi, ou passe le personnage en mode joueur si c'est le joueur
function characterFactory.defineRole(c, category)
    if category == CHARACTERS_STATE.CATEGORY.PLAYER then
        characterFactory.enableController(c)
    else
        characterFactory.enableAgent(c)
    end
end

function characterFactory.enableAgent(c)
    c:addEnnemiAgent()
end

function characterFactory.enableController(c)
    c:setPlayer()
end

-- Va chercher les caractéristiques du personnage dans un fichier portant le nom de son type ("knight", "princess"...)
function characterFactory.setCharacteristics(c, category, p_type, boostable)
    local characterData = require(PATHS.ENTITIES.CHARACTERS .. p_type)
    c:setName(characterData.name)
    c:setSpeed(characterData.speed)
    c:setMaxPV(characterData.pv)
    c:setHandOffset(characterData.handOffset)
    c:setStrenght(characterData.strenght)
    c:setWeaponScaling(characterData.weaponScaling)
    c:setSounds(characterData.talkingSound)
    c:setSilenceIntervalBetweenTalk(characterData.talkingInterval)
    c:setTalkingVolume(characterData.talkingVolume)
end

function characterFactory.setTarget(c, target)
    if (target) then
        c:setTarget(target)
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
                if (v == CHARACTERS_STATE.STATE.ALERT and character:isThePlayer()) == false then
                    print("WARNING CHARACTER FACTORY : " .. type .. " needs images for " .. state .. ".")
                end
            end
        else
            c_state = CHARACTERS_STATE.MODE.NORMAL .. "_" .. CHARACTERS_STATE.STATE.DEAD
            spriteList[c_state] = love.graphics.newImage(PATHS.IMG.CHARACTERS .. "explosion_sprite.png")
        end
    end

    -- Retourne la liste finale au character
    character:setSprites(spriteList)
end

function characterFactory.initCharacter(c)
    c:setMode(CHARACTERS_STATE.MODE.NORMAL)
    c:setState(CHARACTERS_STATE.STATE.IDLE)
    c:load()
end

return characterFactory

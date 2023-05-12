-- Créé un personnage en fonction de sa catégorie et de son type
require("scripts/states/CHARACTERS")
Character = require("scripts/engine/character")

characterFactory = {}

function characterFactory.createCharacter(category, type, boostable, target)
    local character = Character.new()
    characterFactory.createSprites(character, category, type, boostable)

    character:setMode(CHARACTERS.MODE.NORMAL)
    character:setState(CHARACTERS.STATE.IDLE)
    character:setName(type)

    if type == CHARACTERS.TYPE.ORC then
        character:setSpeed(120)
        character:setMaxPV(100)
        character:setStrenght(1)
        character:setWeaponScaling(1)
        character:setSounds(
            "contents/sounds/game/characters/orc.wav",
            "contents/sounds/game/characters/orc_2.wav",
            "contents/sounds/game/characters/orc_3.wav"
        )
        character:setSilenceIntervalBetweenTalk(0.09)
        character:setTalkingVolume(0.8)
        character:load()
    elseif type == CHARACTERS.TYPE.KNIGHT then
        character:setSpeed(math.random(15, 35))
        character:setMaxPV(200)
        character:setStrenght(2)
        character:setHandOffset(10, 16)
        character:setWeaponScaling(0.8)
        character:setSounds("contents/sounds/game/characters/knight.wav")
        character:setSilenceIntervalBetweenTalk(0.6)
        character:load()
    elseif type == CHARACTERS.TYPE.MAGE then
        character:setSpeed(10)
        character:setMaxPV(10)
        character:setStrenght(0.5)
        character:setHandOffset(10, 16)
        character:setWeaponScaling(0.7)
        character:setSounds("contents/sounds/game/characters/mage.wav")
        character:setSilenceIntervalBetweenTalk(0.75)
        character:load()
    elseif type == CHARACTERS.TYPE.PRINCESS then
        character:setSpeed(30)
        character:setMaxPV(60)
        character:setStrenght(0.5)
        character:setHandOffset(10, 22)
        character:setWeaponScaling(0.7)
        character:setSounds("contents/sounds/game/characters/princess.wav")
        character:setSilenceIntervalBetweenTalk(0.75)
        character:load()
    elseif type == CHARACTERS.TYPE.DWARF then
        character:setSpeed(10)
        character:setMaxPV(120)
        character:setStrenght(3)
        character:setHandOffset(10, 16)
        character:setWeaponScaling(0.7)
        character:setSounds("contents/sounds/game/characters/dwarf.wav")
        character:setSilenceIntervalBetweenTalk(0.75)
        character:load()
    end

    if (target) then
        character:setTarget(target)
    else
        print("CHARACTER FACTORY : LA CIBLE EST INCORRECTE.")
    end

    if category == CHARACTERS.CATEGORY.PLAYER then
        character:setPlayer()
    else
        character:addEnnemiAgent()
    end
    return character
end

function characterFactory.createSprites(character, category, type, boostable)
    local spriteList = {}

    for k, v in pairs(CHARACTERS.STATE) do
        if v ~= CHARACTERS.STATE.DEAD then
            local state = CHARACTERS.MODE.NORMAL .. "_" .. v
            local imagePath = CHARACTERS.IMGPATH .. "/" .. category .. "/" .. type .. "_" .. state .. ".png"
            if love.filesystem.getInfo(imagePath) then
                local sprite = love.graphics.newImage(imagePath)
                spriteList[state] = sprite

                if (boostable) then
                    local state = CHARACTERS.MODE.BOOSTED .. "_" .. v
                    local imagePath = CHARACTERS.IMGPATH .. "/" .. category .. "/" .. type .. "_" .. state .. ".png"
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
                print("WARNING CHARACTER FACTORY : " .. type .. " needs images for " .. state .. ".")
            end
        else
            c_state = CHARACTERS.MODE.NORMAL .. "_" .. CHARACTERS.STATE.DEAD
            spriteList[c_state] = love.graphics.newImage("contents/images/characters/explosion_sprite.png")
        end
    end

    character:setSprites(spriteList)
end
return characterFactory

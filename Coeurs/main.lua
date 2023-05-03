-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

-- Setup du jeu
initialPointofLife = 10
currentPointOfLife = 5

-- Setup graphique
squareHeight = 20
squareWidth = 20
marginBetweenSquares = 10

-- Visuels du coeur
heart_sprite = love.graphics.newImage("images/heart.png")
l_half_heart_sprite = love.graphics.newImage("images/l_halfHeart.png")
r_half_heart_sprite = love.graphics.newImage("images/r_halfHeart.png")
spriteToDraw = l_half_heart_sprite

timer_blink = false

function love.load()
end

function love.update()
    UpdateTimer()
end

function love.draw()
    -- Dessin des rectangles
    for i = 1, initialPointofLife do
        local squarePosX = 10
        local squarePosY = (squareHeight + marginBetweenSquares) * i
        love.graphics.rectangle("fill", squarePosX, squarePosY, squareWidth, squareHeight)
    end

    -- Calcul position coeur par rapport au carré
    for i = 0.5, currentPointOfLife, 0.5 do
        local squarePosX = 10
        local squarePosY = (squareHeight + marginBetweenSquares) * math.floor(i + 0.5) --Arrondir le i pour mettre les demi coeur ensemble (1,1 pour les 2 premiers, 2, 2 pour les deux seconds etc)
        local heartPosX = squarePosX + (squareWidth / 2) -- position du coeur sur X
        local heartPosY = squarePosY + (squareHeight / 2)

        if i % 1 == 0 then
            spriteToDraw = r_half_heart_sprite
        else
            spriteToDraw = l_half_heart_sprite
        end

        if currentPointOfLife == 0.5 then
            if timer_blink then
                -- Dessin du coeur
                love.graphics.draw(
                    spriteToDraw,
                    heartPosX,
                    heartPosY,
                    0,
                    calcImgScale(spriteToDraw, squareWidth, squareHeight),
                    calcImgScale(spriteToDraw, squareWidth, squareHeight),
                    spriteToDraw:getHeight() / 2,
                    spriteToDraw:getWidth() / 2
                )
            end
        else
            -- Dessin du coeur
            love.graphics.draw(
                spriteToDraw,
                heartPosX,
                heartPosY,
                0,
                calcImgScale(spriteToDraw, squareWidth, squareHeight),
                calcImgScale(spriteToDraw, squareWidth, squareHeight),
                spriteToDraw:getHeight() / 2,
                spriteToDraw:getWidth() / 2
            )
        end
    end
end

function love.keyreleased(key)
    if currentPointOfLife < initialPointofLife and key == "up" then
        currentPointOfLife = currentPointOfLife + 0.5
    elseif currentPointOfLife > 0 and key == "down" then
        currentPointOfLife = currentPointOfLife - 0.5
    end
end

function calcImgScale(image, newWidth, newHeight)
    local currentWidth, currentHeight = image:getDimensions()
    return (newWidth / currentWidth / 1.5), ((newHeight / currentHeight) / 1.5)
end

function UpdateTimer()
    if (math.floor(love.timer.getTime()) % 2 == 0) then
        timer_blink = true
    else
        timer_blink = false
    end
end

-- Usine à création d'objet
function CreateCharacter()
    MyCharacter = {}

    MyCharacter.sprite = love.graphics.newImage("images/zombie_sprite.png", static)
    MyCharacter.posX = 5
    MyCharacter.posY = 5
    MyCharacter.scaleX = 0.06
    MyCharacter.scaleY = 0.06
    MyCharacter.rotation = 0.1
    MyCharacter.rightCornerX = MyCharacter.posX + MyCharacter.sprite:getWidth() * MyCharacter.scaleX
    MyCharacter.speed = love.math.random(0.2, 2)
    MyCharacter.state = "left"

    MyCharacter.SetPosition = function(posX, posY)
        MyCharacter.posX = posX
        MyCharacter.posY = posY
    end

    return MyCharacter
end

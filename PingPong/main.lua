-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

require("character")

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

MyZombies = {}
ZombiesNumber = 10

characterYMarginFactor = 60

function love.load()
    for i = 1, ZombiesNumber do
        local zombie = CreateCharacter()
        table.insert(MyZombies, zombie)
        zombie.SetPosition(zombie.posX, i * characterYMarginFactor)
    end
end

function love.update(dt)
    for i = 1, #MyZombies do
        MoveCharacter(MyZombies[i])
    end
end

function love.draw()
    for i = 1, #MyZombies do
        love.graphics.draw(
            MyZombies[i].sprite,
            MyZombies[i].posX,
            MyZombies[i].posY,
            MyZombies[i].rotation,
            MyZombies[i].scaleX,
            MyZombies[i].scaleY
        )
    end
end

function MoveCharacter(character)
    CheckCharacterPosition(character)

    if character.state == "left" then
        GoLeft(character)
    elseif character.state == "right" then
        GoRight(character)
    end
end

function CheckCharacterPosition(character)
    character.rightCornerX = character.posX + character.sprite:getWidth() * character.scaleX

    if character.state == "left" then
        if character.posX < 0 then
            character.scaleX = -character.scaleX
            character.posX = character.rightCornerX
            character.state = "right"
        end
    elseif character.state == "right" then
        if character.posX > love.graphics.getWidth() then
            character.scaleX = -character.scaleX
            character.posX = character.rightCornerX
            character.state = "left"
        end
    end
end

function GoRight(character)
    character.posX = character.posX + character.speed
end

function GoLeft(character)
    character.posX = character.posX - character.speed
end

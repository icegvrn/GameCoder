local game = {}
game.map = {}
game.map = {
    {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
    {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
    {2, 2, 2, 2, 2, 4, 2, 2, 2, 2, 2, 2},
    {2, 2, 2, 2, 4, 3, 4, 2, 2, 2, 2, 2},
    {2, 2, 2, 4, 3, 3, 3, 4, 2, 2, 2, 2},
    {2, 2, 2, 4, 3, 3, 3, 4, 2, 2, 2, 2},
    {2, 2, 2, 4, 3, 3, 4, 4, 2, 2, 2, 2},
    {2, 2, 2, 2, 4, 3, 4, 2, 2, 2, 2, 2},
    {2, 2, 2, 2, 2, 4, 2, 2, 2, 2, 2, 2},
    {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}
}
game.TILEWIDTH = 70
game.TILEHEIGHT = 70
game.MAPWIDTH = game.TILEWIDTH * 10
game.MAPHEIGHT = game.TILEHEIGHT * 10
gameCurrentTile = {}
game.tilestextures = {
    {nil, "null"},
    {love.graphics.newImage("images/grassCenter.png"), "Herbe"},
    {love.graphics.newImage("images/liquidWater.png"), "Eau"},
    {love.graphics.newImage("images/snowCenter.png"), "Neige"},
    {love.graphics.newImage("images/stoneCenter.png"), "Pierre"}
}
game.textureToChange = 4

function game.load()
end

function game.draw()
    for n = 1, #game.map do
        for i = 1, #game.map[n] do
            local value = game.map[n][i]
            love.graphics.draw(game.tilestextures[value][1], (i - 1) * game.TILEWIDTH, (n - 1) * game.TILEHEIGHT)
        end
    end

    local col = math.floor(love.mouse.getX() / game.TILEWIDTH) + 1
    local lin = math.floor(love.mouse.getY() / game.TILEHEIGHT) + 1

    local id = game.map[lin][col]
    gameCurrentTile[1] = lin
    gameCurrentTile[2] = col
    if lin >= 0 and lin < game.MAPHEIGHT and col >= 0 and col < game.MAPWIDTH then
        love.graphics.print("ID : " .. game.tilestextures[id][2], love.mouse.getX() + 10, love.mouse.getY())
    end

    love.graphics.print(
        "SPACE TO CHOOSE GROUND",
        love.graphics.getWidth() / 5 - 150,
        love.graphics.getHeight() - 70 - game.TILEHEIGHT
    )

    love.graphics.rectangle(
        "fill",
        love.graphics.getWidth() / 5 - 100 - 2,
        love.graphics.getHeight() - 40 - game.TILEHEIGHT - 2,
        game.TILEWIDTH + 5,
        game.TILEHEIGHT + 5
    )
    love.graphics.setColor(1, 1, 1)
    if game.textureToChange > 1 then
        if game.textureToChange == #game.tilestextures then
            game.textureToChange = 2
        end
        love.graphics.draw(
            game.tilestextures[game.textureToChange][1],
            love.graphics.getWidth() / 5 - 100,
            love.graphics.getHeight() - 40 - game.TILEHEIGHT
        )
    end
end

function game.checkMouse()
    if love.mouse.isDown(1) then -- Versions prior to 0.10.0 use the MouseConstant 'l'
        game.map[gameCurrentTile[1]][gameCurrentTile[2]] = game.textureToChange
    end
end

function game.changeTexture()
    game.textureToChange = game.textureToChange + 1
end

return game

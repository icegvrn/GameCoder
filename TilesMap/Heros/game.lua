local game = {}
game.hero = require("hero")
game.map = {}
game.map = {
    {
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        61,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        14,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15
    },
    {
        10,
        10,
        10,
        10,
        10,
        11,
        11,
        11,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        14,
        15,
        15,
        129,
        15,
        15,
        15,
        15,
        15,
        15,
        68,
        15,
        15
    },
    {
        10,
        10,
        61,
        10,
        11,
        19,
        19,
        19,
        11,
        10,
        10,
        13,
        10,
        10,
        169,
        10,
        10,
        10,
        10,
        13,
        14,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15
    },
    {
        10,
        10,
        10,
        11,
        19,
        19,
        19,
        19,
        19,
        11,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        14,
        15,
        15,
        15,
        68,
        15,
        15,
        15,
        15,
        15,
        15
    },
    {
        10,
        10,
        10,
        11,
        19,
        19,
        19,
        19,
        19,
        11,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        61,
        10,
        10,
        14,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15
    },
    {
        10,
        10,
        61,
        10,
        11,
        19,
        19,
        19,
        11,
        10,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        14,
        15,
        15,
        129,
        15,
        15,
        15,
        68,
        15,
        129,
        15
    },
    {
        10,
        10,
        10,
        10,
        10,
        11,
        11,
        11,
        10,
        10,
        61,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        14,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15
    },
    {
        10,
        10,
        10,
        10,
        10,
        13,
        13,
        13,
        13,
        13,
        13,
        13,
        10,
        10,
        10,
        10,
        10,
        169,
        10,
        10,
        10,
        13,
        14,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15,
        15
    },
    {
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        61,
        10,
        13,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        15,
        129
    },
    {
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        55,
        10,
        58,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        14,
        14
    },
    {
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        55,
        10,
        58,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10
    },
    {
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        10,
        58,
        10,
        10,
        10,
        10,
        10,
        10,
        169,
        10,
        10,
        10,
        10,
        10,
        10,
        61,
        10,
        10,
        10,
        10,
        10,
        1,
        1
    },
    {
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        58,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        61,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        1,
        37,
        37
    },
    {
        13,
        13,
        13,
        13,
        13,
        13,
        13,
        13,
        13,
        10,
        55,
        10,
        10,
        10,
        55,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        1,
        1,
        37,
        37,
        37
    },
    {
        10,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        55,
        10,
        10,
        10,
        10,
        169,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        1,
        37,
        37,
        37,
        37,
        37
    },
    {
        10,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        10,
        142,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        1,
        37,
        37,
        37,
        37,
        37,
        37
    },
    {
        10,
        10,
        10,
        10,
        13,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        142,
        10,
        10,
        10,
        10,
        10,
        10,
        10,
        169,
        10,
        10,
        1,
        37,
        37,
        37,
        37,
        37,
        37,
        37
    },
    {
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        1,
        37,
        37,
        37,
        37,
        37,
        37,
        37
    },
    {
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        14,
        1,
        37,
        37,
        37,
        37,
        37,
        37,
        37
    },
    {
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        19,
        1,
        37,
        37,
        37,
        37,
        37,
        37,
        37
    },
    {
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        1,
        37,
        37,
        37,
        37,
        37,
        37
    },
    {
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        1,
        37,
        37,
        37,
        37
    },
    {
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        21,
        1,
        37,
        37,
        37
    }
}
game.TILEWIDTH = 32
game.TILEHEIGHT = 32
game.MAPWIDTH = 32
game.MAPHEIGHT = 23
game.TILESSHEETWIDTH = game.TILEWIDTH * 9
game.TILESSHEETHEIGHT = game.TILEHEIGHT * 19
gameCurrentTile = {}

game.tileTextures = {}

game.tileType = {}
game.tileType[10] = "Herbe"
game.tileType[11] = "Herbe 2"
game.tileType[12] = "Herbe 3"
game.tileType[13] = "Sand light"
game.tileType[14] = "Sand"
game.tileType[15] = "Sand dark"
game.tileType[19] = "Water"
game.tileType[129] = "Vulcano"
game.tileType[169] = "Stone"
game.tileType[55] = "Tree"
game.tileType[58] = "Strange tree"
game.tileType[61] = "Pin"

game.textureToChange = 4

function game.load()
    game.tileSheet = love.graphics.newImage("images/tilesheet.png")

    local nbColumns = game.tileSheet:getWidth() / game.TILEWIDTH
    local nbLine = game.tileSheet:getHeight() / game.TILEHEIGHT
    local l, c
    local id = 1
    game.tileTextures[0] = nil
    for l = 1, nbLine do
        for c = 1, nbColumns do
            game.tileTextures[id] =
                love.graphics.newQuad(
                (c - 1) * game.TILEWIDTH,
                (l - 1) * game.TILEHEIGHT,
                game.TILEWIDTH,
                game.TILEHEIGHT,
                game.TILESSHEETWIDTH,
                game.TILESSHEETHEIGHT
            )
            id = id + 1
        end
    end
end

function game.draw()
    for n = 1, #game.map do
        for i = 1, #game.map[n] do
            local value = game.map[n][i]
            local texQuad = game.tileTextures[value]
            if texQuad ~= nil then
                love.graphics.draw(game.tileSheet, texQuad, (i - 1) * game.TILEWIDTH, (n - 1) * game.TILEHEIGHT)
            end
        end
    end

    local col = math.floor(love.mouse.getX() / game.TILEWIDTH) + 1
    local lin = math.floor(love.mouse.getY() / game.TILEHEIGHT) + 1

    local id = game.map[lin][col]
    gameCurrentTile[1] = lin
    gameCurrentTile[2] = col

    if id then
        if lin >= 0 and lin < game.TILESSHEETHEIGHT and col >= 0 and col < game.TILESSHEETWIDTH then
            if game.tileType[id] then
                love.graphics.print(
                    "ID : " .. game.tileType[id] .. " (" .. id .. ")",
                    love.mouse.getX() + 10,
                    love.mouse.getY()
                )
            else
                love.graphics.print("ID : (" .. id .. ")", love.mouse.getX() + 10, love.mouse.getY())
            end
        end
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
        love.graphics.draw(
            game.tileSheet,
            game.tileTextures[game.textureToChange],
            love.graphics.getWidth() / 5 - 100,
            love.graphics.getHeight() - 40 - game.TILEHEIGHT
        )
    end
    hero.draw(game)
end

function game.checkMouse()
    if love.mouse.isDown(1) then -- Versions prior to 0.10.0 use the MouseConstant 'l'
        game.map[gameCurrentTile[1]][gameCurrentTile[2]] = game.textureToChange
    end
end

function game.changeTexture(x, y)
    if y > 0 then
        if game.textureToChange == #game.tileTextures then
            game.textureToChange = 2
        else
            game.textureToChange = game.textureToChange + 1
        end
    elseif y < 0 then
        if game.textureToChange == 2 then
            game.textureToChange = #game.tileTextures
        else
            game.textureToChange = game.textureToChange - 1
        end
    end
end

function game.isCurrentTileSolid(pID)
    local tileType = game.tileType[pID]
    if tileType then
        if
            tileType == "Sea" or tileType == "Water" or tileType == "Stone" or tileType == "Tree" or
                tileType == "Strange tree"
         then
            return true
        end
    end
    return false
end

function game.update(dt)
    game.checkMouse()
    hero.update(dt, game)
end

function game.characterController(key)
    hero.controller(key, game)
end

return game

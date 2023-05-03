hero = {}
hero.images = {
    love.graphics.newImage("images/player_1.png"),
    love.graphics.newImage("images/player_2.png"),
    love.graphics.newImage("images/player_3.png"),
    love.graphics.newImage("images/player_4.png")
}
hero.currentImage = 1
hero.line = 1
hero.col = 1

function hero.update(dt, pMap)
    hero.currentImage = hero.currentImage + 5 * dt
    if hero.currentImage >= #hero.images then
        hero.currentImage = 1
    end
end

function hero.draw(pMap)
    local x = (hero.col - 1) * pMap.TILEWIDTH
    local y = (hero.line - 1) * pMap.TILEHEIGHT
    love.graphics.draw(hero.images[math.floor(hero.currentImage)], x, y, 0, 2, 2)
end

function hero.controller(key, pMap)
    local l = hero.line
    local c = hero.col

    if key == "down" and hero.line < pMap.MAPHEIGHT then
        hero.line = hero.line + 1
    end
    if key == "up" and hero.line > 1 then
        hero.line = hero.line - 1
    end
    if key == "right" and hero.col < pMap.MAPWIDTH then
        hero.col = hero.col + 1
    end
    if key == "left" and hero.col > 1 then
        hero.col = hero.col - 1
    end

    local id = pMap.map[hero.line][hero.col]
    if pMap.isCurrentTileSolid(id) then
        hero.line = l
        hero.col = c
    end
end

return hero

-- MODULE DES PORTES, QUI S'AFFICHENT SUR CHAQUE MAP : UNE D'ELLE S'OUVRE QUAND LE NIVEAU EST FINI
local Door = {}
local Door_mt = {__index = Door}

-- Initialisation de l'instance
function Door.new()
    local door = {}
    return setmetatable(door, Door_mt)
end

-- Création d'une nouvelle porte
function Door:create()
    local door = {
        height = 50,
        width = 10,
        map = {width = 0, height = 0},
        open = false,
        position = {x = 0, y = 0},
        openDoorImg = love.graphics.newImage(PATHS.IMG.ROOT .. "doorOpen.png"),
        closeDoorImg = love.graphics.newImage(PATHS.IMG.ROOT .. "doorClose.png"),
        currentDoorImg = nil
    }

    -- Appel le draw de chaque porte
    function door:draw()
        self:drawLeftDoor()
        self:drawRightDoor()
    end

    -- Draw de la porte droite, basé sur currentDoorImg, une image qui bouge
    function door:drawRightDoor()
        love.graphics.draw(
            self.currentDoorImg,
            self.position.x,
            self.position.y,
            math.rad(90),
            1,
            1,
            self.openDoorImg:getWidth() / 2,
            self.openDoorImg:getHeight() / 2
        )
    end

    -- Draw de la porte gauche, basé sur closeDoorImg, celle-ci change jamais (idéalement il faudrait faire une anim d'ouverture au début)
    function door:drawLeftDoor()
        love.graphics.draw(
            self.closeDoorImg,
            10 - self.openDoorImg:getWidth() / 2,
            self.position.y + 5,
            math.rad(-90),
            1,
            1,
            self.openDoorImg:getWidth() / 2,
            self.openDoorImg:getHeight() / 2
        )
    end

    -- Initilisation de la porte en calculant sa position exacte et en veillant à ce qu'elle soit fermée au départ
    function door:init(currentMap)
        local map_width = currentMap.map.width * currentMap.map.tilewidth
        local map_height = currentMap.map.height * currentMap.map.tileheight
        self.position.x = (map_width - self.width) + self.openDoorImg:getWidth() / 2
        self.position.y = (map_height / 2) + 5
        self.currentDoorImg = self.closeDoorImg
        self:closeDoor()
    end

    -- Fonction qui ferme la porte
    function door:closeDoor()
        self.open = false
        self.currentDoorImg = self.closeDoorImg
    end

    -- Fonction qui ouvre la porte, avec un petit son associé indiquant "la porte est ouverte" à l'user (car parfois loin d'elle sur la carte)
    function door:openDoor()
        self.open = true
        self.currentDoorImg = self.openDoorImg
        soundManager:playSound(PATHS.SOUNDS.GAME .. "door_signal.wav", 0.5, false)
        soundManager:playSound(PATHS.SOUNDS.GAME .. "doorOpen.wav", 0.1, false)
    end

    -- Fonction qui vérifie si le joueur emprunte une porte ouverte via la collision :
    -- s'il y a collision alors que la porte est ouverte, cela signifie qu'il change de level
    function door:checkTakeDoor(player)
        playerX, playerY = player.character:getPosition()
        playerH, playerW = player.character.sprites:getDimension(player.character.mode, player.character.state)

        if
            Utils.isCollision(
                self.position.x,
                self.position.y,
                self.width,
                self.height,
                playerX,
                playerY,
                playerH,
                playerW
            )
         then
            return true
        else
            return false
        end
    end

    return door
end

return Door

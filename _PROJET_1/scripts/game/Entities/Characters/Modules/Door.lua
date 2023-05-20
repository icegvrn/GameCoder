local Door = {}
local Door_mt = {__index = Door}

function Door.new()
    local door = {}
    return setmetatable(door, Door_mt)
end

function Door:create()
    local door = {
        height = 50,
        width = 10,
        map = {width = 0, height = 0},
        open = false,
        position = {x = 0, y = 0},
        openDoorImg = love.graphics.newImage("contents/images/doorOpen.png"),
        closeDoorImg = love.graphics.newImage("contents/images/doorClose.png"),
        currentDoorImg = nil
    }

    function door:update(dt)
        if self.open then
            self.currentDoorImg = self.openDoorImg
        else
            self.currentDoorImg = self.closeDoorImg
        end
    end

    function door:draw()
        self:drawLeftDoor()
        self:drawRightDoor()
    end

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

    function door:closeDoor()
        self.open = false
    end

    function door:openDoor()
        self.open = true
        soundManager:playSound("contents/sounds/game/door_signal.wav", 0.5, false)
        soundManager:playSound("contents/sounds/game/doorOpen.wav", 0.1, false)
    end

    function door:init(currentMap)
        local map_width = currentMap.map.width * currentMap.map.tilewidth
        local map_height = currentMap.map.height * currentMap.map.tileheight
        self.position.x = (map_width - self.width) + self.openDoorImg:getWidth() / 2
        self.position.y = (map_height / 2) + 5
        self.currentDoorImg = self.closeDoorImg
        self:closeDoor()
    end

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

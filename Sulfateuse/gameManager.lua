local player = require("player")
local weapons = require("weapons")

gameManager = {}
gameManager.inventory = {}
gameManager.playerCurrentWeapon = 0
gameManager.playerCurrentWeaponPosX = 300
gameManager.playerCurrentWeaponPosY = 300

function gameManager.Init(self)
    gameManager.playerCurrentWeapon = self:getPlayerCurrentWeapon()
    weapons.setWeaponsOrigin(gameManager.playerCurrentWeaponPosX, gameManager.playerCurrentWeaponPosY)
end

function gameManager.setPlayerCurrentWeapon(self, weapon)
    player:setCurrentWeapon(weapon)
end

function gameManager.getPlayerCurrentWeapon(self)
    return player:getCurrentWeapon()
end

function gameManager.drawPlayerWeapon(self)
    fire:drawOrigins()
    love.graphics.setColor(1, 1, 1)
    love.graphics.draw(
        player.getCurrentWeapon().getImage(),
        gameManager.playerCurrentWeaponPosX,
        gameManager.playerCurrentWeaponPosY
    )
end

function gameManager.getPlayer()
    return player
end

function gameManager.updateGame(self)
end

return gameManager

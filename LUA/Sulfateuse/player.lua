local player = {}
player.inventory = {}
player.inventory.weapons = {}
player.inventory.currentWeapon = {}
player.inventory.currentWeapon.id = nil
player.inventory.currentWeapon.image = nil

function player.getCurrentWeapon(self)
    return player.inventory.currentWeapon
end

function player.setCurrentWeapon(self, weapon)
    for k, v in ipairs(weapons) do
        if v.id == weapon then
            player.inventory.currentWeapon = v
        end
    end
end

return player

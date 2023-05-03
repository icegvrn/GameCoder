-- Module Tank
print("je suis le module")
local tank = {}
tank.x = 50
tank.y = 200
tank.angle = 0
tank.tourne = function(self, angle)
    self.angle = self.angle + angle
end

tank.mouvement = function(self, vitesse, angle)
    self.x = self.x + vitesse * math.cos(tank.angle)
    self.y = self.y + vitesse * math.sin(tank.angle)
end

function tank.draw()
    love.graphics.draw(imageTank, tank.x, tank.y, tank.angle, 1, 1, imageTank:getWidth() / 2, imageTank:getHeight() / 2)
end

function tank.update(dt)
end

return tank

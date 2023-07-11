local tank = require("tank")
local tirs = require("tirs")

local tourelle = {}
tourelle.x = love.graphics.getWidth() - 100
tourelle.y = 50
tourelle.size = 20

isTimerStarted = false
timer = 0
fireRate = 1

local listeTirs = {}

function tourelle.update(self, dt)
    self:firing(dt)

    for n = #listeTirs, 1, -1 do
        local t = listeTirs[n]
        t.x = t.x + (t.vitesse * dt)
        t.y = t.y + (t.vitesse * dt)
        --t.vie = t.vie - dt
    end
end

function tourelle.draw(self)
    love.graphics.circle("fill", self.x, self.y, self.size)

    for k, v in ipairs(listeTirs) do
        love.graphics.circle("fill", v.x, v.y, 3)
    end
end

function tourelle.firing(self, dt)
    if isTimerStarted == false then
        isTimerStarted = true
        timer = fireRate
    else
        timer = timer - dt

        if timer <= 0 then
            self:fire()
            timer = fireRate
            isTimerStarted = false
        end
    end
end

function tourelle.fire(self)
    tirs.Tire(self.x, self.y, math.atan2(tank.y - self.y, tank.x - self.x), 100)
end

return tourelle

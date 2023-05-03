local tirs = {}
print("je suis le module tirs")

local moduleTank = require("tank")

print("l'adresse du moduleTank dans tirs est " .. tostring(moduleTank))

local listeTirs = {}

function tirs.Tire(x, y, angle, vitesse)
    local tir = {}
    tir.x = x
    tir.y = y
    tir.angle = angle
    tir.vitesse = vitesse
    tir.vie = 0.5
    table.insert(listeTirs, tir)
end

function tirs.Update(dt)
    for n = #listeTirs, 1, -1 do
        local t = listeTirs[n]
        t.x = t.x + (t.vitesse * dt) * math.cos(t.angle)
        t.y = t.y + (t.vitesse * dt) * math.sin(t.angle)
        --t.vie = t.vie - dt

        if t.vie <= 0 then
            table.remove(listeTirs, n)
        elseif t.x < 0 or t.x > love.graphics.getWidth() or t.y < 0 or t.y > love.graphics.getHeight() then
            table.remove(listeTirs, n)
        end
    end
end

function tirs.Draw()
    for k, v in ipairs(listeTirs) do
        love.graphics.circle("fill", v.x, v.y, 3)
    end
end

function tirs.GetNumberOfTirs()
    return #listeTirs -- ou table.getn(listeTirs)
end

return tirs

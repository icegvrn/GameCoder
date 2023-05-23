
-- MODULE QUI GERE LES BALLES QUI ONT ETE TIREES
local mapManager = require(PATHS.MAPMANAGER)
local c_Bullets = {}
local Bullets_mt = {__index = c_Bullets}

function c_Bullets.new()
    local bullets = {}
    return setmetatable(bullets, Bullets_mt)
end

function c_Bullets:create()
    local bullets = {
        FireList = {},
        lifeFactor = 0.7
    }

    function bullets:update(dt, parent, weapon)
        self:updateFiredElements(dt, parent, weapon)
        self:AddTrailToBall(dt)
    end

    function bullets:draw(parent, weapon)
        self:drawFiredElements(parent, weapon)
    end

    -- Fonction qui permet de tirer une balle depuis la hitbox de l'arme en direction d'une cible et de l'ajouter à un tableau
    -- Si c'est le joueur, la cible est la souris 
    function bullets:fire(dt, weapon, ownerPosition, ownerScale, ownerHandPosition, ownerWeaponScaling, ownerTarget)
        local pX, pY = weapon.hitBox.position.x, weapon.hitBox.position.y
        local mX, mY = ownerTarget:getPosition()
        local angle = math.atan2(mY - pY, mX - pX)

        if ownerTarget == love.mouse then
            angle = Utils.angleWithMouseWorldPosition(pX, pY)
        end

        local fire = {}
        fire.x = weapon.hitBox.position.x
        fire.y = weapon.hitBox.position.y
        fire.angle = angle
        fire.speed = 180 * dt
        fire.lifeTime = 2
        fire.size = 5
        fire.list_trail = {}
        table.insert(self.FireList, fire)
    end

       -- Fonction qui permet d'ajouter une trainée à la balle pour un effet "sortilège"
       -- en ajoutant une sorte de "balle secondaire" toutes les frames, puis disparition si lifetime dépassé
       function bullets:AddTrailToBall(dt)
        for i = #self.FireList, 1, -1 do
            for n = #self.FireList[i].list_trail, 1, -1 do
                local t = self.FireList[i].list_trail[n]
                t.vie = t.vie - dt + self.lifeFactor * dt
                t.color = t.color - 0.7 * dt
                t.x = t.x + t.vx
                t.y = t.y + t.vy
                if t.vie <= 0 then
                    table.remove(self.FireList[i].list_trail, n)
                end
            end

            local maTrainee = {}
            maTrainee.vx = math.random(-0.1, 0.1)
            maTrainee.vy = math.random(-0.02, 0.02)
            maTrainee.x = self.FireList[i].x
            maTrainee.y = self.FireList[i].y
            maTrainee.color = 1
            maTrainee.vie = 0.4

            table.insert(self.FireList[i].list_trail, maTrainee)
        end
    end

    -- Fonction qui permet de mettre à jour la trajectoire de la balle en fonction de sa vélocité.
    -- Vérifie également si un personnage est touché. Si oui, hit.
    function bullets:updateFiredElements(dt, parent, weapon)
        if (parent.isRangedWeapon) then
            if self.FireList then
                for n = #self.FireList, 1, -1 do
                    local t = self.FireList[n]
                    t.x = t.x + t.speed * math.cos(t.angle)
                    t.y = t.y + t.speed * math.sin(t.angle)
                    t.lifeTime = t.lifeTime - dt

                    -- Suppression de la balle si dépasse son lifetime ou collision avec map ou personnage touché.
                    if t.lifeTime <= 0 then
                        table.remove(self.FireList, n)
                    elseif mapManager:isThereASolidElement(t.x, t.y, t.size, t.size, c) then
                        table.remove(self.FireList, n)
                    else
                        for c = #parent.hittableCharacters, 1, -1 do
                            if parent:isCollide(t.x, t.y, t.size, t.size, parent.hittableCharacters[c]) then
                                parent.hittableCharacters[c].fight:hit(weapon.owner, parent.damage)
                                table.remove(self.FireList, n)
                            end
                        end
                    end
                end
                -- On met également à jour sa trainée.
                bullets:AddTrailToBall(dt, self.FireList)
            end
        end
    end

    -- Draw de chaque balles et ses trainées
    function bullets:drawFiredElements(parent, weapon)
        if (parent.isRangedWeapon) then
            if self.FireList then
                for k, v in ipairs(self.FireList) do
                    if weapon.owner.controller.player then
                        love.graphics.setColor(1, 0.1, 0, 1)
                        love.graphics.circle("fill", v.x, v.y, 6)
                        for n = 1, #v.list_trail do
                            local t = v.list_trail[n]
                            love.graphics.setColor(t.color, 0.1, 0.8, t.vie)
                            love.graphics.circle("fill", t.x, t.y, 6)
                        end
                    else
                        love.graphics.setColor(0, 1, 0.1, 0.8)
                        love.graphics.circle("fill", v.x, v.y, 4)
                        for n = 1, #v.list_trail do
                            local t = v.list_trail[n]
                            love.graphics.setColor(t.color, 0.8, 0.1, t.vie)
                            love.graphics.circle("fill", t.x, t.y, 4)
                        end
                    end
                end
            end
        end
    end

    -- Fonction permettant de clear les projectiles
    function bullets:clear()
        for n = #self.FireList, 1, -1 do
            table.remove(self.FireList, n)
        end
    end

    return bullets
end

return c_Bullets

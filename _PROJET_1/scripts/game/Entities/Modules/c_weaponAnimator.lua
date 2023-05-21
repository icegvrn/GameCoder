local c_WeaponAnimator = {}
local WeaponAnimator_mt = {__index = c_WeaponAnimator}

function c_WeaponAnimator.new()
    local wAnimator = {}
    return setmetatable(wAnimator, WeaponAnimator_mt)
end

function c_WeaponAnimator.create()
    local weaponAnimator = {
        owner = {
            position = {x = 0, y = 0},
            scale = {x = 0, y = 0},
            handPosition = {x = 0, y = 0},
            weaponScaling = 0,
            ownerTarget = nil
        }
    }

    -- L'animator fait bouger l'arme selon les données qu'il reçoit
    function weaponAnimator:update(dt, parent)
        self:move(dt, parent)
    end

    -- Calcul la position, l'angle, l'échelle et la hitzone de l'arme
    function weaponAnimator:move(dt, parent)
        self:calcWeaponPosition(dt, parent)
        self:calcWeaponAngle(dt, parent)
        self:calcWeaponScale(dt, parent)
        self:calcWeaponHitZone(dt, parent)
    end

    -- update les données qui permettent de à move de fonctionner et faire bouger l'arme
    function weaponAnimator:updateInformations(
        dt,
        parent,
        ownerPosition,
        ownerScale,
        ownerHandPosition,
        ownerWeaponScaling,
        ownerTarget)
        self.owner.position.x, self.owner.position.y = ownerPosition.x, ownerPosition.y
        self.owner.scale.x, self.owner.scale.y = ownerScale.x, ownerScale.y
        self.owner.handPosition.x, self.owner.handPosition.y = ownerHandPosition.x, ownerHandPosition.y
        self.owner.weaponScaling, self.owner.weaponScaling = ownerWeaponScaling
        self.ownerTarget = ownerTarget
        self:calcWeaponPosition(dt, parent)
        self:calcWeaponAngle(dt, parent)
        self:calcWeaponScale(dt, parent)
        self:calcWeaponHitZone(dt, parent)
    end

    -- Place l'arme dans le weaponSlot
    function weaponAnimator:calcWeaponPosition(dt, parent)
        if parent.owner then
            parent.transform.position.x = self.owner.handPosition.x
            parent.transform.position.y = self.owner.handPosition.y
        end
    end

    function weaponAnimator:calcWeaponAngle(dt, parent)
        local weaponAngle = 0
        if parent.attack.isFiring == false then
            weaponAngle = math.rad(-parent.heldSlot.idleAngle * self.owner.scale.x)
        else
            local pX, pY = parent.hitBox.position.x, parent.hitBox.position.y
            local mX, mY = self.ownerTarget:getPosition()
            weaponAngle = math.atan2((mY - pY) * self.owner.scale.x, (mX - pX) * self.owner.scale.x)
        end

        if parent.owner.controller.player then
            -- Permet de "tracker" la souris par le calcul atan2 qui calcul l'angle entre deux vecteurs pour modifier l'angle si le personnage est joueur
            local mouseWorldPositionX, mouseWorldPositionY = utils.mouseToWorldCoordinates(love.mouse.getPosition())
            weaponAngle =
                math.atan2(
                (mouseWorldPositionY - self.owner.position.y) * self.owner.scale.x,
                (mouseWorldPositionX - self.owner.position.x) * self.owner.scale.x
            )
        end
        parent.sprites.currentAngle = weaponAngle
    end

    function weaponAnimator:calcWeaponScale(dt, parent)
        parent.transform.scale.x = self.owner.weaponScaling * self.owner.scale.x
        parent.transform.scale.y = self.owner.weaponScaling * self.owner.scale.y
    end

    function weaponAnimator:calcWeaponHitZone(dt, parent)
        local weaponLength =
            (((parent.sprites.spritestileSheets[1]:getWidth() / 2) - parent.heldSlot.holdingOffset.x) *
            self.owner.weaponScaling) *
            self.owner.scale.x
        local weaponHitPositionX =
            parent.transform.position.x +
            weaponLength * math.sin(math.cos(parent.sprites.currentAngle)) * self.owner.scale.x
        local weaponHitPositionY =
            parent.transform.position.y +
            weaponLength * math.sin(math.sin(parent.sprites.currentAngle)) * self.owner.scale.y
        if self.owner.scale.x < 0 then
            weaponHitPositionX =
                parent.transform.position.x - weaponLength * math.cos(parent.sprites.currentAngle) * self.owner.scale.x
        end
        if self.owner.scale.y < 0 then
            weaponHitPositionY =
                parent.transform.position.y - weaponLength * math.sin(parent.sprites.currentAngle) * self.owner.scale.y
        end
        parent.hitBox.position.x = weaponHitPositionX
        parent.hitBox.position.y = weaponHitPositionY
    end

    function weaponAnimator:playattackAnimation(dt, parent)
        parent.sprites.rotationAngle = parent.sprites.rotationAngle + parent.attack.speed * 10 * dt
    end

    return weaponAnimator
end

return c_WeaponAnimator

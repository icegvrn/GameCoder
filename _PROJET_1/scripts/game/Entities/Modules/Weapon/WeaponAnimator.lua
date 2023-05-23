-- MODULE QUI PERMET DE DETERMINER LA POSITION DE L'ARME EN FONCTION DES MOUVEMENTS DU PERSONNAGE QU'IL RECOIT
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

    -- Calcul l'angle que doit adopter l'arme : si l'owner ne tire pas, angle IDLE. S'il tire, calcul l'angle
    -- entre l'arme et la cible (principalement à distance car au corps à corps il y a une animation "playattackanimation" cf en dessous)
    function weaponAnimator:calcWeaponAngle(dt, parent)
        local weaponAngle = 0
        if parent.attack.isFiring == false then
            weaponAngle = math.rad(-parent.heldSlot.idleAngle * self.owner.scale.x)
        else
            local pX, pY = parent.hitBox.position.x, parent.hitBox.position.y
            local mX, mY = self.ownerTarget:getPosition()
            weaponAngle = math.atan2((mY - pY) * self.owner.scale.x, (mX - pX) * self.owner.scale.x)
        end

        -- Si le propriétaire de l'arme est le joueur, l'angle est calculé par rapport à la souris.
        if parent.owner.controller.player then
            local mouseWorldPositionX, mouseWorldPositionY = Utils.mouseToWorldCoordinates(love.mouse.getPosition())
            weaponAngle =
                math.atan2(
                (mouseWorldPositionY - self.owner.position.y) * self.owner.scale.x,
                (mouseWorldPositionX - self.owner.position.x) * self.owner.scale.x
            )
        end
        parent.sprites.currentAngle = weaponAngle
    end

    -- Fonction qui calcul l'échelle que doit avoir l'arme par rapport à son propriétaire en utilisant la donné weaponScaling du propriétaire
    function weaponAnimator:calcWeaponScale(dt, parent)
        parent.transform.scale.x = self.owner.weaponScaling * self.owner.scale.x
        parent.transform.scale.y = self.owner.weaponScaling * self.owner.scale.y
    end

    -- Fonction qui calcul où se trouve la hitBox sur l'arme en fonction de sa position : utilisée pour savoir
    -- a partir de quel endroit le projectil doit être tiré pour une arme au corps à corps.
    function weaponAnimator:calcWeaponHitZone(dt, parent)
        -- Calcul de la longueur de l'arme : la moitié du sprite (= origin) - le offset de son heldSlot, le tout multiplié par le weaponScale du proprio et son scale gauche ou droite
        local weaponLength =
            (((parent.sprites.spritestileSheets[1]:getWidth() / 2) - parent.heldSlot.holdingOffset.x) *
            self.owner.weaponScaling) *
            self.owner.scale.x

        -- Calcul du point X de la hitBox : position X de l'arme + sa longueur multiplié par le cos de son angle
        local weaponHitPositionX = parent.transform.position.x + (weaponLength * math.cos(parent.sprites.currentAngle))

        -- Calcul du point Y de la hitBox : position X de l'arme, + sa longueur multiplié par le sin de l'angle
        local weaponHitPositionY = parent.transform.position.y + (weaponLength * math.sin(parent.sprites.currentAngle))

        parent.hitBox.position.x = weaponHitPositionX
        parent.hitBox.position.y = weaponHitPositionY
    end

    -- Fonction pour lire une animation d'attaque en modifiant l'angle de l'arme (qui est lui par un sin qui permet une impression de va et vient)
    function weaponAnimator:playattackAnimation(dt, parent)
        parent.sprites.rotationAngle = parent.sprites.rotationAngle + parent.attack.speed * 10 * dt
    end

    return weaponAnimator
end

return c_WeaponAnimator

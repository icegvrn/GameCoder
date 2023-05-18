local c_Animator = {}
local Animator_mt = {__index = c_Animator}

function c_Animator.new()
    local _Animator = {}
    return setmetatable(_Animator, Animator_mt)
end

function c_Animator:create()
    local animator = {}

    function animator:update(dt, player)
        if player.character.controller.isCinematicMode then
            self:playEntrance(dt, player)
        end
    end

    function animator:playEntrance(dt, player)
        if player.character:getState() ~= CHARACTERS.STATE.WALKING then
            player.character:setState(CHARACTERS.STATE.WALKING)
        end
        local x, y = player.character.transform:getPosition()
        local speed = player.character.controller.speed
        local velocityX = speed / 3 * dt
        x = x + velocityX
        player.character.transform:setPosition(x, y)
    end

    return animator
end

return c_Animator

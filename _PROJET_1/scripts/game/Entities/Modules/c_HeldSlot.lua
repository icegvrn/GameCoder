local c_HeldSlot = {}
local HeldSlot_mt = {__index = c_HeldSlot}

function c_HeldSlot.new()
    local heldSlot = {}
    return setmetatable(heldSlot, HeldSlot_mt)
end

function c_HeldSlot:create()
    local heldSlot = {
        holdingOffset = {x = 0, y = 0},
        initialScale = 1,
        idleAngle = 45
    }

    function heldSlot:setHoldingOffset(array)
        self.holdingOffset.x = array[1]
        self.holdingOffset.y = array[2]
    end

    return heldSlot
end

return c_HeldSlot

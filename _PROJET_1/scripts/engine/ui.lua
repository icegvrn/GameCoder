local Ui = {}
local Ui_mt = {__index = Ui}

function Ui.new()
    local newUi = {}
    return setmetatable(newUi, Ui_mt)
end

function Ui:load()
end

function Ui:update(dt)
end

function Ui:draw()
end

function Ui:keypressed(key)
end

return Ui

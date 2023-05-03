local Scene = {}
local scene_mt = {__index = Scene}

function Scene.new()
    local newScene = {}
    return setmetatable(newScene, scene_mt)
end

function Scene:load()
end

function Scene:update(dt)
end

function Scene:draw()
end

function Scene:keypressed(key)
end

return Scene

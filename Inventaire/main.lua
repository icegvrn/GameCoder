-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

Inventaire = {}

function love.load()
    AddToInventaire("EPEE", 100, 1)
    AddToInventaire("DAGUE", 50, 1)
    AddToInventaire("HACHE", 200, 2)
end

function love.update(dt)
end

function love.draw()
    y = 1
    for k, v in ipairs(Inventaire) do
        love.graphics.print("Nom : " .. v.ID, 1, y)
        y = y + 15
        love.graphics.print("Etat : " .. v.Etat, 1, y)
        y = y + 15
        love.graphics.print("Niveau : " .. v.Niveau, 1, y)
        y = y + 30
    end
end

function love.keypressed(key)
    if key == "space" then
        UpdateLevelObject("DAGUE")
    end
end

function AddToInventaire(pID, pEtat, pNiveau)
    local obj = {}
    obj.ID = pID
    obj.Etat = pEtat
    obj.Niveau = pNiveau

    table.insert(Inventaire, obj)
end

function UpdateLevelObject(pID)
    for k, v in ipairs(Inventaire) do
        if v.ID == pID then
            v.Niveau = v.Niveau + 1
        end
    end
end

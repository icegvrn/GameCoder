-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

instructionsManager = {}

instructions = ""

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

function instructionsManager.setText(state)
    if state == 1 then
        instructions = ""
    elseif state == 2 then
        instructions = "évacuez la tour"
    elseif state == 3 then
        instructions = "Préparez le pas de tir"
    elseif state == 4 then
        instructions = "Retrait des bras cryogéniques"
    elseif state == 5 then
        instructions = "Lancez les moteurs"
    elseif state == 6 then
        instructions = "Décollage réussi"
    end
end

function instructionsManager.getText()
    return instructions
end

return instructionsManager

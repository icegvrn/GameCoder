-- MODULE DE L'UI DU MENU

controller = require(PATHS.CONFIGS.CONTROLLER)
UiButton = require(PATHS.UIBUTTON)

local uiMenus = {}
local uiButton = UiButton.new()

uiMenus.menu = {}
uiMenus.menu.buttons = {}
uiMenus.menu.STATE = {}
uiMenus.menu.STATE.main = "mainMenu"
uiMenus.menu.STATE.settings = "settings"
uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.main
uiMenus.menu.menuBackground = love.graphics.newImage(PATHS.IMG.ROOT .. "menu1.png")
uiMenus.menu.timer = 0
uiMenus.menu.timerStarted = false

uiMenus.menu.settingsText = love.graphics.newText(UIAll.font50, "Choose mode : " .. controller.mode)

-- A l'ouverture du menu, on est dans l'accueil du menu, on initie les boutons
function uiMenus.load()
    uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.main
    uiMenus.initMenuButtons()
end

-- L'update vérifie si un bouton est hover ou en mode leave ou pas en utilisant la fonction isCollision de l'Utils et vérifie s'il y a clic au moment où le bouton est hover.
function uiMenus.update(self, dt)
    if GAMESTATE.currentState == GAMESTATE.STATE.MENU then
        local mouseX, mouseY = love.mouse.getPosition()

        for n = 1, #uiMenus.menu.buttons do
            local bttn = uiMenus.menu.buttons[n]
            if bttn.state == uiMenus.menu.STATE.currentState then
                if Utils.isCollision(bttn.position.x, bttn.position.y, bttn.width, bttn.height, mouseX, mouseY, 0, 0) then
                    bttn:hover()
                    if love.mouse.isDown(1) then
                        if uiMenus.menu.timerStarted == false then
                            bttn:interaction(self)
                            uiMenus.menu.timerStarted = true
                        end
                    end
                elseif bttn.isHover then
                    bttn:leave()
                end
            end
        end
        -- Ajout d'une petite latence pour éviter le double clic.
        if uiMenus.menu.timerStarted then
            uiMenus.menu.timer = uiMenus.menu.timer + 1 * dt
            if uiMenus.menu.timer >= 0.2 then
                uiMenus.menu.timer = 0
                uiMenus.menu.timerStarted = false
            end
        end
    end
end

-- Fonction draw qui va appeler le draw de chaque bouton, ainsi que le texte qui va avec settings si on est dans settings
function uiMenus.draw()
    if GAMESTATE.currentState == GAMESTATE.STATE.MENU then
        love.graphics.draw(uiMenus.menu.menuBackground, 0, 0)

        for n = 1, #uiMenus.menu.buttons do
            if uiMenus.menu.buttons[n].state == uiMenus.menu.STATE.currentState then
                uiMenus.menu.buttons[n]:draw()
            end
        end

        if uiMenus.menu.STATE.currentState == uiMenus.menu.STATE.settings then
            uiMenus.drawSettingsText()
        end
    end
end

-- Création des boutons à l'initiation, via le module UIButton, puis on les initie. On indique s'ils font parti de l'accueil ou du sous-menu "settings"
-- Une string indique l'action qu'ils effectuent au clic dessus via la fonction .action()
function uiMenus.initMenuButtons()
    uiMenus.menu.buttons[1] =
        uiButton:create(PATHS.IMG.UI .. "menu_restart.png", 200, uiMenus.menu.STATE.main, "restart")
    uiMenus.menu.buttons[2] =
        uiButton:create(PATHS.IMG.UI .. "menu_settings.png", 310, uiMenus.menu.STATE.main, "settings")
    uiMenus.menu.buttons[3] = uiButton:create("contents/images/ui/menu_exit.png", 420, uiMenus.menu.STATE.main, "quit")
    uiMenus.menu.buttons[4] =
        uiButton:create(PATHS.IMG.UI .. "menu_mode.png", 310, uiMenus.menu.STATE.settings, "changeMode")
    uiMenus.menu.buttons[5] =
        uiButton:create(PATHS.IMG.UI .. "menu_return.png", 420, uiMenus.menu.STATE.settings, "main")
    for n = 1, #uiMenus.menu.buttons do
        uiMenus.menu.buttons[n]:init()
    end
end

-- Fonction qui permet d'activer les actions au clic sur un bouton dans le menu. Elles sont appelées par le bouton, via une string action.
function uiMenus.action(action)
    if action == "quit" then
        return love.event.quit()
    elseif action == "settings" then
        return uiMenus.changeState(uiMenus.menu.STATE.settings)
    elseif action == "main" then
        return uiMenus.changeState(uiMenus.menu.STATE.main)
    elseif action == "restart" then
        return love.event.quit("restart")
    elseif action == "changeMode" then
        return controller.changeMode()
    end
end

-- Fonction qui permet d'afficher le texte qui va avec l'écran "settings", à savoir le changement de mode AZERTY a QWERTY
function uiMenus.drawSettingsText()
    uiMenus.menu.settingsText:set("Choose mode : " .. controller.mode)
    love.graphics.draw(uiMenus.menu.settingsText, Utils.screenWidth / 2 - uiMenus.menu.settingsText:getWidth() / 2, 200)
    love.graphics.setFont(UIAll.font12)
    love.graphics.print("ESC", 10, 10)
    love.graphics.setFont(UIAll.defaultFont)
end

-- Fonction qui permet de changer le state du menu
function uiMenus.changeState(state)
    uiMenus.menu.STATE.currentState = state
end

return uiMenus

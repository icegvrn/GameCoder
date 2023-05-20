ui = require("scripts/engine/ui")
controller = require("scripts/engine/controller")
UiButton = require("scripts/ui/Modules/m_uiButton")

local uiMenus = ui.new()
local uiButton = UiButton.new()

uiMenus.menu = {}
uiMenus.menu.buttons = {}
uiMenus.menu.STATE = {}
uiMenus.menu.STATE.main = "mainMenu"
uiMenus.menu.STATE.settings = "settings"
uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.main
uiMenus.menu.menuBackground = love.graphics.newImage("contents/images/menu1.png")
uiMenus.menu.timer = 0
uiMenus.menu.timerStarted = false

local textFont = love.graphics.newFont("contents/fonts/pixelfont.ttf", 50)
local font12 = love.graphics.newFont(12)
uiMenus.menu.settingsText = love.graphics.newText(textFont, "Choose mode : " .. controller.mode)

function uiMenus.load()
    uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.main
    uiMenus.initMenuButtons()
end

function uiMenus.initMenuButtons()
    uiMenus.menu.buttons[1] =
        uiButton:create("contents/images/ui/menu_restart.png", 200, uiMenus.menu.STATE.main, "restart")
    uiMenus.menu.buttons[2] =
        uiButton:create("contents/images/ui/menu_settings.png", 310, uiMenus.menu.STATE.main, "settings")
    uiMenus.menu.buttons[3] = uiButton:create("contents/images/ui/menu_exit.png", 420, uiMenus.menu.STATE.main, "quit")
    uiMenus.menu.buttons[4] =
        uiButton:create("contents/images/ui/menu_mode.png", 310, uiMenus.menu.STATE.settings, "changeMode")
    uiMenus.menu.buttons[5] =
        uiButton:create("contents/images/ui/menu_return.png", 420, uiMenus.menu.STATE.settings, "main")
end

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
        -- Pour éviter le double clic
        if uiMenus.menu.timerStarted then
            uiMenus.menu.timer = uiMenus.menu.timer + 1 * dt
            if uiMenus.menu.timer >= 0.2 then
                uiMenus.menu.timer = 0
                uiMenus.menu.timerStarted = false
            end
        end
    end
end

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

function uiMenus.changeState(state)
    uiMenus.menu.STATE.currentState = state
end

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

function uiMenus.drawSettingsText()
    uiMenus.menu.settingsText:set("Choose mode : " .. controller.mode)
    love.graphics.draw(uiMenus.menu.settingsText, Utils.screenWidth / 2 - uiMenus.menu.settingsText:getWidth() / 2, 200)
    love.graphics.setFont(font12)
    love.graphics.print("ESC", 10, 10)
    love.graphics.setFont(defaultFont)
end

return uiMenus

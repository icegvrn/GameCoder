require("scripts/states/GAMESTATE")
ui = require("scripts/engine/ui")
controller = require("scripts/engine/controller")

local uiMenus = ui.new()

uiMenus.menu = {}
uiMenus.menu.STATE = {}

uiMenus.menu.STATE.main = "mainMenu"
uiMenus.menu.STATE.settings = "settings"
uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.main
uiMenus.menu.menuBackground = love.graphics.newImage("contents/images/menu1.png")
uiMenus.menu.buttons = {}
uiMenus.menu.buttons.timer = 0
uiMenus.menu.buttons.timerStarted = false
local textFont = love.graphics.newFont("contents/fonts/pixelfont.ttf", 50)
local font12 = love.graphics.newFont(12)
uiMenus.menu.settingsText = love.graphics.newText(textFont, "Choose mode : " .. controller.mode)

function uiMenus.load()
    uiMenus.initMenuButtons()
end

function uiMenus.update(self, dt)
    if GAMESTATE.currentState == GAMESTATE.STATE.MENU then
        local mouseX, mouseY = love.mouse.getPosition()
        for n = 1, #uiMenus.menu.buttons.elements do
            local el = uiMenus.menu.buttons.elements[n]

            if el.state == uiMenus.menu.STATE.currentState then
                if Utils.isCollision(el.positionX, el.positionY, el.width, el.height, mouseX, mouseY, 0, 0) then
                    el.hover()
                    if love.mouse.isDown(1) then
                        if uiMenus.menu.buttons.timerStarted == false then
                            el.interaction()
                            uiMenus.menu.buttons.timerStarted = true
                        end
                    end
                elseif el.isHover then
                    el.leave()
                end
            end
        end
        if uiMenus.menu.buttons.timerStarted then
            uiMenus.menu.buttons.timer = uiMenus.menu.buttons.timer + 1 * dt
            if uiMenus.menu.buttons.timer >= 0.2 then
                uiMenus.menu.buttons.timer = 0
                uiMenus.menu.buttons.timerStarted = false
            end
        end
    end
end

function uiMenus.draw()
    if GAMESTATE.currentState == GAMESTATE.STATE.MENU then
        
        love.graphics.draw(uiMenus.menu.menuBackground, 0, 0)

        if uiMenus.menu.STATE.currentState == uiMenus.menu.STATE.main then
            local buttonRestart =
                love.graphics.draw(
                uiMenus.menu.buttons.restartSheet,
                uiMenus.menu.buttons.currentRestart,
                Utils.screenWidth / 2 - uiMenus.menu.buttons.restartSheet:getWidth() / 4,
                200
            )
            local buttonSettings =
                love.graphics.draw(
                uiMenus.menu.buttons.settingsSheet,
                uiMenus.menu.buttons.currentSettings,
                Utils.screenWidth / 2 - uiMenus.menu.buttons.settingsSheet:getWidth() / 4,
                310
            )
            local buttonExit =
                love.graphics.draw(
                uiMenus.menu.buttons.exitSheet,
                uiMenus.menu.buttons.currentExit,
                Utils.screenWidth / 2 - uiMenus.menu.buttons.exitSheet:getWidth() / 4,
                420
            )
        elseif uiMenus.menu.STATE.currentState == uiMenus.menu.STATE.settings then
            uiMenus.menu.settingsText:set("Choose mode : " .. controller.mode)

            love.graphics.draw(
                uiMenus.menu.settingsText,
                Utils.screenWidth / 2 - uiMenus.menu.settingsText:getWidth() / 2,
                200
            )

            local buttonBack =
                love.graphics.draw(
                uiMenus.menu.buttons.backSheet,
                uiMenus.menu.buttons.currentBack,
                Utils.screenWidth / 2 - uiMenus.menu.buttons.backSheet:getWidth() / 4,
                420
            )

            local buttonMode =
                love.graphics.draw(
                uiMenus.menu.buttons.modeSheet,
                uiMenus.menu.buttons.currentMode,
                uiMenus.menu.buttons.mode.positionX,
                uiMenus.menu.buttons.mode.positionY
            )
        end
    end
    love.graphics.setFont(font12)
        love.graphics.print("ESC", 10, 10)
        love.graphics.setFont(defaultFont)
end

function uiMenus.initMenuButtons()
    uiMenus.menu.buttons.restartSheet = love.graphics.newImage("contents/images/ui/menu_restart.png")
    uiMenus.menu.buttons.settingsSheet = love.graphics.newImage("contents/images/ui/menu_settings.png")
    uiMenus.menu.buttons.exitSheet = love.graphics.newImage("contents/images/ui/menu_exit.png")
    uiMenus.menu.buttons.backSheet = love.graphics.newImage("contents/images/ui/menu_return.png")
    uiMenus.menu.buttons.modeSheet = love.graphics.newImage("contents/images/ui/menu_mode.png")

    uiMenus.menu.buttons.restart = {}
    uiMenus.menu.buttons.restart.positionX = Utils.screenWidth / 2 - uiMenus.menu.buttons.restartSheet:getWidth() / 4
    uiMenus.menu.buttons.restart.positionY = 200
    uiMenus.menu.buttons.restart.width = uiMenus.menu.buttons.restartSheet:getWidth() / 2
    uiMenus.menu.buttons.restart.height = uiMenus.menu.buttons.restartSheet:getHeight()
    uiMenus.menu.buttons.restart.isHover = false
    uiMenus.menu.buttons.restart.state = uiMenus.menu.STATE.main

    uiMenus.menu.buttons.settings = {}
    uiMenus.menu.buttons.settings.positionX = Utils.screenWidth / 2 - uiMenus.menu.buttons.settingsSheet:getWidth() / 4
    uiMenus.menu.buttons.settings.positionY = 310
    uiMenus.menu.buttons.settings.width = uiMenus.menu.buttons.settingsSheet:getWidth() / 2
    uiMenus.menu.buttons.settings.height = uiMenus.menu.buttons.settingsSheet:getHeight()
    uiMenus.menu.buttons.settings.isHover = false
    uiMenus.menu.buttons.settings.state = uiMenus.menu.STATE.main

    uiMenus.menu.buttons.exit = {}
    uiMenus.menu.buttons.exit.positionX = Utils.screenWidth / 2 - uiMenus.menu.buttons.exitSheet:getWidth() / 4
    uiMenus.menu.buttons.exit.positionY = 420
    uiMenus.menu.buttons.exit.width = uiMenus.menu.buttons.exitSheet:getWidth() / 2
    uiMenus.menu.buttons.exit.height = uiMenus.menu.buttons.exitSheet:getHeight()
    uiMenus.menu.buttons.exit.isHover = false
    uiMenus.menu.buttons.exit.state = uiMenus.menu.STATE.main

    uiMenus.menu.buttons.restart = {}
    uiMenus.menu.buttons.restart.positionX = Utils.screenWidth / 2 - uiMenus.menu.buttons.restartSheet:getWidth() / 4
    uiMenus.menu.buttons.restart.positionY = 200
    uiMenus.menu.buttons.restart.width = uiMenus.menu.buttons.restartSheet:getWidth() / 2
    uiMenus.menu.buttons.restart.height = uiMenus.menu.buttons.restartSheet:getHeight()
    uiMenus.menu.buttons.restart.isHover = false
    uiMenus.menu.buttons.restart.state = uiMenus.menu.STATE.main

    uiMenus.menu.buttons.mode = {}
    uiMenus.menu.buttons.mode.positionX = Utils.screenWidth / 2 - uiMenus.menu.buttons.modeSheet:getWidth() / 4
    uiMenus.menu.buttons.mode.positionY = 310
    uiMenus.menu.buttons.mode.width = uiMenus.menu.buttons.modeSheet:getWidth() / 2
    uiMenus.menu.buttons.mode.height = uiMenus.menu.buttons.modeSheet:getHeight()
    uiMenus.menu.buttons.mode.isHover = false
    uiMenus.menu.buttons.mode.state = uiMenus.menu.STATE.settings

    uiMenus.menu.buttons.back = {}
    uiMenus.menu.buttons.back.positionX = Utils.screenWidth / 2 - uiMenus.menu.buttons.backSheet:getWidth() / 4
    uiMenus.menu.buttons.back.positionY = 420
    uiMenus.menu.buttons.back.width = uiMenus.menu.buttons.backSheet:getWidth() / 2
    uiMenus.menu.buttons.back.height = uiMenus.menu.buttons.backSheet:getHeight()
    uiMenus.menu.buttons.back.isHover = false
    uiMenus.menu.buttons.back.state = uiMenus.menu.STATE.settings

    uiMenus.menu.buttons.elements = {}
    uiMenus.menu.buttons.elements[1] = uiMenus.menu.buttons.restart
    uiMenus.menu.buttons.elements[2] = uiMenus.menu.buttons.settings
    uiMenus.menu.buttons.elements[3] = uiMenus.menu.buttons.exit
    uiMenus.menu.buttons.elements[4] = uiMenus.menu.buttons.back
    uiMenus.menu.buttons.elements[5] = uiMenus.menu.buttons.mode

    uiMenus.menu.buttons.restartButton =
        love.graphics.newQuad(
        0,
        0,
        uiMenus.menu.buttons.restart.width,
        uiMenus.menu.buttons.restart.height,
        uiMenus.menu.buttons.restartSheet:getWidth(),
        uiMenus.menu.buttons.restartSheet:getHeight()
    )
    uiMenus.menu.buttons.restartButtonHover =
        love.graphics.newQuad(
        uiMenus.menu.buttons.restart.width,
        0,
        uiMenus.menu.buttons.restart.width,
        uiMenus.menu.buttons.restart.height,
        uiMenus.menu.buttons.restartSheet:getWidth(),
        uiMenus.menu.buttons.restartSheet:getHeight()
    )

    uiMenus.menu.buttons.settingsButton =
        love.graphics.newQuad(
        0,
        0,
        uiMenus.menu.buttons.settings.width,
        uiMenus.menu.buttons.settings.height,
        uiMenus.menu.buttons.settingsSheet:getWidth(),
        uiMenus.menu.buttons.settingsSheet:getHeight()
    )
    uiMenus.menu.buttons.settingsButtonHover =
        love.graphics.newQuad(
        uiMenus.menu.buttons.settings.width,
        0,
        uiMenus.menu.buttons.settings.width,
        uiMenus.menu.buttons.settings.height,
        uiMenus.menu.buttons.settingsSheet:getWidth(),
        uiMenus.menu.buttons.settingsSheet:getHeight()
    )

    uiMenus.menu.buttons.exitButton =
        love.graphics.newQuad(
        0,
        0,
        uiMenus.menu.buttons.exit.width,
        uiMenus.menu.buttons.exit.height,
        uiMenus.menu.buttons.exitSheet:getWidth(),
        uiMenus.menu.buttons.exitSheet:getHeight()
    )
    uiMenus.menu.buttons.exitButtonHover =
        love.graphics.newQuad(
        uiMenus.menu.buttons.exit.width,
        0,
        uiMenus.menu.buttons.exit.width,
        uiMenus.menu.buttons.exit.height,
        uiMenus.menu.buttons.exitSheet:getWidth(),
        uiMenus.menu.buttons.exitSheet:getHeight()
    )

    uiMenus.menu.buttons.modeButton =
        love.graphics.newQuad(
        0,
        0,
        uiMenus.menu.buttons.mode.width,
        uiMenus.menu.buttons.mode.height,
        uiMenus.menu.buttons.modeSheet:getWidth(),
        uiMenus.menu.buttons.modeSheet:getHeight()
    )
    uiMenus.menu.buttons.modeButtonHover =
        love.graphics.newQuad(
        uiMenus.menu.buttons.mode.width,
        0,
        uiMenus.menu.buttons.mode.width,
        uiMenus.menu.buttons.mode.height,
        uiMenus.menu.buttons.modeSheet:getWidth(),
        uiMenus.menu.buttons.modeSheet:getHeight()
    )

    uiMenus.menu.buttons.backButton =
        love.graphics.newQuad(
        0,
        0,
        uiMenus.menu.buttons.back.width,
        uiMenus.menu.buttons.back.height,
        uiMenus.menu.buttons.backSheet:getWidth(),
        uiMenus.menu.buttons.backSheet:getHeight()
    )
    uiMenus.menu.buttons.backButtonHover =
        love.graphics.newQuad(
        uiMenus.menu.buttons.back.width,
        0,
        uiMenus.menu.buttons.back.width,
        uiMenus.menu.buttons.back.height,
        uiMenus.menu.buttons.backSheet:getWidth(),
        uiMenus.menu.buttons.backSheet:getHeight()
    )

    uiMenus.menu.buttons.currentExit = uiMenus.menu.buttons.exitButton
    uiMenus.menu.buttons.currentSettings = uiMenus.menu.buttons.settingsButton
    uiMenus.menu.buttons.currentRestart = uiMenus.menu.buttons.restartButton
    uiMenus.menu.buttons.currentMode = uiMenus.menu.buttons.modeButton
    uiMenus.menu.buttons.currentBack = uiMenus.menu.buttons.backButton

    uiMenus.menu.buttons.restart.hover = function()
        uiMenus.menu.buttons.currentRestart = uiMenus.menu.buttons.restartButtonHover
        uiMenus.menu.buttons.restart.isHover = true
    end
    uiMenus.menu.buttons.restart.leave = function()
        uiMenus.menu.buttons.currentRestart = uiMenus.menu.buttons.restartButton
        uiMenus.menu.buttons.restart.isHover = false
    end
    uiMenus.menu.buttons.settings.hover = function()
        uiMenus.menu.buttons.currentSettings = uiMenus.menu.buttons.settingsButtonHover
        uiMenus.menu.buttons.settings.isHover = true
    end
    uiMenus.menu.buttons.settings.leave = function()
        uiMenus.menu.buttons.currentSettings = uiMenus.menu.buttons.settingsButton
        uiMenus.menu.buttons.settings.isHover = false
    end
    uiMenus.menu.buttons.exit.hover = function()
        uiMenus.menu.buttons.currentExit = uiMenus.menu.buttons.exitButtonHover
        uiMenus.menu.buttons.exit.isHover = true
    end
    uiMenus.menu.buttons.exit.leave = function()
        uiMenus.menu.buttons.currentExit = uiMenus.menu.buttons.exitButton
        uiMenus.menu.buttons.exit.isHover = false
    end

    uiMenus.menu.buttons.mode.hover = function()
        uiMenus.menu.buttons.currentMode = uiMenus.menu.buttons.modeButtonHover
        uiMenus.menu.buttons.mode.isHover = true
    end
    uiMenus.menu.buttons.mode.leave = function()
        uiMenus.menu.buttons.currentMode = uiMenus.menu.buttons.modeButton
        uiMenus.menu.buttons.mode.isHover = false
    end

    uiMenus.menu.buttons.back.hover = function()
        uiMenus.menu.buttons.currentBack = uiMenus.menu.buttons.backButtonHover
        uiMenus.menu.buttons.back.isHover = true
    end
    uiMenus.menu.buttons.back.leave = function()
        uiMenus.menu.buttons.currentBack = uiMenus.menu.buttons.backButton
        uiMenus.menu.buttons.back.isHover = false
    end

    uiMenus.menu.buttons.restart.interaction = function()
        love.load()
    end

    uiMenus.menu.buttons.settings.interaction = function()
        uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.settings
    end

    uiMenus.menu.buttons.exit.interaction = function()
        love.event.quit()
    end

    uiMenus.menu.buttons.mode.interaction = function()
        controller.changeMode()
    end

    uiMenus.menu.buttons.back.interaction = function()
        uiMenus.menu.STATE.currentState = uiMenus.menu.STATE.main
    end
end

return uiMenus

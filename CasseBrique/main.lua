-- Débogueur Visual Studio Code tomblind.local-lua-debugger-vscode
if pcall(require, "lldebugger") then
    require("lldebugger").start()
end

-- Cette ligne permet d'afficher des traces dans la console pendant l'éxécution
io.stdout:setvbuf("no")

paddle = {}
paddle.width = 80
paddle.height = 25
paddle.position = {}
paddle.position.x = 0
paddle.position.y = 0

ball = {}
ball.position = {}
ball.position.x = 0
ball.position.y = 0
ball.scale = 10
ball.color = {1, 0, 0.4}
ball.speed = 1
ball.start = false
ball.direction = {}
ball.direction.x = 0
ball.direction.y = 0

function love.load()
    largeur = love.graphics.getWidth()
    hauteur = love.graphics.getHeight()
    Init()
end

function love.update(dt)
    paddle.position.x = love.mouse.getX() - paddle.width / 2

    if ball.start == false then
        ball.position.x = love.mouse.getX() - ball.scale / 2
    else
        moveBall(dt)
    end
end

function love.draw()
    love.graphics.setColor(1, 1, 1)
    love.graphics.rectangle("fill", paddle.position.x, paddle.position.y, paddle.width, paddle.height)
    love.graphics.setColor(ball.color)
    love.graphics.circle("fill", ball.position.x, ball.position.y, ball.scale)
end

function love.keypressed(key)
    if key == "space" then
        ball.start = true
    end
end

function Init()
    paddle.position.x = largeur / 2 - paddle.width / 2
    paddle.position.y = hauteur - paddle.height - 20
    ball.position.x = largeur / 2 - paddle.width / 2
    ball.position.y = hauteur - paddle.height - 20 - ball.scale
end

function moveBall(dt)
    ball.position.y = ball.position.y - ball.speed * 250 * dt
end

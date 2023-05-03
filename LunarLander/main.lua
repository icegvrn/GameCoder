io.stdout:setvbuf('no')

local Lander = {}

Lander.x = 0
Lander.y = 0
Lander.angle = -90
Lander.velocityX = 0
Lander.velocityY = 0
Lander.sprite = love.graphics.newImage("ship.png")
Lander.engineSprite = love.graphics.newImage("engine.png")
Lander.engineIsFire = false
Lander.speed = 3
gravityForce = 1

function love.load()

  wWidth = love.graphics.getWidth()
  wHeight = love.graphics.getHeight()
  resetShip()
end

function love.update(dt)
  addGravity(dt)

  if love.keyboard.isDown("up") then
    local angle_radian = math.rad(Lander.angle) 
    local force_x = math.cos(angle_radian) * (Lander.speed * dt)
    local force_y = math.sin(angle_radian) * (Lander.speed * dt) 
    Lander.velocityY = Lander.velocityY + force_y
    Lander.velocityX = Lander.velocityX + force_x
    Lander.engineIsFire = true
  elseif love.keyboard.isDown("down") then
    Lander.velocityY = Lander.velocityY+0.2*dt
    Lander.engineIsFire = false
  else 
    Lander.engineIsFire = false
  end

  if love.keyboard.isDown("left") then
    Lander.angle = Lander.angle - 90*dt
  elseif love.keyboard.isDown("right") then
    Lander.angle = Lander.angle + 90*dt
  end

  if love.keyboard.isDown("space") then
    resetShip()
  end
end

function love.draw()
  love.graphics.draw(Lander.sprite, Lander.x, Lander.y, math.rad(Lander.angle), 1, 1, Lander.sprite:getWidth()/2, Lander.sprite:getHeight()/2)

  if Lander.engineIsFire == true then
    love.graphics.draw(Lander.engineSprite, Lander.x, Lander.y, math.rad(Lander.angle), 1, 1, Lander.engineSprite:getWidth()/2, Lander.engineSprite:getHeight()/2)
  end
end

function addGravity(dt) 
  Lander.velocityY = Lander.velocityY + gravityForce*dt
  Lander.x = Lander.x + Lander.velocityX
  Lander.y = Lander.y + Lander.velocityY
end

function resetShip()
  Lander.x = wWidth/2
  Lander.y = wHeight/2  
  Lander.velocityX = 0
  Lander.velocityY = 0
  Lander.angle = -90
end
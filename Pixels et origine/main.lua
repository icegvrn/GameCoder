
chevalier = {}
chevalier.sprites = {}
chevalier.sprites.idle = {}
chevalier.currentSprite = love.graphics.newImage("idle_2.png")


sprite_id = 1
timer = 500000000

function love.load()
  chevalier.sprites.idle[1] = love.graphics.newImage("idle_2.png")
  chevalier.sprites.idle[2] = love.graphics.newImage("idle_3.png")
  chevalier.sprites.idle[3] = love.graphics.newImage("idle_4.png")
  sprite_id = 1 
  

end

function love.update(dt)
  
  for n=1, timer do
    if n == timer then
      updateSprite()
      end
    end 
  
end

function love.draw()
  love.graphics.draw(chevalier.currentSprite, 50, 50)
end

function updateSprite() 
  
  chevalier.currentSprite = chevalier.sprites.idle[sprite_id]
  
  if sprite_id == #chevalier.sprites.idle then
    sprite_id = 1 
  else 
    sprite_id = sprite_id+1
    end
  end
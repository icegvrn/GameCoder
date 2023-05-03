
-- Mise en forme --
wWidth = love.graphics.getWidth()
wHeight = love.graphics.getHeight()
gameMargin = 5
paddlesMargin = 5

-- Setup des éléments --
-- Pad joueur 1 --
pad = {}
pad.x = 0 
pad.y = 0 
pad.maxX = 0
pad.maxY = 0
pad.width = 20
pad.height = 110
pad.speed = 8
pad.topY = 0
pad.middleTopY = 0
pad.middleY = 0
pad.middleBottomY = 0
pad.bottomY = 0

-- Pad joueur 2 --
pad2 = {}
pad2.x = 0 
pad2.y = 0 
pad2.maxX = 0
pad2.maxY = 0
pad2.width = 20
pad2.height = 110
pad2.speed = 8
pad2.topY = 0
pad2.middleTopY = 0
pad2.middleY = 0
pad2.middleBottomY = 0
pad2.bottomY = 0

-- Balle --
ball = {}
ball.x = 0
ball.y = 0
ball.width = 10
ball.height = 10
ball.speedX = 6
ball.speedY = 6
ball.colorR = 1
ball.colorG = 1
ball.colorB = 1

winGoal = 10
score_j1 = 0
score_j2 = 0
last_player_win = 1




list_trail = {} 

pad_sound = nil
fail_sound = nil
pad2_sound = nil
music = nil

spawnBallrandomer = 1
firstBall = true


lifeFactor = 0

gameState = "start"
winner = "none"
loser = "none"
tickCounter = 0

function love.load() 
  Init()
end

function love.update(dt)

  UpdatePadsLocationInformations()

  if gameState == "game" then 
    CheckPlayersPadsMove()
    MoveBall(dt)
    CheckIfAPlayerWin()

  elseif gameState == "start" or gameState == "win" then
    if love.keyboard.isDown("space") then
      love.audio.play(spaceSound)
      RestartGame()
    end

  end

end

function love.draw() 

  DrawBaseElements()

  if gameState == "start" then 
    DrawStartElements()

  elseif gameState == "game" then 
    DrawGameElements()

  elseif gameState == "win" then
    DrawWinScreen()

  end

end

----------------------------------- Fonctions de draw -----------------------------------
function DrawBaseElements()
  love.graphics.rectangle("line", gameMargin, gameMargin, wWidth - gameMargin*2, wHeight - gameMargin*2)
  font = love.graphics.newFont("munro.ttf", 30)
  love.graphics.setFont(font)
end

function DrawStartElements() 
  local title = "PONG GAME BY SIMON"
  love.graphics.print(title, wWidth/2 - font:getWidth(title)/2, wHeight/2-font:getHeight(title)/2)

  tickCounter = tickCounter+1
  if tickCounter < 50 then
    font = love.graphics.newFont("munro.ttf", 20)
    love.graphics.setFont(font)
    local pressToStart = "Press -SPACE- to start"
    love.graphics.print(pressToStart, wWidth/2-font:getWidth(pressToStart)/2, wHeight/2+50-font:getHeight(pressToStart))
  elseif tickCounter > 100 then
    tickCounter = 0
  end
  font = love.graphics.newFont("munro.ttf", 20)
  love.graphics.setFont(font)
  local instruction = "Goal: "..winGoal.." to win. Remember: Player 1 -Z- or -S- | Player 2 -UP- or -DOWN- "
  love.graphics.print(instruction, wWidth/2-font:getWidth(instruction)/2, wHeight/2+80-font:getHeight(instruction))
end

function DrawGameElements()
  --Affichage de la trainée de balle
  for n=1, #list_trail do 
    local t = list_trail[n]
    love.graphics.setColor(ball.colorR,ball.colorG,ball.colorB, t.vie)
    love.graphics.circle("fill", t.x, t.y, ball.width, ball.height)
  end

  --Affichage balle et raquette
  love.graphics.setColor(45,10,80,1)
  love.graphics.circle("fill", ball.x, ball.y, ball.width, ball.height)
  love.graphics.rectangle("fill", pad.x, pad.y, pad.width, pad.height)
  love.graphics.rectangle("fill", pad2.x, pad2.y, pad2.width, pad2.height)
  scoreDisplay = love.graphics.line( wWidth/2, gameMargin, wWidth/2, wHeight-gameMargin)

  --Affichage des scores
  local score = " Player 1 : "..score_j1.."     Player 2 : "..score_j2
  love.graphics.print(score, wWidth/2-170, wHeight-50)
end

function DrawWinScreen()

  tickCounter = tickCounter+1
  local title = ""..winner.." wins! You are a loser "..loser.."..."
  love.graphics.print(title, wWidth/2 - font:getWidth(title)/2, wHeight/2-font:getHeight(title)/2)

  if tickCounter < 50 then
    font = love.graphics.newFont("munro.ttf", 20)
    love.graphics.setFont(font)
    local pressToStart = "Press -SPACE- to restart"
    love.graphics.print(pressToStart, wWidth/2-font:getWidth(pressToStart)/2, wHeight/2+50-font:getHeight(pressToStart))
  elseif tickCounter > 100 then
    tickCounter = 0
  end

end


----------------------------------- Fonctions de gestion du jeu appelées ponctuellement -----------------------------------
function Init()
  pad_sound = love.audio.newSource("pad1.wav", "static")
  fail_sound = love.audio.newSource("failed.wav", "static")
  pad2_sound = love.audio.newSource("pad2.wav", "static")
  music = love.audio.newSource("background.mp3", "static")
  winMusic = love.audio.newSource("win.mp3", "static")
  spaceSound = love.audio.newSource("space.wav", "static")
  music:setVolume(0.3)
  music:setLooping(true)  
  ChangeBallColor()
end

function RestartGame()
  score_j1 = 0
  score_j2 = 0
  love.audio.play(music)
  pad.x = gameMargin + paddlesMargin
  pad2.x = wWidth - pad2.width - paddlesMargin - gameMargin
  resetBall()
  gameState = "game"
end

function resetBall() 
  
  ChangeBallColor()
  
  pad.y = gameMargin + wHeight/2 - pad.height/2 
  pad2.y = gameMargin + wHeight/2 - pad2.height/2
  ball.x = wWidth/2 - ball.width/2
  ball.y = wHeight/2 - ball.height/2
  
  ball.speedX = 4
  ball.speedY = 4

-- Randomise si la balle va en haut ou en bas au démarrage
  spawnBallrandomer = math.random(0,2)
  
  -- Pour changer le sens du service de la balle selon le dernier joueur a avoir perdu
  if last_player_win == 1 then 
    if (spawnBallrandomer < 1) then ball.speedY = -ball.speedY else ball.speedY = ball.speedY end
    ball.speedX = -ball.speedX

  elseif last_player_win == 2 then
    if (spawnBallrandomer < 1) then ball.speedY = -ball.speedY else ball.speedY = ball.speedY end
  end
end

function ChangeBallColor()
    -- Randomise la couleur de la balle
  ball.colorR = math.random(0.3,1) 
  ball.colorG = math.random(0.3,1) 
  ball.colorB = math.random(0.3,1) 
  end

function CheckIfAPlayerWin()
  if score_j1 == winGoal then
    PlayerWin("Player 1", "Player 2")
  elseif score_j2 == winGoal then
    PlayerWin("Player 2", "Player 1")
  end
end

function PlayerWin(winnerPlayer,loserPlayer)
  gameState = "win"
  winner = winnerPlayer
  loser = loserPlayer
  music:stop()
  love.audio.play(winMusic)
  winMusic:setVolume(0.1)
  winMusic:setLooping(false)   
end

----------------------------------- Fonctions utilisées in-game dans l'update -----------------------------------
function UpdatePadsLocationInformations() 
  pad.maxX = pad.x + pad.width 
  pad.maxY = pad.y + pad.height
  pad2.maxX = pad2.x + pad2.width 
  pad2.maxY = pad2.y + pad2.height

  pad.topY = pad.y
  pad.middleTopY = pad.y + (pad.height/5)
  pad.middleY = pad.y + (pad.height/5)*2
  pad.middleBottomY = pad.y + (pad.height/5)*3
  pad.bottomY = pad.y + (pad.height/5)*4

  pad2.topY = pad2.y
  pad2.middleTopY = pad2.y + (pad2.height/5)
  pad2.middleY = pad2.y + (pad2.height/5)*2
  pad2.middleBottomY = pad2.y + (pad2.height/5)*3
  pad2.bottomY = pad2.y + (pad2.height/5)*4  

end

function CheckPlayersPadsMove()

  -- Commande du joueur 1
  if love.keyboard.isDown("s") 
  then 
    if pad.y < wHeight - gameMargin - pad.height
    then
      pad.y = pad.y + pad.speed
    else
      pad.y = wHeight - gameMargin - pad.height
    end
  elseif love.keyboard.isDown("z") then
    if pad.y > gameMargin
    then
      pad.y = pad.y - pad.speed
    else
      pad.y = gameMargin
    end
  end

  -- Commande du joueur 2
  if love.keyboard.isDown("down") 
  then 
    if pad2.y < wHeight - gameMargin - pad2.height
    then
      pad2.y = pad2.y + pad2.speed
    else 
      pad2.y = wHeight - gameMargin - pad2.height
    end
  elseif love.keyboard.isDown("up") then
    if pad2.y > gameMargin 
    then
      pad2.y = pad2.y - pad2.speed
    else 
      pad2.y = gameMargin
    end
  end
end

function MoveBall(dt) 

-- Ajoute une trainée à la balle
  AddTrailToBall(dt)

-- Dévie la balle si elle touche un bord du jeu
  if (ball.y + ball.height >= wHeight - gameMargin) or (ball.y <= gameMargin)
  then
    ball.speedY = -ball.speedY
    love.audio.play(pad2_sound)
  end

  -- Perdu pour le joueur de droite
  if (ball.x >= wWidth - gameMargin - ball.width) then
    love.audio.play(fail_sound)
    score_j1 = score_j1+1
    last_player_win = 1
    resetBall()

    -- Perdu pour le joueur de gauche
  elseif (ball.x <= gameMargin) then
    love.audio.play(fail_sound)
    score_j2 = score_j2+1
    last_player_win = 2
    resetBall()
  end

  -- La balle atteint la zone de la raquette joueur 1
  if ball.x <= pad.maxX then
    --Test si la balle est sur la raquette de joueur 1
    if  ball.y + ball.height > pad.y and ball.y < pad.maxY then 
      -- Modification de la vitesse de la balle en fonction de l'endroit où elle a touché la raquette 
      CalcBallDirectionPad1()
    end
  end
  -- La balle atteint la zone de la raquette joueur 2
  if ball.x + ball.width >= pad2.x then
    -- Test si elle est sur la raquette
    if ball.y + ball.height > pad2.y and ball.y < pad2.maxY then 
      -- Modification de la vitesse de la balle en fonction de l'endroit où elle a touché la raquette
      CalcBallDirectionPad2()
    end
  end

  -- On applique la direction calculée
  ball.x =   ball.x + ball.speedX
  ball.y =  ball.y + ball.speedY
end


function AddTrailToBall(dt)

  for n=#list_trail, 1, -1 do
    local t = list_trail[n]
    t.vie = t.vie - dt + lifeFactor*dt
    t.x = t.x + t.vx
    t.y = t.y + t.vy
    if t.vie <= 0  then 
      table.remove(list_trail, n)
    end
  end   

  local maTrainee = {}
  maTrainee.vx = math.random(-1,1) 
  maTrainee.vy = math.random(-1,1)
  maTrainee.x = ball.x
  maTrainee.y = ball.y
  maTrainee.vie = 0.2

  table.insert(list_trail, maTrainee)
end

function CalcBallDirectionPad1() 
  if ball.y+ ball.height >= pad.topY and ball.y <= pad.middleTopY then
    ball.speedX = 6
    ball.speedY = -6
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.7
    end

  elseif ball.y+ ball.height >= pad.middleTopY and ball.y <= pad.middleY then
    ball.speedX = 5
    ball.speedY = -5
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.5
    end

  elseif ball.y+ ball.height >= pad.middleY and ball.y <= pad.middleBottomY then
    ball.speedX = 4
    ball.speedY = 4
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.1
    end

  elseif ball.y+ ball.height >= pad.middleBottomY and ball.y <= pad.bottomY then
    ball.speedX = 5
    ball.speedY = 5
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.5
    end

  elseif ball.y+ ball.height >= pad.bottomY and ball.y <= pad.maxY then
    ball.speedX = 6
    ball.speedY = 6
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.7
    end
  end

  ball.x = pad.maxX
  love.audio.play(pad_sound)

end

function CalcBallDirectionPad2()
  if ball.y+ ball.height >= pad2.topY and ball.y <= pad2.middleTopY then
    ball.speedX = -6
    ball.speedY = -6
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.7
    end

  elseif ball.y+ ball.height >= pad2.middleTopY and ball.y <= pad2.middleY then
    ball.speedX = -5
    ball.speedY = -5
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.5
    end

  elseif ball.y+ ball.height >= pad2.middleY and ball.y <= pad2.middleBottomY then
    ball.speedX = -4
    ball.speedY = 4
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0
    end

  elseif ball.y+ ball.height >= pad2.middleBottomY and ball.y <= pad2.bottomY then
    ball.speedX = -5
    ball.speedY = 5
    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.5
    end

  elseif ball.y+ ball.height >= pad2.bottomY and ball.y <= pad2.maxY then
    ball.speedX = -6
    ball.speedY = 6

    for n=#list_trail, 1, -1 do
      local t = list_trail[n]
      lifeFactor = 0.7
    end
  end
  ball.x = pad2.x - ball.width
  love.audio.play(pad_sound)

end 





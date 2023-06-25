using BricksGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace BricksGame
{
    public class Pad : Sprite, ICollider
    {
        private float _speed = 15;
        public override float Speed { get { return _speed; } set { _speed = value; } }
        private Animator animator;
        private List<Texture2D> textures;
        public Gamesystem.CharacterState currentState;
        private Gamesystem.CharacterState lastState;
        public Vector2 Size;

        private Color padColor = Color.White;
        //Barre de vie et vie
        private Texture2D lifeIcon;
        public bool IsDead = false;
        private float life;
        private float initialLife;
        private float timer = 0.2f;
        private Rectangle lifeBar;
        private int lifeBarLenght = 100;
        private int lifeBarHeight = 12;
        private Rectangle currentLife;
        private Color currentLifeColor;
        private Color[] lifeColors = { Color.Green, Color.Yellow, Color.Orange, Color.Red };
        private Texture2D lifeBarTexture;

        //Barre de points 
        private Texture2D pointsIcon;
        private float points;
        private float Points{ get { return points; } set { if (value > maxPoints) { points = maxPoints; } else { points = value; } } }
        private float maxPoints;
        private Rectangle pointsBar;
        private int pointsBarLenght = 100;
        private int pointsBarHeight = 12;
        private Rectangle currentPoints;
        private Texture2D pointsBarTexture;

        //Portrait
        private Texture2D portrait;

        //Ball count
        private int munitionNb = 10;

        public bool HasMunition { get { if (munitionNb <= 0) { return false; } else { return true; } } }


        private Texture2D munitionCounter;
        public Pad(List<Texture2D> p_texture) : base(p_texture)
        {
     
            Speed = 15;
            currentState = Gamesystem.CharacterState.idle;
            lastState = currentState;
            textures = new List<Texture2D>();
            textures = p_texture;
            animator = new Animator(p_texture[(int)Gamesystem.CharacterState.idle], 0.15f);
            Size = new Vector2((currentTexture.Width / (currentTexture.Width / currentTexture.Height)), currentTexture.Height);
            Reset();

            // Vie
            lifeIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_heart");
            initialLife = 3000f;
        
            PlayerState.SetLife((int)initialLife);
           
            lifeBarTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/blank");
            lifeBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 + 50, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, lifeBarLenght, lifeBarHeight);
            currentLife = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 + 50, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, (int)((life / initialLife) * lifeBarLenght), lifeBarHeight);

            //Points
            pointsIcon = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_power");
            maxPoints = 3000f;
            pointsBarTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/blank");
            pointsBar = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width/2 - 50 - pointsBarLenght - pointsIcon.Width/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, pointsBarLenght, pointsBarHeight);
            currentPoints = new Rectangle(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - 50 - pointsBarLenght - pointsIcon.Width/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - 50, (int)((life / initialLife) * pointsBarLenght), pointsBarHeight);

            // Portrait
            portrait = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/character_portrait");

            // Munitions
            munitionCounter = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/icon_counter_true");
        }

        public override void Update(GameTime p_GameTime)
        {
            if (life == PlayerState.Life)
            {
                padColor = Color.White;
            }

            else
            {
                padColor = Color.Red; 
                life = PlayerState.Life;
            }

            Points = PlayerState.Points;
           
  
            if (life <= 0)
            {
                IsDead = true;
            }

            // Update de la barre de vie
            currentLife.Width = (int)((life / initialLife) * lifeBarLenght);

            //Update de la barre de points
            currentPoints.Width = (int)((points / maxPoints) * pointsBarLenght);


            if (life >= initialLife * 0.65f)
            {
                currentLifeColor = lifeColors[0];
            }
            else if (life >= initialLife * 0.55f)
            {
                currentLifeColor = lifeColors[1];
            }
            else if (life >= initialLife * 0.35f)
            {
                currentLifeColor = lifeColors[2];
            }
            else
            {
                currentLifeColor = lifeColors[3];
            }
   

            if (lastState != currentState)
            {
                animator.ChangeSpriteSheet(textures[(int)currentState]);
                lastState = currentState;
            }

            BoundingBox = new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y);
            animator.Update(p_GameTime);

                //    base.Update(p_GameTime);
        }

        public override void Move(float p_x, float p_y)
        {
            if (Position.X + p_x * Speed < 0) 
            {
                Position = new Vector2(0, Position.Y);
            }
                
            else if ((Position.X + p_x * Speed) > ServiceLocator.GetService<GraphicsDevice>().Viewport.Width - Size.X)
            {
                Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width - Size.X, Position.Y);
            }

            else
            {
             base.Move(p_x, p_y);
            }
            
        }

        public override void Draw(SpriteBatch p_SpriteBatch)
        {

            animator.Draw(p_SpriteBatch, Position, padColor);
            //  DrawBoundingBox(p_SpriteBatch);
            //  base.Draw(p_SpriteBatch);

            //Draw lifeBar
            p_SpriteBatch.Draw(lifeBarTexture, lifeBar, Color.Gray);
            p_SpriteBatch.Draw(lifeBarTexture, currentLife, currentLifeColor);
            p_SpriteBatch.Draw(lifeIcon, new Vector2((lifeBar.X + lifeBar.Width) - lifeIcon.Width*0.5f, lifeBar.Y - lifeIcon.Height/3), Color.White);

            //Draw pointsBar 
            p_SpriteBatch.Draw(pointsBarTexture, pointsBar, Color.Gray);
            p_SpriteBatch.Draw(pointsBarTexture, currentPoints, Color.Purple);
            p_SpriteBatch.Draw(pointsIcon, new Vector2((pointsBar.X + pointsBar.Width) - pointsIcon.Width * 0.5f, pointsBar.Y - pointsIcon.Height/2), Color.White);

            //Draw portrait
            Vector2 portraitPosition = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - portrait.Width / 2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - portrait.Height * 1.25f);
            p_SpriteBatch.Draw(portrait, portraitPosition, Color.White);

            //Draw Counter 
            for (int n = 0; n < munitionNb; n++)
            {
                p_SpriteBatch.Draw(munitionCounter, new Vector2(portraitPosition.X + 6 *(n), portraitPosition.Y + portrait.Height + 2) , Color.White);
            }
            
        }

        public void TouchedBy(GameObject p_By)
        {
            if (p_By is Ball)
            {
                Debug.WriteLine("J'ai touché la balle !");
            }

      
        }
        private void DrawBoundingBox(SpriteBatch spriteBatch)
        {
            Rectangle rect = BoundingBox;
            Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });

            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.Red);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.Red);
          
        }

        public void Reset()
        {
            Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Width / 2 - Size.X/2, ServiceLocator.GetService<GraphicsDevice>().Viewport.Height - Size.Y*2f);

        }

        public void ChangeState(Gamesystem.CharacterState state )
        {
            currentState = state;
        }

        public void RemoveMunition(int nb)
        {
            munitionNb -= nb;
        }

    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace BricksGame
{
    /// <summary>
    /// Classe représentant une barre de couleur dont la valeur peut varier. Utilisée pour les points et utilisée comme classe parente des barre évolutive (EvolutiveColoredGauge) qui elles changent en plus de couleur en fonction de leur valeur
    /// </summary>
    public class ColoredGauge
    {
        private float currentValue;
        public float CurrentValue { get { return currentValue; } set { if (value <= maxValue && value >= 0){ currentValue = value;} else {if (value < 0){ currentValue = 0;} else if (value > maxValue){currentValue = maxValue;}}} }
        protected float maxValue;
        protected Rectangle gaugeRectangle;
        protected Rectangle currentValueRectangle;
        protected Texture2D gaugeTexture;
        protected Color barColor;
        private bool displayText = false;
        protected Vector2 textPosition;
        private Vector2 textOffest;
        protected Color textColor;
        public Vector2 Position { get; set; }
       
        public ColoredGauge(float p_maxValue, Rectangle p_rectangle, Color p_color)
        {
            maxValue = p_maxValue;
            currentValue = p_maxValue;
            gaugeRectangle = p_rectangle;
            currentValueRectangle = gaugeRectangle;
            gaugeTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/blank");
            barColor = p_color;
            Position = new Vector2(p_rectangle.X, p_rectangle.Y);
        }

        public ColoredGauge(float p_maxValue, Rectangle p_rectangle, Color p_color, bool p_text, Vector2 p_textPosition, Color p_textColor)
        {
            maxValue = p_maxValue;
            currentValue = p_maxValue;
            gaugeRectangle = p_rectangle;
            currentValueRectangle = gaugeRectangle;
            gaugeTexture = ServiceLocator.GetService<ContentManager>().Load<Texture2D>("images/blank");
            barColor = p_color;
            displayText = p_text;
            textPosition = p_textPosition;
            textColor = p_textColor;
            textOffest = Position - p_textPosition;
            Position = new Vector2(p_rectangle.X, p_rectangle.Y);
        }

        public virtual void Update(GameTime gameTime)
        {
            gaugeRectangle.X = (int)Position.X;
            gaugeRectangle.Y = (int)Position.Y;
            textPosition.X = Position.X + textOffest.X;
            textPosition.Y = Position.Y + textOffest.Y;  
            currentValueRectangle.X = gaugeRectangle.X;
            currentValueRectangle.Y = gaugeRectangle.Y;
            currentValueRectangle.Width = (int)((currentValue/maxValue) * gaugeRectangle.Width);
        }

        public virtual void Update(GameTime gameTime, int value, Vector2 p_position)
        {
            Position = p_position;
            currentValue = value;
            gaugeRectangle.X = (int)Position.X;
            gaugeRectangle.Y = (int)Position.Y;
            textPosition.X = Position.X + textOffest.X;
            textPosition.Y = Position.Y + textOffest.Y;
            currentValueRectangle.X = gaugeRectangle.X;
            currentValueRectangle.Y = gaugeRectangle.Y;
            currentValueRectangle.Width = (int)((currentValue / maxValue) * gaugeRectangle.Width);
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gaugeTexture, gaugeRectangle, Color.LightGray);
            spriteBatch.Draw(gaugeTexture, currentValueRectangle, barColor);

            if (displayText)
            {
                spriteBatch.DrawString(ServiceLocator.GetService<IFontService>().GetFont(IFontService.Fonts.Font14), ((int)currentValue).ToString(), new Vector2(textPosition.X, textPosition.Y), textColor);
            }
        }
    }

}

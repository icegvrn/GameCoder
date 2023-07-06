using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BricksGame
{
    /// <summary>
    /// Classe héritante de ColoredGauge qui permet d'avoir une jauge dont la couleur varie selon sa valeur. 
    /// </summary>
    public class EvolutiveColoredGauge : ColoredGauge
    {
        // Couleurs de la jauge et seuils pour changer de couleur
        private Color[] colors;
        private float[] thresholds;
        
        public EvolutiveColoredGauge(float p_maxValue, Rectangle p_rectangle, Color p_color, float[] p_thresholds, Color[] p_colors) : base(p_maxValue, p_rectangle, p_color)
        {
            colors = p_colors;
            thresholds = p_thresholds;
        }
        public EvolutiveColoredGauge(float p_maxValue, Rectangle p_rectangle, Color p_color, float[] p_thresholds, Color[] p_colors, bool p_text, Vector2 p_textPosition, Color p_textColor ) : base(p_maxValue, p_rectangle, p_color, p_text, p_textPosition, p_textColor)
        {
            colors = p_colors;
            thresholds = p_thresholds;
        }


        
        public override void Update(GameTime gameTime, int value, Vector2 position)
        {
            UpdateGaugeAndColor(value, position);
            base.Update(gameTime );
        }

        // Update permettant d'update la position, la valeur et de faire varier la couleur selon si la valeur courante est passé sous un seuil ou non
        private void UpdateGaugeAndColor(int value, Vector2 position)
        {
            Position = position;
            CurrentValue = value;

            for (int i = thresholds.Length; i >= 0; i--)
            {
                if (i != thresholds.Length)
                {
                    if (CurrentValue >= maxValue * thresholds[i])
                    {
                        barColor = colors[i];
                    }
                }
                else
                {
                    barColor = colors[thresholds.Length];
                }
            }

            textColor = barColor;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

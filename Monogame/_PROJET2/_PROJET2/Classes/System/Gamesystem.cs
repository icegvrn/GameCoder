
namespace BricksGame
{
    public static class Gamesystem
    {
        public enum dice
        {
            none,
            d3,
            d4,
            d6,
            d8,
            d10,
            d12,
            d20,
            dMagic
        }
        public enum CharacterState { idle, l_idle, walk, l_walk, fire, l_fire, hit, die, l_die }
        public enum CharacterDirection { left, right }
        public enum CharacterType {  hero, monster }

        public enum BallState { idle, fired, hit, destroyed }
    }

  
}

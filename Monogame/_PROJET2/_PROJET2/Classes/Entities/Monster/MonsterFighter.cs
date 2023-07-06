using Microsoft.Xna.Framework;


namespace BricksGame {

    /// <summary>
    /// Classe qui gère la dimension "combat" du monstre en lui permettant d'avoir une force et d'attaquer
    /// </summary>
    public class MonsterFighter : Fighter
    {
        private Monster monster;

        public int Strenght { get; private set; }
        public bool Attacks { get; private set; }

        //Timer pour réguler l'attaque
        private float attackTimer = 0f;
        private float attackCooldown = 1.5f;

        public MonsterFighter(Monster p_monster, int lvl)
        {
            monster = p_monster;
            Strenght = lvl * 35;
            IsAttacker = false;
        }

        // Update le timer d'attaque du monstre et enlève le monstre des attaquants quand il meurt
        public override void Update(GameTime p_GameTime)
        {
            UpdateAttackTimer(p_GameTime);

            if (monster.IsDead)
            {
                IsAttacker = false;
            }
        }

        // Méthode qui permet de déclarer le monstre attaquant
        public override void StartAttack()
        {
            IsAttacker = true;
            Attacks = true;
            attackTimer = 0f;
        }

        // Méthode qui permet d'indiquer que le monstre attaquant fait une attaque selon un ryhtme défini par le timer
        public override void Attack()
        {
                if (attackTimer >= attackCooldown)
                {
                    IsAttacker = true;
                    attackTimer = 0f;
                    Attacks = true;
                }
                else
                {
                    IsAttacker = false;
                    Attacks = false;
                }
        }

        // Update du timer d'attaque
        private void UpdateAttackTimer(GameTime p_GameTime)
        {
            attackTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksGame { 
    public class MonsterFighter : Fighter
    {
        private Monster monster;

        public int Power { get; private set; }
        public bool Attacks { get; private set; }

        private float attackTimer = 0f;
        private float attackCooldown = 1.5f;

        public MonsterFighter(Monster p_monster, int lvl)
        {
            monster = p_monster;
            Power = lvl * 35;
            IsAttacker = false;
        }

        public override void Update(GameTime p_GameTime)
        {
            UpdateAttackTimer(p_GameTime);
        }

        public override void StartAttack()
        {
            IsAttacker = true;
            Attacks = true;
            attackTimer = 0f;
        }
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

        private void UpdateAttackTimer(GameTime p_GameTime)
        {
            attackTimer += (float)p_GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}


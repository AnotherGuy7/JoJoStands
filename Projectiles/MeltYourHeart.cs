using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class MeltYourHeart : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 28;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.maxPenetrate = 25;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.AddBuff(BuffID.Confused, 120);
        }
        public int checkNumber = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 4; i++)
            {
                checkNumber = i;
                int Xadd = 0;
                int Yadd = 0;
                if (checkNumber == 0)
                {
                    Xadd = 1;
                    Yadd = 0;
                }
                if (checkNumber == 1)
                {
                    Xadd = 0;
                    Yadd = 1;
                }
                if (checkNumber == 2)
                {
                    Xadd = -1;
                    Yadd = 0;
                }
                if (checkNumber == 3)
                {
                    Xadd = 0;
                    Yadd = -1;
                }
                Tile tileTarget = Main.tile[(int)projectile.position.X + Xadd, (int)projectile.position.Y + Yadd];
                if (tileTarget.type != -1)
                {
                    projectile.ai[0] = 1f;
                }
            }

            return base.OnTileCollide(oldVelocity);
        }
        public override void AI()
        {

        }
    }
}
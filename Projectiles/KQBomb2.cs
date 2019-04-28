using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class KQBomb2 : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GrenadeIV);
            projectile.width = 20;
            projectile.height = 18;
            projectile.timeLeft = 1;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item62);
            int explosionRadius = 7;            //from ExampleMod's ExampleExplosive
            int minTileX = (int)(projectile.position.X / 16f - (float)explosionRadius);
            int maxTileX = (int)(projectile.position.X / 16f + (float)explosionRadius);
            int minTileY = (int)(projectile.position.Y / 16f - (float)explosionRadius);
            int maxTileY = (int)(projectile.position.Y / 16f + (float)explosionRadius);
            if (minTileX < 0)
            {
                minTileX = 0;
            }
            if (maxTileX > Main.maxTilesX)
            {
                maxTileX = Main.maxTilesX;
            }
            if (minTileY < 0)
            {
                minTileY = 0;
            }
            if (maxTileY > Main.maxTilesY)
            {
                maxTileY = Main.maxTilesY;
            }
        }
    }
}
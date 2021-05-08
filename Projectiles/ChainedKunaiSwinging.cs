using System;
using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ChainedKunaiSwinging : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/ChainedKunai_Swinging";

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private int hamonConsumptionTimer = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }


            if (Main.mouseLeft)
            {
                projectile.timeLeft = 2;
                projectile.Center = player.Center;

                projectile.frameCounter++;
                if (projectile.frameCounter % 2 == 0)
                {
                    if (projectile.alpha == 0)
                    {
                        projectile.alpha = 255;
                    }
                    else
                    {
                        projectile.alpha = 0;
                        projectile.rotation += 0.8f;
                    }
                }
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
                Main.dust[dustIndex].noGravity = true;

                hamonConsumptionTimer++;
                if (hamonConsumptionTimer > 120)
                {
                    hPlayer.amountOfHamon -= 2;
                    hamonConsumptionTimer = 0;
                }
            }
            else
            {
                Vector2 velocity = Main.MouseWorld - player.Center;
                velocity.Normalize();
                velocity *= 14f;
                Projectile.NewProjectile(player.Center, velocity, mod.ProjectileType("ChainedKunaiProjectile"), projectile.damage, 3f, projectile.owner);
                projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.alpha == 255)
            {
                damage = 0;
            }
            else
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    target.AddBuff(mod.BuffType("Sunburn"), 12 * 60);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Texture2D chainedKunaiTexture;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (chainedKunaiTexture == null && Main.netMode != NetmodeID.Server)
                chainedKunaiTexture = mod.GetTexture("Projectiles/ChainedKunai_Swinging");

            Vector2 origin = new Vector2(chainedKunaiTexture.Width / 2f, chainedKunaiTexture.Height / 2f);
            spriteBatch.Draw(chainedKunaiTexture, projectile.Center - Main.screenPosition, null, Color.White * projectile.alpha, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
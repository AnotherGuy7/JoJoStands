using System;
using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class ChainedKunaiSwinging : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/ChainedKunai_Swinging";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private int hamonConsumptionTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>());
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }


            if (Main.mouseLeft)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
                Projectile.Center = player.Center;

                Projectile.frameCounter++;
                if (Projectile.frameCounter % 2 == 0)
                {
                    if (Projectile.alpha == 0)
                    {
                        Projectile.alpha = 255;
                    }
                    else
                    {
                        Projectile.alpha = 0;
                        Projectile.rotation += 0.8f;
                    }
                }
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
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
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, velocity, ModContent.ProjectileType<ChainedKunaiProjectile>(), Projectile.damage, 3f, Projectile.owner);
                Projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.alpha == 255)
            {
                damage = 0;
            }
            else
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    target.AddBuff(ModContent.BuffType<Sunburn>(), 12 * 60);
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
                chainedKunaiTexture = Mod.GetTexture("Projectiles/ChainedKunai_Swinging>();

            Vector2 origin = new Vector2(chainedKunaiTexture.Width / 2f, chainedKunaiTexture.Height / 2f);
            spriteBatch.Draw(chainedKunaiTexture, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.alpha, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
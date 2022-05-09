using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class BindingEmeraldString : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.frameCounter += 1;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.Next(0, 1 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 61, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.noGravity)
                target.velocity.X = 0f;
            else
                target.velocity = Vector2.Zero;
            target.GetGlobalNPC<JoJoGlobalNPC>().stunnedByBindingEmerald = true;
            target.GetGlobalNPC<JoJoGlobalNPC>().bindingEmeraldDurationTimer = (int)Projectile.ai[0] * 60;
        }
    }
}
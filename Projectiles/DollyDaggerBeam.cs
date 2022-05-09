using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class DollyDaggerBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 70;
        }

        private int nearestNPCWhoAmI = -1;
        private float nearestDistance = 999f;

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(13f);

            if (nearestNPCWhoAmI == -1)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
                        {
                            float distance = Projectile.Distance(npc.position);
                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestNPCWhoAmI = npc.whoAmI;
                            }
                        }
                    }
                }
            }
            if (nearestNPCWhoAmI >= 0)
            {
                NPC target = Main.npc[nearestNPCWhoAmI];
                Vector2 vel = target.position - Projectile.position;
                vel.Normalize();
                vel *= 9f;
                Projectile.velocity = vel;
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
        }
    }
}
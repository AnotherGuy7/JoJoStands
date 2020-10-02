using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class DollyDaggerBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 70;
        }

        private int nearestNPCWhoAmI = -1;
        private float nearestDistance = 999f;

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(13f);

            if (nearestNPCWhoAmI == -1)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
                        {
                            float distance = projectile.Distance(npc.position);
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
                Vector2 vel = target.position - projectile.position;
                vel.Normalize();
                vel *= 9f;
                projectile.velocity = vel;
            }
            Lighting.AddLight(projectile.Center, 0.5f, 0.5f, 0.5f);
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class WormholeNail : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 34;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 3600;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.rotation += MathHelper.ToRadians(13f * projectile.direction);
            player.AddBuff(BuffID.Obstructed, 2);
            player.position = projectile.Center;
            player.immune = true;
            player.noFallDmg = true;
            player.controlUseItem = false;
            if (player.HasBuff(BuffID.Suffocation))
            {
                player.ClearBuff(BuffID.Suffocation);
            }
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            Lighting.AddLight(projectile.Center, 2f, 2f, 2f);
            if (projectile.owner == Main.myPlayer)
            {
                if (player.GetModPlayer<MyPlayer>().tuskActNumber != 3)
                {
                    projectile.Kill();
                }
                if (Main.mouseLeft && !WorldGen.TileEmpty((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f)))
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 5f;

                    if (Main.MouseWorld.X > projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else
                {
                    if (WorldGen.TileEmpty((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f)))
                    {
                        projectile.velocity.Y += 0.1f;
                        projectile.velocity.X *= 0.04f;
                    }
                    else
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                }
                if ((projectile.timeLeft <= 3540 && Main.mouseRight) || player.dead)
                {
                    projectile.Kill();
                }
                projectile.netUpdate = true;
            }

            int dustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 240);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];

            while (!WorldGen.TileEmpty((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f)))
            {
                projectile.position.Y -= 10f;
            }
            player.position = projectile.position + new Vector2(0f, -35f);
            player.velocity.Y -= 6f;
            player.AddBuff(mod.BuffType("AbilityCooldown"), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(20));
        }
    }
}
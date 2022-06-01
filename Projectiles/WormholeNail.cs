using JoJoStands.Buffs.Debuffs;
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
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += MathHelper.ToRadians(13f * Projectile.direction);
            player.AddBuff(BuffID.Obstructed, 2);
            player.position = Projectile.Center;
            player.immune = true;
            player.noFallDmg = true;
            player.controlUseItem = false;
            player.GetModPlayer<MyPlayer>().hideAllPlayerLayers = true;
            if (player.HasBuff(BuffID.Suffocation))
            {
                player.ClearBuff(BuffID.Suffocation);
            }
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            Lighting.AddLight(Projectile.Center, 2f, 2f, 2f);
            if (Projectile.owner == Main.myPlayer)
            {
                if (player.GetModPlayer<MyPlayer>().tuskActNumber != 3)
                {
                    Projectile.Kill();
                }
                if (Main.mouseLeft && !WorldGen.TileEmpty((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f)))
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.position;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 5f;

                    if (Main.MouseWorld.X > Projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else
                {
                    if (WorldGen.TileEmpty((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f)))
                    {
                        Projectile.velocity.Y += 0.1f;
                        Projectile.velocity.X *= 0.04f;
                    }
                    else
                    {
                        Projectile.velocity = Vector2.Zero;
                    }
                }
                if ((Projectile.timeLeft <= 3540 && Main.mouseRight) || player.dead)
                {
                    Projectile.Kill();
                }
                Projectile.netUpdate = true;
            }

            int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 240);
            Main.dust[dustIndex].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            while (!WorldGen.TileEmpty((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f)))
            {
                Projectile.position.Y -= 10f;
            }
            player.position = Projectile.position + new Vector2(0f, -35f);
            player.velocity.Y -= 6f;
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(20));
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class KillerQueenBomb : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Extras/Bomb"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private const float ExplosionRadius = 5.5f * 16f;
        private bool crit = false;

        public override void Kill(int timeLeft)
        {
            //Normal grenade explosion effects
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 7f;
                dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 3f;
            }

            for (int i = 0; i < 25; i++)        //Extra explosion effects
            {
                float angle = (360f / 25f) * i;
                Vector2 dustPosition = Projectile.Center + (angle.ToRotationVector2() * 7f);
                Vector2 dustVelocity = dustPosition - Projectile.Center;
                dustVelocity.Normalize();
                dustVelocity *= 7f;
                int dustIndex = Dust.NewDust(dustPosition, Projectile.width, Projectile.height, DustID.Torch, dustVelocity.X, dustVelocity.Y, 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
            }

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
                if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                    crit = true;
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (Projectile.ai[1] != 0f && npc.whoAmI == Projectile.ai[1])
                    {
                        npc.StrikeNPC((int)(Projectile.ai[0] * 1.2f), 7f, npc.direction, crit);
                        continue;
                    }

                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= ExplosionRadius)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - Projectile.position.X > 0)
                            hitDirection = 1;

                        npc.StrikeNPC((int)Projectile.ai[0], 7f, hitDirection, crit);
                    }
                }
            }
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active && !player.dead)
                {
                    if (Vector2.Distance(Projectile.Center, player.Center) > ExplosionRadius)
                        continue;

                    int bombDamage = (int)Projectile.ai[0];
                    if (p == player.whoAmI || Main.player[Projectile.owner].team == player.team)
                    {
                        bombDamage = (int)(Projectile.ai[0] * 0.25f);
                    }
                    int hitDirection = -1;
                    if (player.position.X - Projectile.position.X > 0)
                    {
                        hitDirection = 1;
                    }
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " got careless around one of KQ's bombs."), bombDamage, hitDirection);
                }
            }
            SoundEngine.PlaySound(SoundID.Item62);
        }
    }
}
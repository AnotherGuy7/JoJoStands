using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class FireAnkh : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 2;
            Projectile.scale = 0.7f;
        }

        private bool playedSound = false;

        public override void AI()
        {
            if (!playedSound)
            {
                SoundEngine.PlaySound(SoundID.Item20);
                playedSound = true;
            }
            if (Projectile.wet || Projectile.honeyWet)
                Projectile.scale -= 0.05f;
            if (Projectile.scale <= 0f)
                Projectile.Kill();

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Projectile.velocity.X > 0)
                Projectile.direction = 1;
            else
                Projectile.direction = -1;
            Projectile.spriteDirection = Projectile.direction;

            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, Scale: 3.5f);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].velocity *= 1.4f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (Main.rand.Next(0, 101) < Projectile.ai[0])
            {
                target.AddBuff(BuffID.OnFire, (int)Projectile.ai[1]);
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 60 * 9);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                }
            }
        }
    }
}
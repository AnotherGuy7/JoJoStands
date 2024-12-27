using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class KnifeProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.maxPenetrate = 25;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.velocity.Y += 0.3f;
            if (Projectile.velocity.X <= 0)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation += MathHelper.ToRadians(90f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            target.immune[Projectile.owner] = 0;
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 6); i++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width / 2, Projectile.height / 2, DustID.Lead, -Projectile.velocity.X * 0.1f, -Projectile.velocity.Y * 0.1f);
            }
            SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
        }
    }
}
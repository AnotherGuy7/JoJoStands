using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class CrossfireHurricaneAnkh : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/FireAnkh"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
        }

        private float rotationMultIncrementTimer = 0f;
        private float rotationMult = 1f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            rotationMultIncrementTimer++;
            if (rotationMultIncrementTimer >= 25)
            {
                rotationMult += 0.5f;
                rotationMultIncrementTimer = 0;
            }
            Projectile.ai[1] += 0.1f;
            Projectile.ai[0] += 1 * rotationMult;
            Vector2 offset = player.Center + (Projectile.ai[1].ToRotationVector2() * Projectile.ai[0]);
            Projectile.position = offset;
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, Scale: 3.5f);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].velocity *= 1.4f;

            if (Projectile.Center.X - player.Center.X > 0)
                Projectile.direction = 1;
            else
                Projectile.direction = -1;
            Projectile.spriteDirection = Projectile.direction;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;

            target.immune[Projectile.owner] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 101) < 50f)
                target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class CrossfireHurricaneAnkh : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/FireAnkh"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 20;
            projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = 2;
        }

        private float rotationMultIncrementTimer = 0f;
        private float rotationMult = 1f;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            rotationMultIncrementTimer++;
            if (rotationMultIncrementTimer >= 25)
            {
                rotationMult += 0.5f;
                rotationMultIncrementTimer = 0;
            }
            projectile.ai[1] += 0.1f;
            projectile.ai[0] += 1 * rotationMult;
            Vector2 offset = player.Center + (projectile.ai[1].ToRotationVector2() * projectile.ai[0]);
            projectile.position = offset;
            int num109 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 3.5f);
            Main.dust[num109].noGravity = true;
            Main.dust[num109].velocity *= 1.4f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 101) < 50f)
            {
                target.AddBuff(BuffID.OnFire, 300);
            }
        }
    }
}
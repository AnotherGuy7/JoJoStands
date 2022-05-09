using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class ReqNail : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/ControllableNail"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Spin>(), 60);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Spin>(), 60);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            int pinkDustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 205, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            Main.dust[pinkDustIndex].noGravity = true;

            int blueDustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 202, Projectile.velocity.X * -0.3f, Projectile.velocity.Y * -0.3f);
            Main.dust[blueDustIndex].noGravity = true;
        }
    }
}
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class SteelBallProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.LightDisc);
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.maxPenetrate = 2;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Items.Hamon.HamonPlayer hamonPlayer = Main.player[Projectile.owner].GetModPlayer<Items.Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5 && Main.rand.Next(0, 101) <= 25)
                target.AddBuff(ModContent.BuffType<Spin>(), 40);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.oldVelocity = new Vector2(0);
            return base.OnTileCollide(oldVelocity);
        }

        public override void AI()
        {
            Projectile.rotation += (float)Projectile.direction * 0.8f;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 128, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
        }
    }
}
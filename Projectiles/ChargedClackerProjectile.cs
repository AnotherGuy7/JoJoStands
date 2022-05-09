using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class ChargedClackerProjectile : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/ClackerProjectile"; }
        }

        public override void SetStaticDefaults()        //find the autoload thing for the texture and make it the same as the clacker one
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            
            Projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            Projectile.width = 18;
            Projectile.height = 12;
            Projectile.aiStyle = 3;
            Projectile.ranged = true;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.maxPenetrate = 4;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<Vampire>()))
            {
                target.AddBuff(ModContent.BuffType<Sunburn>(), 240);
            }
            base.ModifyHitPvp(target, ref damage, ref crit);
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 169, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            Projectile.rotation *= 0f;
            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.frame = 0;
        }
    }
}
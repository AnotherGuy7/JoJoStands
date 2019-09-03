using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class HamonVinoBottleCap : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 3;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 450;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffID.Confused, 120);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI()
        {
            if (Main.LocalPlayer.HeldItem.type == mod.ItemType("HamonVino"))
            {
                projectile.penetrate = 2;
            }
            if (Main.LocalPlayer.HeldItem.type == mod.ItemType("HamonTequila"))
            {
                projectile.penetrate = 3;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}
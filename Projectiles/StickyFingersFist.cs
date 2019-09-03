using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class StickyFingersFist : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 6;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.LocalPlayer;
            if (player.HeldItem.type == mod.ItemType("StickyFingersT1"))
            {
                target.AddBuff(mod.BuffType("Zipped"), 180);
            }
            if (player.HeldItem.type == mod.ItemType("StickyFingersT2"))
            {
                target.AddBuff(mod.BuffType("Zipped"), 240);
            }
            if (player.HeldItem.type == mod.ItemType("StickyFingersT3"))
            {
                target.AddBuff(mod.BuffType("Zipped"), 360);
            }
            if (player.HeldItem.type == mod.ItemType("StickyFingersFinal"))
            {
                target.AddBuff(mod.BuffType("Zipped"), 480);
            }
        }
    }
}
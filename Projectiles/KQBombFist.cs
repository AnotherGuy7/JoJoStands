using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class KQBombFist : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.timeLeft = 6;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.melee = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            Main.PlaySound(SoundID.Item62);
            if (Main.LocalPlayer.HeldItem.type == mod.ItemType("KillerQueenBTD"))
            {
                Items.KillerQueenBTD.taggedAnything = true;
            }
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class KQMinionFists : ModProjectile        //currently being unused
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.damage = 84;
            projectile.melee = true;
            projectile.timeLeft = 6;
            projectile.alpha = 255;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Minions.KillerQueenMinion.touchedTarget = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
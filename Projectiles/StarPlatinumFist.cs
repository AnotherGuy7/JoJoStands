using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class StarPlatinumFist : ModProjectile
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (MyPlayer.Sounds == true)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (MyPlayer.Sounds == true)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            if (MyPlayer.Sounds == true)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (MyPlayer.Sounds == true)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            base.AI();
        }
    }
}
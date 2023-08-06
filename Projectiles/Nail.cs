using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Nail : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/GeneralBullet"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 5 * 60;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }
    }
}
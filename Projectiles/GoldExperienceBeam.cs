using JoJoStands.Projectiles.Minions;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Projectiles
{
    public class GoldExperienceBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GEScorpion>(), 1, 0f, Main.myPlayer);
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }
    }
}
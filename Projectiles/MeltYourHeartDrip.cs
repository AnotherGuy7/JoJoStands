using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;

namespace JoJoStands.Projectiles
{
    public class MeltYourHeartDrip : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.maxPenetrate = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            target.AddBuff(BuffID.Confused, 120);
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }
    }
}
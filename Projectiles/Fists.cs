using Microsoft.Xna.Framework;
using System.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Fists : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/Fists"; }
        }

        public override void SetDefaults()      //0 = SP; 1 = TW; 2 = GE; 3 = GER; 4 = SF's; 5 = KQ (Stand); 6 = KC; 7 = TH; 8 = GD;
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 4;
            projectile.alpha = 255;     //completely transparent
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (projectile.ai[0] == 3f && mPlayer.BackToZero)
            {
                target.GetGlobalNPC<NPCs.JoJoGlobalNPC>().affectedbyBtz = true;
                target.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
            }
            if (projectile.ai[0] == 4f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 180);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 240);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 360);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 480);
                }
            }
            if (projectile.ai[0] == 5f)
            {
                if (projectile.ai[1] == 1f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandT1.savedTarget = target;
                }
                if (projectile.ai[1] == 2f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandT2.savedTarget = target;
                }
                if (projectile.ai[1] == 3f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandT3.savedTarget = target;
                }
                if (projectile.ai[1] == 4f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandFinal.savedTarget = target;
                }
            }
            if (projectile.ai[0] == 9f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 300);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 360);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 420);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 480);
                }
            }
            if (projectile.ai[0] == 8f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("Old"), 540);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("Old"), 660);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("Old"), 780);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("Old"), 900);
                }
            }

            if (mPlayer.destroyAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 93)
                {
                    target.AddBuff(BuffID.OnFire, 60 * 3);
                }
            }
            if (mPlayer.greaterDestroyEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(BuffID.CursedInferno, 60 * 10);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                {
                    target.AddBuff(mod.BuffType("Infected"), 10 * 60);
                }
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (projectile.ai[0] == 3f && mPlayer.BackToZero)
            {
                target.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
            }
            if (projectile.ai[0] == 4f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 60);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 120);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 180);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 240);
                }
            }
            if (projectile.ai[0] == 8f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("Old"), 120);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("Old"), 180);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("Old"), 240);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("Old"), 300);
                }
            }
            if (projectile.ai[0] == 9f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 60);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 120);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 180);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("MissingOrgans"), 240);
                }
            }

            if (mPlayer.destroyAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 93)
                {
                    target.AddBuff(BuffID.OnFire, 3 * 60);
                }
            }
            if (mPlayer.greaterDestroyEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(BuffID.CursedInferno, 10 * 60);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                {
                    target.AddBuff(mod.BuffType("Infected"), 10 * 60);
                }
            }
        }

        private bool playedSound = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!playedSound)
            {
                Main.PlaySound(SoundID.Item1);
                playedSound = true;
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active && projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (otherProj.type == projectile.type && projectile.owner != otherProj.owner && player.team != Main.player[otherProj.owner].team)
                        {
                            Dust.NewDust(otherProj.position + otherProj.velocity, projectile.width, projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                        }
                        if (otherProj.type == mod.ProjectileType("Knife") && projectile.owner != otherProj.owner && player.team != Main.player[otherProj.owner].team)
                        {
                            otherProj.owner = projectile.owner;
                            otherProj.velocity = -otherProj.velocity * 0.4f;
                            Dust.NewDust(otherProj.position + otherProj.velocity, projectile.width, projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                        }
                    }
                }
            }
        }
    }
}
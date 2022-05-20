using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Fists : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/Fists"; }
        }

        public override void SetDefaults()      //0 = SP; 1 = TW; 2 = GE; 3 = GER; 4 = SF's; 5 = KQ (Stand); 6 = KC; 7 = TH; 8 = GD; 9 = WS; 10 = SC; 11 = Cream
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 4;
            Projectile.alpha = 255;     //completely transparent
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
            if (JoJoStands.SoundsLoaded)
                mPlayer.standHitTime += 2;

            if (Projectile.ai[0] == 2f)
            {
                if (Projectile.ai[1] == 3f)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                }
                if (Projectile.ai[1] == 4f)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                }
            }
            if (Projectile.ai[0] == 3f)
            {
                target.AddBuff(ModContent.BuffType<LifePunch>(), 8 * 60);
                if (mPlayer.backToZeroActive)
                {
                    target.GetGlobalNPC<JoJoGlobalNPC>().affectedbyBtz = true;
                    target.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                }
            }
            if (Projectile.ai[0] == 4f)
            {
                target.AddBuff(ModContent.BuffType<Zipped>(), (2 * (int)Projectile.ai[1]) * 60);
            }
            if (Projectile.ai[0] == 5f)
            {
                if (Projectile.ai[1] == 1f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandT1.savedTarget = target;
                }
                if (Projectile.ai[1] == 2f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandT2.savedTarget = target;
                }
                if (Projectile.ai[1] == 3f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandT3.savedTarget = target;
                }
                if (Projectile.ai[1] == 4f)
                {
                    PlayerStands.KillerQueen.KillerQueenStandFinal.savedTarget = target;
                }
            }
            if (Projectile.ai[0] == 6f)
            {
                JoJoGlobalNPC jojoNPC = target.GetGlobalNPC<JoJoGlobalNPC>();
                damage = (int)(damage * jojoNPC.kingCrimsonDonutMultiplier);
                jojoNPC.kingCrimsonDonutMultiplier += 0.06f;

                if (player.HasBuff(ModContent.BuffType<PowerfulStrike>()))
                {
                    damage *= 6;
                    knockback *= 3f;
                    jojoNPC.kingCrimsonDonutMultiplier += 0.24f;
                    player.ClearBuff(ModContent.BuffType<PowerfulStrike>());
                }
            }
            if (Projectile.ai[0] == 7f)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (4 + (int)Projectile.ai[1]) * 60);
            }
            if (Projectile.ai[0] == 8f)
            {
                target.AddBuff(ModContent.BuffType<Aging>(), (7 + ((int)Projectile.ai[1] * 2)) * 60);
            }
            if (Projectile.ai[0] == 9f)
            {
                if (Main.rand.NextFloat(0, 101) >= 94)
                    target.AddBuff(BuffID.Confused, (2 + (int)Projectile.ai[1]) * 60);
            }
            if (Projectile.ai[0] == 10f)
            {
                if (Main.rand.NextFloat(0, 101) >= 75)
                {
                    target.AddBuff(BuffID.Bleeding, (5 * (int)Projectile.ai[1]) * 60);
                    player.GetArmorPenetration(DamageClass.Generic) += 5 * (int)Projectile.ai[1];
                }
            }
            if (Projectile.ai[0] == 11f)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 120 * mPlayer.creamTier);
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
                    target.AddBuff(BuffID.CursedInferno, 6 * 60);
                }
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 9 * 60);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                }
            }

            if (!target.boss)
                target.velocity.X *= 0.2f;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            /*if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)     //Unlike in ModifyHitNPC, this one doesn't actually change if it's a crit or not, just detects
            {
                crit = true;
            }*/
            if (Projectile.ai[0] == 2f)
            {
                if (Projectile.ai[1] == 3f)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                }
                if (Projectile.ai[1] == 4f)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                }
            }
            if (Projectile.ai[0] == 3f)
            {
                target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                if (mPlayer.backToZeroActive)
                {
                    target.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                }
            }
            if (Projectile.ai[0] == 4f)
            {
                target.AddBuff(ModContent.BuffType<Zipped>(), (int)Projectile.ai[1] * 60);
            }
            if (Projectile.ai[0] == 7f)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (int)Projectile.ai[1] * 60);
            }
            if (Projectile.ai[0] == 8f)
            {
                target.AddBuff(ModContent.BuffType<Aging>(), (1 + (int)Projectile.ai[1]) * 60);
            }
            if (Projectile.ai[0] == 9f)
            {
                if (Main.rand.NextFloat(0, 101) >= 94)
                {
                    target.AddBuff(BuffID.Confused, (2 + (int)Projectile.ai[1]) * 60);
                }
            }
            if (Projectile.ai[0] == 11f)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 60 * mPlayer.creamTier);
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
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                }
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                {
                    target.AddBuff(ModContent.BuffType<Infected>(), 9 * 60);
                }
            }
        }

        private bool playedSound = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!playedSound)
            {
                SoundEngine.PlaySound(SoundID.Item1);
                playedSound = true;
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (otherProj.type == Projectile.type && Projectile.owner != otherProj.owner && player.team != Main.player[otherProj.owner].team)
                        {
                            int dust = Dust.NewDust(otherProj.position + otherProj.velocity, Projectile.width, Projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            Main.dust[dust].noGravity = false;
                            if (MyPlayer.Sounds && Main.netMode != NetmodeID.Server)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/GameSounds/Punch_land").WithVolume(.3f));
                            }
                        }
                        if (otherProj.type == ModContent.ProjectileType<KnifeProjectile>() && Projectile.owner != otherProj.owner && player.team != Main.player[otherProj.owner].team)
                        {
                            otherProj.owner = Projectile.owner;
                            otherProj.velocity = -otherProj.velocity * 0.4f;
                            int dust = Dust.NewDust(otherProj.position + otherProj.velocity, Projectile.width, Projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            Main.dust[dust].noGravity = false;
                            if (MyPlayer.Sounds && Main.netMode != NetmodeID.Server)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/GameSounds/Punch_land").WithVolume(.3f));
                            }
                        }
                    }
                }
            }
        }
    }
}
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.NPCs;
using JoJoStands.Projectiles.PlayerStands.KillerQueen;
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

        public const byte StarPlatinum = 0;
        public const byte TheWorld = 1;
        public const byte GoldExperience = 2;
        public const byte GoldExperienceRequiem = 3;
        public const int StickyFingers = 4;
        public const byte KillerQueen = 5;
        public const byte KingCrimson = 6;
        public const byte TheHand = 7;
        public const byte GratefulDead = 8;
        public const byte Whitesnake = 9;
        public const byte SilverChariot = 10;
        public const byte Cream = 11;

        public override void SetDefaults()
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
            int standType = (int)Projectile.ai[0];
            int standTier = (int)Projectile.ai[1];
            if (Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
            if (JoJoStands.SoundsLoaded)
                mPlayer.standHitTime += 2;

            if (standType == GoldExperience)
            {
                if (standTier == 3)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                }
                if (standTier == 4)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                }
            }

            if (standType == GoldExperienceRequiem)
            {
                target.AddBuff(ModContent.BuffType<LifePunch>(), 8 * 60);
                if (mPlayer.backToZeroActive)
                {
                    target.GetGlobalNPC<JoJoGlobalNPC>().affectedbyBtz = true;
                    target.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                }
            }

            if (standType == StickyFingers)
            {
                target.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = player.whoAmI;
                target.AddBuff(ModContent.BuffType<Zipped>(), (2 * (int)standTier) * 60);
            }

            if (standType == KillerQueen)
            {
                if (standTier == 1)
                {
                    KillerQueenStandT1.savedTarget = target;
                }
                if (standTier == 2)
                {
                    KillerQueenStandT2.savedTarget = target;
                }
                if (standTier == 3)
                {
                    KillerQueenStandT3.savedTarget = target;
                }
                if (standTier == 4)
                {
                    KillerQueenStandFinal.savedTarget = target;
                }
            }

            if (standType == KingCrimson)
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

            if (standType == TheHand)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (4 + (int)standTier) * 60);
            }

            if (standType == GratefulDead)
            {
                target.AddBuff(ModContent.BuffType<Aging>(), (7 + ((int)standTier * 2)) * 60);
            }

            if (standType == Whitesnake)
            {
                if (Main.rand.NextFloat(0, 101) >= 94)
                    target.AddBuff(BuffID.Confused, (2 + (int)standTier) * 60);
            }

            if (standType == SilverChariot)
            {
                if (Main.rand.NextFloat(0, 101) >= 75)
                {
                    target.AddBuff(BuffID.Bleeding, (5 * (int)standTier) * 60);
                    player.GetArmorPenetration(DamageClass.Generic) += 5 * (int)standTier;
                }
            }

            if (standType == Cream)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 120 * mPlayer.creamTier);
            }

            if (mPlayer.destroyAmuletEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 7)
                    target.AddBuff(BuffID.OnFire, 3 * 60);
            }
            if (mPlayer.greaterDestroyEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 20)
                    target.AddBuff(BuffID.CursedInferno, 6 * 60);
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 20)
                    target.AddBuff(ModContent.BuffType<Infected>(), 9 * 60);
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 40)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
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
            int standType = (int)Projectile.ai[0];
            int standTier = (int)Projectile.ai[1];

            if (standType == GoldExperience)
            {
                if (standTier == 3f)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                }
                if (standTier == 4f)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                }
            }

            if (standType == GoldExperienceRequiem)
            {
                target.AddBuff(ModContent.BuffType<LifePunch>(), 8 * 60);
                if (mPlayer.backToZeroActive)
                    target.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
            }

            if (standType == StickyFingers)
            {
                target.AddBuff(ModContent.BuffType<Zipped>(), (int)standTier * 60);
            }

            if (standType == TheHand)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (int)standTier * 60);
            }

            if (standType == GratefulDead)
            {
                target.AddBuff(ModContent.BuffType<Aging>(), (1 + (int)standTier) * 60);
            }

            if (standType == Whitesnake)
            {
                if (Main.rand.NextFloat(0, 101) >= 94)
                {
                    target.AddBuff(BuffID.Confused, (2 + (int)standTier) * 60);
                }
            }

            if (standType == Cream)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 60 * mPlayer.creamTier);
            }

            if (mPlayer.destroyAmuletEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 7)
                    target.AddBuff(BuffID.OnFire, 4 * 60);
            }
            if (mPlayer.greaterDestroyEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 20)
                    target.AddBuff(BuffID.CursedInferno, 6 * 60);
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 20)
                    target.AddBuff(ModContent.BuffType<Infected>(), 8 * 60);
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(1, 100 + 1) <= 40)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
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

            if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active && Projectile.owner != otherProj.owner && player.InOpposingTeam(Main.player[otherProj.owner]) && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (otherProj.type == Projectile.type)
                        {
                            int dust = Dust.NewDust(otherProj.position + otherProj.velocity, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f);
                            Main.dust[dust].noGravity = true;
                            if (MyPlayer.Sounds && Main.netMode != NetmodeID.Server)
                            {
                                SoundStyle punchSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land");
                                punchSound.Volume = 0.6f;
                                punchSound.Pitch = 0f;
                                punchSound.PitchVariance = 0.2f;
                                SoundEngine.PlaySound(punchSound, Projectile.Center);
                            }
                        }
                        else if (otherProj.type == ModContent.ProjectileType<KnifeProjectile>())
                        {
                            otherProj.owner = Projectile.owner;
                            otherProj.velocity = Projectile.velocity * 0.8f;
                            SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
                            if (MyPlayer.Sounds && Main.netMode != NetmodeID.Server)
                            {
                                SoundStyle punchSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land");
                                punchSound.Volume = 0.5f;
                                punchSound.Pitch = 0f;
                                punchSound.PitchVariance = 0.2f;
                                SoundEngine.PlaySound(punchSound, Projectile.Center);
                            }
                        }
                    }
                }
            }
        }
    }
}
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.NPCs;
using JoJoStands.Projectiles.PlayerStands.KillerQueen;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

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
        public const byte CrazyDiamond = 12;
        public const byte TowerOfGray = 13; //"fist"

        private bool onlyOnce = false;
        private int standType = 0;
        private int standTier = 0;

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

            if (standType == TowerOfGray)
            {
                if (mPlayer.towerOfGrayDamageMult != 1f)
                    target.GetGlobalNPC<JoJoGlobalNPC>().towerOfGrayImmunityFrames = 30;
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

            if (standType == CrazyDiamond && player.HasBuff(ModContent.BuffType<BlindRage>()) && !target.HasBuff(ModContent.BuffType<YoAngelo>()))
            {
                target.AddBuff(ModContent.BuffType<Restoration>(), 60);
                target.GetGlobalNPC<JoJoGlobalNPC>().CDstonePunch += 1;
            }

            if (standType == Cream)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (int)standTier * 60);
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

        /*public override void OnHitPvp(Player target, int damage, bool crit)       //this unfortunately does not work, so i left a temporary "solution" to the problem in the AI until the Boss fixes the problem (c) Proos <3
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)     //Unlike in ModifyHitNPC, this one doesn't actually change if it's a crit or not, just detects
            {
                crit = true;
            }
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
        }*/

        private bool playedSound = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            standType = (int)Projectile.ai[0];
            standTier = (int)Projectile.ai[1];
            if (!playedSound && mPlayer.towerOfGrayTier == 0)
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
            for (int p = 0; p < Main.maxPlayers; p++)      //temporary "solution" (ñ) Proos <3
            {
                Player otherPlayer = Main.player[p];
                if (otherPlayer.active && otherPlayer.whoAmI != player.whoAmI && otherPlayer.hostile && player.hostile && player.InOpposingTeam(Main.player[otherPlayer.whoAmI]))
                {
                    if ((Projectile.Hitbox.Intersects(otherPlayer.Hitbox)) && !onlyOnce)
                    {
                        if (standType == GoldExperience)
                        {
                            if (standTier == 3f)
                            {
                                otherPlayer.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                            }
                            if (standTier == 4f)
                            {
                                otherPlayer.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                            }
                        }

                        if (standType == GoldExperienceRequiem)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<LifePunch>(), 8 * 60);
                            if (mPlayer.backToZeroActive)
                            {
                                otherPlayer.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                            }
                        }

                        if (standType == StickyFingers)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<Zipped>(), (int)standTier * 60);
                        }

                        if (standType == TheHand)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<MissingOrgans>(), (int)standTier * 60);
                        }

                        if (standType == GratefulDead)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<Aging>(), (1 + (int)standTier) * 60);
                        }

                        if (standType == Whitesnake)
                        {
                            if (Main.rand.NextFloat(0, 101) >= 94)
                            {
                                otherPlayer.AddBuff(BuffID.Confused, (2 + (int)standTier) * 60);
                            }
                        }

                        if (standType == CrazyDiamond && player.HasBuff(ModContent.BuffType<BlindRage>()) && !otherPlayer.HasBuff(ModContent.BuffType<YoAngelo>()))
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<Restoration>(), 60);
                            otherPlayer.GetModPlayer<MyPlayer>().crazyDiamondStonePunch += 1;
                        }

                        if (standType == Cream)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<MissingOrgans>(), (int)standTier * 60);
                        }

                        if (mPlayer.destroyAmuletEquipped && !mPlayer.crazyDiamondRestorationMode)
                        {
                            if (Main.rand.NextFloat(1, 100 + 1) <= 7)
                            {
                                otherPlayer.AddBuff(BuffID.OnFire, 4 * 60);
                            }
                        }
                        if (mPlayer.greaterDestroyEquipped && !mPlayer.crazyDiamondRestorationMode)
                        {
                            if (Main.rand.NextFloat(1, 100 + 1) <= 20)
                            {
                                otherPlayer.AddBuff(BuffID.CursedInferno, 6 * 60);
                            }
                        }
                        if (mPlayer.awakenedAmuletEquipped && !mPlayer.crazyDiamondRestorationMode)
                        {
                            if (Main.rand.NextFloat(1, 100 + 1) <= 20)
                            {
                                otherPlayer.AddBuff(ModContent.BuffType<Infected>(), 8 * 60);
                            }
                        }
                        if (mPlayer.crackedPearlEquipped && !mPlayer.crazyDiamondRestorationMode)
                        {
                            if (Main.rand.NextFloat(1, 100 + 1) <= 40)
                            {
                                otherPlayer.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                            }
                        }
                        onlyOnce = true;
                    }
                }
            }
            if (mPlayer.crazyDiamondRestorationMode && !mPlayer.standAutoMode)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
                if (mPlayer.standTier > 1)
                {
                    int detectLeftX = (int)(Projectile.position.X / 16f) - 1;
                    int detectRightX = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 1;
                    int detectUpY = (int)(Projectile.position.Y / 16f) - 1;
                    int detectDownY = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 1;

                    if (detectLeftX < 0)
                        detectLeftX = 0;
                    if (detectRightX > Main.maxTilesX)
                        detectRightX = Main.maxTilesX;

                    if (detectUpY < 0)
                        detectUpY = 0;
                    if (detectDownY > Main.maxTilesY)
                        detectDownY = Main.maxTilesY;
                    for (int detectedTileX = detectLeftX; detectedTileX < detectRightX; detectedTileX++)
                    {
                        for (int detectedTileY = detectUpY; detectedTileY < detectDownY; detectedTileY++)
                        {
                            Vector2 tile = new Vector2(detectedTileX, detectedTileY);
                            Tile tile2 = Main.tile[detectedTileX, detectedTileY];
                            var checkTile = new MyPlayer.ExtraTile(tile2.TileType, new Vector2(detectedTileX, detectedTileY), tile2.Slope, tile2.IsHalfBlock, tile2.TileFrameY, tile2.TileFrameX, tile2.TileFrameNumber);
                            if (tile2.TileType != TileID.LihzahrdBrick && tile2.TileType != TileID.LihzahrdAltar && tile2.HasTile && Main.tileSolid[tile2.TileType])
                            {
                                if (!tile2.HasActuator || tile2.HasActuator && !tile2.IsActuated)
                                {
                                    if (mPlayer.ExtraTileCheck.Count < 100 * standTier)
                                    {
                                        mPlayer.ExtraTileCheck.Add(checkTile);
                                        mPlayer.ExtraTileCheck.ForEach(mPlayer.Destroy);
                                    }
                                    if (mPlayer.ExtraTileCheck.Count == 100 * standTier && mPlayer.crazyDiamondMessageCooldown == 0)
                                    {
                                        Main.NewText("Stop destroyng public property! Now restore it!");
                                        mPlayer.crazyDiamondMessageCooldown += 180;
                                    }
                                }
                            }
                            if (mPlayer.ExtraTileCheck.Count < 100 * standTier && mPlayer.crazyDiamondMessageCooldown == 0 && tile2.TileType == TileID.LihzahrdBrick)
                            {
                                Main.NewText("Tile is Unbreakable");
                                mPlayer.crazyDiamondMessageCooldown += 180;
                            }
                        }
                    }
                }
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    int heal = (int)(Projectile.damage * 0.25f);
                    int lifeLeft = npc.lifeMax - npc.life;
                    if (npc.active && !npc.hide)
                    {
                        if (Projectile.Hitbox.Intersects(npc.Hitbox) && !onlyOnce)
                        {
                            SoundEngine.PlaySound(npc.HitSound, npc.Center);
                            if (lifeLeft > 0 && !npc.HasBuff(ModContent.BuffType<MissingOrgans>()))
                            {
                                heal = (int)Main.rand.NextFloat((int)(heal * 0.85f), (int)(heal * 1.15f));
                                if (heal > lifeLeft)
                                    heal = lifeLeft;

                                npc.GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondHeal = heal;

                                if (!npc.townNPC && Main.rand.NextFloat(1, 100) <= 5)
                                    npc.lifeMax = npc.life;
                            }
                            npc.AddBuff(ModContent.BuffType<Restoration>(), 60);
                            onlyOnce = true;
                        }
                    }
                }
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    int heal = (int)(Projectile.damage * 0.25f);
                    int lifeLeft = otherPlayer.statLifeMax - otherPlayer.statLife;
                    if (otherPlayer.active && otherPlayer.whoAmI != player.whoAmI)
                    {
                        if ((Projectile.Hitbox.Intersects(otherPlayer.Hitbox)) && !onlyOnce)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit1, otherPlayer.Center);
                            if (lifeLeft > 0 && !otherPlayer.HasBuff(ModContent.BuffType<MissingOrgans>()))
                            {
                                heal = (int)Main.rand.NextFloat((int)(heal * 0.85f), (int)(heal * 1.15f));
                                if (heal > lifeLeft)
                                    heal = lifeLeft;
                                otherPlayer.Heal(heal);
                                if (Main.rand.NextFloat(1, 100) <= 5)
                                {
                                    if (otherPlayer.HasBuff(BuffID.Lifeforce))
                                        otherPlayer.ClearBuff(BuffID.Lifeforce);
                                    if (!otherPlayer.HasBuff(BuffID.Lifeforce) && otherPlayer.statLifeMax > 100)
                                        otherPlayer.AddBuff(ModContent.BuffType<ImproperRestoration>(), 2);
                                }
                            }
                            otherPlayer.AddBuff(ModContent.BuffType<Restoration>(), 60);
                            onlyOnce = true;
                        }
                    }
                }
            }
            if (player.HasBuff(ModContent.BuffType<BlindRage>()))
            {
                Projectile.ArmorPenetration = 100;
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 1);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
            }
            if (standType == TowerOfGray)
                Projectile.ArmorPenetration = 10 * mPlayer.towerOfGrayTier;
        }
        public override bool? CanHitNPC(NPC target)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.crazyDiamondRestorationMode || target.GetGlobalNPC<JoJoGlobalNPC>().towerOfGrayImmunityFrames != 0)
                return false;
            else
                return null;
        }
        public override bool CanHitPvp(Player target)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.crazyDiamondRestorationMode)
                return false;
            else
                return true;
        }
    }
}
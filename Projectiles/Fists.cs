using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.DataStructures;
using JoJoStands.Networking;
using JoJoStands.NPCs;
using JoJoStands.Projectiles.PlayerStands.KillerQueen;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Fists : ModProjectile
    {
        public override string Texture => Mod.Name + "/Extras/EmptyTexture";

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
        public const byte TowerOfGray = 13;     //"fist"
        public const byte SoftAndWet = 14;
        public const byte Echoes = 15;

        private bool onlyOnce = false;
        private bool playedSound = false;
        private int standType = 0;
        private int standTier = 0;
        public int extraInfo1 = 0;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 4;
            Projectile.alpha = 255;     //completely transparent
            Projectile.netImportant = true;
        }

        public readonly SoundStyle PunchLandSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land")
        {
            Volume = 0.6f,
            Pitch = 0f,
            PitchVariance = 0.2f
        };

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            standType = mPlayer.standFistsType;
            standTier = mPlayer.standTier;
            if (!playedSound && mPlayer.standName != "TowerOfGray")
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                playedSound = true;
            }

            if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
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
                            if (JoJoStands.Sounds && Main.netMode != NetmodeID.Server)
                                SoundEngine.PlaySound(PunchLandSound, Projectile.Center);
                        }
                        else if (otherProj.type == ModContent.ProjectileType<KnifeProjectile>())
                        {
                            otherProj.owner = Projectile.owner;
                            otherProj.velocity = Projectile.velocity * 0.8f;
                            SoundEngine.PlaySound(SoundID.Tink, Projectile.Center);
                            if (JoJoStands.Sounds && Main.netMode != NetmodeID.Server)
                                SoundEngine.PlaySound(PunchLandSound.WithVolumeScale(0.5f), Projectile.Center);
                        }
                    }
                }
            }
            if (mPlayer.crazyDiamondRestorationMode && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (mPlayer.standTier > 1 && mPlayer.crazyDiamondTileDestruction >= 60)
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
                            Tile targetTile = Main.tile[detectedTileX, detectedTileY];
                            DestroyedTileData checkTile = new DestroyedTileData(targetTile.TileType, new Vector2(detectedTileX, detectedTileY), targetTile.Slope, targetTile.IsHalfBlock, targetTile.TileFrameY, targetTile.TileFrameX, targetTile.TileFrameNumber, targetTile.TileColor, targetTile.WallColor);
                            if (targetTile.TileType != TileID.LihzahrdBrick && targetTile.TileType != TileID.LihzahrdAltar && targetTile.HasTile && Main.tileSolid[targetTile.TileType] && !Main.tileSand[targetTile.TileType] && !Main.tileSolidTop[targetTile.TileType])
                            {
                                if (!targetTile.HasActuator || targetTile.HasActuator && !targetTile.IsActuated)
                                {
                                    if (mPlayer.crazyDiamondDestroyedTileData.Count < 100 * standTier)
                                    {
                                        mPlayer.crazyDiamondDestroyedTileData.Add(checkTile);
                                        mPlayer.crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Destroy);
                                        for (int i = 0; i < 15; i++)
                                        {
                                            float circlePos = i;
                                            Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * 8f);
                                            Vector2 velocity = spawnPos - Projectile.Center;
                                            velocity.Normalize();
                                            Dust dustIndex = Dust.NewDustPerfect(spawnPos, DustID.IchorTorch, velocity * 0.8f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                                            dustIndex.noGravity = true;
                                        }
                                    }
                                    else
                                    {
                                        if (mPlayer.crazyDiamondMessageCooldown <= 0)
                                        {
                                            Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.CrazyDiamondTileWarning").Value);
                                            mPlayer.crazyDiamondMessageCooldown += 180;
                                        }
                                    }
                                }
                            }
                            if (mPlayer.crazyDiamondDestroyedTileData.Count < 100 * standTier && mPlayer.crazyDiamondMessageCooldown <= 0 && targetTile.TileType == TileID.LihzahrdBrick)
                            {
                                Main.NewText("Tile is Unbreakable.");
                                mPlayer.crazyDiamondMessageCooldown += 180;
                            }
                        }
                    }
                }
                /*for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    int heal = (int)(Projectile.damage * 0.2f);
                    int lifeLeft = npc.lifeMax - npc.life;
                    if (npc.active && !npc.hide)
                    {
                        if (Projectile.Hitbox.Intersects(npc.Hitbox) && !onlyOnce)
                        {
                            SoundEngine.PlaySound(npc.HitSound, npc.Center);
                            if (lifeLeft > 0 && !npc.HasBuff(ModContent.BuffType<MissingOrgans>()))
                            {
                                if (heal > 20)
                                    heal = 20;
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
                }*/
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    int heal = (int)(Projectile.damage * 0.2f);
                    int lifeLeft = otherPlayer.statLifeMax - otherPlayer.statLife;
                    if (otherPlayer.active)
                    {
                        if (otherPlayer.whoAmI != player.whoAmI && !mPlayer.overHeaven || mPlayer.overHeaven)
                        {
                            if (Projectile.Hitbox.Intersects(otherPlayer.Hitbox) && !onlyOnce)
                            {
                                SoundEngine.PlaySound(SoundID.NPCHit1, otherPlayer.Center);
                                if (lifeLeft > 0 && !otherPlayer.HasBuff(ModContent.BuffType<MissingOrgans>()))
                                {
                                    if (heal > 20)
                                        heal = 20;
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
            }
            if (standType == TowerOfGray)
                Projectile.Hitbox = new Rectangle(Projectile.Hitbox.X, Projectile.Hitbox.Y, (int)(Projectile.Hitbox.Width * 1.1f), (int)(Projectile.Hitbox.Height * 1.1f));
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            bool crit = Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts;
            if (crit)
                modifiers.SetCrit();
            if (JoJoStands.SoundsLoaded)
                mPlayer.standHitTime += 2;

            if (standType == GoldExperience)
            {
                if (standTier == 3)
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                else if (standTier == 4)
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
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
                if (Main.myPlayer == Projectile.owner)
                {
                    if (standTier == 1)
                        (Main.projectile[extraInfo1].ModProjectile as KillerQueenStandT1).autoModeTaggedTargetIndex = target.whoAmI;
                    else if (standTier == 2)
                        (Main.projectile[extraInfo1].ModProjectile as KillerQueenStandT2).autoModeTaggedTargetIndex = target.whoAmI;
                    else if (standTier == 3)
                        (Main.projectile[extraInfo1].ModProjectile as KillerQueenStandT3).autoModeTaggedTargetIndex = target.whoAmI;
                    else if (standTier == 4)
                        (Main.projectile[extraInfo1].ModProjectile as KillerQueenStandFinal).autoModeTaggedTargetIndex = target.whoAmI;
                }
            }

            if (standType == KingCrimson)
            {
                JoJoGlobalNPC jojoNPC = target.GetGlobalNPC<JoJoGlobalNPC>();
                modifiers.FinalDamage *= jojoNPC.kingCrimsonDonutMultiplier;
                jojoNPC.kingCrimsonDonutMultiplier += 0.06f;

                if (player.HasBuff(ModContent.BuffType<PowerfulStrike>()))
                {
                    modifiers.FinalDamage *= 6;
                    modifiers.Knockback *= 3f;
                    jojoNPC.kingCrimsonDonutMultiplier += 0.24f;
                    player.ClearBuff(ModContent.BuffType<PowerfulStrike>());
                }
            }

            if (standType == TheHand)
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (4 + (int)standTier) * 60);

            if (standType == TowerOfGray)
            {
                if (mPlayer.towerOfGrayDamageMult != 1f)
                {
                    target.GetGlobalNPC<JoJoGlobalNPC>().towerOfGrayImmunityFrames = 30;
                    SyncCall.SyncStandEffectInfo(player.whoAmI, target.whoAmI, 13);
                }
            }

            if (standType == GratefulDead)
            {
                target.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = player.whoAmI;
                SyncCall.SyncStandEffectInfo(player.whoAmI, target.whoAmI, 8, player.whoAmI);
                target.AddBuff(ModContent.BuffType<Aging>(), (7 + ((int)standTier * 2)) * 60);
            }

            if (standType == Whitesnake)
            {
                if (Main.rand.Next(1, 100 + 1) >= 94)
                    target.AddBuff(BuffID.Confused, (2 + (int)standTier) * 60);
            }

            if (standType == SilverChariot)
            {
                if (Main.rand.Next(1, 100 + 1) >= 75)
                {
                    target.AddBuff(BuffID.Bleeding, (5 * (int)standTier) * 60);
                    Projectile.ArmorPenetration += 5 * (int)standTier;
                }
            }

            if (standType == CrazyDiamond && mPlayer.crazyDiamondRestorationMode && !target.HasBuff(ModContent.BuffType<ImproperRestoration>()))
            {
                target.AddBuff(ModContent.BuffType<Restoration>(), 60);
                target.GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondPunchCount += 1;
                target.GetGlobalNPC<JoJoGlobalNPC>().taggedByCrazyDiamondRestoration = true;
                target.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = player.whoAmI;
                SyncCall.SyncStandEffectInfo(player.whoAmI, target.whoAmI, 12);

                Vector2 randomOffset = new Vector2(Main.rand.Next(-12, 12 + 1), Main.rand.Next(-12, 12 + 1));
                for (int i = 0; i < 15; i++)
                {
                    float circlePos = i;
                    Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * 8f) + randomOffset;
                    Vector2 velocity = spawnPos - Projectile.Center;
                    velocity.Normalize();
                    Dust dustIndex = Dust.NewDustPerfect(spawnPos, DustID.IchorTorch, velocity * 0.8f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                    dustIndex.noGravity = true;
                }
            }

            if (standType == Cream)
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 2*(int)standTier * 60);

            if (!target.boss && standType != TowerOfGray)
            {
                target.velocity.X *= 0.2f;
                SyncCall.SyncStandEffectInfo(player.whoAmI, target.whoAmI, 0);
            }

            if (standType == Echoes)
            {
                if (mPlayer.echoesTier == 3)
                {
                    if (target.type == NPCID.Golem || target.type == NPCID.GolemFistLeft || target.type == NPCID.GolemFistRight || target.type == NPCID.GolemHead)
                    {
                        int progressAdd = crit ? Projectile.damage * 2 : Projectile.damage;
                        mPlayer.echoesACT3EvolutionProgress += progressAdd;
                    }
                }
                else if (mPlayer.echoesTier == 2)
                {
                    if (target.type == NPCID.Retinazer || target.type == NPCID.Spazmatism)
                    {
                        int progressAdd = crit ? Projectile.damage * 2 : Projectile.damage;
                        mPlayer.echoesACT3EvolutionProgress += progressAdd;
                    }
                }

                if (player.HasBuff(ModContent.BuffType<ThreeFreezeBarrage>()) && mPlayer.currentEchoesAct == 3)
                {
                    target.GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = mPlayer.standCritChangeBoosts;
                    target.GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = mPlayer.standDamageBoosts;
                    if (target.GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer <= 15)
                        target.GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer += 30;
                    SyncCall.SyncStandEffectInfo(player.whoAmI, target.whoAmI, 15, 3, 0, 0, mPlayer.standCritChangeBoosts, mPlayer.standDamageBoosts);
                }
                if (mPlayer.currentEchoesAct == 1)
                {
                    target.GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = mPlayer.standCritChangeBoosts;
                    target.GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = mPlayer.standDamageBoosts;
                    int maxDamage = 36;
                    int soundIntensity = 2 + (2 * (mPlayer.echoesTier - 2));
                    if (mPlayer.echoesTier == 4)
                        maxDamage = 74;
                    else if (mPlayer.echoesTier == 3)
                        maxDamage = 49;
                    if (!target.boss)
                        soundIntensity *= 2;

                    target.AddBuff(ModContent.BuffType<SMACK>(), 10 * 60);
                    target.GetGlobalNPC<JoJoGlobalNPC>().echoesDebuffOwner = player.whoAmI;
                    target.GetGlobalNPC<JoJoGlobalNPC>().echoesSoundMaxIntensity = maxDamage;
                    if (target.GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensity < target.GetGlobalNPC<JoJoGlobalNPC>().echoesSoundMaxIntensity)
                        target.GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensity += soundIntensity;
                    SyncCall.SyncStandEffectInfo(player.whoAmI, target.whoAmI, 15, 1, maxDamage, soundIntensity, mPlayer.standCritChangeBoosts, mPlayer.standDamageBoosts);
                }
            }

            if (mPlayer.standFistsType != TowerOfGray && mPlayer.familyPhotoEquipped && mPlayer.familyPhotoEffectTimer < 30 && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                mPlayer.familyPhotoEffectTimer += 30;
                if (mPlayer.familyPhotoEffectTimer >= 30)
                    mPlayer.familyPhotoEffectTimer = 30;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            MyPlayer mTarget = target.GetModPlayer<MyPlayer>();
            if (!modifiers.PvP)
                return;

            if (standType == GoldExperience)
            {
                if (standTier == 3)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 4 * 60);
                    SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<LifePunch>(), 4 * 60);
                }
                if (standTier == 4)
                {
                    target.AddBuff(ModContent.BuffType<LifePunch>(), 6 * 60);
                    SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<LifePunch>(), 6 * 60);
                }
            }

            if (standType == GoldExperienceRequiem)
            {
                target.AddBuff(ModContent.BuffType<LifePunch>(), 8 * 60);
                SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<LifePunch>(), 8 * 60);
                if (mPlayer.backToZeroActive)
                {
                    target.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                    SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<AffectedByBtZ>(), 2);
                }
            }

            if (standType == StickyFingers)
            {
                target.AddBuff(ModContent.BuffType<Zipped>(), (1 + (int)standTier) * 60);
                SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<Zipped>(), (1 + (int)standTier) * 60);
            }

            if (standType == TheHand)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (1 + (int)standTier) * 60);
                SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<MissingOrgans>(), (1 + (int)standTier) * 60);
            }

            if (standType == GratefulDead)
            {
                target.AddBuff(ModContent.BuffType<Aging>(), (1 + (int)standTier) * 60);
                SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<Aging>(), (1 + (int)standTier) * 60);
            }

            if (standType == Whitesnake)
            {
                if (Main.rand.Next(1, 100 + 1) >= 94)
                {
                    target.AddBuff(BuffID.Confused, (2 + (int)standTier) * 60);
                    SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, BuffID.Confused, (2 + (int)standTier) * 60);
                }
            }

            if (standType == Cream)
            {
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), (1 + (int)standTier) * 60);
                SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<MissingOrgans>(), (1 + (int)standTier) * 60);
            }

            if (standType == Echoes)
            {
                if (player.HasBuff(ModContent.BuffType<ThreeFreezeBarrage>()) && mPlayer.currentEchoesAct == 3)
                {
                    mTarget.echoesDamageBoost = mPlayer.standDamageBoosts;
                    if (mTarget.echoesFreeze <= 15)
                        mTarget.echoesFreeze += 30;
                    SyncCall.SyncOtherPlayerExtraEffect(player.whoAmI, target.whoAmI, 1, 0, 0, mPlayer.standDamageBoosts, 0f);
                }
                if (mPlayer.currentEchoesAct == 1)
                {
                    mTarget.echoesDamageBoost = mPlayer.standDamageBoosts;
                    int maxDamage = 48;
                    int soundIntensity = 2 + (2 * (mPlayer.echoesTier - 2));
                    if (mPlayer.echoesTier == 4)
                        maxDamage = 108;
                    else if (mPlayer.echoesTier == 3)
                        maxDamage = 72;

                    target.AddBuff(ModContent.BuffType<SMACK>(), 10 * 60);
                    mTarget.echoesSoundIntensityMax = maxDamage;
                    if (mTarget.echoesSoundIntensity < mTarget.echoesSoundIntensityMax)
                        mTarget.echoesSoundIntensity += soundIntensity;
                    SyncCall.SyncOtherPlayerDebuff(player.whoAmI, target.whoAmI, ModContent.BuffType<SMACK>(), 10 * 60);
                    SyncCall.SyncOtherPlayerExtraEffect(player.whoAmI, target.whoAmI, 2, maxDamage, soundIntensity, mPlayer.standDamageBoosts, 0f);
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<JoJoGlobalNPC>().towerOfGrayImmunityFrames != 0)
                return false;
            else
                return null;
        }

        public override bool CanHitPvp(Player target)
        {
            return true;
        }
    }
}
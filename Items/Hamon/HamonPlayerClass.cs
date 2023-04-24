using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.Vampire;
using JoJoStands.Mounts;
using JoJoStands.NPCs;
using JoJoStands.Projectiles;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JoJoStands.Items.Hamon
{
    public class HamonPlayer : ModPlayer
    {
        public static bool HamonEffects = true;

        public float hamonDamageBoosts = 1f;
        public float hamonKnockbackBoosts = 1f;
        public int hamonIncreaseBonus = 0;

        public int hamonLevel = 0;
        public int amountOfHamon = 0;
        public int maxHamon = 60;
        public int hamonIncreaseCounter = 0;
        public int hamonDecreaseCounter = 0;
        public int maxHamonCounter = 0;
        public int skillPointsAvailable = 1;
        public int hamonLayerFrame = 0;
        public int hamonLayerFrameCounter = 0;
        public int defensiveHamonLayerFrame = 0;
        public int defensiveHamonLayerFrameCounter = 0;
        public int defensiveAuraDownDoublePressTimer = 0;
        public int hamonOverChargeSpecialDoublePressTimer = 0;
        public int leafGliderGenerationTimer = 0;
        public int enemyToIgnoreDamageFromIndex = -1;
        public int oreDetectionDownHeldTimer = 0;
        public float oreDetectionRingRadius = 0;
        public int muscleOverdriveHeldTimer = 0;
        public int sunTagHeldTimer = 0;
        public int sunShacklesHeldTimer = 0;
        public int hamonStage = 0;
        public int hamonAuraStandTimer = 0;

        public bool passiveRegen = false;
        public bool chargingHamon = false;
        public bool ajaStoneEquipped = false;
        public bool defensiveHamonAuraActive = false;
        public bool defensiveAuraCanPressDownAgain = false;
        public bool usingItemThatIgnoresEnemyDamage = false;
        public bool learnedAnyAbility = false;
        public bool sunShacklesActive = false;
        public bool learnedHamon = false;

        //Adjustable skills
        public const int HamonSkillsLimit = 15;

        public const int BreathingRegenSkill = 0;
        public const int WaterWalkingSKill = 1;
        public const int WeaponsHamonImbueSkill = 2;
        public const int HamonItemHealing = 3;
        public const int DefensiveHamonAura = 4;
        public const int HamonShockwave = 5;
        public const int HamonOvercharge = 6;
        public const int HamonHerbalGrowth = 7;
        public const int PassiveHamonRegenBoost = 8;
        public const int PoisonCancellation = 9;
        public const int OreDetection = 10;
        public const int SunTag = 11;
        public const int SunShackles = 12;
        public const int MuscleOverdrive = 13;
        public const int StandHamonImbue = 14;

        //public bool[] learnedHamonSkills = new bool[HamonSkillsLimit];
        public const int ExpectedAmountOfHamonSkills = 15;
        public Dictionary<int, bool> learnedHamonSkills = new Dictionary<int, bool>();
        public Dictionary<int, int> hamonAmountRequirements = new Dictionary<int, int>();
        public Dictionary<int, int> hamonSkillLevels = new Dictionary<int, int>();


        public override void ResetEffects()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            hamonDamageBoosts = 1f;
            hamonKnockbackBoosts = 1f;
            hamonIncreaseBonus = 0;
            maxHamonCounter = 300;
            hamonStage = 0;
            if (!usingItemThatIgnoresEnemyDamage)
                enemyToIgnoreDamageFromIndex = -1;

            usingItemThatIgnoresEnemyDamage = false;
            chargingHamon = false;
            passiveRegen = true;
            ajaStoneEquipped = false;
        }

        public override void OnEnterWorld()
        {
            if (learnedHamonSkills.Count != ExpectedAmountOfHamonSkills)
                RebuildHamonAbilitiesDictionaries();

            if (learnedHamon)
                HamonBar.ShowHamonBar();
            else
            {
                if (HamonBar.visible)
                    HamonBar.HideHamonBar();
            }
        }

        public override void PreUpdate()
        {
            VampirePlayer vPlayer = Player.GetModPlayer<VampirePlayer>();
            if (NPC.downedBoss1)      //It's written this way so that by the time it gets to the bottom it would have the actual Hamon Level
            {
                hamonLevel = 1;
            }
            if (NPC.downedBoss2)      //the crimson/corruption bosses
            {
                hamonLevel = 2;
            }
            if (NPC.downedBoss3)       //skeletron
            {
                hamonLevel = 3;
            }
            if (Main.hardMode)      //wall of flesh
            {
                hamonLevel = 4;
            }
            if (NPC.downedMechBoss1)
            {
                hamonLevel = 5;
            }
            if (NPC.downedMechBoss2)
            {
                hamonLevel = 6;
            }
            if (NPC.downedMechBoss3)
            {
                hamonLevel = 7;
            }
            if (NPC.downedPlantBoss)       //plantera
            {
                hamonLevel = 8;
            }
            if (NPC.downedGolemBoss)
            {
                hamonLevel = 9;
            }
            if (NPC.downedMoonlord)     //you are an expert with hamon by moon lord
            {
                hamonLevel = 10;
            }

            switch (hamonLevel)     //done this way cause different things will be done with it
            {
                case 1:
                    maxHamon = 72;
                    break;
                case 2:
                    maxHamon = 84;
                    break;
                case 3:
                    maxHamon = 96;
                    break;
                case 4:
                    maxHamon = 108;
                    hamonIncreaseBonus += 1;
                    break;
                case 5:
                    maxHamon = 120;
                    hamonIncreaseBonus += 1;
                    break;
                case 6:
                    maxHamon = 132;
                    hamonIncreaseBonus += 1;
                    break;
                case 7:
                    maxHamon = 144;
                    hamonIncreaseBonus += 2;
                    break;
                case 8:
                    maxHamon = 156;
                    hamonIncreaseBonus += 2;
                    break;
                case 9:
                    maxHamon = 168;
                    hamonIncreaseBonus += 2;
                    break;
                case 10:
                    maxHamon = 180;
                    hamonIncreaseBonus += 3;
                    break;
            }

            if (!learnedHamon)
                return;

            if (vPlayer.zombie || vPlayer.vampire)
                return;

            if (amountOfHamon <= 0)
                hamonStage = 0;
            else if (amountOfHamon <= 60)
                hamonStage = 1;
            else if (amountOfHamon > 60 && amountOfHamon <= 120)
                hamonStage = 2;
            else if (amountOfHamon > 120)
                hamonStage = 3;

            ManageAbilities();
            if (ajaStoneEquipped)           //Hamon charging stuff
                maxHamon *= 2;
            if (Player.velocity.X == 0f)
                hamonIncreaseBonus += 1;

            if (passiveRegen)
            {
                if (learnedHamonSkills.ContainsKey(PassiveHamonRegenBoost) && learnedHamonSkills[PassiveHamonRegenBoost])
                    hamonIncreaseBonus += hamonSkillLevels[PassiveHamonRegenBoost];

                if (amountOfHamon < 60 && Player.breath == Player.breathMax)       //in general, to increase Hamon while it can still be increased, no speeding up or decreasing
                {
                    hamonIncreaseCounter += 1 + hamonIncreaseBonus;
                }
                if (hamonIncreaseCounter >= maxHamonCounter)      //the one that increases Hamon
                {
                    if (amountOfHamon < 60)
                        amountOfHamon += 1;

                    hamonIncreaseCounter = 0;
                }
            }
            if (hamonStage > 1)
            {
                hamonDecreaseCounter++;
                if (hamonDecreaseCounter > 240 / (hamonStage - 1))
                {
                    amountOfHamon -= 1;
                    hamonDecreaseCounter = 0;
                }
            }
            amountOfHamon = (int)MathHelper.Clamp(amountOfHamon, 0, maxHamon);
        }

        public override void MeleeEffects(Item Item, Rectangle hitbox)
        {
            if (Player.HasBuff(ModContent.BuffType<HamonWeaponImbueBuff>()))
            {
                Vector2 hitboxPosition = new Vector2(hitbox.X, hitbox.Y);
                Dust.NewDust(hitboxPosition, hitbox.Width, hitbox.Height, 169);
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (chargingHamon)
            {
                if (learnedHamonSkills.ContainsKey(BreathingRegenSkill) && learnedHamonSkills[BreathingRegenSkill])
                    regen *= 1.2f + (0.2f * hamonSkillLevels[BreathingRegenSkill]);
            }
        }

        public override void PreUpdateMovement()
        {
            if (learnedHamonSkills.ContainsKey(WaterWalkingSKill) && learnedHamonSkills[WaterWalkingSKill])
            {
                if (amountOfHamon > hamonAmountRequirements[WaterWalkingSKill])
                {
                    Player.waterWalk2 = true;
                }
                if (!Player.wet && Main.tile[(int)Player.position.X / 16, ((int)Player.position.Y / 16) + 3].LiquidAmount >= 50)
                {
                    if (Main.rand.Next(0, 2) == 0)
                    {
                        int dustIndex = Dust.NewDust(Player.position + new Vector2(0f, Player.height), Player.width, 2, 169, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
            }
        }

        public override void PreUpdateBuffs()
        {
            if (hamonStage == 2)
                Player.AddBuff(ModContent.BuffType<HamonChargedII>(), 2);
            else if (hamonStage == 1)
                Player.AddBuff(ModContent.BuffType<HamonChargedI>(), 2);
        }

        private void ManageAbilities()
        {
            if (!learnedAnyAbility || learnedHamonSkills.Count == 0)
                return;

            if (defensiveAuraDownDoublePressTimer > 0)
                defensiveAuraDownDoublePressTimer--;

            if (defensiveHamonAuraActive)
            {
                passiveRegen = false;
                if (amountOfHamon <= hamonAmountRequirements[DefensiveHamonAura])
                {
                    defensiveHamonAuraActive = false;
                }
                if (Player.controlDown && defensiveAuraDownDoublePressTimer <= 0 && Player.velocity == Vector2.Zero)
                {
                    defensiveHamonAuraActive = false;
                }
            }
            else
            {
                if (amountOfHamon >= hamonAmountRequirements[DefensiveHamonAura] && Player.velocity == Vector2.Zero)
                {
                    if (Player.controlDown && defensiveAuraDownDoublePressTimer <= 0)
                    {
                        defensiveAuraDownDoublePressTimer += 30;
                    }
                    if (!Player.controlDown && defensiveAuraDownDoublePressTimer > 0)
                    {
                        defensiveAuraCanPressDownAgain = true;
                    }
                    if (Player.controlDown && defensiveAuraDownDoublePressTimer > 0 && defensiveAuraCanPressDownAgain && learnedHamonSkills[DefensiveHamonAura])
                    {
                        defensiveHamonAuraActive = true;
                        defensiveAuraCanPressDownAgain = false;
                        defensiveAuraDownDoublePressTimer += 30;
                    }
                }
            }

            if (learnedHamonSkills[HamonHerbalGrowth])
            {
                if (Player.velocity.Y > 0f)
                {
                    if (Player.controlUp && amountOfHamon > 5 && Player.mount.Type == -1 && !WorldGen.SolidTile((int)(Player.Center.X / 16f), (int)(Player.Center.Y / 16f) + 2))
                    {
                        if (leafGliderGenerationTimer < 15)
                        {
                            leafGliderGenerationTimer++;
                            for (int i = 0; i < 2; i++)
                            {
                                Vector2 position = Player.Center - new Vector2(0f, Player.height);
                                Dust.NewDust(position, Player.width, Player.height / 2, 3);
                            }
                        }
                        else
                        {
                            Player.mount.SetMount(ModContent.MountType<LeafGliderMount>(), Player);
                        }
                    }
                }
                else
                {
                    leafGliderGenerationTimer = 0;
                }
                /*int targetCoordX = (int)(Player.position.X / 16f);
                int targetCoordY = (int)(Player.position.X / 16f);
                if (TileLoader.IsSapling(Main.tile[targetCoordX, targetCoordY].type))
                {
                    Dust.NewDust(Player.position, 5, 5, 169);
                    if (Main.rand.Next(1, 151) == 1)
                    {
                        if (WorldGen.GrowTree(targetCoordX, targetCoordY))
                        {
                            WorldGen.TreeGrowFXCheck(targetCoordX, targetCoordY);
                        }
                    }
                }*/
            }

            if (learnedHamonSkills[HamonOvercharge])
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

                if (specialJustPressed && Player.HeldItem.type == ItemID.None)
                {
                    if (hamonOverChargeSpecialDoublePressTimer <= 0)
                    {
                        hamonOverChargeSpecialDoublePressTimer = 30;
                    }
                    else
                    {
                        if (amountOfHamon <= hamonAmountRequirements[HamonOvercharge] && Player.statLife > (int)(Player.statLifeMax * 0.25f))
                        {
                            amountOfHamon += 30 * hamonSkillLevels[HamonOvercharge];
                            Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " overcharged themselves with Hamon."), (int)(Player.statLifeMax * 0.05), Player.direction);
                        }
                        hamonOverChargeSpecialDoublePressTimer = 0;
                    }
                }
            }

            if (learnedHamonSkills[HamonShockwave])
            {
                if (amountOfHamon > hamonAmountRequirements[HamonShockwave] && Player.controlDown && Player.velocity.Y > 0f && WorldGen.SolidTile((int)(Player.Center.X / 16f), (int)(Player.Center.Y / 16f) + 2))
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(0f, 5f), new Vector2(0.01f * Player.direction, 0f), ModContent.ProjectileType<HamonShockwaveSpike>(), 32 * hamonSkillLevels[HamonShockwave], 4f * hamonSkillLevels[HamonShockwave], Player.whoAmI, Player.direction, hamonSkillLevels[HamonShockwave]);
                    amountOfHamon -= hamonAmountRequirements[HamonShockwave];
                }
            }

            if (learnedHamonSkills[OreDetection])
            {
                if ((Player.controlDown || oreDetectionDownHeldTimer >= 120) && amountOfHamon >= hamonAmountRequirements[OreDetection])
                {
                    oreDetectionDownHeldTimer++;
                }
                else
                {
                    oreDetectionDownHeldTimer = 0;
                }

                if (oreDetectionDownHeldTimer >= 120)
                {
                    if (oreDetectionDownHeldTimer == 120)
                        amountOfHamon -= hamonAmountRequirements[OreDetection];

                    oreDetectionRingRadius += 2.6f;
                    for (int i = 0; i < 80; i++)
                    {
                        float rotation = MathHelper.ToRadians(i * (360f / 80f));
                        Vector2 pos = Player.Center + (rotation.ToRotationVector2() * oreDetectionRingRadius);
                        int dustIndex = Dust.NewDust(pos, 1, 1, 169, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                        Main.dust[dustIndex].noGravity = true;
                    }

                    int detectionRadius = 24 * hamonSkillLevels[OreDetection];
                    Vector2 startingPos = (Player.position - new Vector2(detectionRadius / 2f, detectionRadius / 2f)) / 16f;
                    for (int x = 0; x < detectionRadius; x++)
                    {
                        for (int y = 0; y < detectionRadius; y++)
                        {
                            Tile potentialOreTile = Main.tile[(int)startingPos.X + x, (int)startingPos.Y + y];
                            if (TileID.Sets.Ore[potentialOreTile.TileType])
                            {
                                for (int i = 0; i < Main.rand.Next(1, 3 + 1); i++)
                                {
                                    Vector2 pos = new Vector2(startingPos.X + x, startingPos.Y + y) * 16f;
                                    int dustIndex = Dust.NewDust(pos, 16, 16, 169, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                                    Main.dust[dustIndex].noGravity = true;
                                }
                            }
                        }
                    }

                    if (oreDetectionDownHeldTimer >= 180)
                    {
                        oreDetectionDownHeldTimer = 0;
                        oreDetectionRingRadius = 0f;
                    }
                }
            }

            if (learnedHamonSkills[SunTag])
            {
                bool secondSpecialHeld = false;
                if (!Main.dedServ)
                    secondSpecialHeld = JoJoStands.SecondSpecialHotKey.Current;

                if (secondSpecialHeld)
                    sunTagHeldTimer++;
                else
                    sunTagHeldTimer = 0;

                if (amountOfHamon >= hamonAmountRequirements[SunTag] && sunTagHeldTimer >= 180)
                {
                    float sunTagRadius = 30f * 16f;
                    for (int i = 0; i < 80; i++)
                    {
                        float rotation = MathHelper.ToRadians(i * (360f / 80f));
                        Vector2 pos = Player.Center + (rotation.ToRotationVector2() * sunTagRadius);
                        Vector2 velocity = pos - Player.Center;
                        velocity.Normalize();
                        velocity *= 9f;
                        int dustIndex = Dust.NewDust(pos, 1, 1, 169, velocity.X, velocity.Y, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                        Main.dust[dustIndex].noGravity = true;
                    }

                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.lifeMax > 5 && !npc.townNPC && !npc.immortal && Player.Distance(npc.Center) <= sunTagRadius)
                        {
                            npc.GetGlobalNPC<JoJoGlobalNPC>().sunTagged = true;
                        }
                    }
                    amountOfHamon -= hamonAmountRequirements[SunTag];
                    sunTagHeldTimer = 0;
                }
            }

            if (learnedHamonSkills[SunShackles])
            {
                bool secondSpecialHeld = false;
                if (!Main.dedServ)
                    secondSpecialHeld = JoJoStands.SecondSpecialHotKey.Current;

                if (secondSpecialHeld)
                    sunShacklesHeldTimer++;
                else
                    sunShacklesHeldTimer = 0;

                sunShacklesActive = Player.ownedProjectileCounts[ModContent.ProjectileType<SunShackle>()] > 0;

                if (amountOfHamon >= hamonAmountRequirements[SunShackles] && sunShacklesHeldTimer >= 180)
                {
                    float sunShacklesRadius = 20f * 16f;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.lifeMax > 5 && !npc.townNPC && !npc.immortal && Player.Distance(npc.Center) <= sunShacklesRadius)
                        {
                            npc.GetGlobalNPC<JoJoGlobalNPC>().sunShackled = true;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), npc.position, Vector2.Zero, ModContent.ProjectileType<SunShackle>(), 0, 0f, Player.whoAmI, npc.whoAmI, 0.3f);
                        }
                    }
                    amountOfHamon -= hamonAmountRequirements[SunShackles];
                    sunShacklesHeldTimer = 0;
                    SoundStyle item8 = SoundID.Item8;
                    item8.Pitch = 0.8f;
                    SoundEngine.PlaySound(item8, Player.Center);
                }
            }


            if (learnedHamonSkills[MuscleOverdrive])
            {
                bool leftPressed = false;
                bool rightPressed = false;
                if (Player.whoAmI == Main.myPlayer)
                {
                    leftPressed = PlayerInput.Triggers.Current.Left;        //Terraria is set to make both Player.controlLeft and Player.controlRight to false when both are pressed at the same time.
                    rightPressed = PlayerInput.Triggers.Current.Right;
                }
                if (amountOfHamon >= hamonAmountRequirements[MuscleOverdrive] && leftPressed && rightPressed)
                {
                    muscleOverdriveHeldTimer++;
                    if (muscleOverdriveHeldTimer >= 120)
                    {
                        muscleOverdriveHeldTimer = 0;
                        amountOfHamon -= hamonAmountRequirements[MuscleOverdrive];
                        Player.AddBuff(ModContent.BuffType<HamonChargedII>(), 60 * 60 * 5);
                        for (int i = 0; i < Main.rand.Next(15, 32); i++)
                        {
                            Dust.NewDust(Player.position, Player.width, Player.height, 169, SpeedY: Main.rand.NextFloat(-1.4f + -0.3f + 1f));
                        }
                    }
                }
                else
                {
                    muscleOverdriveHeldTimer = 0;
                }
            }
        }

        public void RebuildHamonAbilitiesDictionaries()
        {
            learnedHamonSkills.Clear();
            hamonSkillLevels.Clear();
            hamonAmountRequirements.Clear();

            learnedHamonSkills.Add(BreathingRegenSkill, false);
            learnedHamonSkills.Add(WaterWalkingSKill, false);
            learnedHamonSkills.Add(WeaponsHamonImbueSkill, false);
            learnedHamonSkills.Add(HamonItemHealing, false);
            learnedHamonSkills.Add(DefensiveHamonAura, false);
            learnedHamonSkills.Add(HamonShockwave, false);
            learnedHamonSkills.Add(HamonOvercharge, false);
            learnedHamonSkills.Add(HamonHerbalGrowth, false);
            learnedHamonSkills.Add(PassiveHamonRegenBoost, false);
            learnedHamonSkills.Add(PoisonCancellation, false);
            learnedHamonSkills.Add(OreDetection, false);
            learnedHamonSkills.Add(SunTag, false);
            learnedHamonSkills.Add(SunShackles, false);
            learnedHamonSkills.Add(MuscleOverdrive, false);
            learnedHamonSkills.Add(StandHamonImbue, false);

            hamonSkillLevels.Add(BreathingRegenSkill, 0);
            hamonSkillLevels.Add(WaterWalkingSKill, 0);
            hamonSkillLevels.Add(WeaponsHamonImbueSkill, 0);
            hamonSkillLevels.Add(HamonItemHealing, 0);
            hamonSkillLevels.Add(DefensiveHamonAura, 0);
            hamonSkillLevels.Add(HamonShockwave, 0);
            hamonSkillLevels.Add(HamonOvercharge, 0);
            hamonSkillLevels.Add(HamonHerbalGrowth, 0);
            hamonSkillLevels.Add(OreDetection, 0);
            hamonSkillLevels.Add(PassiveHamonRegenBoost, 0);
            hamonSkillLevels.Add(PoisonCancellation, 0);
            hamonSkillLevels.Add(SunTag, 0);
            hamonSkillLevels.Add(SunShackles, 0);
            hamonSkillLevels.Add(MuscleOverdrive, 0);
            hamonSkillLevels.Add(StandHamonImbue, 0);

            //Only skills that need hamon to be used should add to the requirements dictionary
            hamonAmountRequirements.Add(WaterWalkingSKill, 0);
            hamonAmountRequirements.Add(WeaponsHamonImbueSkill, 0);
            hamonAmountRequirements.Add(HamonItemHealing, 0);
            hamonAmountRequirements.Add(DefensiveHamonAura, 0);
            hamonAmountRequirements.Add(HamonShockwave, 0);
            hamonAmountRequirements.Add(HamonHerbalGrowth, 0);
            hamonAmountRequirements.Add(OreDetection, 0);
            hamonAmountRequirements.Add(PoisonCancellation, 0);
            hamonAmountRequirements.Add(SunTag, 0);
            hamonAmountRequirements.Add(SunShackles, 0);
            hamonAmountRequirements.Add(MuscleOverdrive, 0);
            hamonAmountRequirements.Add(StandHamonImbue, 0);

            learnedAnyAbility = false;
            if (learnedHamon)
                Main.NewText("Rebuilt Hamon Skills Dictionaries.", Color.Yellow);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (Player.HasBuff(ModContent.BuffType<HamonWeaponImbueBuff>()))
            {
                target.AddBuff(ModContent.BuffType<Sunburn>(), 120 * (Player.GetModPlayer<HamonPlayer>()).hamonSkillLevels[WeaponsHamonImbueSkill] - 1);
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            enemyToIgnoreDamageFromIndex = -1;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (defensiveHamonAuraActive)
            {
                modifiers.FinalDamage *= 0.95f);
                amountOfHamon -= 3;

                if (Main.rand.Next(0, 7) == 0)
                {
                    npc.AddBuff(ModContent.BuffType<Sunburn>(), 80 * hamonSkillLevels[DefensiveHamonAura]);
                }
            }
            if (sunShacklesActive)
            {
                if (npc.GetGlobalNPC<JoJoGlobalNPC>().sunShackled)
                {
                    modifiers.FinalDamage *= 0.85f);
                }
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (npc.whoAmI == enemyToIgnoreDamageFromIndex)
            {
                return false;
            }
            return true;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("hamonSkillKeys", learnedHamonSkills.Keys.ToList());
            tag.Add("hamonSkillValues", learnedHamonSkills.Values.ToList());
            tag.Add("hamonRequirementKeys", hamonAmountRequirements.Keys.ToList());
            tag.Add("hamonRequirementValues", hamonAmountRequirements.Values.ToList());
            tag.Add("hamonLevelKeys", hamonSkillLevels.Keys.ToList());
            tag.Add("hamonLevelValues", hamonSkillLevels.Values.ToList());
            tag.Add("hamonSkillPoints", skillPointsAvailable);
            tag.Add("learnedAnyAbility", learnedAnyAbility);
            tag.Add("learnedHamon", learnedHamon);
        }

        public override void LoadData(TagCompound tag)
        {
            IList<int> skillKeys = tag.GetList<int>("hamonSkillKeys");
            IList<bool> skillValues = tag.GetList<bool>("hamonSkillValues");
            IList<int> requirementKeys = tag.GetList<int>("hamonRequirementKeys");
            IList<int> requirementValues = tag.GetList<int>("hamonRequirementValues");
            IList<int> levelKeys = tag.GetList<int>("hamonLevelKeys");
            IList<int> levelValues = tag.GetList<int>("hamonLevelValues");
            skillPointsAvailable = tag.GetInt("hamonSkillPoints");
            learnedAnyAbility = tag.GetBool("learnedAnyAbility");
            learnedHamon = tag.GetBool("learnedHamon");
            /*for (int i = 0; i < keys.Count; i++)
            {
                if (learnedHamonSkills.ContainsKey(keys[i]))
                {
                    learnedHamonSkills[keys[i]] = values[i];
                }
            }*/

            //if (learnedHamonSkills.Count != 0)
            learnedHamonSkills = skillKeys.Zip(skillValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);

            //if (hamonAmountRequirements.Count != 0)
            hamonAmountRequirements = requirementKeys.Zip(requirementValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);

            //if (hamonSkillLevels.Count != 0)
            hamonSkillLevels = levelKeys.Zip(levelValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }
    }
}
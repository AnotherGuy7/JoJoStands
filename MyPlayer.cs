using JoJoStands.Items;
using JoJoStands.Items.Hamon;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TerraUI.Objects;

namespace JoJoStands
{
    public class MyPlayer : ModPlayer
    {
        public static int DeathSoundID;        //make them static to have them be true for all of you, instead of having to manually set it true for each of your characters
        public static int RangeIndicatorAlpha;
        public static bool Sounds = true;
        public static bool TimestopEffects = false;
        public static bool RangeIndicators = false;
        public static bool AutomaticActivations = false;
        public static bool SecretReferences = false;
        public static int StandSlotPositionX;
        public static int StandSlotPositionY;
        public static float HamonBarPositionX;
        public static float HamonBarPositionY;
        public static float ModSoundsVolume;
        public static bool ColorChangeEffects = false;
        public static ColorChangeStyle colorChangeStyle = ColorChangeStyle.None;
        public static StandSearchType standSearchType = StandSearchType.Bosses;
        public static bool testStandUnlocked = false;

        public int goldenSpinCounter = 0;
        public int spinSubtractionTimer = 0;
        public int shadowDodgeCooldownTimer = 0;        //does Vanilla not have one of these?
        public int aerosmithRadarFrameCounter = 0;
        public int poseDuration = 300;
        public int menacingFrames = 0;
        public int tbcCounter = 0;
        public int standKeyPressTimer = 0;
        public int GEAbilityNumber = 0;
        public int tuskActNumber = 0;
        public int equippedTuskAct = 0;
        public int tuskShootCooldown = 0;
        public int timestopEffectDurationTimer = 0;
        public int sexPistolsLeft = 6;
        public int sexPistolsTier = 0;
        public int gratefulDeadTier = 0;
        public int revolverBulletsShot = 0;
        public int sexPistolsRecoveryTimer = 0;
        public int aerosmithWhoAmI = 0;
        public int revertTimer = 0;     //for all requiems that can change forms back to previous forms
        public float standDamageBoosts = 1;
        public float standRangeBoosts = 0f;
        public float standCritChangeBoosts = 0f;
        public int standSpeedBoosts = 0;
        public float standCooldownReduction = 0f;
        public int standType = 0;           //0 = no type; 1 = Melee; 2 = Ranged;
        public int piercedTimer = 36000;
        public int hermitPurpleTier = 0;
        public int hermitPurpleShootCooldown = 0;
        public int hermitPurpleSpecialFrameCounter = 0;
        public int hermitPurpleHamonBurstLeft = 0;
        public int creamTier = 0;
        public int voidCounter = 0;
        public int voidCounterMax = 0;
        public int voidTimer = 0;
        public int creamFrame = 0;
        public int badCompanyTier = 0;
        public int badCompanySoldiers = 0;
        public int badCompanyTanks = 0;
        public int badCompanyChoppers = 0;
        public int maxBadCompanyUnits = 0;
        public int badCompanyUnitsLeft = 0;
        public int badCompanyUIClickTimer = 0;
        public int standDefenseToAdd = 0;

        public bool wearingEpitaph = false;
        public bool wearingTitaniumMask = false;
        public bool achievedInfiniteSpin = false;
        public bool standOut = false;
        public bool standAutoMode = false;
        public bool standRemoteMode = false;
        public bool destroyAmuletEquipped = false;
        public bool greaterDestroyEquipped = false;
        public bool awakenedAmuletEquipped = false;
        public bool chlorositeShortEqquipped = false;
        public bool crystalArmorSetEquipped = false;
        public bool crackedPearlEquipped = false;
        public bool phantomHoodLongEquipped = false;
        public bool phantomHoodNeutralEquipped = false;
        public bool phantomHoodShortEquipped = false;
        public bool phantomChestplateEquipped = false;
        public bool phantomLeggingsEquipped = false;
        public bool usedEctoPearl = false;
        public bool receivedArrowShard = false;
        public bool creamExposedMode = false;
        public bool creamVoidMode = false;
        public bool creamNormalToExposed = false;
        public bool creamExposedToVoid = false;
        public bool creamAnimationReverse = false;
        public bool creamNormalToVoid = false;
        public bool doobiesskullEquipped = false;
        public bool blackUmbrellaEquipped = false;
        public bool silverChariotShirtless = false;      //hot shirtless daddy silver chariot *moan*
        //Ozi is to blame for the comment above.

        public bool timestopActive;
        public bool timeskipPreEffect;
        public bool timeskipActive;
        public bool backToZeroActive;
        public bool deathLoopActive;
        public bool epitaphForesightActive;
        public bool standAccessory = false;
        public bool bitesTheDustActive = false;
        public bool poseMode = false;
        public bool canRevertFromKQBTD = false;
        public bool showingCBLayer = false;     //this is a bool that's needed to sync so that the Century Boy layer shows up for other clients in Multiplayer

        private bool forceChangedTusk = false;
        private bool revived = false;

        public bool ZoneViralMeteorite;

        public UIItemSlot StandSlot;
        public UIItemSlot StandDyeSlot;

        public static List<int> stopImmune = new List<int>();
        public static List<int> standTier1List = new List<int>();
        public Vector2[] sexPistolsOffsets = new Vector2[6];

        public Vector2 standRemoteModeCameraPosition;
        public Vector2 VoidCamPosition;

        public string poseSoundName = "";       //This is for JoJoStandsSounds
        public string standName = "";

        private int amountOfSexPistolsPlaced = 0;
        private int sexPistolsClickTimer = 0;
        private bool changingSexPistolsPositions = false;

        public enum StandSearchType
        {
            Bosses,
            Closest,
            Farthest,
            LeastHealth,
            MostHealth
        }

        public enum ColorChangeStyle
        {
            None,
            NormalToLightGreen,
            NormalToBlue,
            NormalToPurple,
            NormalToRed,
            NormalToDarkBlue
        }

        public override void ResetEffects()
        {
            BulletCounter.Visible = false;
            standRemoteMode = false;
            wearingEpitaph = false;
            destroyAmuletEquipped = false;
            greaterDestroyEquipped = false;
            crystalArmorSetEquipped = false;
            wearingTitaniumMask = false;
            awakenedAmuletEquipped = false;
            phantomHoodLongEquipped = false;
            phantomHoodNeutralEquipped = false;
            phantomHoodShortEquipped = false;
            phantomChestplateEquipped = false;
            phantomLeggingsEquipped = false;
            doobiesskullEquipped = false;
            blackUmbrellaEquipped = false;
            silverChariotShirtless = false;

            standDamageBoosts = 1f;
            standRangeBoosts = 0f;
            standSpeedBoosts = 0;
            standCritChangeBoosts = 5f;      //standCooldownReductions is in PostUpdateBuffs cause it gets reset before buffs use it
        }


        public override void OnEnterWorld(Player player)
        {
            int type = StandSlot.Item.type;
            if (type == mod.ItemType("TuskAct3") || type == mod.ItemType("TuskAct4"))
            {
                tuskActNumber = 3;
            }
            else if (type == mod.ItemType("TuskAct2"))
            {
                tuskActNumber = 2;
            }
            else if (type == mod.ItemType("TuskAct1"))
            {
                tuskActNumber = 1;
            }
            for (int i = 0; i < sexPistolsOffsets.Length; i++)
            {
                sexPistolsOffsets[i] = new Vector2(Main.rand.NextFloat(-40f, 40f + 1f), Main.rand.NextFloat(-40f, 40f + 1f));
            }
        }

        public override void PlayerDisconnect(Player player)        //runs for everyone that hasn't left
        {
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player otherPlayer = Main.player[p];
                MyPlayer otherModPlayer = otherPlayer.GetModPlayer<MyPlayer>();
                if (otherPlayer.active)
                {
                    if (otherModPlayer.timestopActive && !otherPlayer.HasBuff(mod.BuffType("TheWorldBuff")))       //if everyone has the effect and no one has the owner buff, turn it off
                    {
                        Main.NewText("The user has left, and time has begun to move once more...");
                        otherModPlayer.timestopActive = false;
                    }
                    if (otherModPlayer.timeskipActive && !otherPlayer.HasBuff(mod.BuffType("SkippingTime")))
                    {
                        Main.NewText("The user has left, and time has begun to move once more...");
                        otherModPlayer.timeskipActive = false;
                    }
                    if (otherModPlayer.backToZeroActive && !otherPlayer.HasBuff(mod.BuffType("BackToZero")))
                    {
                        otherModPlayer.backToZeroActive = false;
                    }
                    if (otherModPlayer.deathLoopActive && !otherPlayer.HasBuff(mod.BuffType("DeathLoop")))
                    {
                        otherModPlayer.deathLoopActive = false;
                    }
                }
            }
        }

        public override void ModifyScreenPosition()     //used HERO's Mods FlyCam as a reference for this
        {
            if (standRemoteMode)
                Main.screenPosition = standRemoteModeCameraPosition;

            if (creamVoidMode || creamExposedMode)
                Main.screenPosition = VoidCamPosition;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (JoJoStands.StandAutoMode.JustPressed && !standAutoMode && standKeyPressTimer <= 0)
            {
                standKeyPressTimer += 30;
                Main.NewText("Stand Control: Auto");
                standAutoMode = true;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandAutoMode(256, player.whoAmI, true, player.whoAmI);
                }
            }
            if (JoJoStands.StandAutoMode.JustPressed && standAutoMode && standKeyPressTimer <= 0)
            {
                standKeyPressTimer += 30;
                Main.NewText("Stand Control: Manual");
                standAutoMode = false;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandAutoMode(256, player.whoAmI, true, player.whoAmI);
                }
            }
            if (JoJoStands.PoseHotKey.JustPressed && !poseMode)
            {
                if (Sounds)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/menacing"));
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendPoseMode(256, player.whoAmI, true, player.whoAmI);
                }
                poseMode = true;
            }
            if (JoJoStands.StandOut.JustPressed && !standOut && standKeyPressTimer <= 0 && !player.HasBuff(mod.BuffType("Stolen")))
            {
                standOut = true;
                standKeyPressTimer += 30;
                SpawnStand();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, true, player.whoAmI);      //we send it to 256 cause it's the server
                }
            }
            if (JoJoStands.SpecialHotKey.Current && standAccessory)
            {
                if (StandSlot.Item.type == mod.ItemType("CenturyBoy") || StandSlot.Item.type == mod.ItemType("CenturyBoyT2"))
                {
                    player.AddBuff(mod.BuffType("CenturyBoyBuff"), 2, true);
                }
                if (StandSlot.Item.type == mod.ItemType("LockT3"))
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < (98f * 4f) && !npc.townNPC && !npc.immortal && !npc.hide)
                        {
                            if (npc.boss)
                            {
                                npc.AddBuff(mod.BuffType("Locked"), 60 * 10);
                            }
                            if (!npc.boss && npc.lifeMax > 5)
                            {
                                npc.AddBuff(mod.BuffType("Locked"), 60 * 30);
                            }
                        }
                    }
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was over-guilted."), 50, player.direction);
                    player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(20));
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        float distance = Vector2.Distance(player.Center, otherPlayer.Center);
                        if (distance < (98f * 4f) && player.whoAmI != otherPlayer.whoAmI)
                        {
                            otherPlayer.AddBuff(mod.BuffType("Locked"), 60 * 15);
                        }
                    }
                }
                if (StandSlot.Item.type == mod.ItemType("LockT4"))
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < (98f * 4f) && !npc.townNPC && !npc.immortal && !npc.hide)
                        {
                            if (npc.boss)
                            {
                                npc.AddBuff(mod.BuffType("Locked"), 60 * 15);
                            }
                            if (!npc.boss && npc.lifeMax > 5)
                            {
                                npc.AddBuff(mod.BuffType("Locked"), 60 * 45);
                            }
                        }
                    }
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was over-guilted."), 25, player.direction);
                    player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(20));
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        float distance = Vector2.Distance(player.Center, otherPlayer.Center);
                        if (distance < (98f * 4f) && player.whoAmI != otherPlayer.whoAmI)
                        {
                            otherPlayer.AddBuff(mod.BuffType("Locked"), 60 * 30);
                        }
                    }
                }
            }
            if (JoJoStands.StandOut.JustPressed && standOut && standKeyPressTimer <= 0)
            {
                standOut = false;
                standType = 0;
                poseSoundName = "";
                standName = "";
                standRemoteMode = false;
                standDefenseToAdd = 0;
                creamTier = 0;
                sexPistolsTier = 0;
                hermitPurpleTier = 0;
                gratefulDeadTier = 0;

                voidCounter = 0;
                creamNormalToExposed = false;
                creamNormalToVoid = false;
                creamExposedToVoid = false;
                creamFrame = 0;

                badCompanyTier = 0;

                if (standAccessory)
                {
                    standAccessory = false;
                }
                if (equippedTuskAct != 0)
                {
                    equippedTuskAct = 0;
                    tuskActNumber = 0;
                }
                if (showingCBLayer)
                {
                    showingCBLayer = false;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Networking.ModNetHandler.playerSync.SendCBLayer(256, player.whoAmI, false, player.whoAmI);
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, false, player.whoAmI);
                }
                standKeyPressTimer += 30;
            }
        }

        public override void Initialize()
        {
            StandSlot = new UIItemSlot(Vector2.Zero, hoverText: "Enter Stand Here", scaleToInventory: true);
            StandSlot.BackOpacity = .8f;
            StandSlot.Item = new Terraria.Item();
            StandSlot.Item.SetDefaults(0);

            StandDyeSlot = new UIItemSlot(StandSlot.Position - new Vector2(60f, 0f), 52, context: ItemSlot.Context.EquipDye, "Enter Dye Here", scaleToInventory: true);
            StandDyeSlot.BackOpacity = .8f;
            StandDyeSlot.Item = new Item();
            StandDyeSlot.Item.SetDefaults(0);
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "StandInSlot", ItemIO.Save(StandSlot.Item) },
                { "canRevertBTD", canRevertFromKQBTD },
                { "DyeInDyeSlot", ItemIO.Save(StandDyeSlot.Item) },
                { "usedEctoPearl", usedEctoPearl },
                { "receivedArrowShard", receivedArrowShard },
                { "piercedTimer", piercedTimer }
            };
        }

        public override void Load(TagCompound tag)
        {
            StandSlot.Item = ItemIO.Load(tag.GetCompound("StandInSlot")).Clone();
            canRevertFromKQBTD = tag.GetBool("canRevertBTD");
            StandDyeSlot.Item = ItemIO.Load(tag.GetCompound("DyeInDyeSlot")).Clone();
            usedEctoPearl = tag.GetBool("usedEctoPearl");
            receivedArrowShard = tag.GetBool("receivedArrowShard");
            piercedTimer = tag.GetInt("piercedTimer");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Main.playerInventory)
            {
                float origScale = Main.inventoryScale;

                Main.inventoryScale = 0.85f;

                int slotPosX = StandSlotPositionX * (Main.screenWidth / 100);
                int slotPosY = StandSlotPositionY * (Main.screenHeight / 100);

                StandSlot.Position = new Vector2(slotPosX, slotPosY);
                StandDyeSlot.Position = new Vector2(slotPosX - 60f, slotPosY);

                StandSlot.Draw(spriteBatch);
                StandDyeSlot.Draw(spriteBatch);

                Main.inventoryScale = origScale;

                if (!timestopActive)        //so that it's not interactable during a timestop, cause switching stands during a timestop is... not good
                {
                    if (Main.mouseItem.IsAir || Main.mouseItem.modItem is Items.StandItemClass)
                        StandSlot.Update();
                    if (Main.mouseItem.IsAir || Main.mouseItem.dye != 0)
                        StandDyeSlot.Update();
                }
            }
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (Main.rand.Next(0, 101) <= 20)
            {
                int inheritanceStandChance = Main.rand.Next(0, standTier1List.Count);
                Item standTier1 = new Item();
                standTier1.SetDefaults(standTier1List[inheritanceStandChance]);
                standTier1.stack = 1;
                items.Add(standTier1);
            }
        }

        public override void SetControls()
        {
            if (standRemoteMode || player.HasBuff(mod.BuffType("CenturyBoyBuff")) || creamVoidMode || creamExposedMode)
            {
                player.controlUp = false;
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlJump = false;
                player.controlHook = false;
                player.controlSmart = false;
            }
        }

        public override void PreUpdate()
        {
            if (standKeyPressTimer > 0)
                standKeyPressTimer--;
            if (revertTimer > 0)
                revertTimer--;
            if (shadowDodgeCooldownTimer > 0)
                shadowDodgeCooldownTimer--;

            if (!Main.dedServ)      //if (this isn't the (dedicated server?)) cause shaders don't exist serverside
            {
                if (timestopEffectDurationTimer > 0)
                {
                    if (timestopEffectDurationTimer >= 15 && !Filters.Scene["TimestopEffectShader"].IsActive() && TimestopEffects)
                    {
                        Filters.Scene.Activate("TimestopEffectShader");
                    }
                    if (timestopEffectDurationTimer == 14)
                    {
                        Filters.Scene["TimestopEffectShader"].Deactivate();
                        if (!Filters.Scene["GreyscaleEffect"].IsActive())
                        {
                            Filters.Scene.Activate("GreyscaleEffect");
                        }
                    }
                    timestopEffectDurationTimer--;
                }
                if (!timestopActive)
                {
                    if (Filters.Scene["GreyscaleEffect"].IsActive())
                    {
                        Filters.Scene["GreyscaleEffect"].Deactivate();
                    }
                    if (Filters.Scene["TimestopEffectShader"].IsActive())
                    {
                        Filters.Scene["TimestopEffectShader"].Deactivate();
                    }
                }
                if (backToZeroActive)
                {
                    if (!Filters.Scene["GreenEffect"].IsActive())
                    {
                        Filters.Scene.Activate("GreenEffect");
                    }
                    if (Filters.Scene["GreyscaleEffect"].IsActive())
                    {
                        Filters.Scene["GreyscaleEffect"].Deactivate();
                    }
                    if (Filters.Scene["TimestopEffectShader"].IsActive())
                    {
                        Filters.Scene["TimestopEffectShader"].Deactivate();
                    }
                    if (Filters.Scene["RedEffect"].IsActive())
                    {
                        Filters.Scene["RedEffect"].Deactivate();
                    }
                    if (player.HasBuff(mod.BuffType("TheWorldBuff")))
                    {
                        player.ClearBuff(mod.BuffType("TheWorldBuff"));
                    }
                    if (player.HasBuff(mod.BuffType("SkippingTime")))
                    {
                        player.ClearBuff(mod.BuffType("SkippingTime"));
                    }
                    if (player.HasBuff(mod.BuffType("ForesightBuff")))
                    {
                        player.ClearBuff(mod.BuffType("ForesightBuff"));
                    }
                    timestopActive = false;     //second, get rid of the effects from everyone
                    timeskipActive = false;
                    timeskipPreEffect = false;
                    epitaphForesightActive = false;
                }
                if (!backToZeroActive && Filters.Scene["GreenEffect"].IsActive())
                {
                    Filters.Scene["GreenEffect"].Deactivate();
                }
                if (epitaphForesightActive && !Filters.Scene["RedEffect"].IsActive())
                {
                    Filters.Scene.Activate("RedEffect");
                }
                if (!epitaphForesightActive && Filters.Scene["RedEffect"].IsActive())
                {
                    Filters.Scene["RedEffect"].Deactivate();
                }
                if (ColorChangeEffects)
                {
                    if (JoJoStandsWorld.VampiricNight && !Filters.Scene["ColorChangeEffect"].IsActive())
                    {
                        Filters.Scene.Activate("ColorChangeEffect");
                        var shader = Filters.Scene["ColorChangeEffect"].GetShader();
                        shader.UseProgress((int)ColorChangeStyle.NormalToLightGreen);
                    }
                }
                if (!JoJoStandsWorld.VampiricNight && Filters.Scene["ColorChangeEffect"].IsActive() || (Filters.Scene["ColorChangeEffect"].IsActive() && !ColorChangeEffects))
                {
                    Filters.Scene["ColorChangeEffect"].Deactivate();
                }
            }
            if (standRemoteMode && standName == "Aerosmith")
            {
                player.velocity = Vector2.Zero;
                AerosmithRadar.Visible = StandSlot.Item.type == mod.ItemType("AerosmithT3") || StandSlot.Item.type == mod.ItemType("AerosmithFinal");
            }
            else
            {
                AerosmithRadar.Visible = false;
            }
            if (poseMode)
            {
                poseDuration--;
                if ((poseDuration <= 0 || player.velocity != Vector2.Zero) && !Main.mouseLeft && !Main.mouseRight)
                {
                    menacingFrames = 0;
                    if (poseDuration <= 0)
                    {
                        player.AddBuff(mod.BuffType("StrongWill"), 30 * 60);
                    }
                    poseDuration = 300;
                    poseMode = false;
                    JoJoStands.testStandPassword.Add(Convert.ToChar((int)MathHelper.ToDegrees(1.36136f)));
                    JoJoStands.testStandPassword.Add(Convert.ToChar(Math.Sqrt(4999) + 1));
                    JoJoStands.testStandPassword.Add(Convert.ToChar(byte.MaxValue - 186));
                    JoJoStands.testStandPassword.Add(Convert.ToChar((78 / 4) * Math.Pow(2f, 2f)));
                    JoJoStands.testStandPassword.Add(Convert.ToChar(84));
                }
            }

            if (timestopActive)
            {
                Main.windSpeed = 0f;
            }
            if (!player.dead && player.whoAmI == Main.myPlayer)
            {
                ToBeContinued.Visible = false;
                tbcCounter = 0;
            }

            if (goldenSpinCounter > 0)          //golden spin stuff
            {
                spinSubtractionTimer++;
                if (spinSubtractionTimer >= 90 && player.mount.Type == mod.GetMount("SlowDancerMount").Type)
                {
                    spinSubtractionTimer = 0;
                    goldenSpinCounter -= 2;
                }
                if (spinSubtractionTimer >= 60 && player.mount.Type != mod.GetMount("SlowDancerMount").Type)
                {
                    spinSubtractionTimer = 0;
                    goldenSpinCounter -= 4;
                }
                if (goldenSpinCounter <= 1)
                {
                    achievedInfiniteSpin = false;
                    if (GoldenSpinMeter.Visible)
                    {
                        GoldenSpinMeter.Visible = false;
                    }
                }
                if (equippedTuskAct != 0)
                {
                    if (goldenSpinCounter > 0)
                    {
                        if (!GoldenSpinMeter.Visible)
                        {
                            GoldenSpinMeter.Visible = true;
                        }
                        if (achievedInfiniteSpin && !forceChangedTusk)
                        {
                            tuskActNumber = 4;
                            forceChangedTusk = true;
                        }
                        if (goldenSpinCounter <= 1)     //would reset anyway if the player isn't holding Tusk, cause it resets whenever you hold the item again
                        {
                            forceChangedTusk = false;
                        }
                    }
                }

                if (goldenSpinCounter >= 300)
                {
                    goldenSpinCounter = 300;
                    achievedInfiniteSpin = true;
                }
            }

            if (sexPistolsTier != 0)        //Sex Pistols stuff
            {
                if (!standAutoMode)
                {
                    bool specialPressed = false;
                    if (!Main.dedServ)
                        specialPressed = JoJoStands.SpecialHotKey.JustPressed;

                    bool secondSpecialPressed = false;
                    if (!Main.dedServ)
                        secondSpecialPressed = JoJoStands.SecondSpecialHotKey.JustPressed;

                    if (specialPressed)
                    {
                        amountOfSexPistolsPlaced = 0;
                        changingSexPistolsPositions = true;
                        sexPistolsClickTimer = 0;
                        Main.NewText("Click on any position to have a Sex Pistol stay around it. (Relative to the player)");
                    }

                    if (changingSexPistolsPositions)
                    {
                        if (sexPistolsClickTimer > 0)
                            sexPistolsClickTimer--;

                        if (Main.mouseLeft && sexPistolsClickTimer <= 0)
                        {
                            sexPistolsClickTimer += 30;
                            sexPistolsOffsets[amountOfSexPistolsPlaced] = Main.MouseWorld - player.Center;
                            amountOfSexPistolsPlaced++;
                            if (amountOfSexPistolsPlaced >= 6)
                            {
                                changingSexPistolsPositions = false;
                            }
                        }
                    }

                    if (secondSpecialPressed && sexPistolsTier >= 3)
                    {
                        player.AddBuff(mod.BuffType("BulletKickFrenzy"), 60 * 60 * (sexPistolsTier - 2));
                    }
                }
                else
                {
                    if (sexPistolsLeft < 6)
                    {
                        sexPistolsRecoveryTimer += sexPistolsTier;
                        if (sexPistolsRecoveryTimer >= 120)
                        {
                            sexPistolsLeft++;
                            sexPistolsRecoveryTimer = 0;
                        }
                    }
                }
                SexPistolsUI.Visible = standAutoMode;
            }
            else
            {
                SexPistolsUI.Visible = false;
            }

            if (equippedTuskAct != 0 && player.whoAmI == Main.myPlayer)     //Tusk stuff
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;
                if (specialJustPressed)
                {
                    tuskActNumber += 1;
                }
                if (equippedTuskAct <= 3)
                {
                    if (tuskActNumber > equippedTuskAct)
                        tuskActNumber = 1;
                }
                if (equippedTuskAct == 4)
                {
                    if (achievedInfiniteSpin)
                    {
                        if (tuskActNumber > equippedTuskAct)
                            tuskActNumber = 1;
                    }
                    else
                    {
                        if (tuskActNumber > 3)
                            tuskActNumber = 1;
                    }
                }
                if (tuskShootCooldown > 0)
                    tuskShootCooldown--;

                if (tuskActNumber <= 3)
                {
                    if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct" + tuskActNumber + "Pet")] <= 0)
                    {
                        Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct" + tuskActNumber + "Pet"), 0, 0f, Main.myPlayer);
                    }
                }
                else
                {
                    if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct4Minion")] <= 0)
                        Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct4Minion"), 0, 0f, Main.myPlayer);
                }
                if (tuskActNumber == 1)
                {
                    if (Main.mouseLeft && tuskShootCooldown <= 0)
                    {
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        Main.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();      //to normalize is to turn it into a direction under 1 but greater than 0
                        shootVelocity *= 12f;       //multiply the angle by the speed to get the effect
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("Nail"), (int)(21 * standDamageBoosts) + ((22 + equippedTuskAct - 1) * equippedTuskAct - 1), 4f, player.whoAmI);
                    }
                }
                if (tuskActNumber == 2)
                {
                    if (Main.mouseLeft && !player.channel && tuskShootCooldown <= 0)
                    {
                        player.channel = true;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        Main.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 4f;
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("ControllableNail"), (int)(49 * standDamageBoosts) + ((22 + equippedTuskAct - 2) * equippedTuskAct - 2), 5f, player.whoAmI);
                    }
                }
                if (tuskActNumber == 3)
                {
                    if (Main.mouseLeft && !player.channel && tuskShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] <= 0)
                    {
                        player.channel = true;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        Main.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 4f;
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("ControllableNail"), (int)(122 * standDamageBoosts) + ((22 + equippedTuskAct - 3) * equippedTuskAct - 3), 6f, player.whoAmI);
                    }
                    if (Main.mouseRight && player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] <= 0 && tuskShootCooldown <= 0 && !player.HasBuff(mod.BuffType("AbilityCooldown")))
                    {
                        tuskShootCooldown += 120 - standSpeedBoosts;
                        Main.PlaySound(SoundID.Item78);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 5f;
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("ShadowNail"), 124, 8f, player.whoAmI);
                    }
                }
                if (tuskActNumber == 4)
                {
                    if (Main.mouseLeft && !player.channel && tuskShootCooldown <= 0)
                    {
                        player.channel = true;
                        tuskShootCooldown += 15 - standSpeedBoosts;
                        Main.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 4f;
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("ControllableNail"), (int)(305 * standDamageBoosts), 7f, player.whoAmI);
                    }
                    if (Main.mouseRight && tuskShootCooldown <= 0 && !player.HasBuff(mod.BuffType("AbilityCooldown")))
                    {
                        tuskShootCooldown += 120 - standSpeedBoosts;
                        Main.PlaySound(SoundID.Item78);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 16f;
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("ReqNail"), 512, 0f, player.whoAmI);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(10));
                    }
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] > 0)
                {
                    tuskShootCooldown = 30;
                }
            }
            if (hermitPurpleTier != 0 && player.whoAmI == Main.myPlayer)
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

                HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
                if (specialJustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && hermitPurpleTier > 2 && hPlayer.amountOfHamon > 40)
                {
                    if (hermitPurpleTier == 3)
                    {
                        hermitPurpleHamonBurstLeft = 3;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(30));
                    }
                    if (hermitPurpleTier == 4)
                    {
                        hermitPurpleHamonBurstLeft = 5;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(20));
                    }
                    hPlayer.amountOfHamon -= 40;
                }
                if (hermitPurpleShootCooldown > 0)
                {
                    hermitPurpleShootCooldown--;
                }

                if (!standAutoMode)
                {
                    if (hermitPurpleTier == 1)
                    {
                        if (Main.mouseLeft && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleWhip")] == 0)
                        {
                            hermitPurpleShootCooldown += 40 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleWhip"), (int)(17 * standDamageBoosts), 4f, player.whoAmI);
                        }
                    }
                    if (hermitPurpleTier == 2)
                    {
                        if (Main.mouseLeft && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleWhip")] == 0)
                        {
                            hermitPurpleShootCooldown += 35 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleWhip"), (int)(42 * standDamageBoosts), 6f, player.whoAmI);
                        }
                        if (Main.mouseRight && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleGrab")] == 0)
                        {
                            hermitPurpleShootCooldown += 60 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 8f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleGrab"), (int)(33 * standDamageBoosts), 0f, player.whoAmI);
                        }
                    }
                    if (hermitPurpleTier == 3)
                    {
                        if (Main.mouseLeft && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleWhip")] == 0)
                        {
                            hermitPurpleShootCooldown += 30 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleWhip"), (int)(59 * standDamageBoosts), 7f, player.whoAmI);
                        }
                        if (Main.mouseRight && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleGrab")] == 0)
                        {
                            hermitPurpleShootCooldown += 60 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 8f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleGrab"), (int)(47 * standDamageBoosts), 0f, player.whoAmI);
                        }
                    }
                    if (hermitPurpleTier == 4)
                    {
                        if (Main.mouseLeft && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleWhip")] == 0)
                        {
                            hermitPurpleShootCooldown += 25 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleWhip"), (int)(81 * standDamageBoosts), 8f, player.whoAmI);
                        }
                        if (Main.mouseRight && hermitPurpleShootCooldown <= 0 && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleGrab")] == 0)
                        {
                            hermitPurpleShootCooldown += 60 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - player.position;
                            shootVelocity.Normalize();
                            shootVelocity *= 8f;
                            Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleGrab"), (int)(72 * standDamageBoosts), 0f, player.whoAmI);
                        }
                    }
                    if (player.controlHook && player.miscEquips[4].IsAir && player.ownedProjectileCounts[mod.ProjectileType("HermitPurpleHook")] == 0)       //player.miscEquips[4] is the hook slot
                    {
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 12f;
                        Projectile.NewProjectile(player.Center, shootVelocity, mod.ProjectileType("HermitPurpleHook"), 0, 0f, player.whoAmI);
                    }
                }
            }
            if (creamTier != 0)        //Cream stuff
            {
                VoidBar.Visible = true;
                voidCounterMax = (creamTier - 1) * 4;
                if (voidCounter < voidCounterMax)
                {
                    if (!creamVoidMode && !creamExposedMode)
                    {
                        voidTimer += 1;
                        if (voidTimer >= 120)
                        {
                            voidCounter++;
                            voidTimer = 0;
                        }
                    }
                    if (!creamVoidMode && creamExposedMode)
                    {
                        voidTimer += 1;
                        if (voidTimer >= 150 - ((creamTier - 1) * 30))
                        {
                            voidCounter++;
                            voidTimer = 0;
                        }
                    }
                }
                if (voidCounter > 0)
                {
                    if (creamVoidMode)
                    {
                        voidTimer += 1;
                        if (voidTimer >= 60)
                        {
                            voidCounter--;
                            voidTimer = 0;
                        }
                    }
                }
            }
            else
            {
                VoidBar.Visible = false;
            }

            if (badCompanyTier != 0)
            {
                if (badCompanyUIClickTimer > 0)
                {
                    badCompanyUIClickTimer--;
                }

                bool specialPressed = false;
                if (!Main.dedServ)
                    specialPressed = JoJoStands.SpecialHotKey.JustPressed;

                if (specialPressed)
                {
                    if (badCompanyTier == 3)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Vector2 position = player.Center + new Vector2(((Main.screenWidth / 2f) + Main.rand.Next(0, 100 + 1)) * -player.direction, -Main.screenHeight / 3f);
                            Vector2 velocity = new Vector2(6f * player.direction, 0f);
                            Projectile.NewProjectile(position, velocity, mod.ProjectileType("BadCompanyBomberPlane"), 0, 10f, player.whoAmI, badCompanyTier);
                        }
                        player.AddBuff(mod.BuffType("Reinforcements"), 120 * 60);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(5 * 60));       //6 minutes
                    }
                    if (badCompanyTier == 4)
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            Vector2 position = player.Center + new Vector2(((Main.screenWidth / 2f) + Main.rand.Next(0, 100 + 1)) * -player.direction, -Main.screenHeight / 3f);
                            Vector2 velocity = new Vector2(6f * player.direction, 0f);
                            Projectile.NewProjectile(position, velocity, mod.ProjectileType("BadCompanyBomberPlane"), 0, 10f, player.whoAmI, badCompanyTier);
                        }
                        player.AddBuff(mod.BuffType("Reinforcements"), 180 * 60);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), AbilityCooldownTime(3 * 60));       //5 minutes
                    }
                }

                bool recalculateArmy = false;
                int amountOfSoldiers = player.ownedProjectileCounts[mod.ProjectileType("BadCompanySoldier")];
                int amountOfTanks = player.ownedProjectileCounts[mod.ProjectileType("BadCompanyTank")];
                int amountOfChoppers = player.ownedProjectileCounts[mod.ProjectileType("BadCompanyChopper")];

                int unitsLeft = maxBadCompanyUnits - (amountOfSoldiers + (amountOfTanks * 4) + (amountOfChoppers * 6));
                int expectedUnitsLeft = maxBadCompanyUnits - (badCompanySoldiers + (badCompanyTanks * 4) + (badCompanyChoppers * 6));
                if (unitsLeft != expectedUnitsLeft)
                    recalculateArmy = true;
                badCompanyUnitsLeft = expectedUnitsLeft;

                int troopMult = 1;
                if (player.HasBuff(mod.BuffType("Reinforcements")))
                {
                    troopMult = 2;
                    recalculateArmy = true;
                }
                if (recalculateArmy)
                {
                    if (amountOfSoldiers < badCompanySoldiers * troopMult)     //Adding troops
                    {
                        Projectile.NewProjectile(player.Center, player.velocity, mod.ProjectileType("BadCompanySoldier"), 0, 0f, player.whoAmI, badCompanyTier);
                    }
                    if (amountOfTanks < badCompanyTanks)
                    {
                        Projectile.NewProjectile(player.Center, player.velocity, mod.ProjectileType("BadCompanyTank"), 0, 0f, player.whoAmI, badCompanyTier);
                    }
                    if (amountOfChoppers < badCompanyChoppers)
                    {
                        Projectile.NewProjectile(player.Center, player.velocity, mod.ProjectileType("BadCompanyChopper"), 0, 0f, player.whoAmI, badCompanyTier);
                    }

                    //Removing troops
                    if (amountOfSoldiers > badCompanySoldiers * troopMult)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile projectile = Main.projectile[p];
                            if (projectile.active && projectile.type == mod.ProjectileType("BadCompanySoldier") && projectile.owner == Main.myPlayer)
                            {
                                projectile.Kill();
                                break;
                            }
                        }
                    }
                    if (amountOfTanks > badCompanyTanks)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile projectile = Main.projectile[p];
                            if (projectile.active && projectile.type == mod.ProjectileType("BadCompanyTank") && projectile.owner == Main.myPlayer)
                            {
                                projectile.Kill();
                                break;
                            }
                        }
                    }
                    if (amountOfChoppers > badCompanyChoppers)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile projectile = Main.projectile[p];
                            if (projectile.active && projectile.type == mod.ProjectileType("BadCompanyChopper") && projectile.owner == Main.myPlayer)
                            {
                                projectile.Kill();
                                break;
                            }
                        }
                    }
                }
                if (Main.mouseRight && badCompanyUIClickTimer <= 0 && player.whoAmI == Main.myPlayer)
                {
                    badCompanyUIClickTimer = 30;
                    BadCompanyUnitsUI.Visible = !BadCompanyUnitsUI.Visible;
                }
            }

            if (silverChariotShirtless)
            {
                player.statDefense = (int)(player.statDefense * 0.6f);
                standDamageBoosts += 0.8f;
                standSpeedBoosts += 2;
                standCritChangeBoosts += 25f;
            }

            if (revived && !player.HasBuff(mod.BuffType("ArtificialSoul")))
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + "'s artificial soul has left him."), player.statLife + 1, player.direction);
                revived = false;
            }
        }

        public override void PreUpdateMovement()
        {
            if (timestopActive && !player.HasBuff(mod.BuffType("TheWorldBuff")))
                player.velocity = Vector2.Zero;
        }

        public override void PostUpdateMiscEffects()
        {
            if (usedEctoPearl)
                standRangeBoosts += 64f;

            if (standOut)
                player.statDefense += standDefenseToAdd;
        }

        public void SpawnStand()
        {
            Item inputItem = StandSlot.Item;

            if (inputItem.IsAir)
            {
                if (!JoJoStands.FanStandsLoaded)
                {
                    Main.NewText("There is no stand in the Stand Slot!", Color.Red);
                    standOut = false;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, false, player.whoAmI);
                    }
                    return;
                }
            }

            if (!(inputItem.modItem is StandItemClass))
            {
                Main.NewText("Something went wrong while summoning the Stand.", Color.Red);
                return;
            }

            StandItemClass standItem = inputItem.modItem as StandItemClass;

            standOut = true;
            standDefenseToAdd = 4 + (2 * standItem.standTier);
            standName = standItem.standProjectileName;
            if (standItem.standType == 2)
                standDefenseToAdd /= 2;

            if (!standItem.ManualStandSpawning(player))
            {
                string standClassName = standItem.standProjectileName + "StandT" + standItem.standTier;
                if (standClassName.Contains("T4"))
                    standClassName = standItem.standProjectileName + "StandFinal";

                int standProjectileType = mod.ProjectileType(standClassName);

                Projectile.NewProjectile(player.position, player.velocity, standProjectileType, 0, 0f, Main.myPlayer);
            }
        }

        public override void PreUpdateBuffs()
        {
            if (timestopActive && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                player.AddBuff(mod.BuffType("FrozeninTime"), 2);
            }
            if (deathLoopActive)       //if someone has deathloop turned on and you don't turn it on for you
            {
                player.AddBuff(mod.BuffType("DeathLoop"), 2);
            }
            if (epitaphForesightActive && !player.HasBuff(mod.BuffType("ForesightBuff")) && !player.HasBuff(mod.BuffType("ForeseenDebuff")))
            {
                player.AddBuff(mod.BuffType("ForeseenDebuff"), 2);
            }
        }

        public override void PostUpdateBuffs()
        {
            standCooldownReduction = 0f;        //it's here because it resets before the buffs can use it when its in ResetEffects()
            if (player.HasBuff(mod.BuffType("Stolen")))
                standOut = false;
        }

        public int AbilityCooldownTime(int seconds) //Sometimes we won't want to reduce the cooldown so that's why reduction defaults to 0
        {
            int timeToReturn;
            if (standCooldownReduction >= 0.5f)
                standCooldownReduction = 0.5f;
            timeToReturn = (int)((seconds * 60f) * (1f - standCooldownReduction));
            return timeToReturn;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)        //already only runs for melee weapons
        {
            if (wearingTitaniumMask && shadowDodgeCooldownTimer <= 0)
            {
                player.AddBuff(BuffID.ShadowDodge, 30 * 60);
                shadowDodgeCooldownTimer += 90 * 60;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (wearingTitaniumMask && shadowDodgeCooldownTimer <= 0)
            {
                player.AddBuff(BuffID.ShadowDodge, 30 * 60);
                shadowDodgeCooldownTimer += 90 * 60;
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (crystalArmorSetEquipped)
            {
                Vector2 shootVel = player.Center + new Vector2(0f, -8f);
                shootVel.Normalize();
                shootVel *= 10f;
                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(80);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                    int proj = Projectile.NewProjectile(player.Center, perturbedSpeed, mod.ProjectileType("CrystalShard"), 15, 2f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                }
            }
            if (hermitPurpleTier >= 2)
            {
                int reflectedDamage = (int)(npc.damage * (0.05f * hermitPurpleTier));
                npc.StrikeNPC(reflectedDamage, 3f * hermitPurpleTier, npc.direction);
                if (hPlayer.amountOfHamon >= 30)
                {
                    npc.AddBuff(mod.BuffType("Sunburn"), ((hPlayer.amountOfHamon / 30) * hermitPurpleTier) * 60);
                    hPlayer.amountOfHamon -= 2;
                }
            }
            if (hermitPurpleHamonBurstLeft > 0)
            {
                int reflectedDamage = npc.damage * 4;       //This is becaues npc damage is pretty weak against the NPC itself
                npc.StrikeNPC(reflectedDamage, 14f, npc.direction);
                npc.AddBuff(mod.BuffType("Sunburn"), (10 * (hermitPurpleTier - 2)) * 60);

                for (int i = 0; i < 60; i++)
                {
                    float circlePos = i;
                    Vector2 spawnPos = npc.Center + (circlePos.ToRotationVector2() * 50f);
                    Vector2 velocity = spawnPos - npc.Center;
                    velocity.Normalize();
                    Dust dust = Dust.NewDustPerfect(spawnPos, 169, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                    dust.noGravity = true;
                }
                hermitPurpleHamonBurstLeft -= 1;
            }
            if (doobiesskullEquipped && player.ownedProjectileCounts[mod.ProjectileType("ChimeraSnake")] < 3)
            {
                Vector2 shootVelocity = player.position;
                shootVelocity.Normalize();
                Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ChimeraSnake"), 30, 2f, player.whoAmI);
            }
        }
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (doobiesskullEquipped && player.ownedProjectileCounts[mod.ProjectileType("ChimeraSnake")] < 3)
            {
                Vector2 shootVelocity = player.position;
                shootVelocity.Normalize();
                Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ChimeraSnake"), 30, 2f, player.whoAmI);
            }
        }

        public override void UpdateBiomes()     //from ExampleMod ZoneExample
        {
            ZoneViralMeteorite = JoJoStandsWorld.viralMeteoriteTiles > 20;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            MyPlayer modOther = other.GetModPlayer<MyPlayer>();
            return ZoneViralMeteorite == modOther.ZoneViralMeteorite;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            MyPlayer modOther = other.GetModPlayer<MyPlayer>();
            modOther.ZoneViralMeteorite = ZoneViralMeteorite;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();        //written this way cause there may be more than 1 biome
            flags[0] = ZoneViralMeteorite;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneViralMeteorite = flags[0];
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] > 0)
            {
                return false;
            }
            return true;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)       //that 1 last frame before you completely die
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (DeathSoundID == 1)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/ToBeContinued4"));
                }
                if (DeathSoundID == 2)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/CAESAAAAAAAR"));
                }
                if (DeathSoundID == 3)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/GangTortureDance"));
                }
                if (DeathSoundID == 4)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/LastTrainHome"));
                }
                if (DeathSoundID == 5)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/KORE GA... WAGA KING CRIMSON NO NORIO KU"));
                }
                if (DeathSoundID != 1)
                {
                    ToBeContinued.Visible = true;
                }
            }

            if (player.HasBuff(mod.BuffType("Pierced")))
            {
                receivedArrowShard = false;
                piercedTimer = 36000;
            }

            if (!revived && player.HasItem(mod.ItemType("PokerChip")))
            {
                revived = true;
                player.AddBuff(mod.BuffType("ArtificialSoul"), 3600);
                player.ConsumeItem(mod.ItemType("PokerChip"), true);
                Main.NewText("The chip has given you new life!");
                return false;
            }
            if (backToZeroActive)
            {
                return false;
            }


            standOut = false;
            revived = false;
            return true;
        }

        public override void UpdateDead()
        {
            standOut = false;

            creamTier = 0;
            voidCounter = 0;
            creamNormalToExposed = false;
            creamNormalToVoid = false;
            creamExposedToVoid = false;
            creamFrame = 0;

            if (player.whoAmI == Main.myPlayer && DeathSoundID == 1)
            {
                tbcCounter++;
                if (tbcCounter >= 270)
                {
                    ToBeContinued.Visible = true;
                }
            }
        }

        public static readonly PlayerLayer MenacingPose = new PlayerLayer("JoJoStands", "Menacing Pose", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && drawPlayer.velocity == Vector2.Zero && mPlayer.poseMode)
            {
                Texture2D texture = mod.GetTexture("Extras/Menacing");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                }

                if (mPlayer.poseDuration % 8 == 0)
                    mPlayer.menacingFrames += 1;
                if (mPlayer.menacingFrames >= 6)
                {
                    mPlayer.menacingFrames = 0;
                }

                int frameHeight = texture.Height / 6;
                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle poseAnimRect = new Rectangle(0, frameHeight * mPlayer.menacingFrames, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 poseOrigin = new Vector2(texture.Width / 2f, frameHeight / 2f);
                DrawData data = new DrawData(texture, drawPos, poseAnimRect, drawColor, drawPlayer.fullRotation, poseOrigin, 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer AerosmithRadarCam = new PlayerLayer("JoJoStands", "Aerosmith Radar Cam", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.standRemoteMode && (mPlayer.StandSlot.Item.type == mod.ItemType("AerosmithT3") || mPlayer.StandSlot.Item.type == mod.ItemType("AerosmithFinal")))
            {
                Texture2D texture = mod.GetTexture("Extras/AerosmithRadar");
                int drawX = (int)(drawInfo.position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 7f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX -= 2;
                }
                mPlayer.aerosmithRadarFrameCounter++;
                if (mPlayer.aerosmithRadarFrameCounter >= 30)
                    mPlayer.aerosmithRadarFrameCounter = 0;
                int frame = mPlayer.aerosmithRadarFrameCounter / 16;
                int frameHeight = texture.Height / 2;

                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle animRect = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
                DrawData data = new DrawData(texture, drawPos, animRect, drawColor, 0f, origin, 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });



        public static readonly PlayerLayer KCArmLayer = new PlayerLayer("JoJoStands", "KCArm", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.wearingEpitaph)
            {
                Texture2D texture = mod.GetTexture("Extras/KCArm");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }

                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle animRect = new Rectangle(0, 0, texture.Width, texture.Height);
                Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
                DrawData data = new DrawData(texture, drawPos, animRect, drawColor, 0f, origin, 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer HermitPurpleArmsLayer = new PlayerLayer("JoJoStands", "HermitPurpleArmsLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && mPlayer.hermitPurpleTier != 0)
            {
                Texture2D texture = mod.GetTexture("Extras/HermitPurple_Arms");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y) - 4;
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                {
                    effects = SpriteEffects.None;
                }
                Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                if (mPlayer.hermitPurpleHamonBurstLeft > 0)        //No increment here because it's already incrememnted in the Body layer
                {
                    if (mPlayer.hermitPurpleSpecialFrameCounter >= 5)
                    {
                        color = Color.Yellow;
                        if (mPlayer.hermitPurpleSpecialFrameCounter >= 10)
                        {
                            mPlayer.hermitPurpleSpecialFrameCounter = 0;
                        }
                    }
                }
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer HermitPurpleBodyLayer = new PlayerLayer("JoJoStands", "HermitPurpleBodyLayer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && mPlayer.hermitPurpleTier > 1)
            {
                Texture2D texture = mod.GetTexture("Extras/HermitPurple_Body");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y) - 4;
                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                {
                    effects = SpriteEffects.None;
                }
                Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                if (mPlayer.hermitPurpleHamonBurstLeft > 0)
                {
                    mPlayer.hermitPurpleSpecialFrameCounter++;
                    if (mPlayer.hermitPurpleSpecialFrameCounter >= 5)
                    {
                        color = Color.Yellow;
                        if (mPlayer.hermitPurpleSpecialFrameCounter >= 10)
                        {
                            mPlayer.hermitPurpleSpecialFrameCounter = 0;
                        }
                    }
                }
                Vector2 drawPos = new Vector2(drawX, drawY);
                DrawData data = new DrawData(texture, drawPos, drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer CenturyBoyActivated = new PlayerLayer("JoJoStands", "CenturyBoyActivated", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && mPlayer.showingCBLayer)
            {
                Texture2D texture = mod.GetTexture("Extras/CB");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                {
                    effects = SpriteEffects.None;
                }
                if (mPlayer.StandDyeSlot.Item.dye != 0)
                {
                    ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(mPlayer.StandDyeSlot.Item.type);
                    shader.Apply(null);
                }
                Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY - 9f), drawPlayer.bodyFrame, drawColor, drawPlayer.bodyRotation, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer SexPistolsLayer = new PlayerLayer("JoJoStands", "Sex Pistols Layer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.sexPistolsLeft != 0 && mPlayer.standOut && mPlayer.sexPistolsTier != 0)
            {
                int frame = 6 - mPlayer.sexPistolsLeft;       //frames 0-5
                Texture2D texture = mod.GetTexture("Extras/SexPistolsLayer");
                int drawX = (int)(drawInfo.position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX = drawX - 2;
                }
                int frameHeight = texture.Height / 6;

                Vector2 drawPos = new Vector2(drawX, drawY);
                Rectangle sexPistolsAnimRect = new Rectangle(0, frameHeight * frame, texture.Width, frameHeight);
                Color drawColor = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                DrawData data = new DrawData(texture, drawPos, sexPistolsAnimRect, drawColor, drawPlayer.fullRotation, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PhantomHoodLongGlowmask = new PlayerLayer("JoJoStands", "Phantom Hood Long Glowmask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomHoodLongEquipped && drawPlayer.head == mod.GetEquipSlot("PhantomHoodLong", EquipType.Head))
            {
                Texture2D texture = mod.GetTexture("Extras/PhantomHoodLong_Head_Glowmask");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 offset = new Vector2(0f, 12f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PhantomHoodNeutralGlowmask = new PlayerLayer("JoJoStands", "Phantom Hood Neutral Glowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomHoodNeutralEquipped && drawPlayer.head == mod.GetEquipSlot("PhantomHoodNeutral", EquipType.Head))
            {
                Texture2D texture = mod.GetTexture("Extras/PhantomHoodNeutral_Head_Glowmask");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    //drawX -= 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 offset = new Vector2(0f, 12f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PhantomHoodShortGlowmask = new PlayerLayer("JoJoStands", "Phantom Hood Short Glowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomHoodShortEquipped && drawPlayer.head == mod.GetEquipSlot("PhantomHoodShort", EquipType.Head))
            {
                Texture2D texture = mod.GetTexture("Extras/PhantomHoodShort_Head_Glowmask");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 offset = new Vector2(0f, 12f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData data = new DrawData(texture, pos, drawPlayer.bodyFrame, Color.White * alpha, 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PhantomChestplateGlowmask = new PlayerLayer("JoJoStands", "Phantom Chestplate Glowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomChestplateEquipped && drawPlayer.body == mod.GetEquipSlot("PhantomChestplate", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Extras/PhantomChestplate_Body_Glowmask");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
                float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

                DrawData data = new DrawData(texture, position, drawPlayer.bodyFrame, Color.White * alpha, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PhantomArmsGlowmask = new PlayerLayer("JoJoStands", "Phantom Chestplate Arms Glowmask", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomChestplateEquipped && drawPlayer.body == mod.GetEquipSlot("PhantomChestplate", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Extras/PhantomChestplate_Arms_Glowmask");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
                float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;

                DrawData data = new DrawData(texture, position, drawPlayer.bodyFrame, Color.White * alpha, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer PhantomLeggingsGlowmask = new PlayerLayer("JoJoStands", "Phantom Leggings Glowmask", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.phantomLeggingsEquipped && drawPlayer.legs == mod.GetEquipSlot("PhantomLeggings", EquipType.Legs))
            {
                Texture2D texture = mod.GetTexture("Extras/PhantomLeggings_Legs_Glowmask");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Vector2 offset = new Vector2(0f, 18f);
                float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;      //The reason we do this is cause position as a float moves the glowmask around too much
                float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2;
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.legPosition - Main.screenPosition + offset;

                DrawData data = new DrawData(texture, position, drawPlayer.legFrame, Color.White * alpha, drawPlayer.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0);
                data.shader = drawInfo.legArmorShader;
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer BlackUmbrellaLayer = new PlayerLayer("JoJoStands", "Black Umbrella Layer", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer mPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && mPlayer.blackUmbrellaEquipped)
            {
                Texture2D texture = mod.GetTexture("Extras/UmbrellaHat");
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.position.Y - Main.screenPosition.Y) - 1;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 offset = new Vector2(0f, 0f);
                Vector2 pos = new Vector2(drawX, drawY) + offset;

                DrawData data = new DrawData(texture, pos, null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (player.dead || (player.mount.Type != -1))
            {
                KCArmLayer.visible = false;
                MenacingPose.visible = false;
                AerosmithRadarCam.visible = false;
                CenturyBoyActivated.visible = false;
                SexPistolsLayer.visible = false;
                PhantomHoodLongGlowmask.visible = false;
                PhantomHoodNeutralGlowmask.visible = false;
                PhantomHoodShortGlowmask.visible = false;
                PhantomChestplateGlowmask.visible = false;
                PhantomArmsGlowmask.visible = false;
                PhantomLeggingsGlowmask.visible = false;
                BlackUmbrellaLayer.visible = false;
            }
            else
            {
                SexPistolsLayer.visible = true;
                KCArmLayer.visible = true;
                MenacingPose.visible = true;
                AerosmithRadarCam.visible = true;
                CenturyBoyActivated.visible = true;
                PhantomHoodLongGlowmask.visible = phantomHoodLongEquipped;
                PhantomHoodNeutralGlowmask.visible = phantomHoodNeutralEquipped;
                PhantomHoodShortGlowmask.visible = phantomHoodShortEquipped;
                PhantomChestplateGlowmask.visible = phantomChestplateEquipped;
                PhantomArmsGlowmask.visible = phantomChestplateEquipped;
                PhantomLeggingsGlowmask.visible = phantomLeggingsEquipped;
                BlackUmbrellaLayer.visible = blackUmbrellaEquipped;

                if (player.ownedProjectileCounts[mod.ProjectileType("ShadowNail")] != 0)
                {
                    for (int i = 0; i < layers.Count; i++)
                    {
                        layers[i].visible = false;
                    }
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("Void")] != 0 || player.ownedProjectileCounts[mod.ProjectileType("ExposingCream")] != 0)
                {
                    for (int i = 0; i < layers.Count; i++)
                    {
                        layers[i].visible = false;
                    }
                }
            }

            int headLayer = layers.FindIndex(l => l == PlayerLayer.Head);       //Finding the head layer then as long as it exists we insert the armor over it
            if (headLayer > -1)
            {
                layers.Insert(headLayer + 1, PhantomHoodLongGlowmask);
                layers.Insert(headLayer + 1, PhantomHoodNeutralGlowmask);
                layers.Insert(headLayer + 1, PhantomHoodShortGlowmask);
                layers.Insert(headLayer + 1, BlackUmbrellaLayer);
            }
            int bodyLayer = layers.FindIndex(l => l == PlayerLayer.Body);
            if (bodyLayer > -1)
            {
                layers.Insert(bodyLayer + 1, PhantomChestplateGlowmask);
                int armsLayer = layers.FindIndex(l => l == PlayerLayer.Arms);
                if (armsLayer > -1)
                {
                    layers.Insert(armsLayer + 1, PhantomArmsGlowmask);
                }
            }
            int legsLayer = layers.FindIndex(l => l == PlayerLayer.Legs);
            if (legsLayer > -1)
            {
                layers.Insert(legsLayer + 1, PhantomLeggingsGlowmask);
            }

            layers.Add(AerosmithRadarCam);
            layers.Add(KCArmLayer);
            layers.Add(MenacingPose);
            layers.Add(CenturyBoyActivated);
            layers.Add(SexPistolsLayer);
            layers.Add(HermitPurpleBodyLayer);
            layers.Add(HermitPurpleArmsLayer);
        }
    }
}
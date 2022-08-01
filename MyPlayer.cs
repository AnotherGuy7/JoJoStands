using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items;
using JoJoStands.Items.Dyes;
using JoJoStands.Items.Hamon;
using JoJoStands.Mounts;
using JoJoStands.Networking;
using JoJoStands.Projectiles;
using JoJoStands.Projectiles.PlayerStands.BadCompany;
using JoJoStands.Projectiles.PlayerStands.SilverChariot;
using JoJoStands.Projectiles.PlayerStands.Tusk;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TerraUI.Objects;

namespace JoJoStands
{
    public class MyPlayer : ModPlayer
    {
        public static float RangeIndicatorAlpha;
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
        public static bool TimeskipEffects = false;
        public static bool BiteTheDustEffects = false;
        public static bool RespawnWithStandOut = true;
        public static bool StandPvPMode = false;
        public static bool AbilityWheelDescriptions = true;
        public static DeathSoundType DeathSoundID;
        public static ColorChangeStyle colorChangeStyle = ColorChangeStyle.None;
        public static StandSearchType standSearchType = StandSearchType.Bosses;
        public static bool testStandUnlocked = false;

        public int goldenSpinCounter = 0;
        public int shadowDodgeCooldownTimer = 0;        //does Vanilla not have one of these?
        public int aerosmithRadarFrameCounter = 0;
        public int poseDuration = 300;
        public int tuskActNumber = 0;
        public int equippedTuskAct = 0;
        public int tuskShootCooldown = 0;
        public int timestopEffectDurationTimer = 0;
        public int sexPistolsLeft = 6;
        public int sexPistolsTier = 0;
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
        public int standTier = 0;
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
        public int chosenAbility = 0;
        public int timeSkipEffectTransitionTimer = 0;
        public float biteTheDustEffectProgress = 0f;
        public int poseFrameCounter = 0;
        public int menacingFrames = 0;
        public int slowDancerSprintTime = 0;
        public int kingCrimsonAbilityCooldownTime = 0;

        public bool wearingEpitaph = false;
        public bool wearingTitaniumMask = false;
        public bool achievedInfiniteSpin = false;
        public bool revived = false;
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
        public bool creamDash = false;
        public bool creamNormalToExposed = false;
        public bool creamExposedToVoid = false;
        public bool creamAnimationReverse = false;
        public bool creamNormalToVoid = false;
        public bool silverChariotShirtless = false;      //hot shirtless daddy silver chariot *moan*
        //Ozi is to blame for the comment above.
        public bool standChangingLocked = false;
        public bool hideAllPlayerLayers = false;
        public bool standRespawnQueued = false;
        public bool stickyFingersAmbushMode = false;
        public bool gratefulDeadGasActive = false;
        public bool canStandBasicAttack = true;
        public bool usingStandTextureDye = false;
        public bool forceShutDownEffect = false;
        public bool badCompanyDefaultArmy = true;
        public bool ableToOverrideTimestop = false;
        public bool stoneFreeWeaveAbilityActive = false;
        public bool abilityWheelTipDisplayed = false;
        public bool awaitingViralMeteoriteTip = false;
        public bool hotbarLocked = false;
        public bool playerJustHit = false;

        public bool timestopActive;
        public bool timestopOwner;
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

        private int standKeyPressTimer = 0;
        private int spinSubtractionTimer = 0;
        private int tbcCounter = 0;
        private int playerJustHitTime = 0;

        private bool forceChangedTusk = false;

        public bool ZoneViralMeteorite;

        public UIItemSlot StandSlot;
        public UIItemSlot StandDyeSlot;

        public Vector2 VoidCamPosition;
        public Vector2 standRemoteModeCameraPosition;
        public Vector2[] sexPistolsOffsets = new Vector2[6];
        public Texture2D timeskipNPCMask;

        public string standName = "";
        public string poseSoundName = "";       //This is for JoJoStandsSounds
        public string dyePathAddition = "";     //This is for Stand texture finding
        public int standHitTime = 0;
        public StandTextureDye currentTextureDye;

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

        public enum DeathSoundType
        {
            None,
            Roundabout,
            Caesar,
            KonoMeAmareriMaroreriMerareMaro,
            LastTrainHome,
            KingCrimsonNoNorioKu,
        }

        public enum StandTextureDye
        {
            None,
            Salad
        }

        public override void ResetEffects()
        {
            standRemoteMode = false;
            wearingEpitaph = false;
            timestopOwner = false;
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
            silverChariotShirtless = false;
            hideAllPlayerLayers = false;
            BulletCounter.Visible = false;
            stickyFingersAmbushMode = false;
            gratefulDeadGasActive = false;
            canStandBasicAttack = true;
            usingStandTextureDye = false;
            currentTextureDye = StandTextureDye.None;
            if (StandDyeSlot.SlotItem.ModItem is StandDye)
                (StandDyeSlot.SlotItem.ModItem as StandDye).UpdateEquippedDye(Player);

            standDamageBoosts = 1f;
            standRangeBoosts = 0f;
            standSpeedBoosts = 0;
            standCritChangeBoosts = 5f;      //standCooldownReductions is in PostUpdateBuffs cause it gets reset before buffs use it
            Main.mapEnabled = true;
        }


        public override void OnEnterWorld(Player player)
        {
            int type = StandSlot.SlotItem.type;
            if (type == ModContent.ItemType<TuskAct3>() || type == ModContent.ItemType<TuskAct4>())
            {
                tuskActNumber = 3;
            }
            else if (type == ModContent.ItemType<TuskAct2>())
            {
                tuskActNumber = 2;
            }
            else if (type == ModContent.ItemType<TuskAct1>())
            {
                tuskActNumber = 1;
            }
            for (int i = 0; i < sexPistolsOffsets.Length; i++)
            {
                sexPistolsOffsets[i] = new Vector2(Main.rand.NextFloat(-40f, 40f + 1f), Main.rand.NextFloat(-40f, 40f + 1f));
            }
            forceShutDownEffect = false;
        }

        public override void OnRespawn(Player player)
        {
            forceShutDownEffect = false;
            if (Player.whoAmI == Main.myPlayer)
            {
                tbcCounter = 0;
                ToBeContinued.Visible = false;
            }
        }

        public override void PlayerDisconnect(Player player)        //runs for everyone that hasn't left
        {
            MyPlayer mPlayer = Player.GetModPlayer<MyPlayer>();
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    MyPlayer otherModPlayer = otherPlayer.GetModPlayer<MyPlayer>();
                    if (otherPlayer.active)
                    {
                        if (mPlayer.timestopActive && !otherPlayer.HasBuff(ModContent.BuffType<TheWorldBuff>()))       //if everyone has the effect and no one has the owner buff, turn it off
                        {
                            Main.NewText("The user has left, and time has begun to move once more...");
                            otherModPlayer.timestopActive = false;
                        }
                        if (mPlayer.timeskipActive && !otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
                        {
                            Main.NewText("The user has left, and time has begun to move once more...");
                            otherModPlayer.timeskipActive = false;
                        }
                        if (mPlayer.backToZeroActive && !otherPlayer.HasBuff(ModContent.BuffType<BacktoZero>()))
                        {
                            otherModPlayer.backToZeroActive = false;
                        }
                        if (otherModPlayer.deathLoopActive && !otherPlayer.HasBuff(ModContent.BuffType<DeathLoop>()))
                        {
                            otherModPlayer.deathLoopActive = false;
                        }
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
            if (JoJoStands.StandAutoModeHotKey.JustPressed && !standAutoMode && standKeyPressTimer <= 0)
            {
                standKeyPressTimer += 30;
                Main.NewText("Stand Control: Auto");
                standAutoMode = true;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendStandAutoMode(256, Player.whoAmI, true, Player.whoAmI);
            }
            if (JoJoStands.StandAutoModeHotKey.JustPressed && standAutoMode && standKeyPressTimer <= 0)
            {
                standKeyPressTimer += 30;
                Main.NewText("Stand Control: Manual");
                standAutoMode = false;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendStandAutoMode(256, Player.whoAmI, false, Player.whoAmI);
            }
            if (JoJoStands.PoseHotKey.JustPressed && !poseMode)
            {
                if (Sounds)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/PoseSound"));

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendPoseMode(256, Player.whoAmI, true, Player.whoAmI);

                poseMode = true;
            }
            if (standChangingLocked)
                return;

            if (JoJoStands.StandOutHotKey.JustPressed && !standOut && standKeyPressTimer <= 0 && !Player.HasBuff(ModContent.BuffType<Stolen>()))
            {
                standOut = true;
                standKeyPressTimer += 30;
                SpawnStand();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendStandOut(256, Player.whoAmI, true, Player.whoAmI);      //we send it to 256 cause it's the server
            }
            if (JoJoStands.SpecialHotKey.Current && standAccessory)
            {
                if (StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT1>() || StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT2>())
                {
                    Player.AddBuff(ModContent.BuffType<CenturyBoyBuff>(), 2, true);
                }
                if (StandSlot.SlotItem.type == ModContent.ItemType<LockT3>())
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float distance = Vector2.Distance(Player.Center, npc.Center);
                        if (npc.active && !npc.townNPC && !npc.immortal && !npc.hide && distance < (98f * 4f))
                        {
                            if (npc.boss)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 10);
                            }
                            if (!npc.boss && npc.lifeMax > 5)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 30);
                            }
                        }
                    }
                    Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " was over-guilted."), 50, Player.direction);
                    Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(20));
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        float distance = Vector2.Distance(Player.Center, otherPlayer.Center);
                        if (otherPlayer.active && distance < (98f * 4f) && Player.whoAmI != otherPlayer.whoAmI)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<Locked>(), 60 * 15);
                        }
                    }
                }
                if (StandSlot.SlotItem.type == ModContent.ItemType<LockFinal>())
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float distance = Vector2.Distance(Player.Center, npc.Center);
                        if (npc.active && !npc.townNPC && !npc.immortal && !npc.hide && distance < (98f * 4f))
                        {
                            if (npc.boss)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 15);
                            }
                            if (!npc.boss && npc.lifeMax > 5)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 45);
                            }
                        }
                    }
                    Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " was over-guilted."), 25, Player.direction);
                    Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(20));
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        float distance = Vector2.Distance(Player.Center, otherPlayer.Center);
                        if (otherPlayer.active && distance < (98f * 4f) && Player.whoAmI != otherPlayer.whoAmI)
                        {
                            otherPlayer.AddBuff(ModContent.BuffType<Locked>(), 60 * 30);
                        }
                    }
                }
            }
            if (JoJoStands.StandOutHotKey.JustPressed && standOut && standKeyPressTimer <= 0)
            {
                standOut = false;
                standAccessory = false;
                standRemoteMode = false;
                ableToOverrideTimestop = false;
                poseSoundName = "";
                standName = "";
                standType = 0;
                standTier = 0;
                standDefenseToAdd = 0;
                sexPistolsTier = 0;
                hermitPurpleTier = 0;
                stoneFreeWeaveAbilityActive = false;
                hotbarLocked = false;

                creamTier = 0;
                voidCounter = 0;
                creamNormalToExposed = false;
                creamNormalToVoid = false;
                creamExposedToVoid = false;
                creamDash = false;
                creamFrame = 0;

                badCompanyTier = 0;

                stickyFingersAmbushMode = false;

                if (StoneFreeAbilityWheel.Visible)
                    StoneFreeAbilityWheel.CloseAbilityWheel();
                if (equippedTuskAct != 0)
                {
                    equippedTuskAct = 0;
                    tuskActNumber = 0;
                }
                if (showingCBLayer)
                {
                    showingCBLayer = false;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ModNetHandler.playerSync.SendCBLayer(256, Player.whoAmI, false, Player.whoAmI);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendStandOut(256, Player.whoAmI, false, Player.whoAmI);
                standKeyPressTimer += 30;
            }
        }

        public override void Initialize()
        {
            StandSlot = new UIItemSlot(Vector2.Zero, hoverText: "Enter Stand Here", scaleToInventory: true);
            StandSlot.BackOpacity = .8f;
            StandSlot.SlotItem = new Item();
            StandSlot.SlotItem.SetDefaults(0);

            StandDyeSlot = new UIItemSlot(StandSlot.Position - new Vector2(60f, 0f), 52, context: ItemSlot.Context.EquipDye, "Enter Dye Here", scaleToInventory: true);
            StandDyeSlot.BackOpacity = .8f;
            StandDyeSlot.SlotItem = new Item();
            StandDyeSlot.SlotItem.SetDefaults(0);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("StandInSlot", ItemIO.Save(StandSlot.SlotItem));
            tag.Add("canRevertBTD", canRevertFromKQBTD);
            tag.Add("DyeInDyeSlot", ItemIO.Save(StandDyeSlot.SlotItem));
            tag.Add("usedEctoPearl", usedEctoPearl);
            tag.Add("receivedArrowShard", receivedArrowShard);
            tag.Add("piercedTimer", piercedTimer);
            tag.Add("abilityWheelTipDisplayed", abilityWheelTipDisplayed);
        }

        public override void LoadData(TagCompound tag)
        {
            StandSlot.SlotItem = ItemIO.Load(tag.GetCompound("StandInSlot")).Clone();
            canRevertFromKQBTD = tag.GetBool("canRevertBTD");
            StandDyeSlot.SlotItem = ItemIO.Load(tag.GetCompound("DyeInDyeSlot")).Clone();
            usedEctoPearl = tag.GetBool("usedEctoPearl");
            receivedArrowShard = tag.GetBool("receivedArrowShard");
            piercedTimer = tag.GetInt("piercedTimer");
            abilityWheelTipDisplayed = tag.GetBool("abilityWheelTipDisplayed");
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

                if (!timestopActive && !standChangingLocked)        //so that it's not interactable during a timestop, cause switching stands during a timestop is... not good
                {
                    if (Main.mouseItem.IsAir || Main.mouseItem.ModItem is StandItemClass)
                        StandSlot.Update();
                    if (Main.mouseItem.IsAir || Main.mouseItem.dye != 0 || Main.mouseItem.ModItem is StandDye)
                    {
                        StandDyeSlot.Update();
                        if (Main.mouseItem.ModItem is StandDye)
                            (Main.mouseItem.ModItem as StandDye).OnEquipDye(Player);
                    }
                }
            }

            /*if (timeskipActive)
            {
                RenderTarget2D renderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                {
                    spriteBatch.End();
                    Main.graphics.GraphicsDevice.SetRenderTarget(null);

                    Main.graphics.GraphicsDevice.SetRenderTarget(renderTarget);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active)
                            spriteBatch.Draw(GameContent.TextureAssets.Npc[npc.type].Value, npc.getRect(), npc.frame, Color.Red, npc.rotation, npc.visualOffset, SpriteEffects.None, 0f);
                    }

                    spriteBatch.End();     //ending the spriteBatch that started in PreDraw
                    Main.graphics.GraphicsDevice.SetRenderTarget(null);
                    timeskipNPCMask = renderTarget;
                    Filters.Scene["TimeSkipEffectShader"].GetShader().UseImage(timeskipNPCMask, 2);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                }
            }*/
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> startingItems = new List<Item>();

            if (Main.rand.Next(1, 5 + 1) == 1)
            {
                int inheritanceStandChance = Main.rand.Next(0, JoJoStands.standTier1List.Count);
                Item standTier1 = new Item();
                standTier1.SetDefaults(JoJoStands.standTier1List[inheritanceStandChance]);
                standTier1.stack = 1;
                startingItems.Add(standTier1);
            }
            return startingItems;
        }

        public override void SetControls()
        {
            if (standRemoteMode || Player.HasBuff(ModContent.BuffType<CenturyBoyBuff>()) || stickyFingersAmbushMode || creamVoidMode || creamExposedMode)
            {
                if (standName.Contains("Aerosmith"))
                    return;

                Player.controlUp = false;
                Player.controlDown = false;
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlJump = false;
                Player.controlHook = false;
                Player.controlSmart = false;
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

            UpdateShaderStates();
            if (standRemoteMode && standName == "Aerosmith")
                AerosmithRadar.Visible = StandSlot.SlotItem.type == ModContent.ItemType<AerosmithT3>() || StandSlot.SlotItem.type == ModContent.ItemType<AerosmithFinal>();
            else
                AerosmithRadar.Visible = false;

            if (poseMode)
            {
                poseDuration--;
                if ((poseDuration <= 0 || Player.velocity != Vector2.Zero) && !Main.mouseLeft && !Main.mouseRight)
                {
                    menacingFrames = 0;
                    if (poseDuration <= 0)
                        Player.AddBuff(ModContent.BuffType<StrongWill>(), 30 * 60);

                    poseMode = false;
                    poseDuration = 300;
                    JoJoStands.testStandPassword.Add(Convert.ToChar((int)MathHelper.ToDegrees(1.36136f)));
                    JoJoStands.testStandPassword.Add(Convert.ToChar((int)Math.Sqrt(4999) + 1));
                    JoJoStands.testStandPassword.Add(Convert.ToChar(byte.MaxValue - 186));
                    JoJoStands.testStandPassword.Add(Convert.ToChar((int)((78 / 4) * Math.Pow(2f, 2f)) + 2));
                    JoJoStands.testStandPassword.Add(Convert.ToChar(84));
                }
            }

            if (timestopActive)
                Main.windSpeedCurrent = 0f;
            if (PlayerInput.Triggers.Current.SmartSelect || Player.dead)
                canStandBasicAttack = false;
            if (hotbarLocked && standOut && !standAutoMode)
                Player.selectedItem = 0;
            if (playerJustHitTime > 0)
            {
                playerJustHitTime--;
                if (playerJustHitTime <= 0)
                    playerJustHit = false;
            }

            if (goldenSpinCounter > 0)          //golden spin stuff
            {
                spinSubtractionTimer++;
                if (spinSubtractionTimer >= 90 && Player.mount.Type == ModContent.MountType<SlowDancerMount>())
                {
                    spinSubtractionTimer = 0;
                    goldenSpinCounter -= 2;
                }
                if (spinSubtractionTimer >= 60 && Player.mount.Type != ModContent.MountType<SlowDancerMount>())
                {
                    spinSubtractionTimer = 0;
                    goldenSpinCounter -= 4;
                }
                if (goldenSpinCounter <= 1)
                {
                    achievedInfiniteSpin = false;
                    GoldenSpinMeter.Visible = false;
                }
                if (equippedTuskAct != 0)
                {
                    if (goldenSpinCounter > 0)
                    {
                        GoldenSpinMeter.Visible = true;
                        if (achievedInfiniteSpin && !forceChangedTusk)
                        {
                            tuskActNumber = 4;
                            forceChangedTusk = true;
                        }
                        if (goldenSpinCounter <= 1)     //would reset anyway if the Player isn't holding Tusk, cause it resets whenever you hold the Item again
                            forceChangedTusk = false;
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
                        changingSexPistolsPositions = !changingSexPistolsPositions;
                        amountOfSexPistolsPlaced = 0;
                        sexPistolsClickTimer = 0;
                        if (changingSexPistolsPositions)
                            Main.NewText("Sex Pistol Placement Mode: On");
                        else
                            Main.NewText("Sex Pistol Placement Mode: Off");
                    }

                    if (changingSexPistolsPositions)
                    {
                        if (sexPistolsClickTimer > 0)
                            sexPistolsClickTimer--;

                        if (Main.mouseLeft && sexPistolsClickTimer <= 0)
                        {
                            sexPistolsClickTimer += 20;
                            sexPistolsOffsets[amountOfSexPistolsPlaced] = Main.MouseWorld - Player.Center;
                            amountOfSexPistolsPlaced++;
                            if (amountOfSexPistolsPlaced >= 6)
                                amountOfSexPistolsPlaced = 0;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                ModNetHandler.playerSync.SendSexPistolPosition(256, Player.whoAmI, amountOfSexPistolsPlaced, sexPistolsOffsets[amountOfSexPistolsPlaced]);
                        }
                    }

                    if (secondSpecialPressed && !Player.HasBuff(ModContent.BuffType<BulletKickFrenzy>()) && sexPistolsTier >= 3)
                        Player.AddBuff(ModContent.BuffType<BulletKickFrenzy>(), 60 * 60 * (sexPistolsTier - 2));
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

            if (equippedTuskAct != 0 && Player.whoAmI == Main.myPlayer)     //Tusk stuff
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

                bool secondSpecialJustPressed = false;
                if (!Main.dedServ)
                    secondSpecialJustPressed = JoJoStands.SecondSpecialHotKey.JustPressed;

                if (secondSpecialJustPressed)
                    tuskActNumber += 1;

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
                    if (Player.ownedProjectileCounts[Mod.Find<ModProjectile>("TuskAct" + tuskActNumber + "Pet").Type] <= 0)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.position, Player.velocity, Mod.Find<ModProjectile>("TuskAct" + tuskActNumber + "Pet").Type, 0, 0f, Main.myPlayer);
                    }
                }
                else
                {
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<TuskAct4Stand>()] <= 0)
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.position, Player.velocity, ModContent.ProjectileType<TuskAct4Stand>(), 0, 0f, Main.myPlayer);
                }
                if (tuskActNumber == 1)
                {
                    if (Main.mouseLeft && canStandBasicAttack && tuskShootCooldown <= 0)
                    {
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();      //to normalize is to turn it into a direction under 1 but greater than 0
                        shootVelocity *= 12f;       //multiply the angle by the speed to get the effect
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<Nail>(), (int)(21 * standDamageBoosts) + ((22 + equippedTuskAct - 1) * equippedTuskAct - 1), 4f, Player.whoAmI);
                    }
                    if (Main.mouseRight && !Player.channel && tuskShootCooldown <= 0)
                    {
                        tuskShootCooldown = 5;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<NailSlasher>(), (int)(28 * standDamageBoosts) + ((24 + equippedTuskAct - 1) * equippedTuskAct - 1), 5f, Player.whoAmI);
                    }
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<NailSlasher>()] > 0)
                        tuskShootCooldown = 2;
                }
                if (tuskActNumber == 2)
                {
                    if (Main.mouseLeft && canStandBasicAttack && !Player.channel && tuskShootCooldown <= 0)
                    {
                        Player.channel = true;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();
                        shootVelocity *= 4f;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<ControllableNail>(), (int)(49 * standDamageBoosts) + ((22 + equippedTuskAct - 2) * equippedTuskAct - 2), 5f, Player.whoAmI);
                    }
                    if (Main.mouseRight && !Player.channel && tuskShootCooldown <= 0)
                    {
                        tuskShootCooldown = 5;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<NailSlasher>(), (int)(53 * standDamageBoosts) + ((24 + equippedTuskAct - 1) * equippedTuskAct - 1), 7f, Player.whoAmI);
                    }
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<NailSlasher>()] > 0)
                        tuskShootCooldown = 2;
                }
                if (tuskActNumber == 3)
                {
                    if (Main.mouseLeft && canStandBasicAttack && !Player.channel && tuskShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<ArmWormholeNail>()] <= 0)
                    {
                        Player.channel = true;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item67);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();
                        shootVelocity *= 4f;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<ControllableNail>(), (int)(122 * standDamageBoosts) + ((22 + equippedTuskAct - 3) * equippedTuskAct - 3), 6f, Player.whoAmI);
                    }
                    if (Main.mouseRight && Player.ownedProjectileCounts[ModContent.ProjectileType<ArmWormholeNail>()] <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] <= 0 && tuskShootCooldown <= 0 && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                    {
                        tuskShootCooldown += 60 - standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item78);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();
                        shootVelocity *= 1.1f;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<ArmWormholeNail>(), 80, 2f, Player.whoAmI);
                    }
                    if (specialJustPressed && Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<ArmWormholeNail>()] <= 0 && tuskShootCooldown <= 0 && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                    {
                        tuskShootCooldown += 120 - standSpeedBoosts;
                        SoundEngine.PlaySound(SoundID.Item78);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();
                        shootVelocity *= 5f;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<WormholeNail>(), 124, 8f, Player.whoAmI);
                    }
                }
                if (tuskActNumber == 4)
                {
                    if (standAutoMode)
                    {
                        if (Main.mouseLeft && canStandBasicAttack && !Player.channel && tuskShootCooldown <= 0)
                        {
                            Player.channel = true;
                            tuskShootCooldown += 15 - standSpeedBoosts;
                            SoundEngine.PlaySound(SoundID.Item67);
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 4f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<ControllableNail>(), (int)(305 * standDamageBoosts), 7f, Player.whoAmI);
                        }
                        if (Main.mouseRight && tuskShootCooldown <= 0 && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                        {
                            tuskShootCooldown += 120 - standSpeedBoosts;
                            SoundStyle item67 = SoundID.Item67;
                            item67.Pitch = -1.2f;
                            SoundEngine.PlaySound(item67, Player.Center);
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 16f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<InfiniteSpinNail>(), 512, 0f, Player.whoAmI);
                            Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(10));
                        }
                    }
                }
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] > 0)
                {
                    tuskShootCooldown = 30;
                }
            }
            if (hermitPurpleTier != 0 && Player.whoAmI == Main.myPlayer)
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

                HamonPlayer hPlayer = Player.GetModPlayer<HamonPlayer>();
                if (specialJustPressed && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && hermitPurpleTier > 2 && hPlayer.amountOfHamon > 40)
                {
                    if (hermitPurpleTier == 3)
                    {
                        hermitPurpleHamonBurstLeft = 3;
                        Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(30));
                    }
                    if (hermitPurpleTier == 4)
                    {
                        hermitPurpleHamonBurstLeft = 5;
                        Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(20));
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
                        if (Main.mouseLeft && canStandBasicAttack && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleWhip>()] == 0)
                        {
                            hermitPurpleShootCooldown += 40 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleWhip>(), (int)(38 * standDamageBoosts), 4f, Player.whoAmI);
                            SoundStyle itemSound = SoundID.Item1;
                            itemSound.Pitch = Main.rand.Next(4, 7 + 1) / 10f;
                            SoundEngine.PlaySound(itemSound, Player.Center);
                        }
                    }
                    if (hermitPurpleTier == 2)
                    {
                        if (Main.mouseLeft && canStandBasicAttack && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleWhip>()] == 0)
                        {
                            hermitPurpleShootCooldown += 35 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleWhip>(), (int)(81 * standDamageBoosts), 6f, Player.whoAmI);
                            SoundStyle itemSound = SoundID.Item1;
                            itemSound.Pitch = Main.rand.Next(4, 7 + 1) / 10f;
                            SoundEngine.PlaySound(itemSound, Player.Center);
                        }
                        if (Main.mouseRight && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleGrab>()] == 0)
                        {
                            hermitPurpleShootCooldown += 60 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 8f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleGrab>(), (int)(78 * standDamageBoosts), 0f, Player.whoAmI);
                            SoundStyle itemSound = SoundID.Item1;
                            itemSound.Pitch = Main.rand.Next(4, 7 + 1) / 10f;
                            SoundEngine.PlaySound(itemSound, Player.Center);
                        }
                    }
                    if (hermitPurpleTier == 3)
                    {
                        if (Main.mouseLeft && canStandBasicAttack && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleWhip>()] == 0)
                        {
                            hermitPurpleShootCooldown += 30 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleWhip>(), (int)(157 * standDamageBoosts), 7f, Player.whoAmI);
                            SoundStyle itemSound = SoundID.Item1;
                            itemSound.Pitch = Main.rand.Next(4, 7 + 1) / 10f;
                            SoundEngine.PlaySound(itemSound, Player.Center);
                        }
                        if (Main.mouseRight && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleGrab>()] == 0)
                        {
                            hermitPurpleShootCooldown += 60 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 8f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleGrab>(), (int)(149 * standDamageBoosts), 0f, Player.whoAmI);
                        }
                    }
                    if (hermitPurpleTier == 4)
                    {
                        if (Main.mouseLeft && canStandBasicAttack && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleWhip>()] == 0)
                        {
                            hermitPurpleShootCooldown += 25 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 14f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleWhip>(), (int)(202 * standDamageBoosts), 8f, Player.whoAmI);
                            SoundStyle itemSound = SoundID.Item1;
                            itemSound.Pitch = Main.rand.Next(4, 7 + 1) / 10f;
                            SoundEngine.PlaySound(itemSound, Player.Center);
                        }
                        if (Main.mouseRight && hermitPurpleShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<HermitPurpleGrab>()] == 0)
                        {
                            hermitPurpleShootCooldown += 60 - standSpeedBoosts;
                            Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                            shootVelocity.Normalize();
                            shootVelocity *= 8f;
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleGrab>(), (int)(191 * standDamageBoosts), 0f, Player.whoAmI);
                        }
                    }
                    if (Player.controlHook && Player.miscEquips[4].IsAir)       //Player.miscEquips[4] is the hook slot
                    {
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();
                        shootVelocity *= 12f;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<HermitPurpleHook>(), 0, 0f, Player.whoAmI);
                    }
                }
            }
            if (creamTier != 0)        //Cream stuff
            {
                VoidBar.Visible = true;
                if (creamTier == 1)
                {
                    voidCounterMax = 4;
                }
                if (creamTier > 1)
                {
                    voidCounterMax = (creamTier - 1) * 4;
                }
                if (voidCounter < voidCounterMax)
                {
                    if (!creamVoidMode && !creamExposedMode && !creamDash)
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
                if (!standOut)
                {
                    creamTier = 0;
                    voidCounter = 0;
                }
            }
            else
            {
                VoidBar.Visible = false;
            }

            if (badCompanyTier != 0)
            {
                if (standType != 2)
                    standType = 2;
                if (badCompanyUIClickTimer > 0)
                    badCompanyUIClickTimer--;

                bool specialPressed = false;
                if (!Main.dedServ)
                    specialPressed = JoJoStands.SpecialHotKey.JustPressed;

                if (specialPressed && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    if (badCompanyTier == 3)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Vector2 position = Player.Center + new Vector2(((Main.screenWidth / 2f) + Main.rand.Next(0, 100 + 1)) * -Player.direction, -Main.screenHeight / 3f);
                            Vector2 velocity = new Vector2(6f * Player.direction, 0f);
                            Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BadCompanyBomberPlane>(), 0, 10f, Player.whoAmI, badCompanyTier);
                        }
                        Player.AddBuff(ModContent.BuffType<Reinforcements>(), 120 * 60);
                        Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(5 * 60));       //6 minutes
                    }
                    if (badCompanyTier == 4)
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            Vector2 position = Player.Center + new Vector2(((Main.screenWidth / 2f) + Main.rand.Next(0, 100 + 1)) * -Player.direction, -Main.screenHeight / 3f);
                            Vector2 velocity = new Vector2(6f * Player.direction, 0f);
                            Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BadCompanyBomberPlane>(), 0, 10f, Player.whoAmI, badCompanyTier);
                        }
                        Player.AddBuff(ModContent.BuffType<Reinforcements>(), 180 * 60);
                        Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(3 * 60));       //5 minutes
                    }
                }

                if (badCompanyDefaultArmy)
                    badCompanySoldiers = maxBadCompanyUnits;

                bool recalculateArmy = false;
                int amountOfSoldiers = Player.ownedProjectileCounts[ModContent.ProjectileType<BadCompanySoldier>()];
                int amountOfTanks = Player.ownedProjectileCounts[ModContent.ProjectileType<BadCompanyTank>()];
                int amountOfChoppers = Player.ownedProjectileCounts[ModContent.ProjectileType<BadCompanyChopper>()];

                int unitsLeft = maxBadCompanyUnits - (amountOfSoldiers + (amountOfTanks * 4) + (amountOfChoppers * 6));
                int expectedUnitsLeft = maxBadCompanyUnits - (badCompanySoldiers + (badCompanyTanks * 4) + (badCompanyChoppers * 6));
                if (unitsLeft != expectedUnitsLeft)
                    recalculateArmy = true;
                badCompanyUnitsLeft = expectedUnitsLeft;

                int troopMult = 1;
                if (Player.HasBuff(ModContent.BuffType<Reinforcements>()))
                {
                    troopMult = 2;
                    recalculateArmy = true;
                }
                if (recalculateArmy)
                {
                    if (amountOfSoldiers < badCompanySoldiers * troopMult)     //Adding troops
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.velocity, ModContent.ProjectileType<BadCompanySoldier>(), 0, 0f, Player.whoAmI, badCompanyTier);
                    }
                    if (amountOfTanks < badCompanyTanks)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.velocity, ModContent.ProjectileType<BadCompanyTank>(), 0, 0f, Player.whoAmI, badCompanyTier);
                    }
                    if (amountOfChoppers < badCompanyChoppers)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Player.velocity, ModContent.ProjectileType<BadCompanyChopper>(), 0, 0f, Player.whoAmI, badCompanyTier);
                    }

                    //Removing troops
                    if (amountOfSoldiers > badCompanySoldiers * troopMult)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile Projectile = Main.projectile[p];
                            if (Projectile.active && Projectile.type == ModContent.ProjectileType<BadCompanySoldier>() && Projectile.owner == Main.myPlayer)
                            {
                                Projectile.Kill();
                                break;
                            }
                        }
                    }
                    if (amountOfTanks > badCompanyTanks)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile Projectile = Main.projectile[p];
                            if (Projectile.active && Projectile.type == ModContent.ProjectileType<BadCompanyTank>() && Projectile.owner == Main.myPlayer)
                            {
                                Projectile.Kill();
                                break;
                            }
                        }
                    }
                    if (amountOfChoppers > badCompanyChoppers)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile Projectile = Main.projectile[p];
                            if (Projectile.active && Projectile.type == ModContent.ProjectileType<BadCompanyChopper>() && Projectile.owner == Main.myPlayer)
                            {
                                Projectile.Kill();
                                break;
                            }
                        }
                    }
                }
                Player.AddBuff(ModContent.BuffType<BadCompanyActiveBuff>(), 2);
                if (Main.mouseRight && badCompanyUIClickTimer <= 0 && Player.whoAmI == Main.myPlayer)
                {
                    badCompanyUIClickTimer = 30;
                    badCompanyDefaultArmy = false;
                    BadCompanyUnitsUI.Visible = !BadCompanyUnitsUI.Visible;
                }
            }

            if (silverChariotShirtless)
            {
                standDamageBoosts += 0.08f;
                standSpeedBoosts += 2;
                standCritChangeBoosts -= 10f;
            }

            if (revived && !Player.HasBuff(ModContent.BuffType<ArtificialSoul>()))
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + "'s artificial soul has left him."), Player.statLife + 1, Player.direction);
                revived = false;
            }
        }

        public override void PostUpdate()
        {
            if (Player.whoAmI == Main.myPlayer && RespawnWithStandOut && standRespawnQueued && !Player.dead)
            {
                standOut = true;
                standRespawnQueued = false;
                SpawnStand();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendStandOut(256, Player.whoAmI, true, Player.whoAmI);      //we send it to 256 cause it's the server
            }
            if (hotbarLocked && standOut && !standAutoMode)
                Player.selectedItem = 0;
        }

        private void UpdateShaderStates()
        {
            if (!Main.dedServ)      //if (this isn't the (dedicated server?)) cause shaders don't exist serverside
            {
                if (TimestopEffects && timestopEffectDurationTimer > 0)
                {
                    timestopEffectDurationTimer--;
                    JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.TimestopEffect, timestopEffectDurationTimer >= 15 && !JoJoStandsShaders.ShaderActive(JoJoStandsShaders.TimestopGreyscaleEffect));
                    JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.TimestopGreyscaleEffect, timestopEffectDurationTimer < 15);
                }

                if (!timestopActive)
                {
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopEffect);
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopGreyscaleEffect);
                }

                if (backToZeroActive)
                {
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopGreyscaleEffect);
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopEffect);
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.EpitaphRedEffect);
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.BtZGreenEffect);
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestkipEffect);

                    if (Player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                        Player.ClearBuff(ModContent.BuffType<TheWorldBuff>());
                    if (Player.HasBuff(ModContent.BuffType<SkippingTime>()))
                        Player.ClearBuff(ModContent.BuffType<SkippingTime>());
                    if (Player.HasBuff(ModContent.BuffType<ForesightBuff>()))
                        Player.ClearBuff(ModContent.BuffType<ForesightBuff>());

                    timestopActive = false;     //second, get rid of the effects from everyone
                    timeskipActive = false;
                    timeskipPreEffect = false;
                    epitaphForesightActive = false;
                }
                JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.BtZGreenEffect, backToZeroActive);
                JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.EpitaphRedEffect, epitaphForesightActive);

                if (BiteTheDustEffects)
                {
                    JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.BiteTheDustEffect, bitesTheDustActive);
                    JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.BiteTheDustEffect, biteTheDustEffectProgress);
                }

                if (TimeskipEffects)
                {
                    if (timeskipActive && timeSkipEffectTransitionTimer < 40)
                    {
                        timeSkipEffectTransitionTimer++;
                        JoJoStandsShaders.ActivateShader(JoJoStandsShaders.TimestkipEffect);
                        JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.TimestkipEffect, (float)timeSkipEffectTransitionTimer / 40f);
                    }
                    if (!timeskipActive && timeSkipEffectTransitionTimer > 0)
                    {
                        timeSkipEffectTransitionTimer--;
                        JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.TimestkipEffect, (float)timeSkipEffectTransitionTimer / 40f);
                        if (timeSkipEffectTransitionTimer <= 0)
                            JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestkipEffect);
                    }
                }
                JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.GratefulDeadGasEffect, gratefulDeadGasActive);

                if (ColorChangeEffects)
                {
                    if (JoJoStandsWorld.VampiricNight && !JoJoStandsShaders.ShaderActive(JoJoStandsShaders.BattlePaletteSwitchEffect))
                    {
                        Filters.Scene.Activate(JoJoStandsShaders.BattlePaletteSwitchEffect);
                        JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.BattlePaletteSwitchEffect, (int)ColorChangeStyle.NormalToLightGreen);
                    }
                }

                if (!JoJoStandsWorld.VampiricNight && JoJoStandsShaders.ShaderActive(JoJoStandsShaders.BattlePaletteSwitchEffect) || (JoJoStandsShaders.ShaderActive(JoJoStandsShaders.BattlePaletteSwitchEffect) && !ColorChangeEffects))
                    Filters.Scene[JoJoStandsShaders.BattlePaletteSwitchEffect].Deactivate();
            }
        }

        public override void PreUpdateMovement()
        {
            if (timestopActive && !Player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                Player.velocity = Vector2.Zero;
        }

        public override void PostUpdateMiscEffects()
        {
            if (usedEctoPearl)
                standRangeBoosts += 64f;

            if (standOut)
                Player.statDefense += standDefenseToAdd;

            if (StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT2>())
                Player.endurance = 0.7f;
        }

        public void SpawnStand()
        {
            Item inputItem = StandSlot.SlotItem;
            if (standChangingLocked)
                return;

            if (inputItem.IsAir)
            {
                if (!JoJoStands.FanStandsLoaded)
                {
                    standOut = false;
                    Main.NewText("There is no Stand in the Stand Slot!", Color.Red);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        ModNetHandler.playerSync.SendStandOut(256, Player.whoAmI, false, Player.whoAmI);
                    return;
                }
            }

            if (Player.maxMinions - Player.slotsMinions < 1)
            {
                Main.NewText("There are no available minion slots!", Color.Red);
                standOut = false;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    ModNetHandler.playerSync.SendStandOut(256, Player.whoAmI, false, Player.whoAmI);
                return;
            }

            if (!(inputItem.ModItem is StandItemClass))
            {
                Main.NewText("Something went wrong while summoning the Stand.", Color.Red);
                return;
            }

            StandItemClass standItem = inputItem.ModItem as StandItemClass;

            standOut = true;
            standTier = standItem.standTier;
            standDefenseToAdd = 4 + (2 * standItem.standTier);
            standName = standItem.standProjectileName;
            if (standItem.standType == 2)
                standDefenseToAdd /= 2;

            if (!standItem.ManualStandSpawning(Player))
            {
                string standClassName = standItem.standProjectileName + "StandT" + standItem.standTier;
                if (standClassName.Contains("T4"))
                    standClassName = standItem.standProjectileName + "StandFinal";

                int standProjectileType = Mod.Find<ModProjectile>(standClassName).Type;

                Projectile.NewProjectile(inputItem.GetSource_FromThis(), Player.position, Player.velocity, standProjectileType, 0, 0f, Main.myPlayer);

                if (JoJoStands.SoundsLoaded)
                {
                    SoundStyle summonSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/BasicSummon_" + Main.rand.Next(1, 4 + 1));
                    summonSound.Volume = 0.25f;
                    summonSound.Pitch = 0f;
                    summonSound.PitchVariance = 0.1f;
                    SoundEngine.PlaySound(summonSound, Player.Center);
                }
            }
        }

        public override void PreUpdateBuffs()
        {
            if (timestopActive && !Player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
            {
                Player.AddBuff(ModContent.BuffType<FrozeninTime>(), 2);
            }
            if (deathLoopActive)       //if someone has deathloop turned on and you don't turn it on for you
            {
                Player.AddBuff(ModContent.BuffType<DeathLoop>(), 2);
            }
            if (epitaphForesightActive && !Player.HasBuff(ModContent.BuffType<ForesightBuff>()) && !Player.HasBuff(ModContent.BuffType<ForeseenDebuff>()))
            {
                Player.AddBuff(ModContent.BuffType<ForeseenDebuff>(), 2);
            }
        }

        public override void PostUpdateBuffs()
        {
            standCooldownReduction = 0f;        //it's here because it resets before the buffs can use it when its in ResetEffects()
            if (Player.HasBuff(ModContent.BuffType<Stolen>()))
                standOut = false;
            if (!Player.HasBuff(ModContent.BuffType<SphericalVoid>()))
                Main.mapEnabled = true;
        }

        public int AbilityCooldownTime(int seconds) //Sometimes we won't want to reduce the cooldown so that's why reduction defaults to 0
        {
            int timeToReturn;
            if (standCooldownReduction >= 0.5f)
                standCooldownReduction = 0.5f;
            timeToReturn = (int)((seconds * 60f) * (1f - standCooldownReduction));
            return timeToReturn;
        }

        public override void OnHitNPC(Item Item, NPC target, int damage, float knockback, bool crit)        //already only runs for melee weapons
        {
            if (wearingTitaniumMask && shadowDodgeCooldownTimer <= 0)
            {
                Player.AddBuff(BuffID.ShadowDodge, 30 * 60);
                shadowDodgeCooldownTimer += 90 * 60;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (wearingTitaniumMask && shadowDodgeCooldownTimer <= 0)
            {
                Player.AddBuff(BuffID.ShadowDodge, 30 * 60);
                shadowDodgeCooldownTimer += 90 * 60;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (silverChariotShirtless || Player.ownedProjectileCounts[ModContent.ProjectileType<SilverChariotAfterImage>()] > 0 || Player.HasBuff<Exposing>())
                damage *= 2;

            if (stoneFreeWeaveAbilityActive)
                damage = (int)(damage * 0.93f);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (stoneFreeWeaveAbilityActive)
                damage = (int)(damage * 0.8f);
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            HamonPlayer hPlayer = Player.GetModPlayer<HamonPlayer>();
            if (crystalArmorSetEquipped)
            {
                Vector2 shootVel = Player.Center + new Vector2(0f, -8f);
                shootVel.Normalize();
                shootVel *= 10f;
                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(80);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                    int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed, ModContent.ProjectileType<CrystalShard>(), 15, 2f, Player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                }
            }
            if (hermitPurpleTier >= 2)
            {
                int reflectedDamage = (int)(npc.damage * (0.05f * hermitPurpleTier));
                npc.StrikeNPC(reflectedDamage, 3f * hermitPurpleTier, npc.direction);
                if (hPlayer.amountOfHamon >= 30)
                {
                    npc.AddBuff(ModContent.BuffType<Sunburn>(), ((hPlayer.amountOfHamon / 30) * hermitPurpleTier) * 60);
                    hPlayer.amountOfHamon -= 2;
                }
            }
            if (hermitPurpleHamonBurstLeft > 0)
            {
                int reflectedDamage = npc.damage * 4;       //This is becaues npc damage is pretty weak against the NPC itself
                npc.StrikeNPC(reflectedDamage, 14f, npc.direction);
                npc.AddBuff(ModContent.BuffType<Sunburn>(), (10 * (hermitPurpleTier - 2)) * 60);

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
            if (Player.HasBuff<ZipperDodge>())
            {
                if (JoJoStands.SoundsLoaded)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/Zip"));
                Player.ClearBuff(ModContent.BuffType<ZipperDodge>());
                Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(10 - (2 * (standTier - 2))));
            }
            playerJustHit = true;
            playerJustHitTime = 2;
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            playerJustHit = true;
            playerJustHitTime = 2;
            if (Player.HasBuff<ZipperDodge>())
            {
                if (JoJoStands.SoundsLoaded)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/Zip"));
                Player.ClearBuff(ModContent.BuffType<ZipperDodge>());
                Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(10 - (2 * (standTier - 2))));
            }
        }

        public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        {
            if (StandPvPMode && Main.netMode != NetmodeID.SinglePlayer && target.GetModPlayer<MyPlayer>().standOut)
                damage /= 2;
        }

        /*public override bool CustomBiomesMatch(Player other)
        {
            MyPlayer modOther = other.GetModPlayer<MyPlayer>());
            return ZoneViralMeteorite == modOther.ZoneViralMeteorite;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            MyPlayer modOther = other.GetModPlayer<MyPlayer>());
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
        }*/

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] > 0)
                return false;
            return true;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)       //that 1 last frame before you completely die
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                if (DeathSoundID == DeathSoundType.Roundabout)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/ToBeContinued"));
                else if (DeathSoundID == DeathSoundType.Caesar)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/Caesar"));
                else if (DeathSoundID == DeathSoundType.KonoMeAmareriMaroreriMerareMaro)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/GangTortureDance"));
                else if (DeathSoundID == DeathSoundType.LastTrainHome)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/LastTrainHome"));
                else if (DeathSoundID == DeathSoundType.KingCrimsonNoNorioKu)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/KingCrimsonSpeech"));

                if (DeathSoundID != DeathSoundType.Roundabout)
                    ToBeContinued.Visible = true;
            }

            if (Player.HasBuff(ModContent.BuffType<Pierced>()))
            {
                receivedArrowShard = false;
                piercedTimer = 36000;
            }

            if (!revived && Player.HasItem(ModContent.ItemType<PokerChip>()))
            {
                revived = true;
                Player.AddBuff(ModContent.BuffType<ArtificialSoul>(), 3600);
                Player.ConsumeItem(ModContent.ItemType<PokerChip>(), true);
                Main.NewText("The chip has given you new life!");
                return false;
            }
            if (backToZeroActive)
            {
                return false;
            }


            standOut = false;
            revived = false;
            standChangingLocked = false;
            standRespawnQueued = true;
            forceShutDownEffect = true;
            return true;
        }

        public override void UpdateDead()
        {
            standOut = false;
            standAccessory = false;
            standRemoteMode = false;
            ableToOverrideTimestop = false;
            poseSoundName = "";
            standName = "";
            standType = 0;
            standTier = 0;
            standDefenseToAdd = 0;
            sexPistolsTier = 0;
            hermitPurpleTier = 0;
            stoneFreeWeaveAbilityActive = false;

            creamTier = 0;
            voidCounter = 0;
            creamNormalToExposed = false;
            creamNormalToVoid = false;
            creamExposedToVoid = false;
            creamDash = false;
            creamFrame = 0;

            if (Player.whoAmI == Main.myPlayer && DeathSoundID == DeathSoundType.Roundabout)
            {
                tbcCounter++;
                if (tbcCounter >= 270)
                    ToBeContinued.Visible = true;
            }
        }

        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            MyPlayer mPlayer = Player.GetModPlayer<MyPlayer>();
            if (mPlayer.hideAllPlayerLayers)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    layer.Hide();
                }
            }
        }
    }
}
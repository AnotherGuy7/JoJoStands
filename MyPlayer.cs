using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.DataStructures;
using JoJoStands.Items;
using JoJoStands.Items.Dyes;
using JoJoStands.Items.Hamon;
using JoJoStands.Mounts;
using JoJoStands.Networking;
using JoJoStands.NPCs;
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
        public int goldenSpinCounter = 0;
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
        /// <summary>
        /// Boosts Stand Damage. Stand damage is multiplied by this value.
        /// </summary>
        public float standDamageBoosts = 1;
        /// <summary>
        /// Boosts Stand Range. Stand Range is added on to by this value.
        /// </summary>
        public float standRangeBoosts = 0f;
        /// <summary>
        /// Boosts Stand Crit chanches. Stand Crit Chances are added on to by this value.
        /// </summary>
        public float standCritChangeBoosts = 0f;
        /// <summary>
        /// Boosts Stand Dodge Changes. Stand Dodge Changes are added on to by this value.
        /// </summary>
        public float standDodgeChance = 5f;
        public int standDodgeGuarantee = 0;
        /// <summary>
        /// Boosts Stand Attack Speeds. Stand Speeds are reduced by this value. Lower is better.
        /// </summary>
        public int standSpeedBoosts = 0;
        public int standAccessoryDefense = 0;
        public int standArmorPenetration = 0;
        public float standCooldownReduction = 0f;
        public int standType = 0;           //0 = no type; 1 = Melee; 2 = Ranged;
        public int standTier = 0;
        public int piercedTimer = 36000;
        public int hermitPurpleShootCooldown = 0;
        public int hermitPurpleSpecialFrameCounter = 0;
        public int hermitPurpleHamonBurstLeft = 0;
        public int creamTier = 0;
        public bool creamReturnBack = false;
        public int voidCounter = 0;
        public int voidCounterMax = 0;
        public int voidTimer = 0;
        public int creamFrame = 0;
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
        public int deathLoopTarget = -1;
        public StandControlStyle standControlStyle;
        public int softAndWetBubbleRotation;
        public int standDesummonTimer = 0;

        public int crazyDiamonUncraftCooldown = 0;

        public bool wearingEpitaph = false;
        public bool wearingTitaniumMask = false;
        public bool achievedInfiniteSpin = false;
        public bool revivedByPokerChip = false;
        public bool standOut = false;
        public bool chlorositeShortEqquipped = false;
        public bool crystalArmorSetEquipped = false;
        public bool crackedPearlEquipped = false;
        public bool heartHeadbandEquipped = false;
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
        public bool collideY = false;

        public bool immuneToTimestopEffects = false;
        public bool timestopActive;
        public bool timestopOwner;
        public bool timeskipActive;
        public bool backToZeroActive;
        public bool epitaphForesightActive;
        public bool standAccessory = false;
        public bool bitesTheDustActive = false;
        public bool posing = false;
        public bool canRevertFromKQBTD = false;
        public bool canRevertFromGoBeyond = false;

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
        public DrawData standDesummonTextureData;
        public List<DestroyedTileData> crazyDiamondDestroyedTileData = new List<DestroyedTileData>();
        public static SoundStyle SummonSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/BasicSummon_")
        {
            Volume = 0.25f,
            PitchVariance = 0.1f,
            Variants = new int[4] { 1, 2, 3, 4 }
        };
        public List<TokenData> polaroidTokenData = new List<TokenData>();

        public string standName = "";
        public string poseSoundName = "";       //This is for JoJoStandsSounds
        public string dyePathAddition = "";     //This is for Stand texture finding
        public int standHitTime = 0;
        public StandTextureDye currentTextureDye;

        private int amountOfSexPistolsPlaced = 0;
        private int sexPistolsClickTimer = 0;
        private bool changingSexPistolsPositions = false;


        public int echoesTier = 0;
        public int currentEchoesAct = 0;
        public int echoesFreeze = 0;
        public int echoesSoundIntensity = 2;
        public int echoesSoundIntensityMax = 48;
        public int echoesKaboom3 = 0;
        public int echoesTailTip = -1;
        public int echoesACT2EvolutionProgress = 0;
        public int echoesACT3EvolutionProgress = 0;
        public float echoesDamageBoost = 1f;
        public bool echoesBoing = false;
        public bool echoesBoing2 = false;
        public bool echoesKaboom = false;
        public bool echoesKaboom2 = false;

        public bool crazyDiamondRestorationMode = false;
        public int crazyDiamondMessageCooldown = 0;
        public int crazyDiamondStonePunch = 0;
        public int crazyDiamondTileDestruction = 0;

        public float towerOfGrayDamageMult = 1f;

        public int standTarget = -1;

        public bool siliconLifeformCarapace = false;
        public bool manifestedWillEmblem = false;
        public bool fightingSpiritEmblem = false;
        public int fightingSpiritEmblemStack = 0;
        public bool arrowEarringEquipped = false;
        public bool vampiricBangleEquipped = false;
        public int arrowEarringCooldown = 0;
        public bool firstNapkinEquipped = false;
        public bool herbalTeaBag = false;
        public int herbalTeaBagCount = 10;
        public int herbalTeaBagCooldown = 0;
        public bool zippedHandEquipped = false;
        public bool zippedHandDeath = false;
        public bool familyPhotoEquipped = false;
        public int familyPhotoEffectTimer = 0;
        public bool underbossPhoneEquipped = false;
        public int underbossPhoneCount = 0;
        public bool sealedPokerDeckEquipped = false;
        public int sealedPokerDeckCooldown = 0;
        public bool requiemArrow = false;
        public bool overHeaven = false; // haha (C) Proos :)
        public bool iceCreamEquipped = false;
        public int iceCreamEnemyHitCount = 0;
        public bool polaroidEquipped = false;
        public int polaroidTokens = 0;
        public int polaroidTokenGainTimer = 0;
        public int[] polaroidTokenAnimationTimers = new int[5];
        public Vector2[] polaroidTokenPositionOffsets = new Vector2[5]; 
        public bool stickyHandEquipped = false;
        public int stickyHandHealthLossTimer = 0;
        public bool diedWithStickyHand = false;
        public bool viralPearlEarringEquipped = false;
        public bool centuryBoyActive = false;

        private bool notDeadYet = false;

        private int echoesBoingUpd = 0;
        private int echoesDamageTimer1 = 60; //3 Freeze
        public int echoesSmackDamageTimer = 120; //ACT 1 sounds
        private int remoteDodge = 0;

        public int standFistsType = 0;
        public int kingCrimsonBuffIndex = -1;

        public enum StandSearchTypeEnum
        {
            Bosses,
            Closest,
            Farthest,
            LeastHealth,
            MostHealth
        }

        public enum StandControlStyle
        {
            Manual,
            Auto,
            Remote
        }

        public enum StandTextureDye
        {
            None,
            Salad,
            Part4
        }

        public struct TokenData
        {
            public int frameIndex;
            public Vector2 offset;

            public TokenData(Vector2 offset)
            {
                frameIndex = Main.rand.Next(0, 2 + 1);
                this.offset = offset;
            }

            public TokenData(int frameIndex, Vector2 offset)
            {
                this.frameIndex = frameIndex;
                this.offset = offset;
            }
        }

        public void ItemBreak(Item item, int maxRarity)
        {
            if (item.rare >= maxRarity)
                return;

            if (crazyDiamonUncraftCooldown <= 0)
            {
                List<Recipe> howManyRecipesHere = new List<Recipe>();
                for (int r1 = 0; r1 < Main.recipe.Length; r1++)
                {
                    if (Main.recipe[r1] != null && Main.recipe[r1].createItem.type == item.type && item.stack >= Main.recipe[r1].createItem.stack)
                    {
                        bool skipRecipe = false;
                        int[] requireItemTypes = new int[Main.recipe[r1].requiredItem.Count];
                        for (int i = 0; i < Main.recipe[r1].requiredItem.Count; i++)     //Checks to see if there's a cyclical crafting cycle here
                        {
                            requireItemTypes[i] = Main.recipe[r1].requiredItem[i].type;
                        }
                        for (int i = 0; i < requireItemTypes.Length; i++)
                        {
                            for (int r2 = 0; r2 < Main.recipe.Length; r2++)
                            {
                                if (Main.recipe[r2] != null && Main.recipe[r2].createItem.type == requireItemTypes[i])
                                {
                                    for (int j = 0; j < Main.recipe[r2].requiredItem.Count; j++)
                                    {
                                        if (Main.recipe[r2].requiredItem[j].type == item.type)
                                        {
                                            skipRecipe = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (skipRecipe)
                            continue;

                        if (item.type != ItemID.IronBar && item.type != ItemID.LeadBar)
                        {
                            SoundEngine.PlaySound(SoundID.Grab);
                            howManyRecipesHere.Add(Main.recipe[r1]);
                            for (int i = 0; i < Main.recipe[r1].createItem.stack; i++)
                                Player.ConsumeItem(item.type);
                            Uncraft(howManyRecipesHere[0]);
                            crazyDiamonUncraftCooldown = 5;
                            break;
                        }
                        else if (item.type == ItemID.IronBar)
                        {
                            SoundEngine.PlaySound(SoundID.Grab);
                            Player.ConsumeItem(item.type);
                            Player.QuickSpawnItem(Player.InheritSource(Player), ItemID.IronOre, 3);
                            crazyDiamonUncraftCooldown = 5;
                            break;
                        }
                        else if (item.type == ItemID.LeadBar)
                        {
                            SoundEngine.PlaySound(SoundID.Grab);
                            Player.ConsumeItem(item.type);
                            Player.QuickSpawnItem(Player.InheritSource(Player), ItemID.LeadOre, 3);
                            crazyDiamonUncraftCooldown = 5;
                            break;
                        }
                    }
                }
            }
        }

        private void Uncraft(Recipe recipe)
        {
            recipe.requiredItem.ForEach(SpawnRecipeComponent);
        }

        private void SpawnRecipeComponent(Item item)
        {
            Player.QuickSpawnItem(Player.InheritSource(Player), item.type, item.stack);
        }

        public override void ResetEffects()
        {
            wearingEpitaph = false;
            timestopOwner = false;
            crystalArmorSetEquipped = false;
            wearingTitaniumMask = false;
            heartHeadbandEquipped = false;
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
            standAccessoryDefense = 0;
            standArmorPenetration = 0;
            standCritChangeBoosts = 5f;      //standCooldownReductions is in PostUpdateBuffs cause it gets reset before buffs use it
            standDodgeChance = 0f;

            towerOfGrayDamageMult = 1f;

            siliconLifeformCarapace = false;
            manifestedWillEmblem = false;
            fightingSpiritEmblem = false;
            arrowEarringEquipped = false;
            vampiricBangleEquipped = false;
            firstNapkinEquipped = false;
            herbalTeaBag = false;
            zippedHandEquipped = false;
            familyPhotoEquipped = false;
            underbossPhoneEquipped = false;
            sealedPokerDeckEquipped = false;
            iceCreamEquipped = false;
            polaroidEquipped = false;
            stickyHandEquipped = false;
            viralPearlEarringEquipped = false;
            overHeaven = false;
            collideY = false;
        }


        public override void OnEnterWorld()
        {
            int type = StandSlot.SlotItem.type;
            if (type == ModContent.ItemType<TuskAct3>() || type == ModContent.ItemType<TuskAct4>())
                tuskActNumber = 3;
            else if (type == ModContent.ItemType<TuskAct2>())
                tuskActNumber = 2;
            else if (type == ModContent.ItemType<TuskAct1>())
                tuskActNumber = 1;

            for (int i = 0; i < sexPistolsOffsets.Length; i++)
            {
                sexPistolsOffsets[i] = new Vector2(Main.rand.NextFloat(-40f, 40f + 1f), Main.rand.NextFloat(-40f, 40f + 1f));
            }
            forceShutDownEffect = false;
            ToBeContinued.Visible = false;
            tbcCounter = 0;
            if (StandDyeSlot.SlotItem.ModItem is StandDye)
                (StandDyeSlot.SlotItem.ModItem as StandDye).OnEquipDye(Main.LocalPlayer);
        }

        public override void OnRespawn()
        {
            forceShutDownEffect = false;
            if (Player.whoAmI == Main.myPlayer)
            {
                tbcCounter = 0;
                ToBeContinued.Visible = false;
            }
        }

        public override void PlayerDisconnect()        //runs for everyone that hasn't left
        {
            MyPlayer mPlayer = Player.GetModPlayer<MyPlayer>();
            MyPlayer otherMPlayer = Player.GetModPlayer<MyPlayer>();
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                    {
                        MyPlayer otherModPlayer = otherPlayer.GetModPlayer<MyPlayer>();
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
                    }
                }
            }
            otherMPlayer.crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Restore);
            otherMPlayer.crazyDiamondMessageCooldown = 0;
            otherMPlayer.crazyDiamondDestroyedTileData.Clear();
        }

        public override void ModifyScreenPosition()     //used HERO's Mods FlyCam as a reference for this
        {
            if (standOut && standControlStyle == StandControlStyle.Remote)
                Main.screenPosition = standRemoteModeCameraPosition;

            if (standOut && (creamVoidMode || creamExposedMode))
                Main.screenPosition = VoidCamPosition;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (JoJoStands.StandAutoModeHotKey.JustPressed && standControlStyle == StandControlStyle.Manual && standKeyPressTimer <= 0 && !Player.HasBuff(ModContent.BuffType<Rampage>()))
            {
                standKeyPressTimer += 30;
                standControlStyle = StandControlStyle.Auto;
                Main.NewText("Stand Control: Auto");
                SyncCall.SyncControlStyle(Player.whoAmI, standControlStyle);
            }
            if (JoJoStands.StandAutoModeHotKey.JustPressed && standControlStyle == StandControlStyle.Auto && standKeyPressTimer <= 0 && !Player.HasBuff(ModContent.BuffType<Rampage>()))
            {
                standKeyPressTimer += 30;
                standControlStyle = StandControlStyle.Manual;
                Main.NewText("Stand Control: Manual");
                SyncCall.SyncControlStyle(Player.whoAmI, standControlStyle);
            }
            if (JoJoStands.PoseHotKey.JustPressed && !posing && !Player.HasBuff(ModContent.BuffType<Rampage>()))
            {
                if (JoJoStands.Sounds)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/PoseSound"));
                posing = true;
                SyncCall.SyncPoseState(Player.whoAmI, true);
            }
            if (standChangingLocked)
                return;

            if (JoJoStands.StandOutHotKey.JustPressed && !standOut && standKeyPressTimer <= 0 && !Player.HasBuff(ModContent.BuffType<Stolen>()) && !Player.HasBuff(ModContent.BuffType<Rampage>()))
            {
                SpawnStand();
                if (standControlStyle == StandControlStyle.Remote)
                    standControlStyle = StandControlStyle.Manual;
                standKeyPressTimer += 30;
                SyncCall.SyncStandOut(Player.whoAmI, standOut, standName, standTier);
            }
            if (JoJoStands.SpecialHotKey.Current && standAccessory)
            {
                if (StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT1>() || StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT2>())
                {
                    Player.AddBuff(ModContent.BuffType<CenturyBoyBuff>(), 2, true);
                }
                if (StandSlot.SlotItem.type == ModContent.ItemType<LockT3>() && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float distance = Vector2.Distance(Player.Center, npc.Center);
                        if (npc.active && !npc.townNPC && !npc.immortal && !npc.hide && distance < (98f * 4f))
                        {
                            npc.GetGlobalNPC<JoJoGlobalNPC>().theLockCrit = standCritChangeBoosts;
                            npc.GetGlobalNPC<JoJoGlobalNPC>().theLockDamageBoost = standDamageBoosts;
                            if (npc.boss)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 10);
                                npc.GetGlobalNPC<JoJoGlobalNPC>().lockRegenCounter += 20;
                            }
                            if (!npc.boss && npc.lifeMax > 5)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 30);
                                npc.GetGlobalNPC<JoJoGlobalNPC>().lockRegenCounter += 40;
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
                if (StandSlot.SlotItem.type == ModContent.ItemType<LockFinal>() && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        float distance = Vector2.Distance(Player.Center, npc.Center);
                        if (npc.active && !npc.townNPC && !npc.immortal && !npc.hide && distance < (98f * 4f))
                        {
                            npc.GetGlobalNPC<JoJoGlobalNPC>().theLockCrit = standCritChangeBoosts;
                            npc.GetGlobalNPC<JoJoGlobalNPC>().theLockDamageBoost = standDamageBoosts;
                            if (npc.boss)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 15);
                                npc.GetGlobalNPC<JoJoGlobalNPC>().lockRegenCounter += 40;
                            }
                            if (!npc.boss && npc.lifeMax > 5)
                            {
                                npc.AddBuff(ModContent.BuffType<Locked>(), 60 * 45);
                                npc.GetGlobalNPC<JoJoGlobalNPC>().lockRegenCounter += 80;
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
            if (JoJoStands.StandOutHotKey.JustPressed && standOut && standKeyPressTimer <= 0 && !Player.HasBuff(ModContent.BuffType<Rampage>()))
                ResetStandVariables();
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

                int slotPosX = JoJoStands.StandSlotPositionX * (Main.screenWidth / 100);
                int slotPosY = JoJoStands.StandSlotPositionY * (Main.screenHeight / 100);

                StandSlot.Position = new Vector2(slotPosX, slotPosY);
                StandDyeSlot.Position = new Vector2(slotPosX - 60f, slotPosY);

                StandSlot.Draw(spriteBatch);
                if (StandSlot.SlotItem != null && StandSlot.SlotItem.ModItem != null)
                {
                    Rectangle slotFrame = new Rectangle(slotPosX, slotPosY, (int)StandSlot.Size.X, (int)StandSlot.Size.Y);
                    StandSlot.SlotItem.ModItem.PostDrawInInventory(spriteBatch, StandSlot.Position + (StandSlot.Size / 4f), slotFrame, Color.White, StandSlot.SlotItem.color, Vector2.Zero, Main.inventoryScale);
                }
                StandDyeSlot.Draw(spriteBatch);

                MyPlayer mPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
                if (!mPlayer.timestopActive && !mPlayer.standChangingLocked)
                {
                    if (Main.mouseItem.IsAir || Main.mouseItem.ModItem is StandItemClass)
                        mPlayer.StandSlot.Update();

                    if (Main.mouseItem.IsAir || Main.mouseItem.dye != 0 || Main.mouseItem.ModItem is StandDye)
                    {
                        mPlayer.StandDyeSlot.Update();
                        if (Main.mouseItem.ModItem is StandDye)
                            (Main.mouseItem.ModItem as StandDye).OnEquipDye(Main.LocalPlayer);
                    }
                }

                Main.inventoryScale = origScale;
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

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (standDesummonTimer > 0 && standDesummonTextureData.texture != null)
            {
                if (StandDyeSlot.SlotItem.dye != 0)
                {
                    ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(StandDyeSlot.SlotItem.type);
                    shader.Apply(null);     //has to be on top of whatever it's applying to (I spent hours and hours trying to find a solution and the only problem was that this was under it)
                }

                Vector2 drawPosition = Vector2.Lerp(Player.Center - Main.screenPosition, standDesummonTextureData.position, (standDesummonTimer / 15f));
                Main.spriteBatch.Draw(standDesummonTextureData.texture, drawPosition, standDesummonTextureData.sourceRect, Color.White * (standDesummonTimer / 15f),
                    standDesummonTextureData.rotation, standDesummonTextureData.origin, standDesummonTextureData.scale, standDesummonTextureData.effect, 0f);
            }
            if (Player.HasBuff(ModContent.BuffType<BelieveInMe>()) || Player.HasBuff(ModContent.BuffType<SMACK>()))
            {
                Texture2D texture = null;
                Rectangle animRect = Rectangle.Empty;
                if (Player.HasBuff(ModContent.BuffType<BelieveInMe>()))
                {
                    texture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_BelieveInMe").Value;
                    animRect = new Rectangle(0, 0, 54, 32);
                }
                else if (Player.HasBuff(ModContent.BuffType<SMACK>()))
                {
                    texture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_SMACK").Value;
                    animRect = new Rectangle(0, 0, 56, 30);
                }
                Main.spriteBatch.Draw(texture, Player.Center - new Vector2(0f, Player.height / 2) - Main.screenPosition, animRect, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
            }
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
            if (standControlStyle == StandControlStyle.Remote || Player.HasBuff(ModContent.BuffType<CenturyBoyBuff>()) || stickyFingersAmbushMode || creamVoidMode || creamExposedMode)
            {
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
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            if (!Player.HasBuff(ModContent.BuffType<SkippingTime>()))
                kingCrimsonBuffIndex = -1;
            if (overHeaven && standOut && tuskActNumber != 0 && StandSlot.SlotItem.type == ModContent.ItemType<TuskAct4>())
                goldenSpinCounter = 300;
            if (sealedPokerDeckCooldown > 0)
                sealedPokerDeckCooldown--;
            if (familyPhotoEffectTimer > 0)
                familyPhotoEffectTimer--;
            if (polaroidTokenGainTimer > 0)
                polaroidTokenGainTimer--;
            if (stickyHandEquipped && stickyHandHealthLossTimer > 0)
                stickyHandHealthLossTimer--;
            if ((!revivedByPokerChip && Player.HasItem(ModContent.ItemType<PokerChip>())) || (zippedHandEquipped && !zippedHandDeath) || (stickyHandEquipped && !diedWithStickyHand))
                notDeadYet = true;
            else
                notDeadYet = false;
            if (zippedHandDeath)
            {
                if (!Player.HasBuff(ModContent.BuffType<SwanSong>()))
                {
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " could no longer live."), Player.statLifeMax, 0, false);
                    zippedHandDeath = false;
                }
            }
            if (diedWithStickyHand)
            {
                if (!Player.HasBuff(ModContent.BuffType<SwanSong>()))
                {
                    Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + " could no longer live."), Player.statLifeMax, 0, false);
                    diedWithStickyHand = false;
                }

            }
            if (standOut)
            {
                if (StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT1>() || StandSlot.SlotItem.type == ModContent.ItemType<CenturyBoyT2>())
                    centuryBoyActive = true;
            }
            if (StandSlot.SlotItem.type != ModContent.ItemType<CenturyBoyT1>() && StandSlot.SlotItem.type != ModContent.ItemType<CenturyBoyT2>() && standOut)
                centuryBoyActive = false;
            if (standAccessory)
            {
                Player.slotsMinions += 1;
                if (Player.maxMinions - Player.slotsMinions < 0)
                    ResetStandVariables();
            }
            if (deathLoopTarget != -1)
            {
                if (!Main.npc[deathLoopTarget].active)
                {
                    Player.ClearBuff(ModContent.BuffType<DeathLoop>());
                    Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), 1800);
                    deathLoopTarget = -1;
                }
            }
            if (deathLoopTarget == -1)
                Player.ClearBuff(ModContent.BuffType<DeathLoop>());
            if (herbalTeaBagCount < 10)
            {
                herbalTeaBagCooldown++;
                if (herbalTeaBagCooldown >= 60)
                {
                    herbalTeaBagCooldown = 0;
                    herbalTeaBagCount += 1;
                }
            }
            if (arrowEarringCooldown > 0)
                arrowEarringCooldown--;
            if (!fightingSpiritEmblem)
                fightingSpiritEmblemStack = 0;
            if (Player.HasBuff(ModContent.BuffType<Dodge>()))
                standDodgeGuarantee = 1;
            if (standControlStyle == StandControlStyle.Remote)
                remoteDodge = 2;
            if (standControlStyle == StandControlStyle.Remote && remoteDodge > 0)
                remoteDodge--;
            if (crazyDiamonUncraftCooldown > 0)
                crazyDiamonUncraftCooldown--;
            if (standKeyPressTimer > 0)
                standKeyPressTimer--;
            if (standDesummonTimer > 0)
                standDesummonTimer--;
            if (revertTimer > 0)
                revertTimer--;

            UpdateShaderStates();
            if (standControlStyle == StandControlStyle.Remote && standName == "Aerosmith")
                AerosmithRadar.Visible = StandSlot.SlotItem.type == ModContent.ItemType<AerosmithT3>() || StandSlot.SlotItem.type == ModContent.ItemType<AerosmithFinal>();
            else
                AerosmithRadar.Visible = false;

            if (posing)
            {
                poseDuration--;
                if (poseDuration <= 0 || Player.velocity != Vector2.Zero || Main.mouseLeft || Main.mouseRight)
                {
                    menacingFrames = 0;
                    if (poseDuration <= 0)
                        Player.AddBuff(ModContent.BuffType<StrongWill>(), 30 * 60);

                    posing = false;
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
            if (backToZeroActive)
            {
                timestopActive = false;
                timeskipActive = false;
                epitaphForesightActive = false;

                if (Player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                    Player.ClearBuff(ModContent.BuffType<TheWorldBuff>());
                if (Player.HasBuff(ModContent.BuffType<SkippingTime>()))
                    Player.ClearBuff(ModContent.BuffType<SkippingTime>());
                if (Player.HasBuff(ModContent.BuffType<ForesightBuff>()))
                    Player.ClearBuff(ModContent.BuffType<ForesightBuff>());
            }
            if (PlayerInput.Triggers.Current.SmartSelect || Player.dead)
                canStandBasicAttack = false;
            if (hotbarLocked && standOut && standControlStyle == StandControlStyle.Manual)
                Player.selectedItem = 0;
            if (playerJustHitTime > 0)
            {
                playerJustHitTime--;
                if (playerJustHitTime <= 0)
                    playerJustHit = false;
            }

            if (goldenSpinCounter > 0)          //golden spin stuff
            {
                if (!overHeaven)
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
                    }
                    if (goldenSpinCounter <= 1)     //would reset anyway if the Player isn't holding Tusk, cause it resets whenever you hold the Item again
                        forceChangedTusk = false;
                }

                if (goldenSpinCounter >= 300)
                {
                    goldenSpinCounter = 300;
                    achievedInfiniteSpin = true;
                }
            }

            if (!standOut)
            {
                crazyDiamondRestorationMode = false;
                crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Restore);
                crazyDiamondMessageCooldown = 0;
                crazyDiamondDestroyedTileData.Clear();

                echoesTier = 0;
                currentEchoesAct = 0;
            }

            if (crazyDiamondMessageCooldown > 0)
                crazyDiamondMessageCooldown--;

            if (Player == Main.player[Main.myPlayer])
            {
                if (Main.mouseLeft && standControlStyle == StandControlStyle.Manual)
                {
                    if (crazyDiamondTileDestruction < 60)
                        crazyDiamondTileDestruction++;
                }
                else
                    crazyDiamondTileDestruction = 0;
            }

            if (crazyDiamondStonePunch > 2)
                Player.AddBuff(ModContent.BuffType<ImproperRestoration>(), 180);
            if (Player.HasBuff(ModContent.BuffType<ImproperRestoration>()))
                crazyDiamondStonePunch = 0;

            if (echoesKaboom) //echoes act 2 stuff 
            {
                Player.noFallDmg = false;
                if (!echoesKaboom2)
                {
                    Player.fallStart -= 100;
                    echoesKaboom2 = true;
                    echoesKaboom3 = 4;
                }
            }

            if (echoesBoing2)
                Player.noFallDmg = true;

            if (collideY || Player.velocity.Y == 0f)
                echoesBoingUpd++;

            if (echoesBoingUpd >= 4)
            {
                echoesBoingUpd = 0;
                echoesBoing2 = false;
                echoesBoing = false;
                echoesKaboom = false;
                echoesKaboom2 = false;
            }


            if (echoesFreeze > 0) // echoes act 3 stuff
            {
                if (Player.velocity.Y != 0f)
                    Player.velocity.Y += 12f;
                Player.noFallDmg = false;
                Player.velocity.X *= 0.66f;
                if (collideY)
                {
                    if (echoesDamageTimer1 > 0)
                        echoesDamageTimer1--;
                    if (echoesDamageTimer1 == 0)
                    {
                        echoesDamageTimer1 = 60;
                        int freezeDamage = (int)(136 * echoesDamageBoost) / 2;
                        Player.Hurt(PlayerDeathReason.ByCustomReason(Player.name + " could no longer live."), (int)Main.rand.NextFloat((int)(freezeDamage * 0.85f), (int)(freezeDamage * 1.15f)) + Player.statDefense, 0, true);
                    }
                }
                echoesFreeze--;
            }
            else
                echoesDamageTimer1 = 60;

            if (sexPistolsTier != 0)        //Sex Pistols stuff
            {
                if (standControlStyle == StandControlStyle.Auto)
                    SexPistolsUI.Visible = true;
                if (standControlStyle == StandControlStyle.Manual)
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
                            if (JoJoStands.SoundsLoaded)
                            {
                                int soundNumber = amountOfSexPistolsPlaced;
                                if (soundNumber >= 4)
                                    soundNumber += 1;
                                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/SexPistols_" + soundNumber));
                            }
                            if (amountOfSexPistolsPlaced >= 6)
                                amountOfSexPistolsPlaced = 0;
                            SyncCall.SyncSexPistolPosition(Player.whoAmI, amountOfSexPistolsPlaced, sexPistolsOffsets[amountOfSexPistolsPlaced]);
                        }
                    }

                    if (secondSpecialPressed && !Player.HasBuff(ModContent.BuffType<BulletKickFrenzy>()) && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && sexPistolsTier >= 3)
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
                        if (Main.MouseWorld.X > Player.position.X)
                            Player.direction = 1;
                        else
                            Player.direction = -1;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        SoundStyle shootSound = SoundID.Item67;
                        shootSound.Volume = 0.33f;
                        SoundEngine.PlaySound(shootSound);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();      //to normalize is to turn it into a direction under 1 but greater than 0
                        shootVelocity *= 12f;       //multiply the angle by the speed to get the effect
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<Nail>(), (int)(21 * standDamageBoosts) + ((22 + equippedTuskAct - 1) * equippedTuskAct - 1), 4f, Player.whoAmI);
                    }
                    if (Main.mouseRight && !Player.channel && tuskShootCooldown <= 0)
                    {
                        tuskShootCooldown = 5;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<NailSlasher>(), (int)(28 * standDamageBoosts) + ((24 + equippedTuskAct - 1) * equippedTuskAct - 1), 0f, Player.whoAmI);
                    }
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<NailSlasher>()] > 0)
                        tuskShootCooldown = 2;
                }
                else if (tuskActNumber == 2)
                {
                    if (Main.mouseLeft && canStandBasicAttack && !Player.channel && tuskShootCooldown <= 0)
                    {
                        if (Main.MouseWorld.X > Player.position.X)
                            Player.direction = 1;
                        else
                            Player.direction = -1;
                        Player.channel = true;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        SoundStyle shootSound = SoundID.Item67;
                        shootSound.Volume = 0.33f;
                        SoundEngine.PlaySound(shootSound);
                        Vector2 shootVelocity = Main.MouseWorld - Player.Center;
                        shootVelocity.Normalize();
                        shootVelocity *= 4f;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, shootVelocity, ModContent.ProjectileType<ControllableNail>(), (int)(49 * standDamageBoosts) + ((22 + equippedTuskAct - 2) * equippedTuskAct - 2), 5f, Player.whoAmI);
                    }
                    if (Main.mouseRight && !Player.channel && tuskShootCooldown <= 0)
                    {
                        tuskShootCooldown = 5;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<NailSlasher>(), (int)(53 * standDamageBoosts) + ((24 + equippedTuskAct - 1) * equippedTuskAct - 1), 0f, Player.whoAmI);
                    }
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<NailSlasher>()] > 0)
                        tuskShootCooldown = 2;
                }
                else if (tuskActNumber == 3)
                {
                    if (Main.mouseLeft && canStandBasicAttack && !Player.channel && tuskShootCooldown <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] <= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<ArmWormholeNail>()] <= 0)
                    {
                        if (Main.MouseWorld.X > Player.position.X)
                            Player.direction = 1;
                        if (Main.MouseWorld.X < Player.position.X)
                            Player.direction = -1;
                        Player.channel = true;
                        tuskShootCooldown += 35 - standSpeedBoosts;
                        SoundStyle shootSound = SoundID.Item67;
                        shootSound.Volume = 0.33f;
                        SoundEngine.PlaySound(shootSound);
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
                else if (tuskActNumber == 4)
                {
                    if (standControlStyle == StandControlStyle.Auto)
                    {
                        if (Main.mouseLeft && canStandBasicAttack && !Player.channel && tuskShootCooldown <= 0)
                        {
                            if (Main.MouseWorld.X > Player.position.X)
                                Player.direction = 1;
                            else
                                Player.direction = -1;
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
                            Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(15));
                        }
                    }
                }
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] > 0)
                    tuskShootCooldown = 30;
            }
            if (standName == "HermitPurple" && standTier != 0 && Player.whoAmI == Main.myPlayer)
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

                HamonPlayer hPlayer = Player.GetModPlayer<HamonPlayer>();
                if (specialJustPressed && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && standTier > 2 && hPlayer.amountOfHamon > 40)
                {
                    if (standTier == 3)
                    {
                        hermitPurpleHamonBurstLeft = 3;
                        Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(30));
                    }
                    else if (standTier == 4)
                    {
                        hermitPurpleHamonBurstLeft = 5;
                        Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(20));
                    }
                    hPlayer.amountOfHamon -= 40;
                }
                if (hermitPurpleShootCooldown > 0)
                    hermitPurpleShootCooldown--;

                if (standControlStyle == StandControlStyle.Manual)
                {
                    if (standTier == 1)
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
                    else if (standTier == 2)
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
                    else if (standTier == 3)
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
                    else if (standTier == 4)
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
            else if (creamTier != 0)        //Cream stuff
            {
                VoidBar.Visible = true;
                int overheavenMod = 1;
                if (mPlayer.overHeaven)
                    overheavenMod = 2;
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
                        voidTimer += 1 * overheavenMod;
                        if (voidTimer >= 120)
                        {
                            voidCounter++;
                            voidTimer = 0;
                        }
                    }
                    if (!creamVoidMode && creamExposedMode)
                    {
                        voidTimer += 1 * overheavenMod;
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
                        if (voidTimer >= 60 * overheavenMod)
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
            else if (standName == "BadCompany" && standTier != 0 && Player.whoAmI == Main.myPlayer)
            {
                if (standType != 2)
                    standType = 2;
                if (badCompanyUIClickTimer > 0)
                    badCompanyUIClickTimer--;

                bool specialPressed = false;
                if (!Main.dedServ)
                    specialPressed = JoJoStands.SpecialHotKey.JustPressed;

                if (specialPressed && !Player.HasBuff(ModContent.BuffType<Reinforcements>()) && !Player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    if (standTier == 3)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Vector2 position = Player.Center + new Vector2(((Main.screenWidth / 2f) + Main.rand.Next(0, 100 + 1)) * -Player.direction, -Main.screenHeight / 3f);
                            Vector2 velocity = new Vector2(6f * Player.direction, 0f);
                            Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BadCompanyBomberPlane>(), 0, 10f, Player.whoAmI, standTier);
                        }
                        Player.AddBuff(ModContent.BuffType<Reinforcements>(), 2 * 60 * 60);
                    }
                    else if (standTier == 4)
                    {
                        for (int i = 0; i < 24; i++)
                        {
                            Vector2 position = Player.Center + new Vector2(((Main.screenWidth / 2f) + Main.rand.Next(0, 100 + 1)) * -Player.direction, -Main.screenHeight / 3f);
                            Vector2 velocity = new Vector2(6f * Player.direction, 0f);
                            Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BadCompanyBomberPlane>(), 0, 10f, Player.whoAmI, standTier);
                        }
                        Player.AddBuff(ModContent.BuffType<Reinforcements>(), 3 * 60 * 60);
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
                    if (standOut)
                    {
                        Vector2 randomOffset = new Vector2(Main.rand.Next(-4 * 16, (4 * 16) + 1), Main.rand.Next(-4 * 16, 0));
                        if (amountOfSoldiers < badCompanySoldiers * troopMult)     //Adding troops
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + randomOffset, Player.velocity, ModContent.ProjectileType<BadCompanySoldier>(), 0, 0f, Player.whoAmI, standTier);
                        if (amountOfTanks < badCompanyTanks)
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + randomOffset, Player.velocity, ModContent.ProjectileType<BadCompanyTank>(), 0, 0f, Player.whoAmI, standTier);
                        if (amountOfChoppers < badCompanyChoppers)
                            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + randomOffset, Player.velocity, ModContent.ProjectileType<BadCompanyChopper>(), 0, 0f, Player.whoAmI, standTier);
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

            if (revivedByPokerChip && !Player.HasBuff(ModContent.BuffType<ArtificialSoul>()))
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(Player.name + "'s artificial soul has left him."), Player.statLife + 1, Player.direction);
                revivedByPokerChip = false;
            }
        }

        public override void PostUpdate()
        {
            if (Player.whoAmI == Main.myPlayer && JoJoStands.RespawnWithStandOut && standRespawnQueued && !Player.dead)
            {
                SpawnStand();
                standRespawnQueued = false;
                SyncCall.SyncStandOut(Player.whoAmI, standOut, standName, standTier);
            }
            if (hotbarLocked && standOut && standControlStyle == StandControlStyle.Manual)
                Player.selectedItem = 0;
        }

        public override void PostUpdateEquips()
        {
            standDamageBoosts = Player.GetDamage(DamageClass.Generic).ApplyTo(standDamageBoosts);
            if (standDodgeChance > 30f)
                standDodgeChance = 30f;
        }

        private void UpdateShaderStates()
        {
            if (!Main.dedServ && Player.whoAmI == Main.myPlayer)      //if (this isn't the (dedicated server?)) cause shaders don't exist serverside
            {
                if (JoJoStands.TimestopEffects && timestopEffectDurationTimer > 0)
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
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestkipEffect);
                }
                JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.BtZGreenEffect, backToZeroActive);
                JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.EpitaphRedEffect, epitaphForesightActive);

                if (JoJoStands.BiteTheDustEffects)
                {
                    JoJoStandsShaders.ChangeShaderActiveState(JoJoStandsShaders.BiteTheDustEffect, bitesTheDustActive);
                    JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.BiteTheDustEffect, biteTheDustEffectProgress);
                }

                if (JoJoStands.TimeskipEffects)
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
            }
        }

        public override void UpdateLifeRegen()
        {
            if (standOut)
            {
                if (siliconLifeformCarapace || (stickyHandEquipped && stickyHandHealthLossTimer > 0))
                {
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                    Player.lifeRegenTime = 0;

                    if (vampiricBangleEquipped)
                        Player.lifeRegen -= 9;
                    else
                        Player.lifeRegen -= 3;
                }
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (siliconLifeformCarapace && standOut)
                regen = 0f;
            if (stickyHandEquipped && stickyHandHealthLossTimer > 0 && standOut)
                regen = 0f;
        }

        public override void PostUpdateMiscEffects()
        {
            if (usedEctoPearl)
                standRangeBoosts += 64f;

            standDefenseToAdd = 4 + (2 * standTier);

            if (standType == 2)
            {
                standDefenseToAdd /= 2;
                standAccessoryDefense /= 2;
            }

            if (siliconLifeformCarapace || stickyHandEquipped)
            {
                standAccessoryDefense *= 2;
                standDefenseToAdd *= 2;
            }

            if (crazyDiamondRestorationMode)
            {
                standDamageBoosts = 1f;
                standSpeedBoosts = 0;
            }

            if (standOut && standControlStyle == StandControlStyle.Manual)
            {
                Player.statDefense += standDefenseToAdd;
                Player.statDefense += standAccessoryDefense;
            }

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
                    return;
                }
            }

            if (Player.maxMinions - Player.slotsMinions < 1)
            {
                Main.NewText("There are no available minion slots!", Color.Red);
                standOut = false;
                return;
            }

            if (!(inputItem.ModItem is StandItemClass))
            {
                Main.NewText("Something went wrong while summoning the Stand.", Color.Red);
                return;
            }

            StandItemClass standItem = inputItem.ModItem as StandItemClass;

            standOut = true;
            standTier = standItem.StandTier;
            standName = standItem.StandProjectileName;

            if (!standItem.ManualStandSpawning(Player))
            {
                string standClassName = standItem.StandProjectileName + "StandT" + standItem.StandTier;
                if (standClassName.Contains("T4"))
                    standClassName = standItem.StandProjectileName + "StandFinal";

                int standProjectileType = Mod.Find<ModProjectile>(standClassName).Type;
                Projectile.NewProjectile(inputItem.GetSource_FromThis(), Player.position, Player.velocity, standProjectileType, 0, 0f, Main.myPlayer);
                if (JoJoStands.SoundsLoaded)
                    SoundEngine.PlaySound(SummonSound, Player.Center);
            }
        }

        public void ResetStandVariables()
        {
            standOut = false;
            standAccessory = false;
            ableToOverrideTimestop = false;
            poseSoundName = "";
            standName = "";
            standType = 0;
            standTier = 0;
            standDefenseToAdd = 0;
            sexPistolsTier = 0;
            stoneFreeWeaveAbilityActive = false;
            hotbarLocked = false;

            crazyDiamondRestorationMode = false;
            crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Restore);
            crazyDiamondMessageCooldown = 0;
            crazyDiamondDestroyedTileData.Clear();

            creamTier = 0;
            voidCounter = 0;
            creamNormalToExposed = false;
            creamNormalToVoid = false;
            creamExposedToVoid = false;
            creamDash = false;
            creamFrame = 0;

            standTier = 0;
            echoesTier = 0;
            stickyFingersAmbushMode = false;

            if (StoneFreeAbilityWheel.Visible)
                StoneFreeAbilityWheel.CloseAbilityWheel();
            if (equippedTuskAct != 0)
            {
                equippedTuskAct = 0;
                tuskActNumber = 0;
            }
            standKeyPressTimer += 30;
            SyncCall.SyncStandOut(Player.whoAmI, false, "", 0);
        }

        public override void PreUpdateBuffs()
        {
            if (timestopActive && !Player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                Player.AddBuff(ModContent.BuffType<FrozeninTime>(), 2);
            if (epitaphForesightActive && !Player.HasBuff(ModContent.BuffType<ForesightBuff>()) && !Player.HasBuff(ModContent.BuffType<ForeseenDebuff>()))
                Player.AddBuff(ModContent.BuffType<ForeseenDebuff>(), 2);
        }

        public override void PostUpdateBuffs()
        {
            standCooldownReduction = 0f;        //it's here because it resets before the buffs can use it when its in ResetEffects()
            if (Player.HasBuff(ModContent.BuffType<Stolen>()))
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

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Player.HasBuff<LockActiveBuff>() && npc.HasBuff<Locked>())
            {
                if (npc.boss)
                    modifiers.FinalDamage *= 1f - (standTier * 0.05f);
                else
                    modifiers.FinalDamage *= 1f - (standTier * 0.1f);
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
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
                    int projIndex = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed, ModContent.ProjectileType<CrystalShard>(), 15, 2f, Player.whoAmI);
                    Main.projectile[projIndex].netUpdate = true;
                }
            }
            if (standName == "HermitPurple" && standTier >= 2)
            {
                int reflectedDamage = (int)(npc.damage * (0.05f * standTier));
                NPC.HitInfo npcHitInfo = new NPC.HitInfo()
                {
                    Damage = reflectedDamage,
                    Knockback = 3f * standTier,
                    HitDirection = npc.direction
                };
                npc.StrikeNPC(npcHitInfo);
                if (hPlayer.amountOfHamon >= 30)
                {
                    npc.AddBuff(ModContent.BuffType<Sunburn>(), ((hPlayer.amountOfHamon / 30) * standTier) * 60);
                    hPlayer.amountOfHamon -= 2;
                }
            }
            if (hermitPurpleHamonBurstLeft > 0)
            {
                int reflectedDamage = npc.damage * 4;       //This is becaues npc damage is pretty weak against the NPC itself
                NPC.HitInfo npcHitInfo = new NPC.HitInfo()
                {
                    Damage = reflectedDamage,
                    Knockback = 14f,
                    HitDirection = npc.direction
                };
                npc.StrikeNPC(npcHitInfo);
                npc.AddBuff(ModContent.BuffType<Sunburn>(), (10 * (standTier - 2)) * 60);

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
            if (!Player.shadowDodge)
                arrowEarringCooldown = 300;
            if (stickyHandEquipped)
                stickyHandHealthLossTimer = 30 * 60;
            playerJustHit = true;
            playerJustHitTime = 2;
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            playerJustHit = true;
            playerJustHitTime = 2;
            if (!Player.shadowDodge)
                arrowEarringCooldown = 300;
            if (stickyHandEquipped)
                stickyHandHealthLossTimer = 30 * 60;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (silverChariotShirtless || Player.ownedProjectileCounts[ModContent.ProjectileType<SilverChariotAfterImage>()] > 0 || Player.HasBuff<Exposing>())
                modifiers.FinalDamage *= 2;
            if (stoneFreeWeaveAbilityActive || Player.HasBuff(ModContent.BuffType<BelieveInMe>()))
                modifiers.FinalDamage *= 0.8f;
            if (Player.HasBuff(ModContent.BuffType<ImproperRestoration>()))
                modifiers.FinalDamage *= 0.1f;
            if (vampiricBangleEquipped)
                modifiers.FinalDamage *= 1.33f;
            if (familyPhotoEffectTimer > 0)
                modifiers.FinalDamage *= 0.66f;
            if (polaroidEquipped)
            {
                if (polaroidTokens > 0)
                {
                    polaroidTokens--;
                    modifiers.FinalDamage *= 0.5f;
                }
                else
                    modifiers.FinalDamage *= 1.4f;
            }
            if (modifiers.PvP)
            {
                if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer && standOut)
                    modifiers.FinalDamage *= 0.5f;
            }
        }

        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<WormholeNail>()] > 0 && Main.rand.NextFloat(1, 100) <= 50 && remoteDodge > 0)
                return true;
            if ((Main.rand.Next(1, 100) <= standDodgeChance || standDodgeGuarantee > 0) && remoteDodge == 0 && standControlStyle == StandControlStyle.Manual && standOut)
            {
                if (standDodgeGuarantee > 0)
                    standDodgeGuarantee--;
                return true;
            }
            if (standControlStyle == StandControlStyle.Manual && standOut && Player.shadowDodge)
                return true;
            if (Player.HasBuff(ModContent.BuffType<SwanSong>()))
                return true;
            if (Player.HasBuff<ZipperDodge>() && Player.whoAmI == Main.myPlayer)
            {
                if (JoJoStands.SoundsLoaded)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/Zip"));
                Player.ClearBuff(ModContent.BuffType<ZipperDodge>());
                Player.AddBuff(ModContent.BuffType<AbilityCooldown>(), AbilityCooldownTime(10 - (2 * (standTier - 2))));
                return true;
            }

            return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)       //that 1 last frame before you completely die
        {
            if (Player.HasBuff(ModContent.BuffType<Pierced>()))
            {
                receivedArrowShard = false;
                piercedTimer = 36000;
            }
            if (!revivedByPokerChip && Player.HasItem(ModContent.ItemType<PokerChip>()))
            {
                revivedByPokerChip = true;
                Player.AddBuff(ModContent.BuffType<ArtificialSoul>(), 3600);
                Player.ConsumeItem(ModContent.ItemType<PokerChip>(), true);
                Main.NewText("The chip has given you new life!");
                return false;
            }
            if (!stickyHandEquipped && zippedHandEquipped && !zippedHandDeath)
            {
                if ((!Player.HasItem(ModContent.ItemType<PokerChip>()) || revivedByPokerChip) && !diedWithStickyHand)
                {
                    Player.AddBuff(ModContent.BuffType<SwanSong>(), 7 * 60);
                    Player.statLife = 100;
                    Player.HealEffect(100);
                    return false;
                }
            }
            if (stickyHandEquipped && !diedWithStickyHand)
            {
                if (!Player.HasItem(ModContent.ItemType<PokerChip>()) || revivedByPokerChip)
                {
                    Player.AddBuff(ModContent.BuffType<SwanSong>(), 15 * 60);
                    if (Player.statLifeMax >= 150)
                    {
                        Player.statLife = 150;
                        Player.HealEffect(150);
                    }
                    else
                    {
                        Player.statLife = Player.statLifeMax;
                        Player.HealEffect(Player.statLifeMax);
                    }

                    return false;
                }
            }
            if (backToZeroActive)
            {
                return false;
            }


            standOut = false;
            revivedByPokerChip = false;
            standChangingLocked = false;
            standRespawnQueued = true;
            forceShutDownEffect = true;
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Player.whoAmI == Main.myPlayer && !notDeadYet)
            {
                if (JoJoStands.DeathSoundID == JoJoStands.DeathSoundType.Roundabout)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/ToBeContinued"));
                else if (JoJoStands.DeathSoundID == JoJoStands.DeathSoundType.Caesar)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/Caesar"));
                else if (JoJoStands.DeathSoundID == JoJoStands.DeathSoundType.KonoMeAmareriMaroreriMerareMaro)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/GangTortureDance"));
                else if (JoJoStands.DeathSoundID == JoJoStands.DeathSoundType.LastTrainHome)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/LastTrainHome"));
                else if (JoJoStands.DeathSoundID == JoJoStands.DeathSoundType.KingCrimsonNoNorioKu)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/Deathsounds/KingCrimsonSpeech"));

                if (JoJoStands.DeathSoundID != JoJoStands.DeathSoundType.Roundabout)
                    ToBeContinued.Visible = true;
            }
        }

        public override void UpdateDead()
        {
            standOut = false;
            standAccessory = false;
            ableToOverrideTimestop = false;
            poseSoundName = "";
            standName = "";
            standType = 0;
            standTier = 0;
            standDefenseToAdd = 0;
            sexPistolsTier = 0;
            stoneFreeWeaveAbilityActive = false;

            crazyDiamondRestorationMode = false;
            crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Restore);
            crazyDiamondMessageCooldown = 0;
            crazyDiamondDestroyedTileData.Clear();

            echoesTier = 0;
            currentEchoesAct = 0;

            creamTier = 0;
            voidCounter = 0;
            creamNormalToExposed = false;
            creamNormalToVoid = false;
            creamExposedToVoid = false;
            creamDash = false;
            creamFrame = 0;

            if (Player.whoAmI == Main.myPlayer && JoJoStands.DeathSoundID == JoJoStands.DeathSoundType.Roundabout)
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
        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            if (nurse.HasBuff(ModContent.BuffType<BelieveInMe>()))
                price -= (int)(price * 0.2f);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using Terraria.Graphics.Effects;
using Terraria.UI;
using TerraUI.Objects;
using Terraria.ModLoader.IO;

namespace JoJoStands
{
    public class MyPlayer : ModPlayer
    {
        public static int deathsoundint;        //make them static to have them be true for all of you, instead of having to manually set it true for each of your characters
        public static int RangeIndicatorAlpha;
        public static bool Sounds = true;
        public static bool HamonEffects = true;
        public static bool TimestopEffects = false;
        public static bool RangeIndicators = false;
        public static bool AutomaticActivations = false;
        public static bool SecretReferences = false;
        public static int StandSlotPositionX;
        public static int StandSlotPositionY;
        public static float HamonBarPositionX;
        public static float HamonBarPositionY;
        public static float soundVolume;

        public int goldenSpinCounter = 0;
        public int spinSubtractionTimer = 0;
        public int aerosmithRadarCounter = 0;
        public int poseDuration = 300;
        public int poseDurationMinus = 290;
        public int menacingFrames = 0;
        int tbcCounter = 0;
        public int ActivationTimer = 0;
        public int GEAbilityNumber = 0;
        public int TuskActNumber = 0;
        public int TimestopEffectDurationTimer = 0;
        public int hamonChargeCounter = 0;
        public int sexPistolsLeft = 6;
        public int sexPistolsTier = 0;
        public int revolverBulletsShot = 0;
        public int sexPistolsRecoveryTimer = 0;
        public int aerosmithWhoAmI = 0;
        public int revertTimer = 0;     //for all requiems that can change forms back to previous forms
        public double standDamageBoosts = 1;
        public float standRangeBoosts = 0f;
        public int standSpeedBoosts = 0;

        public bool TuskAct1Pet = false;
        public bool TuskAct2Pet = false;
        public bool TuskAct3Pet = false;
        public bool TuskAct4Minion = false;
        public bool wearingEpitaph = false;
        public bool achievedInfiniteSpin = false;
        public bool StandOut = false;
        public bool StandAutoMode = false;

        public bool TheWorldEffect;
        public bool TimeSkipPreEffect;
        public bool TimeSkipEffect;
        public bool BackToZero;
        public bool DeathLoop;
        public bool Foresight;
        public bool standAccessory = false;
        public bool BitesTheDust = false;
        public bool poseMode = false;
        public bool controllingAerosmith = false;
        public bool Vampire;
        public bool canRevertFromKQBTD = false;
        public bool showingCBLayer = false;     //this is a bool that's needed to sync so that the Century Boy layer shows up for other clients in Multiplayer
        //public bool dyingVampire = false;

        public bool ZoneViralMeteorite;

        public UIItemSlot StandSlot;
        public UIItemSlot StandDyeSlot;

        public static List<int> stopImmune = new List<int>();
        public static List<int> standTier1List = new List<int>();


        public Vector2 aerosmithCamPosition;


        public override void ResetEffects()
        {
            TuskAct1Pet = false;
            TuskAct2Pet = false;
            TuskAct3Pet = false;
            TuskAct4Minion = false;
            UI.BulletCounter.Visible = false;
            controllingAerosmith = false;
            wearingEpitaph = false;

            standDamageBoosts = 1;
            standRangeBoosts = 0f;
            standSpeedBoosts = 0;
        }

        public override void OnEnterWorld(Player player)
        {
            if (player.HasItem(mod.ItemType("TuskAct3")) || player.HasItem(mod.ItemType("TuskAct4")))
            {
                TuskActNumber = 3;
            }
            else if (player.HasItem(mod.ItemType("TuskAct2")))
            {
                TuskActNumber = 2;
            }
            else if (player.HasItem(mod.ItemType("TuskAct1")))
            {
                TuskActNumber = 1;
            }
            base.OnEnterWorld(player);
        }

        public override void PlayerDisconnect(Player player)        //runs for everyone that hasn't left
        {
            for (int i = 0; i < 255; i++)
            {
                Player otherPlayer = Main.player[i];
                MyPlayer otherModPlayer = otherPlayer.GetModPlayer<MyPlayer>();
                if (otherPlayer.active)
                {
                    if (otherPlayer.active && otherModPlayer.TheWorldEffect && !otherPlayer.HasBuff(mod.BuffType("TheWorldBuff")))       //if everyone has the effect and no one has the owner buff, turn it off
                    {
                        Main.NewText("The user has left, and time has begun to move once more...");
                        otherModPlayer.TheWorldEffect = false;
                    }
                    if (otherPlayer.active && otherModPlayer.TimeSkipEffect && !otherPlayer.HasBuff(mod.BuffType("SkippingTime")))
                    {
                        Main.NewText("The user has left, and time has begun to move once more...");
                        otherModPlayer.TimeSkipEffect = false;
                    }
                    if (otherPlayer.active && otherModPlayer.BackToZero && !otherPlayer.HasBuff(mod.BuffType("BackToZero")))
                    {
                        otherModPlayer.BackToZero = false;
                    }
                    if (otherPlayer.active && otherModPlayer.DeathLoop && !otherPlayer.HasBuff(mod.BuffType("DeathLoop")))
                    {
                        otherModPlayer.DeathLoop = false;
                    }
                    if (otherPlayer.active && otherModPlayer.DeathLoop && !otherPlayer.HasBuff(mod.BuffType("DeathLoop")))
                    {
                        otherModPlayer.DeathLoop = false;
                    }
                }
            }
        }

        public override void ModifyScreenPosition()     //used HERO's Mods FlyCam as a reference for this
        {
            if (controllingAerosmith)
            {
                Main.screenPosition = aerosmithCamPosition;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (JoJoStands.StandAutoMode.JustPressed && !StandAutoMode && ActivationTimer <= 0)
            {
                ActivationTimer += 30;
                Main.NewText("Stand Control: Auto");
                StandAutoMode = true;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandAutoMode(256, player.whoAmI, true, player.whoAmI);
                }
            }
            if (JoJoStands.StandAutoMode.JustPressed && StandAutoMode && ActivationTimer <= 0)
            {
                ActivationTimer += 30;
                Main.NewText("Stand Control: Manual");
                StandAutoMode = false;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandAutoMode(256, player.whoAmI, true, player.whoAmI);
                }
            }
            if (!player.HasBuff(mod.BuffType("AbilityCooldown")) && player.ownedProjectileCounts[mod.ProjectileType("SheerHeartAttack")] == 0)
            {
                if (JoJoStands.SpecialHotKey.JustPressed && StandSlot.Item.type == mod.ItemType("KillerQueenT3"))       //KQ Tier 3 Sheer Heart Attack spawning
                {
                    Projectile.NewProjectile(player.position.X + 10f * player.direction, player.position.Y, 0f, 0f, mod.ProjectileType("SheerHeartAttack"), 1, 0f, Main.myPlayer, 0f);
                }
                if (JoJoStands.SpecialHotKey.JustPressed && StandSlot.Item.type == mod.ItemType("KillerQueenFinal"))    //KQ Final Tier Sheer Heart Attack spawning
                {
                    Projectile.NewProjectile(player.position.X + 10f * player.direction, player.position.Y, 0f, 0f, mod.ProjectileType("SheerHeartAttack"), 1, 0f, Main.myPlayer, 1f);
                }
            }
            if (JoJoStands.PoseHotKey.JustPressed && !poseMode)
            {
                poseMode = true;
                if (Sounds)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/menacing"));
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendPoseMode(256, player.whoAmI, true, player.whoAmI);
                }
            }
            if (JoJoStands.StandOut.JustPressed && !StandOut && ActivationTimer <= 0)
            {
                StandOut = true;
                ActivationTimer += 30;
                SpawnStand();
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, true, player.whoAmI);      //we send it to 256 cause it's the server?
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
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < (98f * 4f) && npc.boss && !npc.townNPC && !npc.immortal && !npc.hide)
                        {
                            npc.AddBuff(mod.BuffType("Locked"), 60 * 10);
                        }
                        if (distance < (98f * 4f) && !npc.boss && !npc.townNPC && !npc.immortal && !npc.hide && npc.lifeMax > 5)
                        {
                            npc.AddBuff(mod.BuffType("Locked"), 60 * 30);
                        }
                    }
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was over-guilted."), 50, player.direction);
                    player.AddBuff(mod.BuffType("AbilityCooldown"), 20 * 60);
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
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < (98f * 4f) && npc.boss && !npc.townNPC && !npc.immortal && !npc.hide)
                        {
                            npc.AddBuff(mod.BuffType("Locked"), 60 * 15);
                        }
                        if (distance < (98f * 4f) && !npc.boss && !npc.townNPC && !npc.immortal && !npc.hide && npc.lifeMax > 5)
                        {
                            npc.AddBuff(mod.BuffType("Locked"), 60 * 45);
                        }
                    }
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was over-guilted."), 25, player.direction);
                    player.AddBuff(mod.BuffType("AbilityCooldown"), 20 * 60);
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
            if (JoJoStands.StandOut.JustPressed && StandOut && ActivationTimer <= 0)
            {
                StandOut = false;
                if (standAccessory)
                {
                    standAccessory = false;
                }
                if (sexPistolsTier != 0)
                {
                    sexPistolsTier = 0;
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
                ActivationTimer += 30;
            }
        }

        public override void Initialize()
        {
            StandSlot = new UIItemSlot(Vector2.Zero, 52, 0, "Enter Stand Here");
            StandSlot.BackOpacity = .8f;
            StandSlot.Item = new Item();
            StandSlot.Item.SetDefaults(0);

            StandDyeSlot = new UIItemSlot(StandSlot.Position - new Vector2(60f, 0f), 52, context: ItemSlot.Context.EquipDye, "Enter Dye Here");
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
                { "DyeInDyeSlot", ItemIO.Save(StandDyeSlot.Item) }
            };
        }

        public override void Load(TagCompound tag)
        {
            StandSlot.Item = ItemIO.Load(tag.GetCompound("StandInSlot")).Clone();
            canRevertFromKQBTD = tag.GetBool("canRevertBTD");
            StandDyeSlot.Item = ItemIO.Load(tag.GetCompound("DyeInDyeSlot")).Clone();
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

                if (!TheWorldEffect)        //so that it's not interactable during a timestop, cause switching stands during a timestop is... not good
                {
                    StandSlot.Update();
                    StandDyeSlot.Update();
                }
            }
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item item = new Item();
            item.SetDefaults(mod.ItemType("WrappedPicture"));
            item.stack = 1;
            items.Add(item);
            if (Main.rand.Next(0, 101) <= 20)
            {
                int inheritanceStandChance = Main.rand.Next(0, standTier1List.Count /*+ 1*/);
                Item standTier1 = new Item();
                standTier1.SetDefaults(standTier1List[inheritanceStandChance]);
                standTier1.stack = 1;
                items.Add(item);
            }
        }

        /*public override void clientClone(ModPlayer clientClone)     //these 3 mehtods are from ExampleMod
        {
            MyPlayer clone = clientClone as MyPlayer;
            clone.TheWorldEffect = TheWorldEffect;
            clone.TimeSkipEffect = TimeSkipEffect;
            clone.BackToZero = BackToZero;
            clone.poseMode = poseMode;
            clone.StandOut = StandOut;
            clone.StandAutoMode = StandAutoMode;
            clone.Foresight = Foresight;
            clone.showingCBLayer = showingCBLayer;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)JoJoStands.JoJoMessageType.SyncPlayer);
            packet.Write((byte)player.whoAmI);
            packet.Write(TheWorldEffect);
            packet.Write(TimeSkipEffect);
            packet.Write(BackToZero);
            packet.Write(poseMode);
            packet.Write(StandOut);
            packet.Write(StandAutoMode);
            packet.Write(Foresight);
            packet.Write(showingCBLayer);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            MyPlayer clone = clientPlayer as MyPlayer;
            if (clone.TheWorldEffect != TheWorldEffect)
            {
                // Send a Mod Packet with the changes.
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.TheWorld);
                packet.Write((byte)player.whoAmI);
                packet.Write(TheWorldEffect);
                packet.Send();
                //Main.NewText("SendClientChanges", Color.Red);
            }
            if (clone.TimeSkipEffect != TimeSkipEffect)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.Timeskip);
                packet.Write((byte)player.whoAmI);
                packet.Write(TimeSkipEffect);
                packet.Send();
            }
            if (clone.BackToZero != BackToZero)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.BacktoZero);
                packet.Write((byte)player.whoAmI);
                packet.Write(BackToZero);
                packet.Send();
            }
            if (clone.poseMode != poseMode)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.PoseMode);
                packet.Write((byte)player.whoAmI);
                packet.Write(poseMode);
                packet.Send();
            }
            if (clone.StandOut != StandOut)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.StandOut);
                packet.Write((byte)player.whoAmI);
                packet.Write(StandOut);
                packet.Send();
            }
            if (clone.StandAutoMode != StandAutoMode)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.StandAutoMode);
                packet.Write((byte)player.whoAmI);
                packet.Write(StandAutoMode);
                packet.Send();
            }
            if (clone.Foresight != Foresight)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.Foresight);
                packet.Write((byte)player.whoAmI);
                packet.Write(Foresight);
                packet.Send();
            }
            if (clone.showingCBLayer != showingCBLayer)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)JoJoStands.JoJoMessageType.CBLayer);
                packet.Write((byte)player.whoAmI);
                packet.Write(showingCBLayer);
                packet.Send();
            }
        }*/

        public override void SetControls()
        {
            if (controllingAerosmith || player.HasBuff(mod.BuffType("CenturyBoyBuff")))
            {
                player.controlUp = false;
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlJump = false;
            }
        }

        public override void PreUpdate()
        {
            /*if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText(player.whoAmI);
                Main.NewText(TheWorldEffect, Color.Blue);
                Main.NewText(TimeSkipEffect, Color.Red);
                Main.NewText(BackToZero, Color.Green);
                Main.NewText(Foresight, Color.Cyan);
            }*/
            //Main.NewText(TheWorldEffect + "; " + player.whoAmI, Color.Blue);
            hamonChargeCounter++;
            if (ActivationTimer > 0)
            {
                ActivationTimer--;
            }
            if (revertTimer > 0)
            {
                revertTimer--;
            }
            if (TimestopEffectDurationTimer > 0)
            {
                if (TimestopEffectDurationTimer >= 15 && !Filters.Scene["TimestopEffectShader"].IsActive() && TimestopEffects)
                {
                    Filters.Scene.Activate("TimestopEffectShader");
                }
                if (TimestopEffectDurationTimer == 14)
                {
                    Filters.Scene["TimestopEffectShader"].Deactivate();
                    if (!Filters.Scene["GreyscaleEffect"].IsActive())
                    {
                        Filters.Scene.Activate("GreyscaleEffect");
                    }
                }
                TimestopEffectDurationTimer--;
            }
            if (BackToZero)
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
                TheWorldEffect = false;     //second, get rid of the effects from everyone
                TimeSkipEffect = false;
                TimeSkipPreEffect = false;
                Foresight = false;
            }
            if (!BackToZero && Filters.Scene["GreenEffect"].IsActive())
            {
                Filters.Scene["GreenEffect"].Deactivate();
            }
            if (controllingAerosmith)
            {
                aerosmithRadarCounter++;
                player.controlLeft = false;
                player.controlJump = false;
                player.controlRight = false;
                player.controlDown = false;
                player.controlUp = false;
                player.velocity = Vector2.Zero;
                if (StandSlot.Item.type == mod.ItemType("AerosmithT3") || StandSlot.Item.type == mod.ItemType("AerosmithFinal"))
                {
                    UI.AerosmithRadar.Visible = true;
                }
            }
            else
            {
                aerosmithRadarCounter = 0;
                UI.AerosmithRadar.Visible = false;
            }
            if (aerosmithRadarCounter >= 30)
            {
                aerosmithRadarCounter = 0;
            }
            if (poseMode)
            {
                poseDuration--;
            }
            if (poseMode && (poseDuration <= 0 || player.velocity.X > 1f || player.velocity.Y > 1f || player.velocity.X < -1f || player.velocity.Y < -1f))
            {
                poseMode = false;
                poseDuration = 300;
                poseDurationMinus = 290;
                menacingFrames = 0;
            }
            if (poseDuration < poseDurationMinus)
            {
                menacingFrames += 1;
                poseDurationMinus -= 8;
            }
            if (hamonChargeCounter >= 56)
            {
                hamonChargeCounter = 0;
            }

            if (TheWorldEffect)
            {
                Main.windSpeed = 0f;
            }
            if (player.dead && player.whoAmI == Main.myPlayer && deathsoundint == 1)
            {
                tbcCounter++;
                if (tbcCounter >= 270)
                {
                    UI.ToBeContinued.Visible = true;
                }
            }
            if (!player.dead && player.whoAmI == Main.myPlayer)
            {
                UI.ToBeContinued.Visible = false;
                tbcCounter = 0;
            }

            if (goldenSpinCounter > 0)
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
                    if (UI.GoldenSpinMeter.Visible)
                    {
                        UI.GoldenSpinMeter.Visible = false;
                    }
                }
            }
            if (goldenSpinCounter >= 300)
            {
                goldenSpinCounter = 300;
                achievedInfiniteSpin = true;
            }

            if (sexPistolsTier != 0)
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

        public void SpawnStand()
        {
            Item inputItem = StandSlot.Item;

            if (inputItem.type == mod.ItemType("CenturyBoy"))       //the accessory stands
            {
                standAccessory = true;
                showingCBLayer = true;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendCBLayer(256, player.whoAmI, true, player.whoAmI);
                }
            }
            else if (inputItem.type == mod.ItemType("CenturyBoyT2"))
            {
                standAccessory = true;
                showingCBLayer = true;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendCBLayer(256, player.whoAmI, true, player.whoAmI);
                }
            }
            else if (inputItem.type == mod.ItemType("LockT1"))
            {
                standAccessory = true;
            }
            else if (inputItem.type == mod.ItemType("LockT2"))
            {
                standAccessory = true;
            }
            else if (inputItem.type == mod.ItemType("LockT3"))
            {
                standAccessory = true;
            }
            else if (inputItem.type == mod.ItemType("LockT4"))
            {
                standAccessory = true;
            }
            else if (inputItem.type == mod.ItemType("StarPlatinumT1"))       //the normal stands
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StarPlatinumStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StarPlatinumT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StarPlatinumStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StarPlatinumT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StarPlatinumStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StarPlatinumFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StarPlatinumStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("TheWorldT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TheWorldStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("TheWorldT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TheWorldStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("TheWorldT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TheWorldStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("TheWorldFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TheWorldStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("GoldExperienceT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("GoldExperienceStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("GoldExperienceT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("GoldExperienceStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("GoldExperienceT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("GoldExperienceStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("GoldExperienceFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("GoldExperienceStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("GoldExperienceRequiem"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("GoldExperienceRequiemStand"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KingCrimsonT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KingCrimsonStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KingCrimsonT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KingCrimsonStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KingCrimsonT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KingCrimsonStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KingCrimsonFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KingCrimsonStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("HierophantGreenT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("HierophantGreenStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("HierophantGreenT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("HierophantGreenStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("HierophantGreenT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("HierophantGreenStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("HierophantGreenFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("HierophantGreenStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KillerQueenT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KillerQueenStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KillerQueenT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KillerQueenStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KillerQueenT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KillerQueenStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KillerQueenFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KillerQueenStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("KillerQueenBTD"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("KillerQueenBTDStand"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StickyFingersT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StickyFingersStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StickyFingersT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StickyFingersStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StickyFingersT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StickyFingersStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("StickyFingersFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StickyFingersStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("SexPistolsT1"))
            {
                sexPistolsTier = 1;
            }
            else if (inputItem.type == mod.ItemType("SexPistolsT2"))
            {
                sexPistolsTier = 2;
            }
            else if (inputItem.type == mod.ItemType("SexPistolsT3"))
            {
                sexPistolsTier = 3;
            }
            else if (inputItem.type == mod.ItemType("SexPistolsFinal"))
            {
                sexPistolsTier = 4;
            }
            else if (inputItem.type == mod.ItemType("MagiciansRedT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("MagiciansRedStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("MagiciansRedT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("MagiciansRedStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("MagiciansRedT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("MagiciansRedStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("MagiciansRedFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("MagiciansRedStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("AerosmithT1"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("AerosmithStandT1"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("AerosmithT2"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("AerosmithStandT2"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("AerosmithT3"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("AerosmithStandT3"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("AerosmithFinal"))
            {
                Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("AerosmithStandFinal"), 0, 0f, Main.myPlayer);
            }
            else if (inputItem.type == mod.ItemType("TestStand"))
            {
                if (player.name == "Mod Test Shadow")
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TestStand"), 0, 0f, Main.myPlayer);
                }
                else
                {
                    StandOut = false;
                    Main.NewText("You are not worthy.", Color.Red);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, false, player.whoAmI);
                    }
                }
            }
            else
            {
                StandOut = false;
                if (!inputItem.IsAir)
                {
                    if (inputItem.type == mod.ItemType("TuskAct1") || inputItem.type == mod.ItemType("TuskAct2") || inputItem.type == mod.ItemType("TuskAct3") || inputItem.type == mod.ItemType("TuskAct4"))
                    {
                        Main.NewText(inputItem.Name + " is not a stand that belongs in the Stand Slot! (Check the item tooltips)", Color.Red);
                    }
                    else
                    {
                        Main.NewText(inputItem.Name + " is not a stand!", Color.Red);
                    }
                }
                if (inputItem.IsAir)
                {
                    Main.NewText("There is no stand in the Stand Slot!", Color.Red);

                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    Networking.ModNetHandler.playerSync.SendStandOut(256, player.whoAmI, false, player.whoAmI);
                }
            }
        }

        public override void PreUpdateBuffs()
        {
            if (TheWorldEffect && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                player.AddBuff(mod.BuffType("FrozeninTime"), 2);
            }
            if (DeathLoop)       //if someone has deathloop turned on and you don't turn it on for you
            {
                player.AddBuff(mod.BuffType("DeathLoop"), 2);
            }
            if (Foresight && !player.HasBuff(mod.BuffType("ForesightBuff")) && !player.HasBuff(mod.BuffType("ForeseenDebuff")))
            {
                player.AddBuff(mod.BuffType("ForeseenDebuff"), 2);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)        //already only runs for melee weapons
        {
            if (Vampire && target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                int newDamage = damage / 4;
                if (newDamage < player.statLifeMax - player.statLife)
                {
                    player.statLife += newDamage;
                    player.HealEffect(newDamage, true);
                }
                if (newDamage >= player.statLifeMax - player.statLife)
                {
                    int healthReduction = player.statLifeMax - player.statLife;
                    int healingAmount = newDamage - healthReduction;
                    player.statLife += healingAmount;
                    player.HealEffect(healingAmount, true);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (Vampire && proj.type == mod.ProjectileType("Fists") && target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                int newDamage = damage / 4;
                if (newDamage < player.statLifeMax - player.statLife)
                {
                    player.statLife += newDamage;
                    player.HealEffect(newDamage, true);
                }
                if (newDamage >= player.statLifeMax - player.statLife)
                {
                    int healthReduction = player.statLifeMax - player.statLife;
                    int healingAmount = newDamage - healthReduction;
                    player.statLife += healingAmount;
                    player.HealEffect(healingAmount, true);
                }
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

        public override void UpdateBadLifeRegen()
        {
            if (Vampire)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                if (player.lifeRegenTime > 0)
                {
                    player.lifeRegenTime = 0;
                }
                if (player.lifeRegenCount > 0)
                {
                    player.lifeRegenCount = 0;
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)       //that 1 last frame before you completely die
        {
            if (deathsoundint == 1 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/ToBeContinued4"));
            }
            if (deathsoundint == 2 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/CAESAAAAAAAR"));
            }
            if (deathsoundint == 3 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/GangTortureDance"));
            }
            if (deathsoundint == 4 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/LastTrainHome"));
            }
            if (deathsoundint == 5 && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Deathsounds/KORE GA... WAGA KING CRIMSON NO NORIO KU"));
            }
            if (player.HasItem(mod.ItemType("PokerChip")))
            {
                player.ConsumeItem(mod.ItemType("PokerChip"), true);
                Main.NewText("The chip has given you new life!");
                return false;
            }
            if (BackToZero)
            {
                return false;
            }
            if (deathsoundint != 1 && player.whoAmI == Main.myPlayer)
            {
                UI.ToBeContinued.Visible = true;
            }
            /*if (Vampire && !dyingVampire)
            {
                player.AddBuff(mod.BuffType("DyingVampire"), 60);
                player.statLife = player.statLifeMax2;
                dyingVampire = true;
                return false;
            }
            if (dyingVampire)
            {
                dyingVampire = false;
            }*/
            if (player.ZoneSkyHeight && Vampire)
            {
                int karsText = Main.rand.Next(0, 3);
                if (karsText == 0)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " couldn't to become a bird in time and has frozen in space... then eventually stopped thinking...");
                }
                if (karsText == 1)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " was unable to change directions in time... then eventually stopped thinking...");
                }
                if (karsText == 2 && player.Male)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " became half-mineral, half-animal and floated forever through space, and though he wished for death, he was unable to die... then " + player.name + " eventually stopped thinking");
                }
                if (karsText == 2 && !player.Male)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " became half-mineral, half-animal and floated forever through space, and though she wished for death, she was unable to die... then " + player.name + " eventually stopped thinking");
                }
            }
            return true;
        }

        public override void UpdateDead()
        {
            StandOut = false;
        }

        public static readonly PlayerLayer MenacingPose = new PlayerLayer("JoJoStands", "Menacing Pose", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && drawPlayer.velocity.X == 0f && drawPlayer.velocity.Y == 0f && modPlayer.poseMode)
            {
                Texture2D texture = mod.GetTexture("Extras/Menacing");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX = drawX + 2;
                }
                int frameHeight = texture.Height / 6;
                if (modPlayer.menacingFrames >= 6)
                {
                    modPlayer.menacingFrames = 0;
                }
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameHeight * modPlayer.menacingFrames, texture.Width, frameHeight), Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer AerosmithRadarCam = new PlayerLayer("JoJoStands", "Aerosmith Radar Cam", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            int frame = 0;
            if (drawPlayer.active && modPlayer.controllingAerosmith)
            {
                Texture2D texture = mod.GetTexture("Extras/AerosmithRadar");
                int drawX = (int)(drawInfo.position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 7f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX = drawX - 2;
                }
                int frameHeight = texture.Height / 2;
                if (modPlayer.aerosmithRadarCounter > 12)
                {
                    frame = 0;
                }
                if (modPlayer.aerosmithRadarCounter <= 12)
                {
                    frame = 1;
                }
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameHeight * frame, texture.Width, frameHeight), Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer HamonChargesFront = new PlayerLayer("JoJoStands", "HamonChargesFront", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            Items.Hamon.HamonPlayer hamonPlayer = drawPlayer.GetModPlayer<Items.Hamon.HamonPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && hamonPlayer.HamonCounter >= hamonPlayer.maxHamon / 3 && drawPlayer.velocity.X == 0f && drawPlayer.velocity.Y == 0f)
            {
                Texture2D texture = mod.GetTexture("Extras/HamonChargeI");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (hamonPlayer.HamonCounter >= hamonPlayer.maxHamon / 2)
                {
                    texture = mod.GetTexture("Extras/HamonChargeII");
                }
                if (hamonPlayer.HamonCounter >= hamonPlayer.maxHamon / 1.5)
                {
                    texture = mod.GetTexture("Extras/HamonChargeIII");
                }
                if (drawPlayer.direction == -1)
                {
                    drawX = drawX + 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                {
                    effects = SpriteEffects.None;
                }
                if (modPlayer.hamonChargeCounter > 0)
                {
                    int frame = 0;
                    int frameHeight = texture.Height / 7;
                    if (modPlayer.hamonChargeCounter >= 8 && modPlayer.hamonChargeCounter <= 15)
                    {
                        frame = 1;
                    }
                    if (modPlayer.hamonChargeCounter >= 16 && modPlayer.hamonChargeCounter <= 23)
                    {
                        frame = 2;
                    }
                    if (modPlayer.hamonChargeCounter >= 24 && modPlayer.hamonChargeCounter <= 31)
                    {
                        frame = 3;
                    }
                    if (modPlayer.hamonChargeCounter >= 32 && modPlayer.hamonChargeCounter <= 39)
                    {
                        frame = 4;
                    }
                    if (modPlayer.hamonChargeCounter >= 40 && modPlayer.hamonChargeCounter <= 47)
                    {
                        frame = 5;
                    }
                    if (modPlayer.hamonChargeCounter >= 48 && modPlayer.hamonChargeCounter <= 55)
                    {
                        frame = 6;
                    }
                    if (modPlayer.hamonChargeCounter == 56)
                    {
                        frame = 7;
                    }
                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameHeight * frame, texture.Width, frameHeight), Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
                    Main.playerDrawData.Add(data);
                }
            }
        });

        public static readonly PlayerLayer KCArm = new PlayerLayer("JoJoStands", "KCArm", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && modPlayer.wearingEpitaph)
            {
                Texture2D texture = mod.GetTexture("Extras/KCArm");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX = drawX + 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                if (drawPlayer.direction == 1)
                {
                    effects = SpriteEffects.None;
                }
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, texture.Height), Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer CenturyBoyActivated = new PlayerLayer("JoJoStands", "CenturyBoyActivated", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && modPlayer.showingCBLayer)
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
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY - 9f), drawPlayer.bodyFrame, Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, drawPlayer.height / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer SexPistolsLayer = new PlayerLayer("JoJoStands", "Sex Pistols Layer", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //made it a BackAcc so it draws at the very back
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            if (drawPlayer.active && modPlayer.sexPistolsLeft != 0 && modPlayer.StandOut && modPlayer.sexPistolsTier != 0)
            {
                int frame = 6 - modPlayer.sexPistolsLeft;       //frames 0-5
                Texture2D texture = mod.GetTexture("Extras/SexPistolsLayer");
                int drawX = (int)(drawInfo.position.X + 4f + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (drawPlayer.direction == -1)
                {
                    drawX = drawX - 2;
                }
                int frameHeight = texture.Height / 6;
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameHeight * frame, texture.Width, frameHeight), Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });

        /*public static readonly PlayerLayer TimestopOverlay = new PlayerLayer("JoJoStands", "TimestopOverlay", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
            int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
            if (modPlayer.TheWorldEffect && modPlayer.TimestopEffectDurationTimer <= 5)
            {
                Main.NewText("Active");
                Texture2D texture = mod.GetTexture("Extras/TimestopEffectGrey");
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Gray, 0f, new Vector2(drawX, drawY), new Vector2(texture.Width * 3f, texture.Height * 3f), SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        });*/

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (HamonEffects)
            {
                HamonChargesFront.visible = true;
            }
            else
            {
                HamonChargesFront.visible = false;
            }
            if (player.dead || (player.mount.Type != -1))
            {
                KCArm.visible = false;
                MenacingPose.visible = false;
                HamonChargesFront.visible = false;
                AerosmithRadarCam.visible = false;
                CenturyBoyActivated.visible = false;
                SexPistolsLayer.visible = false;
            }
            else
            {
                SexPistolsLayer.visible = true;
                KCArm.visible = true;
                MenacingPose.visible = true;
                AerosmithRadarCam.visible = true;
                CenturyBoyActivated.visible = true;
            }
            layers.Add(AerosmithRadarCam);
            layers.Add(KCArm);
            layers.Add(HamonChargesFront);
            layers.Add(MenacingPose);
            layers.Add(CenturyBoyActivated);
            layers.Add(SexPistolsLayer);
            //layers.Add(TimestopOverlay);
        }
    }
}
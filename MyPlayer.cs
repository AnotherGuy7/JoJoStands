using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace JoJoStands
{
    public class MyPlayer : ModPlayer
    {
        public static int deathsoundint;        //make them static to have them be true for all of you, instead of having to manually set it true for each of your characters (I think?)
        public static bool Sounds = true;
        public static bool StandControlBinds = false;       //stand control set: binds
        public static bool StandControlMouse = false;       //stand control set: mouse
        public static bool PlayerEffects = true;

        public int HamonCounter = 0;
        public int maxHamon = 60;
        public int counter = 0;
        public int hamonChargeCounter = 0;
        public double time = 0;
        public int poseDuration = 300;
        public int poseDurationMinus = 290;
        public int menacingFrames = 0;
        int tbcCounter = 0;
        public int ActivationTimer = 0;
        public int GEAbilityNumber = 0;
        public int TuskActNumber = 0;

        private const int saveVersion = 0;
        public bool StarPlatinumMinion = false;
        public bool Aerosmith = false;
        public bool TuskAct1Pet = false;
        public bool TuskAct2Pet = false;
        public bool TuskAct3Pet = false;
        public bool TuskAct4Minion = false;

        public bool TheWorldEffect;      //the worlds first effect
        public bool TimeSkipPreEffect;
        public bool TimeSkipEffect;
        public bool BackToZero;
        public bool DeathLoop;
        public bool MinionCurrentlyActive = false;       //to determine if a stand minion is currently active
        public bool SHAactive = false;      //to determine if SHA is active at the moment
        public bool BitesTheDust = false;
        public bool StandControlActive = false;
        public bool AjaStone = false;
        public bool poseMode = false;
        public bool Vampire;

        public static List<int> stopimmune = new List<int>();

        public override void ResetEffects()
        {
            AjaStone = false;
            StarPlatinumMinion = false;
            Aerosmith = false;
            TuskAct1Pet = false;
            TuskAct2Pet = false;
            TuskAct3Pet = false;
            TuskAct4Minion = false;
            UI.BulletCounter.Visible = false;
        }

        public override void PostUpdateEquips()
        {}

        public override void OnEnterWorld(Player player)
        {
            if (player.HasItem(mod.ItemType("TuskAct4")))
            {
                TuskActNumber = 4;
            }
            else if (player.HasItem(mod.ItemType("TuskAct3")))
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

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (JoJoStands.StandControl.JustPressed && !StandControlActive && ActivationTimer <= 0)     //without the timer, it never went true
            {
                ActivationTimer += 30;
                StandControlActive = true;
            }
            if (JoJoStands.StandControl.JustPressed && StandControlActive && ActivationTimer <= 0)
            {
                ActivationTimer += 30;
                StandControlActive = false;
            }
            if (!SHAactive && !player.HasBuff(mod.BuffType("SheerHeartAttackCooldown")))
            {
                if (JoJoStands.ItemHotKey.JustPressed && player.HeldItem.type == mod.ItemType("KillerQueenT3") && Mplayer.SHAactive)       //KQ Tier 3 Sheer Heart Attack spawning
                {
                    Projectile.NewProjectile(player.position.X + 10f * player.direction, player.position.Y, 0f, 0f, mod.ProjectileType("SheerHeartAttack"), 1, 0f, Main.myPlayer, 0f);
                }
                if (JoJoStands.ItemHotKey.JustPressed && player.HeldItem.type == mod.ItemType("KillerQueenFinal") && Mplayer.SHAactive)    //KQ Final Tier Sheer Heart Attack spawning
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
            }
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item item = new Item();
            item.SetDefaults(mod.ItemType("WrappedPicture"));
            item.stack = 1;
            items.Add(item);
        }

        public override void clientClone(ModPlayer clientClone)     //these 3 mehtods are from ExampleMod
        {
            MyPlayer clone = clientClone as MyPlayer;
            clone.TheWorldEffect = TheWorldEffect;
            clone.TimeSkipEffect = TimeSkipEffect;
            clone.BackToZero = BackToZero;
            clone.poseMode = poseMode;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)JoJoStands.JoJoMessageType.SyncPlayer);
            packet.Write((byte)player.whoAmI);
            packet.Write(TheWorldEffect); // While we sync nonStopParty in SendClientChanges, we still need to send it here as well so newly joining players will receive the correct value.
            packet.Write(TimeSkipEffect);
            packet.Write(BackToZero);
            packet.Write(poseMode);
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
        }

        public override void PreUpdate()
        {
            hamonChargeCounter++;
            ActivationTimer--;
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
            if (poseDuration < poseDurationMinus && !MinionCurrentlyActive)
            {
                menacingFrames += 1;
                poseDurationMinus -= 8;
            }
            if (hamonChargeCounter >= 56)
            {
                hamonChargeCounter = 0;
            }
            if (AjaStone)
            {
                maxHamon *= 2;
            }
            if (Vampire)
            {
                HamonCounter = 0;
            }
            if (player.velocity.X == 0f && player.velocity.Y == 0f && !Vampire && !(player.wet || player.honeyWet || player.lavaWet))       //2 seconds while standing still, if you have vampire, your Hamon never rises
            {
                counter += 2;
            }
            if ((player.velocity.X != 0f || player.velocity.Y != 0f) && !Vampire && !(player.wet || player.honeyWet || player.lavaWet))     //4 seconds while running
            {
                counter++;
            }
            if (counter >= 240 && !AjaStone)     //this way, by standing, since it adds 2 every frame, you get there in 120 frames, then if you run, you get there at 240 frames
            {
                HamonCounter += 1;
                counter = 0;
            }
            if (counter >= 120 && AjaStone)
            {
                HamonCounter += 1;
                counter = 0;
            }
            if (HamonCounter >= maxHamon)
            {
                HamonCounter = maxHamon;
            }
            if(HamonCounter <= 0)
            {
                HamonCounter = 0;
            }
            time = Main.time;
            if (TheWorldEffect)
            {
                Main.time = time;
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
            if (ActivationTimer <= 0)
            {
                ActivationTimer = 0;
            }
        }

        public override void PreUpdateBuffs()
        {
            if (StandControlActive && StandControlBinds && !StandControlMouse)
            {
                player.AddBuff(mod.BuffType("StandControlBinds"), 2);
            }
            if (StandControlActive && !StandControlBinds && StandControlMouse)
            {
                player.AddBuff(mod.BuffType("StandControlMouse"), 2);
            }
            if (TheWorldEffect && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                player.AddBuff(mod.BuffType("FrozeninTime"), 2);
            }
            if (DeathLoop)       //if someone has deathloop turned on and you don't turn it on for you
            {
                player.AddBuff(mod.BuffType("DeathLoop"), 2);
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
            if (deathsoundint != 1 && player.whoAmI == Main.myPlayer)
            {
                UI.ToBeContinued.Visible = true;
            }
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

        public static readonly PlayerLayer HamonChargesFront = new PlayerLayer("JoJoStands", "HamonChargesFront", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's ExamplePlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            MyPlayer modPlayer = drawPlayer.GetModPlayer<MyPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && modPlayer.HamonCounter >= modPlayer.maxHamon / 3 && drawPlayer.velocity.X == 0f && drawPlayer.velocity.Y == 0f)
            {
                Texture2D texture = mod.GetTexture("Extras/HamonChargeI");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (modPlayer.HamonCounter >= modPlayer.maxHamon / 2)
                {
                    texture = mod.GetTexture("Extras/HamonChargeII");
                }
                if (modPlayer.HamonCounter >= modPlayer.maxHamon / 1.5)
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
            if (drawPlayer.active)
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

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (PlayerEffects)
            {
                HamonChargesFront.visible = true;
                if (player.GetModPlayer<MyPlayer>().poseMode)
                {
                    MenacingPose.visible = true;
                }
                else
                {
                    MenacingPose.visible = false;
                }
                if ((player.HeldItem.type == mod.ItemType("KingCrimsonT1") || player.HeldItem.type == mod.ItemType("KingCrimsonT2") || player.HeldItem.type == mod.ItemType("KingCrimsonT3") || player.HeldItem.type == mod.ItemType("KingCrimsonFinal")) && player.ownedProjectileCounts[mod.ProjectileType("KingCrimsonDonut")] == 0)
                {
                    KCArm.visible = true;
                }
                else
                {
                    KCArm.visible = false;
                }
            }
            if (!PlayerEffects)
            {
                KCArm.visible = false;
                HamonChargesFront.visible = false;
            }
            layers.Add(KCArm);
            layers.Add(HamonChargesFront);
            layers.Add(MenacingPose);
        }
    }
}
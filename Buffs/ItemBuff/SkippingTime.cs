using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using JoJoStands.Networking;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class SkippingTime : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Skipping Time");
            Description.SetDefault("Time is skipping");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }

        public bool setToTrue = false;
        public bool sendFalse = false;
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType("SkippingTime")))
            {
                player.immune = true;
                player.controlUseItem = false;
                player.AddBuff(BuffID.NightOwl, 2);
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].HasBuff(mod.BuffType("PreTimeSkip")) && !Main.player[i].HasBuff(mod.BuffType("SkippingTime")))
                    {
                        Main.player[i].velocity = PreTimeSkip.playerVelocity[i];
                        Main.player[i].AddBuff(BuffID.Obstructed, 2);
                        Main.player[i].controlUseItem = false;
                        Main.player[i].controlLeft = false;
                        Main.player[i].controlJump = false;
                        Main.player[i].controlRight = false;
                        Main.player[i].controlDown = false;
                        Main.player[i].controlQuickHeal = false;
                        Main.player[i].controlQuickMana = false;
                        Main.player[i].controlRight = false;
                        Main.player[i].controlUseTile = false;
                        Main.player[i].controlUp = false;
                        Main.player[i].controlMount = false;
                        Main.player[i].gravControl = false;
                        Main.player[i].gravControl2 = false;
                        Main.player[i].controlTorch = false;
                    }
                }
                if (!setToTrue)
                {
                    player.GetModPlayer<MyPlayer>().TimeSkipEffect = true;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModNetHandler.effectSync.SendTimeskip(256, player.whoAmI, true, player.whoAmI);
                    }
                    setToTrue = true;
                }
            }
            else
            {
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    Main.npc[i].AddBuff(mod.BuffType("TimeSkipConfusion"), 120);
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timeskip_end"));
                player.AddBuff(mod.BuffType("AbilityCooldown"), 1800);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.TimeSkipEffect = false;
                }
                else
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayers = Main.player[i];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                        {
                            if (otherPlayers.HasBuff(mod.BuffType(Name)))
                            {
                                sendFalse = false;      //don't send the packet and let the buff end if you weren't the only timestop owner
                            }
                            else
                            {
                                sendFalse = true;       //send the packet if no one is owning timestop
                            }
                        }
                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                        {
                            sendFalse = true;
                        }
                        Array.Clear(PreTimeSkip.playerVelocity, i, 1);
                        if (Main.player[i].active && i != player.whoAmI)
                        {
                            Main.player[i].AddBuff(mod.BuffType("TimeSkipConfusion"), 240);
                        }
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && sendFalse)
                {
                    mPlayer.TimeSkipEffect = false;
                    ModNetHandler.effectSync.SendTimeskip(256, player.whoAmI, false, player.whoAmI);
                }
                PreTimeSkip.userIndex = -1;
            }
        }
    }
}
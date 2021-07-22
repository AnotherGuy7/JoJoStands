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

        private bool setToTrue = false;
        private bool sendFalse = false;
 
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (player.HasBuff(mod.BuffType("SkippingTime")))
            {
                player.immune = true;
                player.controlUseItem = false;
                player.AddBuff(BuffID.NightOwl, 2);
                mPlayer.TimeSkipEffect = true;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && !otherPlayer.HasBuff(mod.BuffType("PreTimeSkip")) && !otherPlayer.HasBuff(mod.BuffType("SkippingTime")))
                    {
                        otherPlayer.velocity = PreTimeSkip.playerVelocity[i];
                        otherPlayer.AddBuff(BuffID.Obstructed, 2);
                        otherPlayer.controlUseItem = false;
                        otherPlayer.controlLeft = false;
                        otherPlayer.controlJump = false;
                        otherPlayer.controlRight = false;
                        otherPlayer.controlDown = false;
                        otherPlayer.controlQuickHeal = false;
                        otherPlayer.controlQuickMana = false;
                        otherPlayer.controlRight = false;
                        otherPlayer.controlUseTile = false;
                        otherPlayer.controlUp = false;
                        otherPlayer.controlMount = false;
                        otherPlayer.gravControl = false;
                        otherPlayer.gravControl2 = false;
                        otherPlayer.controlTorch = false;
                    }
                }
                if (!setToTrue)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModNetHandler.effectSync.SendTimeskip(256, player.whoAmI, true, player.whoAmI);
                    }
                    setToTrue = true;
                }
            }
            else
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    Main.npc[n].AddBuff(mod.BuffType("TimeSkipConfusion"), 120);
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timeskip_end"));
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(30));
                player.AddBuff(mod.BuffType("PowerfulStrike"), 2);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.TimeSkipEffect = false;
                }
                else
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        if (otherPlayer.active && otherPlayer.whoAmI != player.whoAmI)
                        {
                            if (otherPlayer.HasBuff(mod.BuffType(Name)))
                            {
                                sendFalse = false;      //don't send the packet and let the buff end if you weren't the only timeskip owner
                            }
                            else
                            {
                                sendFalse = true;       //send the packet if no one is owning timeskip
                            }
                        }
                        if (player.active && !otherPlayer.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                        {
                            sendFalse = true;
                        }
                        Array.Clear(PreTimeSkip.playerVelocity, i, 1);
                        if (otherPlayer.active && i != player.whoAmI)
                        {
                            otherPlayer.AddBuff(mod.BuffType("TimeSkipConfusion"), 5 * 60);
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
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace JoJoStands.Networking
{
    public class SyncCall
    {
        public static void DisplayText(string text)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, Color.White);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text, new object[0]), Color.White, -1);
        }

        public static void DisplayText(string text, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, color);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text, new object[0]), color, -1);
        }

        public static void SyncControlStyle(int whoAmI, MyPlayer.StandControlStyle standControlStyle)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendStandControlStyle(256, (byte)whoAmI, standControlStyle, (byte)whoAmI);
        }

        public static void SyncPoseState(int whoAmI, bool poseState)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendPoseMode(256, (byte)whoAmI, poseState, (byte)whoAmI);
        }

        public static void SyncStandOut(int whoAmI, bool standOut, string standName, int standTier)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendStandOut(256, whoAmI, standOut, standName, (byte)standTier, (byte)whoAmI);      //we send it to 256 cause it's the server
        }

        public static void SyncTimestop(int whoAmI, bool timestopValue)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (timestopValue)
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active)
                        {
                            otherPlayer.GetModPlayer<MyPlayer>().timestopActive = timestopValue;
                            otherPlayer.GetModPlayer<MyPlayer>().timestopOwner = otherPlayer.HasBuff<TheWorldBuff>();
                        }
                    }
                    ModNetHandler.EffectSync.SendTimestop(256, whoAmI, timestopValue, (short)whoAmI);
                }
                else
                {
                    bool otherTimestopsActive = false;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        if (p == whoAmI)
                            continue;

                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active && otherPlayer.HasBuff<TheWorldBuff>() && otherPlayer.GetModPlayer<MyPlayer>().timestopOwner)
                        {
                            otherTimestopsActive = true;
                            break;
                        }
                    }

                    if (!otherTimestopsActive)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.active)
                            {
                                otherPlayer.GetModPlayer<MyPlayer>().timestopActive = timestopValue;
                                otherPlayer.GetModPlayer<MyPlayer>().timestopOwner = false;
                            }
                        }
                        ModNetHandler.EffectSync.SendTimestop(256, whoAmI, false);
                    }
                    else
                        ModNetHandler.EffectSync.SendTimestop(256, whoAmI, true, (short)whoAmI);
                }
            }
        }

        public static void SyncTimeskip(int whoAmI, bool timeskipValue)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.EffectSync.SendTimeskip(256, whoAmI, timeskipValue);
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().timeskipActive = timeskipValue;
                }
            }
        }

        public static void SyncBackToZero(int whoAmI, bool backtoZeroValue)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.EffectSync.SendBackToZero(256, whoAmI, backtoZeroValue);
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().backToZeroActive = backtoZeroValue;
                }
            }
        }

        public static void SyncForesight(int whoAmI, bool foresightValue)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.EffectSync.SendForesight(256, whoAmI, foresightValue);
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().epitaphForesightActive = foresightValue;
                }
            }
        }

        public static void SyncBitesTheDust(int whoAmI, bool btdValue)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModNetHandler.EffectSync.SendBitesTheDust(256, whoAmI, btdValue);      //we send it to 256 cause it's the server
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().bitesTheDustActive = btdValue;
                }
            }
        }

        public static void SyncCurrentDyeItem(int whoAmI, int dyeType)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendDyeItem(256, whoAmI, dyeType, (byte)whoAmI);
        }

        public static void SyncSexPistolPosition(int whoAmI, int sexPistolIndex, Vector2 sexPistolPosition)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendSexPistolPosition(256, whoAmI, (byte)whoAmI, (byte)sexPistolIndex, sexPistolPosition);
        }

        public static void SyncDeathLoopInfo(int whoAmI, int targetWhoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendDeathLoopInfo(256, whoAmI, (byte)whoAmI, targetWhoAmI);
        }

        public static void SyncArrowEarringInfo(int whoAmI, int targetNPCwhoAmI, int damage, bool crit)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendArrowEarringInfo(256, whoAmI, (byte)whoAmI, targetNPCwhoAmI, damage, crit);
        }

        public static void SyncStandEffectInfo(int whoAmI, int targetWhoAmI, int fistWhoAmI, int stat1 = 0, int stat2 = 0, int stat3 = 0, float stat4 = 0f, float stat5 = 0f)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendStandEffectInfo(256, whoAmI, (byte)whoAmI, targetWhoAmI, fistWhoAmI, stat1, stat2, stat3, stat4, stat5);
        }

        public static void SyncNPCEffectInfo(int whoAmI, byte effectIndex, bool state, int info, int npcWhoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.WorldSync.SendEnemySync(256, whoAmI, effectIndex, state, info, npcWhoAmI);
        }

        public static void SyncOtherPlayerDebuff(int whoAmI, int targetPlayerWhoAmI, int debuffType, int debuffTime)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendOtherPlayerDebuff(256, whoAmI, (byte)whoAmI, targetPlayerWhoAmI, debuffType, debuffTime);
        }

        public static void SyncOtherPlayerExtraEffect(int whoAmI, int targetPlayerWhoAmI, int effectType, int int1, int int2, float float1, float float2)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendOtherPlayerExtraEffect(256, whoAmI, (byte)whoAmI, targetPlayerWhoAmI, effectType, int1, int2, float1, float2);
        }
    }
}

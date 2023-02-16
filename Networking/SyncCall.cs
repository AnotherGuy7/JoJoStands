using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using System;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Networking
{
    public class SyncCall
    {
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

        public static void SyncStandOut(int whoAmI, bool standOut)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendStandOut(256, whoAmI, standOut, (byte)whoAmI);      //we send it to 256 cause it's the server
        }

        public static void SyncCenturyBoyState(int whoAmI, bool visible)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.PlayerSync.SendCBLayer(256, whoAmI, visible, (byte)whoAmI);
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

        public static void SyncVampiricNight(int whoAmI, bool active)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.WorldSync.SendVampiricNight(256, whoAmI, active);
        }
    }
}

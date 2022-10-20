using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Networking
{
    public class SyncCall
    {
        public static void SyncAutoMode(int whoAmI, bool autoModeState)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendStandAutoMode(256, (byte)whoAmI, autoModeState, (byte)whoAmI);
        }

        public static void SyncPoseState(int whoAmI, bool poseState)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendPoseMode(256, (byte)whoAmI, poseState, (byte)whoAmI);
        }

        public static void SyncStandOut(int whoAmI, bool standOut)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendStandOut(256, whoAmI, standOut, (byte)whoAmI);      //we send it to 256 cause it's the server
        }

        public static void SyncCenturyBoyState(int whoAmI, bool visible)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                Networking.ModNetHandler.playerSync.SendCBLayer(256, whoAmI, visible, (byte)whoAmI);
        }

        public static void SyncCurrentDyeItem(int whoAmI, int dyeType)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendDyeItem(256, whoAmI, dyeType, (byte)whoAmI);
        }

        public static void SyncSexPistolPosition(int whoAmI, int sexPistolIndex, Vector2 sexPistolPosition)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendSexPistolPosition(256, whoAmI, (byte)whoAmI, (byte)sexPistolIndex, sexPistolPosition);
        }

        public static void SyncDeathLoopInfo(int whoAmI, int targetWhoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendDeathLoopInfo(256, whoAmI, (byte)whoAmI, targetWhoAmI);
        }
        public static void SyncArrowEarringInfo(int whoAmI, int targetNPCwhoAmI, int damage, bool crit)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendArrowEarringInfo(256, whoAmI, (byte)whoAmI, targetNPCwhoAmI, damage, crit);
        }
        public static void SyncFistsEffectNPCInfo(int whoAmI, int targetNPCwhoAmI, int fistWhoAmI, int stat1, int stat2, int stat3, float stat4, float stat5)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendFistsEffectNPCInfo(256, whoAmI, (byte)whoAmI, targetNPCwhoAmI, fistWhoAmI, stat1, stat2, stat3, stat4, stat5);
        }
        public static void SyncOtherPlayerDebuff(int whoAmI, int targetPlayerWhoAmI, int debuffType, int debuffTime)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendOtherPlayerDebuff(256, whoAmI, (byte)whoAmI, targetPlayerWhoAmI, debuffType, debuffTime);
        }
        public static void SyncOtherPlayerExtraEffect(int whoAmI, int targetPlayerWhoAmI, int effectType, int int1, int int2, float float1, float float2)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                ModNetHandler.playerSync.SendOtherPlayerExtraEffect(256, whoAmI, (byte)whoAmI, targetPlayerWhoAmI, effectType, int1, int2, float1, float2);
        }
    }
}

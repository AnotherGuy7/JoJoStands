using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands
{
    public class JoJoStandsEffectUtils : ModSystem
    {
        private static readonly SoundStyle TimestopStopSound = new SoundStyle("JoJoStands/Sounds/GameSounds/timestop_stop");

        public static void EndTimestop(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            bool otherTimestopsActive = false;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                mPlayer.timestopActive = false;
                SoundEngine.PlaySound(TimestopStopSound);
            }
            else
            {
                otherTimestopsActive = false;
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active && p != player.whoAmI && otherPlayer.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                    {
                        otherTimestopsActive = true;
                        if (otherTimestopsActive)
                            break;
                    }
                }

                if (Main.netMode == NetmodeID.MultiplayerClient && !otherTimestopsActive)
                {
                    mPlayer.timestopActive = false;
                    SyncCall.SyncTimestop(player.whoAmI, false);
                    SoundEngine.PlaySound(TimestopStopSound);
                }
            }

            mPlayer.timestopOwner = false;
            mPlayer.ableToOverrideTimestop = false;
            if (!otherTimestopsActive)
                JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopGreyscaleEffect);
        }
    }
}

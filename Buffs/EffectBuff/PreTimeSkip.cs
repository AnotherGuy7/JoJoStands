using JoJoStands.Projectiles.PlayerStands.KingCrimson;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class PreTimeSkip : JoJoBuff
    {
        public static int userIndex = -1;
        public static Vector2[] playerVelocity;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skipping Time");
            Description.SetDefault("Time is skipping");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }

        public override void OnApply(Player player)
        {
            playerVelocity = new Vector2[Main.maxPlayers];
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            userIndex = player.whoAmI;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(ModContent.BuffType<PreTimeSkip>()) && !mPlayer.forceShutDownEffect)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && !otherPlayer.HasBuff(ModContent.BuffType<PreTimeSkip>()) && !otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
                    {
                        playerVelocity[i] = otherPlayer.velocity;
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
                    }
                }
                mPlayer.timeskipPreEffect = true;
            }
            else
            {
                int timeSkipDuration = 0;
                if (mPlayer.standTier == 2)
                    timeSkipDuration = 180;
                else if (mPlayer.standTier == 3)
                    timeSkipDuration = 300;
                else if (mPlayer.standTier == 4)
                    timeSkipDuration = 600;

                if (mPlayer.overHeaven)
                    timeSkipDuration *= 2;
                if (timeSkipDuration != 0 && !mPlayer.forceShutDownEffect)
                    player.AddBuff(ModContent.BuffType<SkippingTime>(), timeSkipDuration);
                mPlayer.timeskipPreEffect = false;
            }
        }
    }
}
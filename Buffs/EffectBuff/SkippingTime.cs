using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class SkippingTime : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Skipping Time");
            // Description.SetDefault("Time is skipping");
            Main.debuff[Type] = true;       //so that it can't be canceled
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        private Vector2[] playerVelocity;
        private readonly SoundStyle timeskipEndSound = new SoundStyle("JoJoStands/Sounds/GameSounds/timeskip_end");

        public override void OnApply(Player player)
        {
            playerVelocity = new Vector2[Main.maxPlayers];
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player otherPlayer = Main.player[p];
                if (otherPlayer.active && !otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
                    playerVelocity[p] = otherPlayer.velocity;
            }
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.buffTime[buffIndex] > 10)
                mPlayer.kingCrimsonBuffIndex = buffIndex;

            if (player.HasBuff(ModContent.BuffType<SkippingTime>()) && !mPlayer.forceShutDownEffect)
            {
                player.shadowDodge = true;
                player.shadowDodgeCount = -100f;
                player.controlUseItem = false;
                player.nightVision = true;
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active && !otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
                    {
                        otherPlayer.velocity = playerVelocity[p];
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
            }
            else
            {
                mPlayer.kingCrimsonBuffIndex = -1;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    Main.npc[n].AddBuff(ModContent.BuffType<TimeSkipConfusion>(), 2 * 60);
                }
                SoundEngine.PlaySound(timeskipEndSound);
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(mPlayer.kingCrimsonAbilityCooldownTime));
                player.AddBuff(ModContent.BuffType<PowerfulStrike>(), 2);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.timeskipActive = false;
                }
                else
                {
                    bool otherTimeskipsActive = false;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active && otherPlayer.whoAmI != player.whoAmI)
                        {
                            otherTimeskipsActive = otherPlayer.HasBuff(Type);
                            if (otherTimeskipsActive)
                                break;
                        }

                        if (player.active && !otherPlayer.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                            otherTimeskipsActive = false;

                        if (otherPlayer.active && p != player.whoAmI)
                            otherPlayer.AddBuff(ModContent.BuffType<TimeSkipConfusion>(), 5 * 60);
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient && !otherTimeskipsActive)
                    {
                        mPlayer.timeskipActive = false;
                        SyncCall.SyncTimeskip(player.whoAmI, false);
                    }
                }
            }
        }
    }
}
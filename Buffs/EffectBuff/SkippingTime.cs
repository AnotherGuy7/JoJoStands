using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Networking;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class SkippingTime : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skipping Time");
            Description.SetDefault("Time is skipping");
            Main.debuff[Type] = true;       //so that it can't be canceled
        }

        private bool setToTrue = false;
        private bool sendFalse = false;

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(ModContent.BuffType<SkippingTime>()) && !mPlayer.forceShutDownEffect)
            {
                player.shadowDodge = true;
                player.shadowDodgeCount = -1;
                player.controlUseItem = false;
                player.nightVision = true;
                mPlayer.timeskipActive = true;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && !otherPlayer.HasBuff(ModContent.BuffType<PreTimeSkip>()) && !otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
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
                    Main.npc[n].AddBuff(ModContent.BuffType<TimeSkipConfusion>(), 120);
                }
                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/timeskip_end"));
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(mPlayer.kingCrimsonAbilityCooldownTime));
                player.AddBuff(ModContent.BuffType<PowerfulStrike>(), 2);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.timeskipActive = false;
                }
                else
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayer = Main.player[i];
                        if (otherPlayer.active && otherPlayer.whoAmI != player.whoAmI)
                            sendFalse = !otherPlayer.HasBuff(Type);

                        if (player.active && !otherPlayer.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                            sendFalse = true;

                        Array.Clear(PreTimeSkip.playerVelocity, i, 1);
                        if (otherPlayer.active && i != player.whoAmI)
                            otherPlayer.AddBuff(ModContent.BuffType<TimeSkipConfusion>(), 5 * 60);
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && sendFalse)
                {
                    mPlayer.timeskipActive = false;
                    ModNetHandler.effectSync.SendTimeskip(256, player.whoAmI, false, player.whoAmI);
                }
                PreTimeSkip.userIndex = -1;
            }
        }
    }
}
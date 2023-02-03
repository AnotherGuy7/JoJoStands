using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class TheWorldBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World");
            Description.SetDefault("Time... has been stopped!");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        private bool otherTimestopsActive = false;

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.timestopOwner = true;
            mPlayer.ableToOverrideTimestop = true;
            if (!player.HasBuff(Type) || mPlayer.forceShutDownEffect)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.timestopActive = false;
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/timestop_stop"));
                }
                else
                {
                    otherTimestopsActive = false;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.active && p != Main.myPlayer && otherPlayer.HasBuff(Type))
                        {
                            otherTimestopsActive = true;
                            break;
                        }
                    }

                    if (Main.netMode == NetmodeID.MultiplayerClient && !otherTimestopsActive)
                    {
                        mPlayer.timestopActive = false;
                        ModNetHandler.effectSync.SendTimestop(256, player.whoAmI, false, player.whoAmI);
                        SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/timestop_stop"));
                    }
                }

                mPlayer.timestopOwner = false;
                mPlayer.ableToOverrideTimestop = false;
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                if (!otherTimestopsActive)
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopGreyscaleEffect);
            }
        }
    }
}
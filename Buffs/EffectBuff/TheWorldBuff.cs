using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class TheWorldBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World");
            Description.SetDefault("Time... has been stopped!");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public bool sendFalse = false;

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.timestopOwner = true;
            if (!player.HasBuff(Type) || mPlayer.forceShutDownEffect)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.timestopActive = false;
                }
                else
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayers = Main.player[i];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                            sendFalse = !otherPlayers.HasBuff(Type);      //don't send the packet and let the buff end if you weren't the only timestop owner

                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen) (Who actually does this!?! - AG 2022)
                            sendFalse = true;
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && sendFalse)
                {
                    mPlayer.timestopActive = false;
                    ModNetHandler.effectSync.SendTimestop(256, player.whoAmI, false, player.whoAmI);
                }
                SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/timestop_stop"));
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                if (Filters.Scene["GreyscaleEffect"].IsActive())
                    Filters.Scene["GreyscaleEffect"].Deactivate();
            }
        }
    }
}
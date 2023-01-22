using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class ForesightBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foresight");
            Description.SetDefault("You are staring into the future...");
            Main.debuff[Type] = true;       //so that it can't be canceled
        }

        private bool sendFalse = false;

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!player.HasBuff(Type) || mPlayer.forceShutDownEffect)
            {
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(mPlayer.kingCrimsonAbilityCooldownTime));
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.epitaphForesightActive = false;
                }
                else
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayers = Main.player[p];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                            sendFalse = !otherPlayers.HasBuff(Type);

                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                            sendFalse = true;
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && sendFalse)
                {
                    mPlayer.epitaphForesightActive = false;
                    ModNetHandler.effectSync.SendForesight(256, player.whoAmI, false, player.whoAmI);
                }
            }
        }
    }
}
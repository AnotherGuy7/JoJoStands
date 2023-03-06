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
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }


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
                    bool otherForesightsActive = false;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayers = Main.player[p];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                        {
                            otherForesightsActive = otherPlayers.HasBuff(Type);
                            if (otherForesightsActive)
                                break;
                        }

                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                            otherForesightsActive = false;
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient && !otherForesightsActive)
                    {
                        mPlayer.epitaphForesightActive = false;
                        SyncCall.SyncForesight(player.whoAmI, false);
                    }
                }
            }
        }
    }
}
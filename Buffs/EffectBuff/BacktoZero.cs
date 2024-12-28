using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BacktoZero : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Back to Zero");
            // Description.SetDefault("Enemies that hurt you never touched you at all...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)       //it should only affect the user with this buff on
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(Type) && !mPlayer.forceShutDownEffect)
            {
                player.shadowDodge = true;
                player.shadowDodgeCount = -100f;
                player.lifeRegen += 2;
            }
            else
            {
                bool otherBackToZeroUsersActive = false;
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(35));
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.backToZeroActive = false;
                }
                else
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayers = Main.player[p];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                        {
                            otherBackToZeroUsersActive = otherPlayers.HasBuff(Type);
                            if (otherBackToZeroUsersActive)
                                break;
                        }

                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                            otherBackToZeroUsersActive = false;
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && !otherBackToZeroUsersActive)
                {
                    mPlayer.backToZeroActive = false;
                    SyncCall.SyncBackToZero(player.whoAmI, false);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    if (Filters.Scene["GreenEffect"].IsActive())
                        Filters.Scene["GreenEffect"].Deactivate();
                }
            }
        }
    }
}
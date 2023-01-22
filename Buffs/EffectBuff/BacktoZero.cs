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
            DisplayName.SetDefault("Back to Zero");
            Description.SetDefault("Enemies that hurt you never touched you at all...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        private bool sendFalse = false;

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
                            sendFalse = !otherPlayers.HasBuff(Type);

                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                            sendFalse = true;
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && sendFalse)
                {
                    mPlayer.backToZeroActive = false;
                    ModNetHandler.effectSync.SendBTZ(256, player.whoAmI, false, player.whoAmI);
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
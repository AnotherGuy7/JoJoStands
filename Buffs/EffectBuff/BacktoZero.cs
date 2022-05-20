using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BacktoZero : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Back to Zero");
            Description.SetDefault("Enemies that hurt you never touched you at all...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public bool sentTrue = false;
        public bool sendFalse = false;

        public override void Update(Player player, ref int buffIndex)       //it should only affect the user with this buff on
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(Type))
            {
                player.statDefense += 99999;
                player.endurance = 1f;
                player.lifeRegen += 2;
                player.noKnockback = true;
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
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayers = Main.player[i];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                        {
                            sendFalse = !otherPlayers.HasBuff(Type);
                        }
                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                        {
                            sendFalse = true;
                        }
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
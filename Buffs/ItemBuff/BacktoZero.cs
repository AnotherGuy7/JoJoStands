using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using JoJoStands.Networking;
using Terraria.Graphics.Effects;

namespace JoJoStands.Buffs.ItemBuff
{
    public class BacktoZero : ModBuff
    {
        public override void SetDefaults()
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
            if (player.HasBuff(mod.BuffType(Name)))
            {
                player.statDefense += 99999;
                player.endurance = 1f;
                player.lifeRegen += 2;
                player.noKnockback = true;
            }
            else
            {
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(35));
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.backToZero = false;
                }
                else
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player otherPlayers = Main.player[i];
                        if (otherPlayers.active && otherPlayers.whoAmI != player.whoAmI)
                        {
                            if (otherPlayers.HasBuff(mod.BuffType(Name)))
                            {
                                sendFalse = false;      //don't send the packet and let the buff end if you weren't the only timestop owner
                            }
                            else
                            {
                                sendFalse = true;       //send the packet if no one is owning timestop
                            }
                        }
                        if (player.active && !otherPlayers.active)       //for those people who just like playing in multiplayer worlds by themselves... (why does this happen)
                        {
                            sendFalse = true;
                        }
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && sendFalse)
                {
                    mPlayer.backToZero = false;
                    ModNetHandler.effectSync.SendBTZ(256, player.whoAmI, false, player.whoAmI);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    if (Filters.Scene["GreenEffect"].IsActive())
                    {
                        Filters.Scene["GreenEffect"].Deactivate();
                    }
                }
            }
        }
    }
}
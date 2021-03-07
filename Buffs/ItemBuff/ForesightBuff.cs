using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using JoJoStands.Networking;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class ForesightBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Foresight");
            Description.SetDefault("You are staring into the future...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }

        public bool sendFalse = false;
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (!player.HasBuff(mod.BuffType(Name)))
            {
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(30));
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    mPlayer.epitaphForesight = false;
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
                    mPlayer.epitaphForesight = false;
                    ModNetHandler.effectSync.SendForesight(256, player.whoAmI, false, player.whoAmI);
                }
            }
        }
    }
}
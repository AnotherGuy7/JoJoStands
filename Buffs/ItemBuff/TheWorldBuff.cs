using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class TheWorldBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("The World");
            Description.SetDefault("Time... has been stopped");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].HasBuff(mod.BuffType("TheWorldBuff")))     //i is the other players, use player.whoamI to say 'you'
                    {
                        Main.player[i].AddBuff(mod.BuffType("FrozeninTime"), 10);
                    }
                }
                MyPlayer.TheWorldEffect = true;
            }
            else
            {
                player.AddBuff(mod.BuffType("TheWorldAfterBuff"), 10);
                for (int i = 0; i < 255; i++)
                {
                    Main.player[i].AddBuff(mod.BuffType("TheWorldAfterBuff"), 10);
                }
                MyPlayer.TheWorldEffect = false;
            }
        }
    }
}
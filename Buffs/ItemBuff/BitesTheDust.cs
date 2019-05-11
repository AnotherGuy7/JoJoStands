using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class BitesTheDust : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Bite The Dust");
            Description.SetDefault("You are now restarting the day");           //draw the icon as a sun going backwards with lines representing it's speed going back
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType("BitesTheDust")))
            {
                if (Main.time != 1600)
                {
                    player.AddBuff(mod.BuffType("BitesTheDust"), 10);       //to constantly refresh the buff
                }
                if (Main.time <= 1600)
                {
                    Main.time += 60;        //drag time down to 1600
                }
                if (Main.time >= 1600)
                {
                    Main.time -= 60;
                }
                if (Main.time == 1600)
                {
                    player.AddBuff(mod.BuffType("BitesTheDustCoolDown"), 25200);
                    player.ClearBuff(mod.BuffType("BitesTheDust"));
                    player.statLife += 500;
                    player.Spawn();
                    if (!Main.dayTime)
                    {
                        Main.dayTime = true;
                    }
                }
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active)
                    {
                        if (Main.time <= 1600)
                        {
                            Main.time += 60;
                        }
                        if (Main.time >= 1600)
                        {
                            Main.time -= 60;
                        }
                        if (Main.time == 1600)
                        {
                            player.statLife += 500;
                            player.Spawn();
                            if (!Main.dayTime)
                            {
                                Main.dayTime = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
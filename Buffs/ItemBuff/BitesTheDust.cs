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
            Description.SetDefault("You are now restarting the day");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(mod.BuffType(Name)))
            {
                for (int i = 0; i < Main.maxPlayers; i++)       //first, get rid of all effect owners
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && otherPlayer.team != player.team)
                    {
                        if (otherPlayer.HasBuff(mod.BuffType("TheWorldBuff")))
                        {
                            otherPlayer.ClearBuff(mod.BuffType("TheWorldBuff"));
                        }
                        if (otherPlayer.HasBuff(mod.BuffType("SkippingTime")))
                        {
                            otherPlayer.ClearBuff(mod.BuffType("SkippingTime"));
                        }
                        if (otherPlayer.HasBuff(mod.BuffType("ForesightBuff")))
                        {
                            otherPlayer.ClearBuff(mod.BuffType("ForesightBuff"));
                        }
                    }
                }
                mPlayer.TheWorldEffect = false;     //second, get rid of the effects from everyone
                mPlayer.TimeSkipEffect = false;
                mPlayer.TimeSkipPreEffect = false;
                mPlayer.Foresight = false;
                if (Main.time != 1600)
                {
                    player.AddBuff(mod.BuffType(Name), 10);       //to constantly refresh the buff
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
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(420));
                    player.ClearBuff(mod.BuffType(Name));
                    player.statLife = player.statLifeMax;
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
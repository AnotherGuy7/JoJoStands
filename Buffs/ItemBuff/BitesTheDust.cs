using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class BitesTheDust : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bites The Dust");
            Description.SetDefault("The ultimate weapon against failure.");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            /*MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.netMode != NetmodeID.SinglePlayer)
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
            }
            mPlayer.timestopActive = false;     //second, get rid of the effects from everyone
            mPlayer.timeskipActive = false;
            mPlayer.timeskipPreEffect = false;
            mPlayer.epitaphForesightActive = false;
            if (Main.time != 1600)
            {
                player.AddBuff(mod.BuffType(Name), 2);       //to constantly refresh the buff
            }
            if (Main.time < 1600)
            {
                Main.time += 60;        //drag time down to 1600
            }
            if (Main.time > 1600)
            {
                Main.time -= 60;
            }
            if (Main.time == 1600)
            {
                if (!Main.dayTime)
                {
                    Main.dayTime = true;
                }
                player.statLife = player.statLifeMax;
                player.Spawn();
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(420));
                player.ClearBuff(mod.BuffType(Name));

            }*/
            /*MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.bitesTheDustActive = true;
            if (!player.HasBuff(mod.BuffType(Name)))
            {
                mPlayer.bitesTheDustActive = false;
                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(120));
            }*/
        }
    }
}
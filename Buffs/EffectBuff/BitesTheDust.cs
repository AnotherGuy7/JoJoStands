using Terraria;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BitesTheDust : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bites The Dust");
            Description.SetDefault("The ultimate weapon against failure.");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            /*MyPlayer mPlayer = GetDebuffOwnerModPlayer(npc);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int i = 0; i < Main.maxPlayers; i++)       //first, get rid of all effect owners
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active && otherPlayer.team != player.team)
                    {
                        if (otherPlayer.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                        {
                            otherPlayer.ClearBuff(ModContent.BuffType<TheWorldBuff>());
                        }
                        if (otherPlayer.HasBuff(ModContent.BuffType<SkippingTime>()))
                        {
                            otherPlayer.ClearBuff(ModContent.BuffType<SkippingTime>());
                        }
                        if (otherPlayer.HasBuff(ModContent.BuffType<ForesightBuff>()))
                        {
                            otherPlayer.ClearBuff(ModContent.BuffType<ForesightBuff>());
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
                player.AddBuff(Type, 2);       //to constantly refresh the buff
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
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(420));
                player.ClearBuff(Type);

            }*/
            /*MyPlayer mPlayer = GetDebuffOwnerModPlayer(npc);
            mPlayer.bitesTheDustActive = true;
            if (!player.HasBuff(Type))
            {
                mPlayer.bitesTheDustActive = false;
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(120));
            }*/
        }
    }
}
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class TheWorldBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The World");
            // Description.SetDefault("Time... has been stopped!");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.timestopOwner = true;
            mPlayer.ableToOverrideTimestop = true;
            if (mPlayer.forceShutDownEffect)
                OnBuffEnd(player);
        }

        public override void OnBuffEnd(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            JoJoStandsEffectUtils.EndTimestop(player);
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
        }
    }
}
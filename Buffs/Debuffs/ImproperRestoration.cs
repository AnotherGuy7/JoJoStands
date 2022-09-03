using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class ImproperRestoration : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Weak; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Improper Restoration");
            Description.SetDefault("You are ugly! Max health reduced. Unable to increase maximum health. You need to see a REAL doctor.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.buffTime[buffIndex] = 2;
            if (mPlayer.maxHP == 0)
            {
                mPlayer.oldMaxHP = player.statLifeMax;
                mPlayer.maxHP = player.statLifeMax - 20;
            }
            if (mPlayer.maxHP != 0)
                player.statLifeMax = mPlayer.maxHP;
            if (mPlayer.maxHP > 0 && mPlayer.maxHP < 100)
                mPlayer.maxHP = 100;
        }
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.maxHP > 100)
            {
                mPlayer.maxHP -= 20;
                mPlayer.improperRestorationstack += 1;
            }
            return true;
        }
    }
}
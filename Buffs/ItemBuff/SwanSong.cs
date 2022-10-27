using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SwanSong : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Rage; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swan Song");
            Description.SetDefault("You have not finished yet...");
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.debuff[Type] = true;    
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().zippedHandDeath = true;
            player.GetModPlayer<MyPlayer>().standDamageBoosts *= 2f;
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts *= 2f;
        }
    }
}
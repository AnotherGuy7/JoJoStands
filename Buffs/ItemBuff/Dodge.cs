using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Dodge : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Invisibility; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dodged!");
            Description.SetDefault("It was close call!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.shadowDodge = true;
            player.shadowDodgeCount = -100f;
        }
    }
}
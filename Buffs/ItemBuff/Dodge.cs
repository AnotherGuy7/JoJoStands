using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Dodge : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Invisibility;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dodged!");
            // Description.SetDefault("It was close call!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.shadowDodge = true;
            player.shadowDodgeCount = -100f;
        }
    }
}
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Dyes
{
    public class StandDye : ModItem
    {
        public virtual string DyePath { get; }

        public virtual void OnEquipDye(Player player)
        {
            player.GetModPlayer<MyPlayer>().dyePathAddition = DyePath;
        }

        public virtual void OnUnEquipDye(Player player)
        {
            player.GetModPlayer<MyPlayer>().dyePathAddition = "";
        }

        public virtual void UpdateEquippedDye(Player player)
        { }
    }
}

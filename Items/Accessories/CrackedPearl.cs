using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class CrackedPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cracked Pearl");
            Tooltip.SetDefault("A not well removed pearl from that ring. It seems to leak some sort of Infecting Gas though.");
        }
        public override void SetDefaults()
        {
            item.width = 100;
            item.height = 8;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = 6;
            item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().crackedPearlEquipped = true;
        }
    }
}

using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
            Item.width = 100;
            Item.height = 8;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = 6;
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().crackedPearlEquipped = true;
        }
    }
}

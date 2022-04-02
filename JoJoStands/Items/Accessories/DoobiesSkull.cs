using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Localization;

namespace JoJoStands.Items.Accessories
{
    public class DoobiesSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doobies Skull");
            Tooltip.SetDefault("WIP");
        }
        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 50;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {      
            player.GetModPlayer<MyPlayer>().doobiesskullEquipped = true;
        }
    }
}

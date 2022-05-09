using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
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
            Item.width = 44;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = 2;
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {      
            player.GetModPlayer<MyPlayer>().doobiesskullEquipped = true;
        }
    }
}

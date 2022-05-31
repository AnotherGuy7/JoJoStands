using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Items.Accessories
{
    public class DoobiesSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doobies Skull");
            Tooltip.SetDefault("WIP");
            SacrificeTotal = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().doobiesskullEquipped = true;
        }
    }
}

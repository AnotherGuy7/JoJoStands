using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class HeartHeadband : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A headband worn by a truly loyal warrior.\nWhen worn, while Cream is in void mode, Cream will bounce off of tiles rather than destroy them. Decreases Stand Damage by 8%.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.maxStack = 1;
            Item.defense = 2;
            Item.value = Item.buyPrice(silver: 32);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().heartHeadbandEquipped = true;
            player.GetModPlayer<MyPlayer>().standDamageBoosts -= 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Amethyst, 2)
                .AddRecipeGroup("JoJoStandsIron-TierBar", 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

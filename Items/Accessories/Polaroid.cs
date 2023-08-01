using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class Polaroid : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Polaroid");
            /* Tooltip.SetDefault("Successful Stand Melee Attacks reduce damage taken.\nAllows Stand Attacks to perform life steal." +
                "\n33% increased damage to user!"); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 10);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().vampiricBangleEquipped = true;
            player.GetModPlayer<MyPlayer>().familyPhotoEquipped = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VampiricBangle>())
                .AddIngredient(ModContent.ItemType<FamilyPhoto>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
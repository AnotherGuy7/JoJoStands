using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.Defiled
{
    [AutoloadEquip(EquipType.Legs)]
    public class DefiledGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Dark greaves constructed of feared life.\nIncreases jump speed");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += 1.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 14)
                .AddIngredient(ItemID.RottenChunk, 20)
                .AddIngredient(ItemID.ShadowScale, 8)
                .AddIngredient(ItemID.WormTooth, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
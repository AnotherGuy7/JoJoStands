using JoJoStands.Buffs.ItemBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class UltraSunscreen : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ultra Sunscreen");
            // Tooltip.SetDefault("Has an SPF of 247!");
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(silver: 10);
            Item.rare = ItemRarityID.Green;
            Item.consumable = true;
            Item.maxStack = 99;
            Item.buffTime = 3 * 60 * 60;
            Item.buffType = ModContent.BuffType<UltraSunscreenBuff>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ModContent.ItemType<Sunscreen>(), 3)
                .AddIngredient(ItemID.Sunflower)
                .AddRecipeGroup("JoJoStandsEvilBar")
                .AddIngredient(ItemID.CrimtaneBar)
                .AddTile(TileID.WorkBenches)
                .Register();

        }
    }
}

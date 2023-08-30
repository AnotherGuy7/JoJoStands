using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.Networking;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class FreeDomT1 : StandItemClass
    {
        public override int StandSpeed => 20;
        public override int StandType => 0;
        public override string StandProjectileName => "FreeDom";
        public override int StandTier => 1;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("FreeDom (Tier 1)");
            //Tooltip.SetDefault("Special: Rewind time by 6 seconds.");
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<SoulofTime>(),3)
                .AddIngredient(ModContent.ItemType<WillToFight>(),5)
                .AddIngredient(98, 1)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

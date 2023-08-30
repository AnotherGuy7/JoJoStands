using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PearlJamT2 : StandItemClass
    {
        public override int StandSpeed => 20;
        public override int StandType => 2;
        public override string StandProjectileName => "PearlJam";
        public override int StandTier => 2;
        public override string Texture
        {
            get { return Mod.Name + "/Items/PearlJamT1"; }
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pearl Jam (Tier 2)");
            //Tooltip.SetDefault("Heal yourself and others with even better food.");
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
                .AddIngredient(ItemID.HealingPotion, 4)
                .AddIngredient(ModContent.ItemType<WillToProtect>(),3)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

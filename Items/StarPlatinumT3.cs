using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StarPlatinumT3 : StandItemClass
    {
        public override int StandSpeed => 9;
        public override int StandType => 1;
        public override string StandProjectileName => "StarPlatinum";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.LightPink;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StarPlatinumT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Star Platinum (Tier 3)");
            // Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nIf there are no bullets in your inventory, Star Finger will be used instead.\nUsed in Stand Slot");
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TheWorldT3>();
        }

        public override void SetDefaults()
        {
            Item.damage = 92;
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
                .AddIngredient(ModContent.ItemType<StarPlatinumT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.Amethyst, 4)
                .AddIngredient(ItemID.FallenStar, 6)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

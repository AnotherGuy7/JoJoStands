using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class AerosmithFinal : StandItemClass
    {
        public override int StandType => 2;
        public override int StandSpeed => 8;
        public override string StandIdentifierName => "Aerosmith";
        public override int StandTier => 4;
        public static readonly Color AerosmithTierColor = new Color(248, 112, 104);
        public override Color StandTierDisplayColor => AerosmithTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/AerosmithT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aerosmith (Final Tier)");
            // Tooltip.SetDefault("Left-click to move and right-click to shoot bullets at the enemies!\nSpecial: Remote Control\nSecond Special: Drop a bomb on enemies!\nPassive: Carbon Radar\nThe farther the stand is from you, the less damage it does.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
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
                .AddIngredient(ModContent.ItemType<AerosmithT3>())
                .AddIngredient(ItemID.ShroomiteBar, 10)
                .AddIngredient(ItemID.MartianConduitPlating, 75)
                .AddIngredient(ModContent.ItemType<CaringLifeforce>())
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

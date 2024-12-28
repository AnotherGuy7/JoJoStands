using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesAct3 : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandIdentifierName => "Echoes";
        public override int StandTier => 4;
        public override int StandTierDisplayOffset => -1;
        public override Color StandTierDisplayColor => Color.LightGreen;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Echoes (ACT 3)");
            /* Tooltip.SetDefault("Left-click to punch enemies at a really fast rate!" +
                "\nRight-click: Three Freeze! Pin any enemy to the ground!" +
                "\nSpecial: Three Freeze Barrage! Normal attacks inflict Three Freeze for a short time!" +
                "\nSecond Special: Switch to previous acts!" +
                "\nEnemies pinned by Three Freeze take damage over time."); */
        }

        public override void SetDefaults()
        {
            Item.damage = 68;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().echoesTier = 4;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EchoesAct2>())
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddIngredient(ItemID.EncumberingStone)
                .AddIngredient(ItemID.Emerald, 7)
                .AddIngredient(ModContent.ItemType<CaringLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

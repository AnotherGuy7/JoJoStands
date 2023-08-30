using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesAct2 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;
        public override string StandProjectileName => "Echoes";
        public override int StandTier => 3;
        public override int StandTierDisplayOffset => -1;
        public override Color StandTierDisplayColor => Color.LightGreen;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Echoes (ACT 2)");
            /* Tooltip.SetDefault("Left-click to punch enemies at a really fast rate!" +
                "\nRight-click: Scroll through Sound Effects!" +
                "\nHold right-click: Create an aura with the chosen Sound Effect!" +
                "\nSpecial: Remote Control" +
                "\nSecond Special: Switch to previous acts!"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.width = 50;
            Item.height = 28;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().echoesTier = 3;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EchoesAct1>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.MusicBox)
                .AddIngredient(ItemID.Emerald, 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 4)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

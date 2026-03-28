using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.ManhattanTransfer;
using JoJoStands.Projectiles.PlayerStands.PurpleHaze;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PurpleHazeT3 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/PurpleHazeT1";

        public override int StandTier => 3;
        public override string StandIdentifierName => "PurpleHaze";
        public override Color StandTierDisplayColor => ManhattanTransferFinal.ManhattanTransferTierColor;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Orange;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standHasNoPrimary = true;
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<PurpleHazeStandT3>(), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ManhattanTransferT2>())
                .AddRecipeGroup("JoJoStandsGold-TierBar", 20)
                .AddIngredient(ItemID.FallenStar, 8)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.ManhattanTransfer;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ManhattanTransferT1 : StandItemClass
    {
        public override int StandTier => 1;
        public override string StandIdentifierName => "ManhattanTransfer";
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
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standHasNoPrimary = true;
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ManhattanTransferStandT1>(), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

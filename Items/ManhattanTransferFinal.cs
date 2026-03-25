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
    public class ManhattanTransferFinal : StandItemClass
    {
        public static readonly Color ManhattanTransferTierColor = new Color(255, 180, 50);

        public override string Texture => Mod.Name + "/Items/ManhattanTransferT1";

        public override int StandTier => 4;
        public override string StandIdentifierName => "ManhattanTransfer";
        public override Color StandTierDisplayColor => ManhattanTransferTierColor;

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
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standHasNoPrimary = true;
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<ManhattanTransferStandFinal>(), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ManhattanTransferT3>())
                .AddIngredient(ModContent.ItemType<WillToControl>(), 3)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 3)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddIngredient(ItemID.Ectoplasm, 15)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

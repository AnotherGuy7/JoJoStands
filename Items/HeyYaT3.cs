using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.HeyYa;
using JoJoStands.Projectiles.PlayerStands.ManhattanTransfer;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HeyYaT3 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/HeyYaT1";
        public override int StandTier => 3;
        public override string StandIdentifierName => "HeyYa";
        public override Color StandTierDisplayColor => HeyYaStandT1.HeyYaTierColor;

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
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
                ModContent.ProjectileType<HeyYaStandT3>(), 0, 0f, Main.myPlayer);
            mPlayer.heyYaTier = StandTier;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HeyYaT2>())
                .AddIngredient(ItemID.HallowedBar, 16)
                .AddIngredient(ItemID.PinkPearl, 1)
                .AddIngredient(ModContent.ItemType<WillToProtect>(),2)
                .AddIngredient(ModContent.ItemType<WillToChange>(),2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

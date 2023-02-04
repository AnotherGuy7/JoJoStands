using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CreamT2 : StandItemClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/CreamT1"; }
        }
        public override int StandSpeed => 26;
        public override int StandType => 1;
        public override string StandProjectileName => "Cream";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.MediumPurple;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cream (Tier 2)");
            Tooltip.SetDefault("Chop an enemy with a powerful chop and right-click to consume 4 of Void Gauge to do Cream dash!\nSpecial: Completely become a ball of Void and consume everything in your way!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 68;
            Item.width = 58;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().creamTier = StandTier;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CreamT1>())
                .AddIngredient(ItemID.HellstoneBar, 6)
                .AddIngredient(ItemID.Bone, 20)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

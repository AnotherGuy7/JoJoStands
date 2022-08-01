using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CreamT3 : StandItemClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/CreamT1"; }
        }

        public override int standSpeed => 24;
        public override int standType => 1;
        public override string standProjectileName => "Cream";
        public override int standTier => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cream (Tier 3)");
            Tooltip.SetDefault("Chop an enemy with a powerful chop and right-click to consume 4 of Void Gauge to do Cream dash! (works differently when owner is in Void)\nSpecial: Completely become a ball of Void and consume everything in your way!\nSecond Special: Envelop yourself in Void!\nWarning! Cream's abilities are extremely destructive to the area around!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 116;
            Item.width = 58;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().creamTier = standTier;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CreamT2>())
                .AddIngredient(ItemID.HallowedBar, 7)
                .AddIngredient(ItemID.Bone, 40)
                .AddIngredient(ModContent.ItemType<Hand>(), 2)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 4)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class BadCompanyFinal : StandItemClass
    {
        public override int standSpeed => 60;
        public override int standType => 2;
        public override string standProjectileName => "BadCompany";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/BadCompanyT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bad Company (Final Tier)");
            Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nSpecial: Order an arial bombing on the land around you and call in soldier reinforcements!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.width = 46;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.badCompanyTier = standTier;
            mPlayer.maxBadCompanyUnits = 6 * standTier;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BadCompanyT3>())
                .AddIngredient(ItemID.Ectoplasm, 4)
                .AddIngredient(ItemID.ChlorophyteBullet, 200)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

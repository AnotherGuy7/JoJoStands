using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldFinal : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandProjectileName => "TheWorld";
        public override int StandTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Final Tier)");       //To refer to things like "Special during Timestop" where you press special and special again a little later, use "Double-Tap Special"
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to throw knives! \nSpecial: Stop time for 9 seconds!\nSpecial during Timestop: Throw a Road Roller during a timestop!\nSecond Special: Stop time and surround an enemy with knives!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 87;
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
                .AddIngredient(ModContent.ItemType<TheWorldT3>())
                .AddIngredient(ItemID.Ectoplasm, 15)
                .AddIngredient(ModContent.ItemType<SoulofTime>(), 7)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 3)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 3)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

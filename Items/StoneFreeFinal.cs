using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeFinal : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandProjectileName => "StoneFree";
        public override int StandTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to create a string trap!\nSpecial: This ability is set by the ability selected in the Abiliy Wheel.\nSecond Special: Toggles the Ability Wheel.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 91;
            Item.width = 50;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            StoneFreeAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 5);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StoneFreeT3>())
                .AddIngredient(ItemID.Ectoplasm, 16)
                .AddIngredient(ItemID.Silk, 20)
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeT3 : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "StoneFree";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to create a string trap!\nSpecial: This ability is set by the ability selected in the Abiliy Wheel.\nSecond Special: Toggles the Ability Wheel.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.width = 50;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            StoneFreeAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 4);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StoneFreeT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.Silk, 16)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}

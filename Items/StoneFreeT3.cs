using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeT3 : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 3;
        public override string standProjectileName => "StoneFree";
        public override int standTier => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to create a string trap!\nSpecial: This ability is set by the ability selected in the Abiliy Wheel.\nSecond Special: Expands the Ability Wheel.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 65;
            item.width = 50;
            item.height = 50;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            StoneFreeAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>());
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(ItemID.HallowedBar, 14);
            recipe.AddIngredient(ItemID.Silk, 16);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeFinal : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "StoneFree";
        public override int standTier => 4;

        public override string Texture
        {
            get { return mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to create a string trap!\nSpecial: This ability is set by the ability selected in the Abiliy Wheel.\nSecond Special: Toggles the Ability Wheel.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 91;
            item.width = 50;
            item.height = 50;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            StoneFreeAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 5);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(ItemID.Ectoplasm, 16);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(mod.ItemType("DeterminedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ViralArmorTabi : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Tabi");
			Tooltip.SetDefault("A pair of light boots from a surprisingly nimble meteor, powered by a strange virus.\nProvides a 3% stand damage boost");
		}

		public override void SetDefaults()
        {
			item.width = 22;
			item.height = 16;
			item.value = Item.buyPrice(0, 3, 50, 0);
			item.rare = 8;
			item.defense = 7;
		}

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.03;       //3% damage increase
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class ViralArmorKaruta : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Karuta");
            Tooltip.SetDefault("A suit of armor made from a mysterious meteoric alloy, powered up by a strange virus.\nProvides a 3% stand damage boost");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = 8;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.03;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 35);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
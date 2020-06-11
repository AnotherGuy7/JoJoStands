using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CrystalHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Helmet");
            Tooltip.SetDefault("A suit of armor made from a mysterious meteoric alloy, powered up by a strange virus.\nProvides a 5% stand damage boost");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = 8;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.standType == 1)
            {
                player.statDefense += 10;
                modPlayer.standDamageBoosts += 0.05;
            }
            if (modPlayer.standType == 2)
            {
                player.statDefense += 5;
                modPlayer.standDamageBoosts += 0.15;
                modPlayer.standSpeedBoosts += 1;
            }
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("CrystalChestplate") && legs.type == mod.ItemType("CrystalLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.crystalArmorBonus = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrystalShard, 15);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
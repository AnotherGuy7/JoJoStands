using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ViralArmorHelmet : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Helmet (Ranged)");
            Tooltip.SetDefault("A helmet created from a meteor, powered up by a strange virus. \nStand Damage Increase: -4%\nStand Range Increase: +0.5 Radius\nSet Bonus: +0.5 Radius");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = Item.buyPrice(0, 3, 50, 0);
            item.rare = 8;
            item.defense = 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ViralArmorKaruta") && legs.type == mod.ItemType("ViralArmorTabi");
        }

        public override void UpdateArmorSet(Player player)      //to make it be a 2% increase
        {
            player.GetModPlayer<MyPlayer>().standRangeBoosts += 8f;     //to make it a total of 1 tile in each side increase
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts -= 0.04;
            mPlayer.standRangeBoosts += 8f;       //8% range increase
            /*if (mPlayer.standType == 0)
            {
                player.inventory[item.type].type = mod.ItemType("ViralArmorHelmetNeutral");
            }
            if (mPlayer.standType == 2)
            {
                player.inventory[item.type].type = mod.ItemType("ViralArmorHelmet");
            }
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (mPlayer.standType == 0 && player.inventory[i].type == item.type)
                {
                    player.inventory[i].type = mod.ItemType("ViralArmorHelmetNeutral");
                }
                if (mPlayer.standType == 1 && player.inventory[i].type == item.type)
                {
                    player.inventory[i].type = mod.ItemType("ViralArmorKabuto");
                }
            }*/
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
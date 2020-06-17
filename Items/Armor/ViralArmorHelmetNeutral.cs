using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.ComponentModel.Design;

namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ViralArmorHelmetNeutral : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Helmet (Neutral)");
            Tooltip.SetDefault("A helmet created from a meteor, powered up by a strange virus. The helmet seems to morph depending on your soul...\nStand stat buffs change depending on stand type.");
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

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 8f;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts -= 0.04;
            mPlayer.standRangeBoosts += 8f;       //8% range increase
            Main.NewText(mPlayer.standType);
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (mPlayer.standType == 1 && player.armor[i].headSlot == mod.ItemType(Name))
                {
                    player.armor[i].headSlot = mod.ItemType("ViralArmorKabuto");
                    //player.armor[i].
                }
                if (mPlayer.standType == 2 && player.armor[i].headSlot == mod.ItemType(Name))
                {
                    player.armor[i].headSlot = mod.ItemType("ViralArmorHelmet");
                }
                Main.NewText(player.armor[i].headSlot);
            }
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
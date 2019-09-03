using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenBTD : ModItem
	{
        public static bool taggedAnything = false;

        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (Stray Cat)");
			Tooltip.SetDefault("Shoot bubbles that explode and right-click to bite the dust!");
		}

		public override void SetDefaults()
		{
			item.damage = 201;      //endgame
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = SoundID.Item85;
			item.autoReuse = false;
            item.shoot = mod.ProjectileType("Bubble");
			item.maxStack = 1;
            item.shootSpeed = 2f;
			item.channel = true;
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2 && !player.HasBuff(mod.BuffType("TimeCooldown")) && !player.HasBuff(mod.BuffType("BitesTheDust")) && taggedAnything)
            {
                item.shoot = 0;
                player.AddBuff(mod.BuffType("BitesTheDust"), 10);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BiteTheDustEffect"));
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].friendly)
                    {
                        Main.npc[k].life -= Main.rand.Next(90, 136);
                        Main.npc[k].netUpdate = true;
                    }
                }
                taggedAnything = false;
            }
            if (player.altFunctionUse == 2 && !taggedAnything && !player.HasBuff(mod.BuffType("BitesTheDustCoolDown")))
            {
                item.damage = 1;
                item.useTime = 80;
                item.useAnimation = 80;
                item.useStyle = 5;
                item.UseSound = SoundID.Item1;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("KQBombFist");
                item.shootSpeed = 25f;
            }
			if (player.altFunctionUse != 2)
			{
				item.damage = 201;
				item.useTime = 60;
				item.useAnimation = 60;
				item.useStyle = 5;
				item.knockBack = 2f;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("Bubble");
	            item.shootSpeed = 2.1f;
			}
			return true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KillerQueenFinal"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
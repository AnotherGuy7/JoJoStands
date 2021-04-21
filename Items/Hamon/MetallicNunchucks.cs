using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class MetallicNunchucks : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Swing around these metallic nunchucks and then deal heavy blows to enemies!\nSpecial: Hamon Breathing");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 65;
            item.width = 24;
            item.height = 28;        //hitbox's width and height when the item is in the world
            item.useTime = 60;
            item.useAnimation = 60;
            item.maxStack = 1;
            item.noUseGraphic = true;
            item.knockBack = 12f;
            item.rare = ItemRarityID.Orange;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.noMelee = true;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("MetallicNunchucksSwinging");
            item.shootSpeed = 10f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[mod.ProjectileType("MetallicNunchucksSwinging")] == 0 && player.ownedProjectileCounts[mod.ProjectileType("MetallicNunchucksProjectile")] == 0;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 5);
            recipe.AddIngredient(ItemID.IronBar, 8);
            recipe.AddIngredient(ItemID.DemoniteBar, 6);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
			recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 5);
            recipe.AddIngredient(ItemID.LeadBar, 8);
            recipe.AddIngredient(ItemID.DemoniteBar, 6);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 5);
            recipe.AddIngredient(ItemID.LeadBar, 8);
            recipe.AddIngredient(ItemID.CrimtaneBar, 6);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe(); recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 5);
            recipe.AddIngredient(ItemID.LeadBar, 8);
            recipe.AddIngredient(ItemID.CrimtaneBar, 6);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

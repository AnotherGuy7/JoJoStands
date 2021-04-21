using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class ChainedKunai : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Swing around and throw this hamon-infused Kunai!\nSpecial: Hamon Breathing");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 7;
            item.width = 24;
            item.height = 28;        //hitbox's width and height when the item is in the world
            item.useTime = 60;
            item.useAnimation = 60;
            item.maxStack = 1;
            item.noUseGraphic = true;
            item.knockBack = 3f;
            item.rare = ItemRarityID.Pink;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.noMelee = true;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ChainedKunaiSwinging");
            item.shootSpeed = 2f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[mod.ProjectileType("ChainedKunaiSwinging")] == 0 && player.ownedProjectileCounts[mod.ProjectileType("ChainedKunaiProjectile")] == 0;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WhiteString);
            recipe.AddIngredient(ItemID.IronBar, 5);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WhiteString);
            recipe.AddIngredient(ItemID.LeadBar, 5);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class ChainedClaw : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throw this chained claw to grab enemies and inject Hamon into them!\nRequires 5 or more Hamon to inject Hamon while an enemy is grabbed.\nSpecial: Hamon Breathing");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 19;
            item.width = 32;
            item.height = 32;        //hitbox's width and height when the item is in the world
            item.useTime = 80;
            item.useAnimation = 80;
            item.maxStack = 1;
            item.noUseGraphic = true;
            item.knockBack = 1f;
            item.rare = ItemRarityID.Pink;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.noMelee = true;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ChainedClawProjectile");
            item.shootSpeed = 9f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[mod.ProjectileType("ChainedClaw")] == 0;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 6);
            recipe.AddIngredient(ItemID.IronBar, 3);
            recipe.AddIngredient(ItemID.Sapphire);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 6);
            recipe.AddIngredient(ItemID.LeadBar, 3);
            recipe.AddIngredient(ItemID.Sapphire);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

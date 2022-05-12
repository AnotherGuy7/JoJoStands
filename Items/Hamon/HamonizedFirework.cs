using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonizedFirework : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Light these Hamon-Infused fireworks and burn your enemies!\nRequires 7 or more Hamon to be used.\nSpecial: Hamon Breathing");
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.width = 26;
            Item.height = 30;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.maxStack = 999;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Pink;
            Item.noWet = true;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.shoot = ModContent.ProjectileType<HamonizedFireworkProjectile>();
            Item.shootSpeed = 15f;
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<HamonPlayer>().amountOfHamon >= 7;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<HamonPlayer>().amountOfHamon -= 7;
                player.ConsumeItem(Item.type);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ItemID.Bomb, 2)
                .AddIngredient(ItemID.Gel, 8)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 4)
                .Register();
        }
    }
}

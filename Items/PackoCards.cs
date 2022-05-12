using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PackoCards : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pack'o'Cards");
            Tooltip.SetDefault("D'Arby's favorite set of cards... Just throw them at enemies");
        }

        public override void SetDefaults()
        {
            Item.damage = 74;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 5;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.noUseGraphic = true;
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CardProjectile>();
            Item.maxStack = 1;
            Item.shootSpeed = 28f;
        }
    }
}

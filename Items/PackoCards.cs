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
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 68;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CardProjectile>();
            Item.maxStack = 1;
            Item.shootSpeed = 28f;
        }
    }
}

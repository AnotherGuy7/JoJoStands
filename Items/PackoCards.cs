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
            Tooltip.SetDefault("D'Arby's favorite set of cards... It's best to throw them at enemies!");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 56;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.knockBack = 1.2f;
            Item.shootSpeed = 16f;
            Item.maxStack = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 25);
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CardProjectile>();
        }
    }
}

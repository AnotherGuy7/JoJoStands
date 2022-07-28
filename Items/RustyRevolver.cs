using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class RustyRevolver : ModItem
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/PurpleRevolver"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Revolver");
            Tooltip.SetDefault("An antique revolver, rusted by time. It still functions.");
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsIron-TierBar", 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
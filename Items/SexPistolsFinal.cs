using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class SexPistolsFinal : StandItemClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/SexPistolsT1"; }
        }

        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sex Pistols (Final)>();
            Tooltip.SetDefault("Use a gun and have Sex Pistols kick the bullet!\nIncreases bullet damages by 20% and adds two penetration points.\nSpecial: Configure all Sex Pistols's placement!\nSecond Special: Bullet Kick Frenzy\nUsed in Stand Slot>();
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());

            mPlayer.sexPistolsTier = standTier;
            mPlayer.poseSoundName = "SexPistolsIsDesignedToKill";
            for (int i = 0; i < 6; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<SexPistolsStand>()), 0, 0f, Main.myPlayer, i + 1);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SexPistolsT3>()));
                .AddIngredient(ItemID.Ectoplasm, 15);
                .AddIngredient(ItemID.LargeTopaz);
                .AddIngredient(ItemID.FallenStar, 7);
                .AddIngredient(ModContent.ItemType<WillToEscape>()), 2);
                .AddIngredient(ModContent.ItemType<WillToProtect>()), 4);
                .AddIngredient(ItemID.ChlorophyteBullet, 100);
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>()));
                .AddTile(ModContent.TileType<RemixTableTile>()));

                .Register();
        }
    }
}
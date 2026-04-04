using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.Anubis;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    internal static class AnubisColors
    {
        public static readonly Color TierColor = new Color(210, 165, 60);
    }

    public class AnubisT1 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/AnubisT1";
        public override int StandTier => 1;
        public override string StandIdentifierName => "Anubis";
        public override Color StandTierDisplayColor => AnubisColors.TierColor;

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
            Item.rare = ItemRarityID.Blue;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
                ModContent.ProjectileType<AnubisStandT1>(), 0, 0f, Main.myPlayer);
            mPlayer.standHasNoPrimary = true;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 10)
                .AddIngredient(ItemID.SandBlock, 25)
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class AnubisT2 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/AnubisT1";
        public override int StandTier => 2;
        public override string StandIdentifierName => "Anubis";
        public override Color StandTierDisplayColor => AnubisColors.TierColor;

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
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
                ModContent.ProjectileType<AnubisStandT2>(), 0, 0f, Main.myPlayer);
            mPlayer.standHasNoPrimary = true;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AnubisT1>())
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }

    public class AnubisT3 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/AnubisT1";
        public override int StandTier => 3;
        public override string StandIdentifierName => "Anubis";
        public override Color StandTierDisplayColor => AnubisColors.TierColor;

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
            Item.rare = ItemRarityID.Pink;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
                ModContent.ProjectileType<AnubisStandT3>(), 0, 0f, Main.myPlayer);
            mPlayer.standHasNoPrimary = true;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AnubisT2>())
                .AddIngredient(ItemID.ChlorophyteBar, 8)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }

    public class AnubisFinal : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/AnubisT1";
        public override int StandTier => 4;
        public override string StandIdentifierName => "Anubis";
        public override Color StandTierDisplayColor => AnubisColors.TierColor;

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
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 2;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
                ModContent.ProjectileType<AnubisStandT4>(), 0, 0f, Main.myPlayer);
            mPlayer.standHasNoPrimary = true;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AnubisT3>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ModContent.ItemType<CaringLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
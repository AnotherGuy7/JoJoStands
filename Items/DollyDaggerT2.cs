using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class DollyDaggerT2 : StandItemClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/DollyDaggerT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dolly Dagger (Tier 2)");
            Tooltip.SetDefault("As an Item: Left-click to use this as a dagger to stab enemies and right-click to stab yourself and reflect damage to the nearest enemy!\nIn the Stand Slot: Equip it to nullify and reflect 70% of all damage!");
        }

        public override void SetDefaults()
        {
            Item.damage = 63;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.maxStack = 1;
            Item.noUseGraphic = false;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<MyPlayer>().standOut)
                return false;

            if (player.altFunctionUse == 2)
            {
                int stabDamage = Main.rand.Next(50, 81);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " couldn't reflect enough damage back."), stabDamage, player.direction);
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<DollyDaggerBeam>(), stabDamage, Item.knockBack, player.whoAmI);
            }
            else
            {
                Item.damage = 63;
                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.useStyle = ItemUseStyleID.Thrust;
                Item.UseSound = SoundID.Item1;
            }
            return true;
        }

        public override void RightClick(Player player)
        { }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.standAccessory = true;
            player.AddBuff(ModContent.BuffType<DollyDaggerActiveBuff>(), 10);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DollyDaggerT1>())
                .AddIngredient(ItemID.HallowedBar, 4)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
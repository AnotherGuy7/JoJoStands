using JoJoStands.Items.CraftingMaterials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralShortsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A shiny but tiny sword that gives you the speed and strength to dash away!");
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 1;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.melee = true;
        }

        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseLeft)
                {
                    if (Math.Abs(player.velocity.X) <= 12f)         //Abs is absolute value
                    {
                        player.velocity.X += 0.4f * player.direction;
                        Item.knockBack = Math.Abs(player.velocity.X) * 2f;
                    }
                }
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage.Flat = Math.Abs(player.velocity.X) * 2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}

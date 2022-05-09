using JoJoStands.Buffs.PetBuffs;
using JoJoStands.Projectiles.Pets.Part1;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.PetItems
{
    public class ArcheologyJournal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A journal which contains info and sketches of a familiar stone mask.\nSummons a Mini Jonathan Joestar");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(silver: 50);
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<JonathanPet>()] == 0)
            {
                player.AddBuff(ModContent.BuffType<JonathanPetBuff>(), 60);
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + new Vector2(0f, -6f), player.velocity, ModContent.ProjectileType<JonathanPet>(), 0, 0f, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TatteredCloth, 3)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}

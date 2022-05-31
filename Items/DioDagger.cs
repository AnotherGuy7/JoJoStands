using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class DioDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dio's Dagger");
            Tooltip.SetDefault("Right-click to stab yourself with this dagger and attract the attention of the zombies.");
            SacrificeTotal = 3;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.maxStack = 1;
            Item.noUseGraphic = false;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 0, 9, 50);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.mouseRight && !Main.dayTime && NPC.downedBoss1 && !JoJoStandsWorld.VampiricNight)
            {
                JoJoStandsWorld.VampiricNight = true;
                Main.NewText("Dio's Minions have arrived!", new Color(50, 255, 130));
                SoundStyle item15 = SoundID.Item15;
                item15.Pitch = -1.9f;
                SoundEngine.PlaySound(item15, player.Center);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " has succumbed to fate, just like a certain father once did."), 1, -player.direction);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 6)
                .AddIngredient(ItemID.Ruby, 2)
                .AddIngredient(ItemID.DemoniteBar, 4)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 6)
                .AddIngredient(ItemID.Ruby, 2)
                .AddIngredient(ItemID.CrimtaneBar, 4)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 6)
                .AddIngredient(ItemID.Ruby, 2)
                .AddIngredient(ItemID.DemoniteBar, 4)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 6)
                .AddIngredient(ItemID.Ruby, 2)
                .AddIngredient(ItemID.CrimtaneBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
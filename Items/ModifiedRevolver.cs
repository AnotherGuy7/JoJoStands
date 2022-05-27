using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ModifiedPurpleRevolver : ModItem
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/PurpleRevolver"; }
        }

        public int reloadCounter = 0;
        public int reloadStart = 0;
        public int soundCounter = 0;
        public bool canAltUse = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Modified Revolver");
            Tooltip.SetDefault("A revolver packing more of a punch than before. Press the special key to reload.");
        }

        public override void SetDefaults()
        {
            Item.damage = 61;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 5;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = 10;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
            Item.shootSpeed = 16f;
        }

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;
            if (reloadCounter > 0)
                reloadCounter--;
            if (mPlayer.revolverBulletsShot >= 6)       //do you really need this line?
                reloadStart++;
            if (mPlayer.revolverBulletsShot != 6)
                reloadStart = 0;

            if (reloadStart == 1)
            {
                reloadCounter = 60;
                if (MyPlayer.Sounds)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/Reload60"));
            }
            if (specialPressed && reloadCounter <= 1 && player.whoAmI == Main.myPlayer)
            {
                reloadCounter = 30;
                if (MyPlayer.Sounds)
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/Reload30"));
            }
            if (reloadCounter == 1)
            {
                mPlayer.revolverBulletsShot = 0;
                canAltUse = true;
            }
            if (player.altFunctionUse == 2 && reloadCounter == 0)
            {
                soundCounter++;
                if (soundCounter > 5)
                {
                    SoundEngine.PlaySound(SoundID.Item41, player.Center);
                    mPlayer.revolverBulletsShot += 1;
                    soundCounter = 0;
                }
            }
            UI.BulletCounter.Visible = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return canAltUse;
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (reloadCounter >= 1 || mPlayer.revolverBulletsShot >= 6)
                return false;

            if (mPlayer.revolverBulletsShot < 6)
                mPlayer.revolverBulletsShot += 1;

            if (player.altFunctionUse == 2 && reloadCounter == 0)
            {
                Item.useTime = 5;
                Item.useAnimation = 30;
            }
            if (player.altFunctionUse == 0)
            {
                Item.useTime = 15;
                Item.useAnimation = 15;
                canAltUse = false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PurpleRevolver>())
                .AddIngredient(ItemID.IllegalGunParts, 6)
                .AddIngredient(ItemID.ShroomiteBar, 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Intangible : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Intangible!");
            // Description.SetDefault("Zip through blocks for 2 minutes!");
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.controlDown)
                player.position.Y += 1f;

            if (Collision.SolidTiles((int)player.position.X / 16, (int)player.position.X / 16, (int)player.position.Y / 16, (int)player.position.Y / 16))       //or convert player.position into a tile coordinate then check 1 to the right and 2 to the bottom, 2x3 = 6 total checks, the suggestion by direwolf
            {
                if (player.controlLeft)
                    player.position.X -= 1f;

                if (player.controlRight)
                    player.position.X += 1f;

                if (player.controlJump)
                    player.position.Y -= 1f;
            }
        }

        public override void OnBuffEnd(Player player)
        {
            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(30));
        }
    }
}
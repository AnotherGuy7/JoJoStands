using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class DuelingSpirit : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dueling Spirit");
            //Description.SetDefault("You are ready to fight again!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 1.08f;
            player.GetDamage(DamageClass.Generic) *= 1.3f ;
            player.jumpSpeedBoost *= 1.15f;
            player.maxRunSpeed += 1f;
        }
    }
}
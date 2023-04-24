using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class BulletKickFrenzy : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bullet Kick Frenzy");
            // Description.SetDefault("The Sex Pistols are ready to kick any amount of bullets!");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.buffTime[buffIndex] -= 6;
            if (player.buffTime[buffIndex] <= 2)
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(120));
        }
    }
}
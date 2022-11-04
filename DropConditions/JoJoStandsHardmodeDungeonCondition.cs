using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class JoJoStandsHardmodeDungeonCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Hardmode Dungeon";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return NPC.downedPlantBoss && info.player.ZoneDungeon;

            return false;
        }
    }
}

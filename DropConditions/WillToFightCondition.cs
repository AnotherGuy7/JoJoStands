using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class WillToFightCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops during daytime while in the Forest";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return Main.dayTime && info.player.ZoneForest;

            return false;
        }
    }
}

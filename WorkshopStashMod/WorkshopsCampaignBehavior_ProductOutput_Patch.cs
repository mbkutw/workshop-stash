using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace WorkshopStashMod
{
    [HarmonyPatch(typeof(WorkshopsCampaignBehavior), "ProduceOutput")]
    public static class WorkshopsCampaignBehavior_ProductOutput_Patch
    {
        public static bool Prefix(
            ItemObject outputItem,
            Town town,
            Workshop workshop,
            int count,
            bool doNotEffectCapital)
        {
            if (workshop.Owner != Hero.MainHero) { return true; }

            var stash = MBObjectManager.Instance.GetObject<TownWorkshopStash>(x => x.Town == town);

            if (stash != null && stash.OutputToStash)
            {
                stash.Stash.AddToCounts(outputItem, count);
                return false;
            }
            return true;
        }
    }
}

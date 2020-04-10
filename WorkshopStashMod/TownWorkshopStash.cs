using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace WorkshopStashMod
{
    [SaveableClass(4004000)]
    public class TownWorkshopStash : MBObjectBase
    {
        [SaveableProperty(10)]
        public ItemRoster Stash { get; set; }

        [SaveableProperty(20)]
        public Town Town { get; set; }

        [SaveableProperty(30)]
        public bool InputFromStash { get; set; }
        [SaveableProperty(40)]
        public bool OutputToStash { get; set; }

        public TownWorkshopStash()
        {
            Stash = new ItemRoster();
        }
    }
}

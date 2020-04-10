using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace WorkshopStashMod
{
    public class WorkshopStashSaveableTypeDefiner : SaveableTypeDefiner
    {
        public WorkshopStashSaveableTypeDefiner() : base(4005000) { }
        protected override void DefineClassTypes()
        {
            this.AddClassDefinition(typeof(TownWorkshopStash), 1);
        }

        protected override void DefineGenericClassDefinitions()
        {
            this.ConstructGenericClassDefinition(typeof(MBObjectManager.ObjectTypeRecord<TownWorkshopStash>));
        }

        protected override void DefineContainerDefinitions()
        {
            this.ConstructContainerDefinition(typeof(List<TownWorkshopStash>));
            this.ConstructContainerDefinition(typeof(Dictionary<MBGUID, TownWorkshopStash>));
            this.ConstructContainerDefinition(typeof(Dictionary<string, TownWorkshopStash>));
        }
    }
}

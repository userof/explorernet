using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Modularity;
using Prism.Navigation.Regions;
using Prism.Ioc;

namespace ExplorerNet.Attributer
{

    public class AttributerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AttributerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion("AttributerRegion",
                          typeof(AttributeView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AttributeView>();
        }
    }
}

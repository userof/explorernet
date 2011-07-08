using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace ExplorerNet.Attributer
{
    
    public class AttributerModule : IModule
    {
        public void Initialize()
        {
            /* We register always-available controls 
             *  with the Prism Region Manager, and on-demand 
             * controls with the DI container.
             * On-demand controls will be loaded when we invoke
             * IRegionManager.RequestNavigate() to load the controls. */

            // Register task button with Prism Region
            var regionManager =
                ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.RegisterViewWithRegion("AttributerRegion",
                          typeof(AttributeView));

            /* View objects have to be registered 
             *  with Unity using the overload shown below. By
             * default, Unity resolves view objects as 
             * type System.Object, which this overload 
             * maps to the correct view type. See "Developer's 
             * Guide to Microsoft Prism" (Ver 4), p. 120. */

            // Register other view objects with DI Container (Unity)
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterType<Object, AttributeView>("AttributeView");
            //container.RegisterType<Object, ModuleANavigator>("ModuleANavigator");
            //container.RegisterType<Object, ModuleAWorkspace>("ModuleAWorkspace");
        }
    }
}

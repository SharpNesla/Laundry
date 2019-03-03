using System;
using System.Collections.Generic;
using Laundry.Views;
using Laundry.Model;
using Caliburn.Micro;
namespace Laundry
{
  using Utils;

  public class AppBootstrapper : BootstrapperBase
  {
    SimpleContainer container;

    public AppBootstrapper()
    {
      Initialize();
    }

    protected override void Configure()
    {
      container = new SimpleContainer();

      container.Singleton<IWindowManager, WindowManager>();
      container.Singleton<IEventAggregator, EventAggregator>();
      container.Singleton<IModel, MockModel>();
      
      container.Handler<IScreenFactory>((container) => new ScreenFactory(container));

      container.PerRequest<IShell, ShellViewModel>();


      container.PerRequest<LoginScreenViewModel>();
      container.PerRequest<DashBoardViewModel>();

      container.PerRequest<ClientDictionaryViewModel>();
      container.PerRequest<ClientEditorViewModel>();

      container.PerRequest<EmployeeDictionaryViewModel>();

      container.PerRequest<OrderEditorViewModel>();
      container.PerRequest<SettingsViewModel>();
      container.PerRequest<AboutViewModel>();

    }

    protected override object GetInstance(Type service, string key)
    {
      return container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
      return container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
      container.BuildUp(instance);
    }

    protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
    {
      DisplayRootViewFor<IShell>();
    }
  }
}
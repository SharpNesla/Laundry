using System;
using System.Collections.Generic;
using Laundry.Views;
using Laundry.Model;
using Caliburn.Micro;
namespace Laundry
{
  using Laundry.Utils.Controls;
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


      //Root Laundry level things
      container.Singleton<IWindowManager, WindowManager>();
      container.Singleton<IEventAggregator, EventAggregator>();
      container.Singleton<IModel, DataBaseModel>();
      
      container.Handler<IScreenFactory>((container) => new ScreenFactory(container));

      container.PerRequest<IShell, ShellViewModel>();

      //ViewModels
      container.PerRequest<LoginScreenViewModel>();
      container.Singleton<DashBoardViewModel>();
      container.Singleton<AnalyticsViewModel>();

      container.Singleton<ClientDictionaryViewModel>();
      container.PerRequest<ClientEditorViewModel>();

      container.Singleton<EmployeeDictionaryViewModel>();
      container.PerRequest<EmployeeEditorViewModel>();

      container.Singleton<SubsidiaryDictionaryViewModel>();
      container.PerRequest<SubsidiaryEditorViewModel>();

      container.Singleton<CarDictionaryViewModel>();
      container.PerRequest<CarEditorViewModel>();

      container.Singleton<OrderDictionaryViewModel>();
      container.PerRequest<OrderEditorViewModel>();

      container.Singleton<ClothKindEditorViewModel>();

      container.Singleton<DiscountSystemViewModel>();
      container.Singleton<SettingsViewModel>();
      container.Singleton<AboutViewModel>();
      
      //Non-screen views like dialog views
      container.Singleton<ClientCardViewModel>();
      //container.Singleton<OrderCard>();

      container.PerRequest<PaginatorViewModel>();
      container.PerRequest<OrderDataGridViewModel>();
      container.PerRequest<EmployeeDataGridViewModel>();

      container.PerRequest<ClientSearchViewModel>();
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
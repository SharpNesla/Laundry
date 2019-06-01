using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Views;

namespace Laundry.Utils
{
  public interface IScreenFactory
  {
    ActivityScreen GetScreen(Screens screen);
  }
  /// <summary>
  /// Фабрика порождающая экраны приложения по значению перечесления из DI контейнера
  /// </summary>
  class ScreenFactory : IScreenFactory
  {
    private readonly SimpleContainer _container;
    public ScreenFactory(SimpleContainer container)
    {
      _container = container;
    }

    public ActivityScreen GetScreen(Screens screen)
    {
      switch (screen)
      {
        case Screens.DashBoard:
          return _container.GetInstance<DashBoardViewModel>();
        case Screens.Login:
          return _container.GetInstance<LoginScreenViewModel>();
        case Screens.OrderEditor:
          return _container.GetInstance<OrderEditorViewModel>();
        case Screens.ClientDictionary:
          return _container.GetInstance<ClientDictionaryViewModel>();
        case Screens.ClientEditor:
          return _container.GetInstance<ClientEditorViewModel>();
        case Screens.About:
          return _container.GetInstance<AboutViewModel>();
        case Screens.Settings:
          return _container.GetInstance<SettingsViewModel>();
        case Screens.EmployeeDictionary:
          return _container.GetInstance<EmployeeDictionaryViewModel>();
        case Screens.Analytics:
          return _container.GetInstance<AnalyticsViewModel>();
        case Screens.EmployeeEditor:
          return _container.GetInstance<EmployeeEditorViewModel>();
        case Screens.SubsidiaryDictionary:
          return _container.GetInstance<SubsidiaryDictionaryViewModel>();
        case Screens.SubsidiaryEditor:
          return _container.GetInstance<SubsidiaryEditorViewModel>();
        case Screens.DiscountSystem:
          return _container.GetInstance<DiscountSystemViewModel>();
        case Screens.CarDictionary:
          return _container.GetInstance<CarDictionaryViewModel>();
        case Screens.CarEditor:
          return _container.GetInstance<CarEditorViewModel>();
        case Screens.ClothKindEditor:
          return _container.GetInstance<ClothKindDictionaryViewModel>();
        case Screens.OrderDictionary:
          return _container.GetInstance<OrderDictionaryViewModel>();
        default:
          throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
      }
    }
  }
}

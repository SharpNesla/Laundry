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
        default:
          throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
      }
    }
  }
}

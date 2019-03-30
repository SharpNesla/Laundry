using System.Security;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for LoginScreen.xaml
  /// </summary>
  public class LoginScreenViewModel : ActivityScreen
  {
    public SecureString Password { get; set; }
    

    public LoginScreenViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }

    public void Login()
    {
      this.ChangeApplicationScreen(Utils.Screens.DashBoard);
    }

    public void Settings()
    {
      DialogHost.Show(new LoginSettings());
    }



    public void PasswordChanged(PasswordBox box)
    {
      Password = box.SecurePassword;
    }
  }
}

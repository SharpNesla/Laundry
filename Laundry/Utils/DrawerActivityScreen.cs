using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Model;
using PropertyChanged;

namespace Laundry.Utils
{

  /// <summary>
  /// Расширение ActivityScreen для работы с drawer'ом
  /// </summary>
  public abstract class DrawerActivityScreen : ActivityScreen
  {
    private bool _isDrawerButtonChecked;
    /// <summary>
    /// Своство, к которому привязывается значение
    /// гамбургер-кнопки для сворачивания-разворачивание drawer'а
    /// при смене состояния передаёт соответствующее сообщение в Shell
    /// </summary>
    public bool IsDrawerButtonChecked
    {
      get { return _isDrawerButtonChecked; }
      set
      {
        _isDrawerButtonChecked = value;
        this.EventAggregator.PublishOnUIThread(value ? DrawerState.Opened : DrawerState.Closed);
        _isDrawerButtonChecked = false;
      }
    }

    protected DrawerActivityScreen(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }

  }
}

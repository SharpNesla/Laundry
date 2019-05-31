using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using Model;
using Laundry.Views;

namespace Laundry.Utils
{
  /// <summary>
  /// Базовый класс для работы с экранами
  /// </summary>
  public abstract class ActivityScreen : Screen
  {
    public IEventAggregator EventAggregator { get; private set; }
    public IModel Model { get; private set; }


    public Visibilities Visibilities { get; private set; }
    public ActivityScreen Context { get; set; }

    protected ActivityScreen(IEventAggregator aggregator, IModel model)
    {
      this.EventAggregator = aggregator;
      this.Model = model;

      this.Visibilities = new Visibilities(model);
    }

    /// <summary>
    /// Смена экрана приложения
    /// </summary>
    /// <param name="screen">Перечисление-экран</param>
    public void ChangeApplicationScreen(Screens screen)
    {
      this.EventAggregator.PublishOnUIThread(screen);
    }

    /// <summary>
    /// Часто activitysceen имеет основную кнопку с тремя точками,
    /// за который скрывается вложенной меню, данный метод показывает
    /// меню (реализованное как контекстное) для данной кнопки
    /// </summary>
    /// <param name="button">Кнопка</param>
    public void ShowActivityMenu(Button button)
    {
      if (button.ContextMenu != null) button.ContextMenu.IsOpen = true;
    }

    public void RaiseActivated()
    {
      this.OnActivate();
    }
  }
}
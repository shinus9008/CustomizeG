using DynamicData.Binding;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace WpfAppSketch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private gToolbaManager _gToolbaManager;
        private gToolbarLayout _gToolbarLayout;
        private IDisposable listen;
        public MainWindow()
        {

            _gToolbaManager = new gToolbaManager();
            _gToolbarLayout = new gToolbarLayout(_gToolbaManager);

            InitializeComponent();


            listen =
            _gToolbarLayout.Toolbars
                .ObserveCollectionChanges()                
                .Subscribe(x =>
                {
                    switch (x.EventArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Add:

                            if(x.EventArgs.NewItems.Count == 1)
                            {
                                __ToolbarLayout_Top.ToolBars.Insert(x.EventArgs.NewStartingIndex,  ToolBarFactory(x.EventArgs.NewItems[0] as gToolbarLayoutItem));
                            }
                            else
                            {
                                foreach (gToolbarLayoutItem item in x.EventArgs.NewItems)
                                {
                                    __ToolbarLayout_Top.ToolBars.Add(ToolBarFactory(item));
                                }
                            }

                            
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            {
                                if (x.EventArgs.OldItems.Count == 1)
                                {
                                    __ToolbarLayout_Top.ToolBars.RemoveAt(x.EventArgs.OldStartingIndex);
                                }
                                else
                                {

                                }

                                
                            }
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            break;
                        case NotifyCollectionChangedAction.Move:
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            __ToolbarLayout_Top.ToolBars.Clear();

                            if(x.Sender is ReadOnlyObservableCollection<gToolbarLayoutItem> collection )
                            {
                                foreach (var item in collection)
                                {
                                    __ToolbarLayout_Top.ToolBars.Add(ToolBarFactory(item));
                                }
                            }

                             
                           
                            break;
                        default:
                            break;
                    }
                });

           






            __Toolbars.DataContext = new gToolbarViewModels(_gToolbaManager);
        }

        private ToolBar ToolBarFactory(gToolbarLayoutItem layoutItem)
        {
            var ToolBar = new ToolBarReactive()
            {
                DataContext = layoutItem,                
            };

            ToolBar.Items.Add(layoutItem);
            return ToolBar;

        }


        private class ToolBarReactive : ToolBar, IViewFor<gToolbarLayoutItem>
        {
            public ToolBarReactive()
            {
                this.WhenActivated(disposeble =>
                {
                    this.Bind(ViewModel, vm => vm.Band,      v => v.Band)     .DisposeWith(disposeble);
                    this.Bind(ViewModel, vm => vm.BandIndex, v => v.BandIndex).DisposeWith(disposeble);
                });
            }

            public gToolbarLayoutItem? ViewModel 
            {
                get => DataContext as gToolbarLayoutItem;
                set => DataContext = value;
            }
            object? IViewFor.ViewModel 
            { 
                get => DataContext as gToolbarLayoutItem;
                set => DataContext = value;
            }
        }

    }
}

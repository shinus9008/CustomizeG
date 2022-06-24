using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace WpfAppSketch
{
    public class gToolbarViewModels
    {
        private readonly gToolbaManager gToolbaManager;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly ReadOnlyObservableCollection<gToolbarViewModel> _bindingCollection;

        

        public gToolbarViewModels(gToolbaManager gToolbaManager)
        {
            this.gToolbaManager = gToolbaManager ?? throw new ArgumentNullException(nameof(gToolbaManager));
            this.gToolbaManager.ObservableToolbar
                .Connect()
                .Transform(m =>
                {
                    return new gToolbarViewModel()
                    {
                        Name        = m.Key,
                        Visibility  = m.Visibility,
                    };
                })  
                .SubscribeMany(vm =>
                {
                    return 
                        vm.VisibilitySwitch.Select(_ =>
                        {
                            return new gToolbarModel()
                            {
                                Key        = vm.Name,
                                Visibility = vm.Visibility,
                            };
                        })
                        .Subscribe(this.gToolbaManager.Updater);
                })
                .ObserveOnDispatcher()       
                .Bind(out _bindingCollection)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(_disposables);
        }

        
        [Reactive]
        public gToolbarViewModel Toolbar { get; set; }

        public ReadOnlyObservableCollection<gToolbarViewModel> Toolbars => _bindingCollection;

    }
}

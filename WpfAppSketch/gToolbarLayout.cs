using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WpfAppSketch
{
    public class gToolbarLayout
    {
        private readonly gToolbaManager gToolbaManager;
        private readonly ReadOnlyObservableCollection<gToolbarLayoutItem> _bindingCollection;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public gToolbarLayout(gToolbaManager gToolbaManager)
        {
            this.gToolbaManager = gToolbaManager ?? throw new ArgumentNullException(nameof(gToolbaManager));
            this.gToolbaManager.ObservableToolbar
                .Preview(x => x.Visibility)               
                .Transform(m =>
                {
                    return new gToolbarLayoutItem()
                    {
                        Text = m.Key,
                    };
                })
                .ObserveOnDispatcher()
                .Bind(out _bindingCollection)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(_disposables);
        }

        public ReadOnlyObservableCollection<gToolbarLayoutItem> Toolbars => _bindingCollection;
    }

    public class gToolbarLayoutItem : ReactiveObject
    {
        [Reactive]
        public string Text { get; set; }

        public gToolbarLayoutItem()
        {
            this.WhenAnyValue(x => x.Band, x => x.BandIndex, (b1, b2) =>
            $"Band: {b1}; BandIndex: {b2}")
                .Subscribe(x => Text = x);
        }

        [Reactive]
        public int Band { get; set; }

        [Reactive]
        public int BandIndex { get; set; }
    }
}

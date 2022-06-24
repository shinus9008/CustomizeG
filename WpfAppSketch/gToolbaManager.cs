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
using System.Xml.Linq;

namespace WpfAppSketch
{
    public class gToolbaManager
    {
        public SourceCache<gToolbarModel, string> ObservableToolbar = new SourceCache<gToolbarModel, string>(x => x.Key);


        public Subject<gToolbarModel> Updater { get; } = new Subject<gToolbarModel>();

        public gToolbaManager()
        {
            string[] names = new string[]
            {
                "Example Tool 1",
                "Example Tool 2",
                "Example Tool 3",
                "Example Tool 4",
            };

            foreach (var item in names)
            {
                ObservableToolbar.AddOrUpdate(new gToolbarModel()
                {
                    Key = item,
                }); ;
            }

           

            Updater.Select(x => Observable.FromAsync(() => UpdateModelAsync(x)))
                   .Concat()
                   .Subscribe();
        }

        public async Task<Unit> UpdateModelAsync(gToolbarModel gToolbarModel)
        {
            await Task.Delay(1);

            UpdateModel(gToolbarModel);
            return Unit.Default;
        }

        public void UpdateModel(gToolbarModel gToolbarModel)
        {
            if (gToolbarModel is null)
            {
                throw new ArgumentNullException(nameof(gToolbarModel));
            }

            ObservableToolbar.AddOrUpdate(gToolbarModel);
        }


    }
}

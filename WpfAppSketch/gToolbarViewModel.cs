using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace WpfAppSketch
{
    public class gToolbarViewModel : ReactiveObject, IDisposable
    {
        [Reactive]
        public string Name { get; init; }

        [Reactive]
        public bool Visibility { get; set; }

        public ReactiveCommand<Unit, Unit> VisibilitySwitch { get; }


        public gToolbarViewModel()
        {
            VisibilitySwitch = ReactiveCommand.Create(visibilitySwitch);
        }

        private void visibilitySwitch()
        {
            Visibility = !Visibility;
        }


        public void Dispose()
        {
            VisibilitySwitch.Dispose();
        }
    }
}

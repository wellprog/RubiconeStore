using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace RubiconeStore.MyModels
{

    public interface IExecutableModel
    {
        string Text { get; }
        string Description { get; }
        bool ShowDescription { get; }
        void Exec();
        ICommand Command { get; }
        SwipeItems LeftSwipeItems { get; }
        SwipeItems RightSwipeItems { get; }
    }

    public class ActionModel<T> : IExecutableModel
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public bool ShowDescription => !string.IsNullOrWhiteSpace(Description);
        public Func<T, Task> ExecAction { get; set; }
        public T Parametr { get; set; }
        public ICommand Command { get; private set; }
        public SwipeItems LeftSwipeItems { get; private set; } = new SwipeItems();
        public SwipeItems RightSwipeItems { get; private set; } = new SwipeItems();

        public ActionModel(T parametr)
        {
            Parametr = parametr;
            Command = new Command(Exec, () => ExecAction != null);
        }

        public async void Exec()
        {
            await ExecAction?.Invoke(Parametr);
        }

        public void AddLeftSwipe(string title, Color color, ICommand command)
        {
            LeftSwipeItems.Add(new SwipeItem()
            {
                Command = command,
                Text = title,
                BackgroundColor = color,
                CommandParameter = Parametr
            });
        }

        public void AddRightSwipe(string title, Color color, ICommand command)
        {
            RightSwipeItems.Add(new SwipeItem()
            {
                Command = command,
                Text = title,
                BackgroundColor = color,
                CommandParameter = Parametr
            });
        }
    }
}

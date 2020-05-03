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
    }

    public class ActionModel<T> : IExecutableModel
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public bool ShowDescription => !string.IsNullOrWhiteSpace(Description);
        public Func<T, Task> ExecAction { get; set; }
        public T Parametr { get; set; }
        public ICommand Command { get; private set; }
        //IEnumerable<{Command, color, text}> Actions { get; }

        public ActionModel(T parametr)
        {
            Parametr = parametr;
            Command = new Command(Exec, () => ExecAction != null);
        }

        public async void Exec()
        {
            await ExecAction?.Invoke(Parametr);
        }
    }
}

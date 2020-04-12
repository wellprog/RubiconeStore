using System;
using System.Collections.Generic;
using System.Text;

namespace RubiconeStore.MyModels
{

    public interface IExecutableModel
    {
        string Text { get; }
        void Exec();
    }

    public class ActionModel<T> : IExecutableModel
    {
        public string Text { get; set; }
        public Action<T> ExecAction { get; set; }
        public T Parametr { get; set; }

        public ActionModel(T parametr)
        {
            Parametr = parametr;
        }

        public void Exec()
        {
            if (ExecAction != null)
                ExecAction.Invoke(Parametr);
        }
    }
}

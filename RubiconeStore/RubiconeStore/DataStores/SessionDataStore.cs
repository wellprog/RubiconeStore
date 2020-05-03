using Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace RubiconeStore.DataStores
{
    public class SessionDataStore
    {
        private const string userSession = "{EDEBC7D5-DBB4-48A8-AF3B-8817FF3DF15D}";
        public UserAuthModel UserAuthModel 
        {
            get 
            {
                var app = Application.Current;
                if (!app.Properties.ContainsKey(userSession))
                    return null;

                return app.Properties[userSession] as UserAuthModel;
            } 
            set
            {
                var app = Application.Current;
                app.Properties.Add(userSession, value);
            } 
        }

        public string SessionToken => UserAuthModel?.UserSession.SessionToken;
    }
}

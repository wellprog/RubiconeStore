using RubiconeStore.DataStores;
using RubiconeStore.Helpers;

using Shared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RubiconeStore.MyViewModels
{
    public class PayViewModel : BaseViewModel
    {
        private readonly RequestHelper requestHelper;
        private readonly SessionDataStore sessionData;
        private readonly Page page;

        public Command PayCommand { get; }

        public string CardNumber
        {
            get
            {
                return CardNumber;
            }
            set
            {
                CardNumber = value;
                OnPropertyChanged();
            }
        }

        public string CVCCode
        {
            get
            {
                return CVCCode;
            }
            set
            {
                CVCCode = value;
                OnPropertyChanged();
            }
        }

        public PayViewModel(HttpClient httpClient, Page page)
        {
            requestHelper = new RequestHelper(httpClient);
            sessionData = new SessionDataStore();
            PayCommand = new Command(EndPay, canEndPay);
            this.page = page;
        }

        public bool canEndPay()
        {
            return (!String.IsNullOrWhiteSpace(CardNumber) && !String.IsNullOrWhiteSpace(CVCCode)); //TODO regex!
        }

        public async void EndPay()
        {
            //*Throw money using cvc and card code*

            await requestHelper.Post<bool, RequestModel<bool>>($"http://rstore.kikoriki.space/Cart{ sessionData.SessionToken }", null);

            await page.Navigation.PopAsync();
        }

        public async Task Appearing()
        {

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PayCommand.ChangeCanExecute();
            base.OnPropertyChanged(propertyName);
        }
    }
}

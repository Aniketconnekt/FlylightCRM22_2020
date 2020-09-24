using Acr.UserDialogs;
using CRM.Common.Command;
using CRM.Common.Constants;
using CRM.Common.Helpers;
using CRM.Model;
using CRM.Model.AdminModel;
using CRM.View.Admin.Lead;
using CRM.View.Admin.Successful;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CRM.ViewModel
{
    public class LeadDetailsViewModel : BaseViewModel
    {
        #region Data Members
        int _userID = 0;
        int _count = 0;
        string _fromDate, _toDate = String.Empty;
        public List<LeadDetailsList> TransferLeadsList = new List<LeadDetailsList>();
        public List<LeadDetailsData> LeadsDataList = new List<LeadDetailsData>();
        #endregion

        #region CTOR
        public LeadDetailsViewModel(INavigation navigation) : base(navigation)
        {
            
        }
        #endregion

        #region Commands
        public DelegateCommand Day1Command => new DelegateCommand(ExecuteOnDay1);
        public DelegateCommand Day7Command => new DelegateCommand(ExecuteOnDay7);
        public DelegateCommand Day30Command => new DelegateCommand(ExecuteOnDay30);
        public DelegateCommand DownloadLeadCommand => new DelegateCommand(ExecuteOnDownloadLead);
        public DelegateCommand AddNewLeadCommand => new DelegateCommand(ExecuteOnAddNewLead);
        public DelegateCommand TransferLeadsCommand => new DelegateCommand(ExecuteOnTransferLeads);
        public DelegateCommand LoadMoreItemsCommand => new DelegateCommand(ExecuteOnLoadMoreItems);
        public DelegateCommand LoadMorePendingItemsCommand => new DelegateCommand(ExecuteOnLoadMorePendingItems);
      //  public DelegateCommand SearchCommand => new DelegateCommand(ExecuteOnLeadsSearch);
        #endregion

        #region Properties
        private Color _day1Color = Color.FromHex("#4C4C4C");
        public Color Day1Color
        {
            get => _day1Color;
            set { _day1Color = value; OnPropertyChanged(); }
        }

        private Color _day7Color = Color.FromHex("#2baae1");
        public Color Day7Color
        {
            get => _day7Color;
            set { _day7Color = value; OnPropertyChanged(); }
        }

        private Color _day30Color = Color.FromHex("#4C4C4C");
        public Color Day30Color
        {
            get => _day30Color;
            set { _day30Color = value; OnPropertyChanged(); }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _leadName;
        public string LeadName
        {
            get => _leadName;
            set { _leadName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<StatusData> _statusData;
        public ObservableCollection<StatusData> StatusData
        {
            get => _statusData;
            set { _statusData = value; OnPropertyChanged(); }
        }
        public StatusData _Selectedstatus = new StatusData();
        public StatusData Selectedstatus
        {
            get => _Selectedstatus;
            set { _Selectedstatus = value; OnPropertyChanged(); }
        }
        private ObservableCollection<LeadDetailsList> _pendinglist;
        public ObservableCollection<LeadDetailsList> Pendinglist
        {
            get => _pendinglist;
            set { _pendinglist = value; OnPropertyChanged(); }
        }

        private ObservableCollection<LeadDetailsList> _calledlist;
        public ObservableCollection<LeadDetailsList> Calledlist
        {
            get => _calledlist;
            set { _calledlist = value; OnPropertyChanged(); }
        }

        private string _leadsCalledCount;
        public string LeadsCalledCount
        {
            get => _leadsCalledCount;
            set { _leadsCalledCount = value; OnPropertyChanged(); }
        }

        private string _leadsPendingCount;
        public string LeadsPendingCount
        {
            get => _leadsPendingCount;
            set { _leadsPendingCount = value; OnPropertyChanged(); }
        }

        private LoadMoreOption _loadMorePendingOption = LoadMoreOption.None;
        public LoadMoreOption LoadMorePendingOption
        {
            get => _loadMorePendingOption;
            set { _loadMorePendingOption = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        public async void Initialize(int userID, int count)
        {
            _userID = userID;
            _count = count;
            if (count == 1)
                ExecuteOnDay1(null);
            else if (count == 7)
                ExecuteOnDay7(null);
            else if (count == 30)
                ExecuteOnDay30(null);

            StatusData = await GetStatusData();

        }
        //public async Task Initialize()
        //{
        //    Calledlist = new ObservableCollection<LeadDetailsList>();
        //    await GetLeadsCalledList(Calledlist.LastOrDefault().Id, _userID, _fromDate, _toDate);
        //    if (!Calledlist.Any())
        //        await ShowAlert("Record Not Found");
        //    LeadsDataList = new List<LeadDetailsData>();
        //}

        
        public async void ExecuteOnDay1(object obj)
        {
            Day1Color = Color.FromHex("#2baae1");
            Day7Color = Day30Color = Color.FromHex("#4C4C4C");
            _fromDate = DateTime.Now.ToString("MM-dd-yyyy");
            _toDate = DateTime.Now.ToString("MM-dd-yyyy");
            Calledlist = new ObservableCollection<LeadDetailsList>();
            Pendinglist = new ObservableCollection<LeadDetailsList>();
            await GetLeadsCalledList(0, _userID, _fromDate, _toDate);
            await GetLeadsPendingList(0, _userID, _fromDate, _toDate);
            TransferLeadsList = new List<LeadDetailsList>();
        }
        public async void ExecuteOnDay7(object obj)
        {
            Day7Color = Color.FromHex("#2baae1");
            Day1Color = Day30Color = Color.FromHex("#4C4C4C");
            _toDate = DateTime.Now.ToString("MM-dd-yyyy");
            _fromDate = DateTime.Now.AddDays(-7).ToString("MM-dd-yyyy");
            Calledlist = new ObservableCollection<LeadDetailsList>();
            Pendinglist = new ObservableCollection<LeadDetailsList>();
            await GetLeadsCalledList(0, _userID, _fromDate, _toDate);
            await GetLeadsPendingList(0, _userID, _fromDate, _toDate);
            TransferLeadsList = new List<LeadDetailsList>();
        }
        public async void ExecuteOnDay30(object obj)
        {
            Day30Color = Color.FromHex("#2baae1");
            Day7Color = Day1Color = Color.FromHex("#4C4C4C");
            _toDate = DateTime.Now.ToString("MM-dd-yyyy");
            _fromDate = DateTime.Now.AddDays(-30).ToString("MM-dd-yyyy");
            Calledlist = new ObservableCollection<LeadDetailsList>();
            Pendinglist = new ObservableCollection<LeadDetailsList>();
            await GetLeadsCalledList(0, _userID, _fromDate, _toDate);
            await GetLeadsPendingList(0, _userID, _fromDate, _toDate);
            TransferLeadsList = new List<LeadDetailsList>();
        }
        private async void ExecuteOnLoadMoreItems(object obj)
        {
            if (Calledlist.Any())
                await GetLeadsCalledList(Calledlist.LastOrDefault().Id, _userID, _fromDate, _toDate);
        }
        public async Task GetLeadsCalledList(int lastrecordsid, int userID, string fromDate, string toDate)
        {
            try
            {
                ShowLoading();
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    HttpClientHelper apicall = new HttpClientHelper(String.Format(ApiUrls.GetLeadsCalledListbyuseridUrl, userID, lastrecordsid, fromDate, toDate), string.Empty);
                    var response = await apicall.Get<LeadDetailsModel>();

                    if (response != null && response.Result && response.Object != null && response.Message.Equals("Success"))
                    {
                        UserName = response.Object.Username;
                        foreach (var lead in response.Object.CalledList)
                            Calledlist.Add(lead);

                        LeadsCalledCount = Calledlist.Count() + "/" + response.TotalCount;

                        if (Calledlist.Count() < 20 || Calledlist.Count() == response.TotalCount)
                            LoadMoreOption = LoadMoreOption.None;
                        else
                            LoadMoreOption = LoadMoreOption.Manual;
                    }
                    else
                        LoadMoreOption = LoadMoreOption.None;
                }
                else
                {
                    HideLoading();
                    await UserDialogs.Instance.AlertAsync(AppConstant.NETWORK_FAILURE, "", "Ok");
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
                HideLoading();
            }
            HideLoading();
        }
        private async void ExecuteOnLoadMorePendingItems(object obj)
        {
            if (Pendinglist.Any())
                await GetLeadsPendingList(Pendinglist.LastOrDefault().Id, _userID, _fromDate, _toDate);
        }

        //public async void GetLeadsSearch(int status, string searchvalue, int userid, int lastrecordsid, string fromDate, string toDate)
        //{
        //    var list = new ObservableCollection<LeadDetailsList>();
        //    int statusId = 0;
        //    if (Selectedstatus != null)
        //        statusId = Selectedstatus.Id;
        //    try
        //    {
        //        ShowLoading();
        //        var current = Connectivity.NetworkAccess;
        //        if (current == NetworkAccess.Internet)
        //        {
        //            var userId = Settings.CRM_UserId; //await SecureStorage.GetAsync(AppConstant.UserId);
        //            HttpClientHelper apicall = new HttpClientHelper(String.Format(ApiUrls.SearchForPerformance, statusId, userId, searchvalue, fromDate, toDate), string.Empty);
        //            var response = await apicall.Get<LeadDetailsList>();
        //            if (response != null && response.Result && response.List != null && response.Message.Equals("Success"))
        //            {
        //                foreach (var lead in response.List)
        //                    list.Add(lead);
        //                var data = new ObservableCollection<LeadDetailsList>(list.OrderByDescending(o => o.CreationDate));
        //                list = data;
        //            }
        //            else
        //            {
        //                HideLoading();
        //                await ShowAlert("Record Not Found");
        //            }
        //        }
        //        else
        //        {
        //            HideLoading();
        //            await UserDialogs.Instance.AlertAsync(AppConstant.NETWORK_FAILURE, "", "Ok");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await ShowAlert(ex.Message);
        //        HideLoading();
        //    }
        //    HideLoading();
        //    return list;
        //}

        public async Task GetLeadsPendingList(int lastrecordsid, int userID, string fromDate, string toDate)
        {
            try
            {
                ShowLoading();
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    HttpClientHelper apicall = new HttpClientHelper(String.Format(ApiUrls.GetLeadsPendingListbyuseridUrl, userID, lastrecordsid, fromDate, toDate), string.Empty);
                    var response = await apicall.Get<LeadDetailsModel>();
                    if (response != null && response.Result && response.Object != null && response.Message.Equals("Success"))
                    {
                        UserName = response.Object.Username;
                        foreach (var lead in response.Object.PendingList)
                            Pendinglist.Add(lead);

                        LeadsPendingCount = Pendinglist.Count() + "/" + response.TotalCount;

                        if (Pendinglist.Count() < 20 || Pendinglist.Count() == response.TotalCount)
                            LoadMorePendingOption = LoadMoreOption.None;
                        else
                            LoadMorePendingOption = LoadMoreOption.Manual;
                    }
                    else
                        LoadMorePendingOption = LoadMoreOption.None;
                }
                else
                {
                    HideLoading();
                    await UserDialogs.Instance.AlertAsync(AppConstant.NETWORK_FAILURE, "", "Ok");
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
                HideLoading();
            }
            HideLoading();
        }
        public async void ExecuteOnDownloadLead(object obj)
        {

            var toDate = DateTime.Now.ToString("MM-dd-yyyy");
            var fromDate = DateTime.Now.AddDays(-_count).ToString("MM-dd-yyyy");
            HttpClientHelper apicall = new HttpClientHelper(String.Format(ApiUrls.CheckForDownloadLeads, _userID, fromDate, toDate), string.Empty);
            var response = await apicall.Get<string>();
            if (response == "Success")
            {
                var todate = DateTime.Now.ToString("MM-dd-yyyy");
                var fromdate = DateTime.Now.AddDays(-_count).ToString("MM-dd-yyyy");
                Uri uri = new Uri(string.Format("http://flylight.connekt.in///api/service/downloadleads?adminid={0}&fromdate={1}&todate={2}", _userID, fromdate, todate));
                await Browser.OpenAsync(uri);
                await Task.Delay(100);
                await _navigation.PushAsync(new SuccessfullyDownloaded());
            }
            else
            {
                HideLoading();
                await ShowAlert("No Record found");
            }

        }
        public void ExecuteOnAddNewLead(object obj)
        {
            App.MasterDetailPage.Detail = new NavigationPage(new AddLead(null))
            {
                BarBackgroundColor = Color.FromHex(App.nav_bar_color),
                BarTextColor = Color.FromHex(App.nav_bar_text_color)
            };
        }
        public async void ExecuteOnTransferLeads(object obj)
        {
            if (TransferLeadsList.Count() > 0)
                await _navigation.PushAsync(new TransferLeads(TransferLeadsList, _userID));
            else
                await ShowAlert("Please select atleast single lead.");
        }
        private async Task<ObservableCollection<StatusData>> GetStatusData()
        {
            var list = new ObservableCollection<StatusData>();
            try
            {
                ShowLoading();
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var userId = Settings.CRM_UserId; //await SecureStorage.GetAsync(AppConstant.UserId);
                    HttpClientHelper apicall = new HttpClientHelper(String.Format(ApiUrls.GetstatusListUrl, userId), string.Empty);
                    var response = await apicall.Get<GetStatus>();
                    if (response != null && response.Result && response.List != null)
                    {
                        foreach (var status in response.List)
                            list.Add(status);

                        var data = new ObservableCollection<StatusData>(list.OrderByDescending(o => o.CreationDate));
                        list = data;
                    }
                    else
                    {
                        HideLoading();
                        await ShowAlert("Record Not Found.");
                    }
                }
                else
                {
                    HideLoading();
                    await UserDialogs.Instance.AlertAsync(AppConstant.NETWORK_FAILURE, "", "Ok");
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
                HideLoading();
            }
            HideLoading();
            return list;
        }
        #endregion
    }
}

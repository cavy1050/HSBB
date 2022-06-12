using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Ioc;
using Prism.Mvvm;
using HSBB.Services;

namespace HSBB.Models
{
    public class QueryModel : BindableBase
    {
        DateTime beginDate = DateTime.Now.Date;
        public DateTime BeginDate
        {
            get => beginDate;
            set => SetProperty(ref beginDate, value);
        }

        DateTime endDate = DateTime.Now.Date;
        public DateTime EndDate
        {
            get => endDate;
            set => SetProperty(ref endDate, value);
        }

        List<QueryNetworkRegisterDataType> queryNetworkRegisterDataTypes;
        public List<QueryNetworkRegisterDataType> QueryNetworkRegisterDataTypes
        {
            get => queryNetworkRegisterDataTypes;
            set => queryNetworkRegisterDataTypes = value;
        }

        List<QueryNativeRegisterDataType> queryNativeRegisterDataTypes;
        public List<QueryNativeRegisterDataType> QueryNativeRegisterDataTypes
        {
            get => queryNativeRegisterDataTypes;
            set => queryNativeRegisterDataTypes = value;
        }

        CollectionView nativeQueryResult;
        public CollectionView NativeQueryResult
        {
            get => nativeQueryResult;
            set => SetProperty(ref nativeQueryResult, value);
        }

        CollectionView networkQueryResult;
        public CollectionView NetworkQueryResult
        {
            get => networkQueryResult;
            set => SetProperty(ref networkQueryResult, value);
        }

        bool isNetworkService;
        public bool IsNetworkService
        {
            get => isNetworkService;
            set => SetProperty(ref isNetworkService, value);
        }

        int currentPage;
        public int CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        int totalRecordCount;
        public int TotalRecordCount
        {
            get => totalRecordCount;
            set
            {
                totalRecordCount = value;
                TotalRecordText = "共" + value.ToString() + "条记录";
            }
        }

        string totalRecordText;
        public string TotalRecordText
        {
            get => totalRecordText;
            set => SetProperty(ref totalRecordText, value);
        }

        IContainerProvider containerProvider;
        IApplictionController applictionController;
        IDataBaseController dataBaseController;

        string defaultDataBaseServiceType;

        public QueryModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            this.applictionController = containerProviderArgs.Resolve<IApplictionController>();

            defaultDataBaseServiceType = applictionController.ConfigSettings["defaultDataBaseServiceType"];
        }

        public void LoadData()
        {
            int pageID = 1, rowID = 1;

            this.dataBaseController = containerProvider.Resolve<IDataBaseController>(defaultDataBaseServiceType);
            if (defaultDataBaseServiceType == "NetWork")
            {
                QueryNetworkRegisterDataTypes = dataBaseController.Query<QueryNetworkRegisterDataType>(BeginDate.ToShortDateString(), EndDate.ToShortDateString()).ToList();
                NetworkQueryResult = (CollectionView)CollectionViewSource.GetDefaultView(QueryNetworkRegisterDataTypes);
                TotalRecordCount = NetworkQueryResult.Count;
                IsNetworkService = true;
                NetworkQueryResult.Filter = PagingFilter;
            }            
            else
            {
                QueryNativeRegisterDataTypes = dataBaseController.Query<QueryNativeRegisterDataType>(BeginDate.ToShortDateString(), EndDate.ToShortDateString()).ToList();

                foreach (QueryNativeRegisterDataType queryNativeRegisterDataType in QueryNativeRegisterDataTypes)
                {
                    queryNativeRegisterDataType.RowID = rowID;
                    queryNativeRegisterDataType.PageID = pageID;

                    if (rowID < 10)
                        rowID++;
                    else
                    {
                        rowID = 1;
                        pageID++;
                    }
                        
                }

                NativeQueryResult = (CollectionView)CollectionViewSource.GetDefaultView(QueryNativeRegisterDataTypes);
                TotalRecordCount = NativeQueryResult.Count;
                IsNetworkService = false;
                NativeQueryResult.Filter = PagingFilter;
            }       
 
        }

        private bool PagingFilter(object obj)
        {
            return defaultDataBaseServiceType == "NetWork" ? (obj as QueryNetworkRegisterDataType).PageID == CurrentPage : (obj as QueryNativeRegisterDataType).PageID == CurrentPage;
        }

        public void RefreshData()
        {
            if (defaultDataBaseServiceType == "NetWork")
                NetworkQueryResult.Refresh();
            else
                NativeQueryResult.Refresh();
        }
    }
}

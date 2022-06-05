using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using Prism.Commands;
using MaterialDesignThemes.Wpf;
using Npoi.Mapper;
using HSBB.Models;
using HSBB.Services;

namespace HSBB.ViewModels
{
    public class QueryViewModel : BindableBase,INavigationAware
    {
        QueryModel queryModel;
        public QueryModel QueryModel
        {
            get => queryModel;
            set => SetProperty(ref queryModel, value);
        }

        ISnackbarMessageQueue messageQueue;
        public ISnackbarMessageQueue MessageQueue
        {
            get => messageQueue;
            set => SetProperty(ref messageQueue, value);
        }

        IContainerProvider containerProvider;
        IRegionManager regionManager;
        IAppConfigController appConfigController;
        NetWorkDataBaseController networkDataBaseController;

        List<QueryOutputParamerterType> queryOutputParamerterTypes;

        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand NavigateToRegisterCommand { get; private set; }
        public DelegateCommand QueryCommand { get; private set; }
        public DelegateCommand ExportCommand { get; private set; }
        public DelegateCommand NavigateFirstPageCommand { get; private set; }
        public DelegateCommand NavigateBeforePageCommand { get; private set; }
        public DelegateCommand NavigateNextPageCommand { get; private set; }
        public DelegateCommand NavigateLastPageCommand { get; private set; }
        public DelegateCommand NavigateCurrentPageCommand { get; private set; }

        public QueryViewModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            this.QueryModel = new QueryModel();
            this.regionManager = containerProviderArgs.Resolve<IRegionManager>();
            this.MessageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
            NavigateToRegisterCommand = new DelegateCommand(OnNavigateToRegister);
            QueryCommand= new DelegateCommand(OnQuery);
            ExportCommand = new DelegateCommand(OnExport);

            NavigateFirstPageCommand = new DelegateCommand(OnNavigateFirstPage);
            NavigateBeforePageCommand = new DelegateCommand(OnNavigateBeforePage);
            NavigateNextPageCommand = new DelegateCommand(OnNavigateNextPage);
            NavigateLastPageCommand = new DelegateCommand(OnNavigateLastPage);
            NavigateCurrentPageCommand = new DelegateCommand(OnNavigateCurrentPage);
        }

        private void OnWindowLoaded()
        {
            this.appConfigController = containerProvider.Resolve<IAppConfigController>();
            this.networkDataBaseController = (NetWorkDataBaseController)containerProvider.Resolve<IDataBaseController>("NetWork");
        }

        private void OnExport()
        {
            var mapper = new Mapper();

            mapper.Save(appConfigController.AppEnvironmentSetting.ExportXlsFilePath, queryOutputParamerterTypes, sheetIndex: 1, overwrite: true, xlsx: false);

            MessageQueue.Enqueue("成功导出文件至桌面目录!");
        }

        private void OnNavigateCurrentPage()
        {
            QueryModel.PagingResult.Refresh();
        }

        private void OnNavigateLastPage()
        {
            QueryModel.CurrentPage = (int)Math.Ceiling(QueryModel.TotalRecordCount / 10.0);
            QueryModel.PagingResult.Refresh();
        }

        private void OnNavigateNextPage()
        {
            if (QueryModel.CurrentPage <= Math.Ceiling(QueryModel.TotalRecordCount / 10.0))
            {
                QueryModel.CurrentPage += 1;
                QueryModel.PagingResult.Refresh();
            }
        }

        private void OnNavigateBeforePage()
        {
            if (QueryModel.CurrentPage>1)
            {
                QueryModel.CurrentPage -= 1;
                QueryModel.PagingResult.Refresh();
            }
        }

        private void OnNavigateFirstPage()
        {
            QueryModel.CurrentPage = 1;
            QueryModel.PagingResult.Refresh();
        }

        private void OnQuery()
        {
            queryOutputParamerterTypes = networkDataBaseController.Query(QueryModel.BeginDate.ToShortDateString(),QueryModel.EndDate.ToShortDateString());

            QueryModel.LoadData(queryOutputParamerterTypes);
        }

        private void OnNavigateToRegister()
        {
            regionManager.RequestNavigate("MainRegion", "RegisterView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
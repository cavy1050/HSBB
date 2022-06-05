using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Mvvm;

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

        CollectionView pagingResult;
        public CollectionView PagingResult
        {
            get => pagingResult;
            set => SetProperty(ref pagingResult, value);
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

        public void LoadData(List<QueryOutputParamerterType> queryOutputParamerterTypesArgs)
        {
            PagingResult= (CollectionView)CollectionViewSource.GetDefaultView(queryOutputParamerterTypesArgs);

            TotalRecordCount = PagingResult.Count;

            CurrentPage = 1;
            PagingResult.Filter = PagingFilter;
        }

        private bool PagingFilter(object obj)
        {
            return (obj as QueryOutputParamerterType).PageID == CurrentPage;
        }
    }
}

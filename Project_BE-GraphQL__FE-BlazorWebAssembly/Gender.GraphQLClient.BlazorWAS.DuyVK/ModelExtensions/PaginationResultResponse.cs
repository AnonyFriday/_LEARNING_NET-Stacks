using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.ModelExtensions
{
    public class PaginationResultResponse<T> where T : class
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public T Items { get; set; }
    }
}

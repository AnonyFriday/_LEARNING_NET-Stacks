namespace Gender.Repositories.DuyVK.ModelExtensions
{
    public class PaginationResultResponse<T> where T : class
    {
        public int TotalItems { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public T Items { get; set; }
    }
}

namespace Gender.Repositories.DuyVK.ModelExtensions
{
    public class SearchRequest
    {
        // /api/something?CurrentPage=&PageSize=
        /*
         You get 
            searchRequest.CurrentPage == null
            searchRequest.PageSize == null
         */

        public int? CurrentPage { get; set; } = 1; // ← this is ignored if "?CurrentPage=" is provided, ? only apply if not sending CurrentPage
        public int? PageSize { get; set; } = 5;
    }

    public class SearchMenstrualCycleReminderRequest : SearchRequest
    {
        public string? Title { get; set; } = null;
        public string? ColorCode { get; set; } = null;
        public double? ImportantScore { get; set; } = null;
    }
}

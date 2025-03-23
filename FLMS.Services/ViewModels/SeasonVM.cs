namespace FLMS.Web.Areas.ViewModels
{
    public class SeasonVM
    {
        public List<SeasonVMs> seasonVMs { get; set; }
    }
    public class SeasonVMs
    {
        public int id { get; set; }
        public string seasonName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool isCurrentSeason { get; set; }
    }
}

namespace SatoshisMarketplace.Web.Models.Admin
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public List<CategoryViewModel> SubCategories { get; set; }
    }
}

namespace SatoshisMarketplace.Web.Models.Product
{
    public class ProductsViewModel
    {
        public List<ProductViewModel> MyProducts { get; set; } = new List<ProductViewModel>();

        public List<ProductViewModel> MyPurchases { get; set; } = new List<ProductViewModel>();

        public List<ProductViewModel> MyFavorites { get; set; } = new List<ProductViewModel>();
    }
}

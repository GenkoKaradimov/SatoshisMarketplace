namespace SatoshisMarketplace.Web.Models.Product
{
    public class ProductsViewModel
    {
        public List<ManageProductViewModel> MyProducts { get; set; } = new List<ManageProductViewModel>();

        public List<ManageProductViewModel> MyPurchases { get; set; } = new List<ManageProductViewModel>();

        public List<ManageProductViewModel> MyFavorites { get; set; } = new List<ManageProductViewModel>();
    }
}

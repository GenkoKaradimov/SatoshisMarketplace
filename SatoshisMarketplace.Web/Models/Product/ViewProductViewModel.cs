using SatoshisMarketplace.Entities;
using System.ComponentModel.DataAnnotations;

namespace SatoshisMarketplace.Web.Models.Product
{
    public class ViewProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public uint Price { get; set; }

        public DateTime FirstPublication { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsListed { get; set; }

        public bool IsOwner { get; set; }

        public bool IsBoughtByUser { get; set; }

        public bool IsAddedFavoritis { get; set; }

        public bool IsUserLoggedIn { get; set; }

		public int? CategoryId { get; set; }

        public string CategoryPath { get; set; }

        public string OwnerUsername { get; set; }

        public ICollection<int> Images { get; set; } = new List<int>();

        public ICollection<ProductFileViewModel> Files { get; set; } = new List<ProductFileViewModel>();

        public ICollection<Models.Product.TagViewModel> Tags { get; set; } = new List<TagViewModel>();
    }
}

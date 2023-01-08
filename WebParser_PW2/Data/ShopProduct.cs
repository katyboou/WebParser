using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebParser_PW2.Data
{
    public class ShopProduct
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [ForeignKey(nameof(Shop))]

        public int ShopId { get; set; }

        public Shop? Shop { get; set; }

        public string Link { get; set; }

        public double Price { get; set; }
    }
}

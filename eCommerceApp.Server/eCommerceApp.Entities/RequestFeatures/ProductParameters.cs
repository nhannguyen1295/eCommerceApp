namespace eCommerceApp.Entities.RequestFeatures
{
    public class ProductParameters : RequestParameters
    {

        public string SearchTerm { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; } = int.MaxValue;
        public bool ValidPriceRange => MaxPrice > MinPrice;
    }
}
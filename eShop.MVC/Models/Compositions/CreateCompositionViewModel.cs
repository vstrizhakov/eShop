namespace eShop.MVC.Models.Compositions
{
    public class CreateCompositionViewModel
    {
        public IFormFile Image { get; set; }
        public IEnumerable<CreateProduct> Products { get; set; }
    }
}

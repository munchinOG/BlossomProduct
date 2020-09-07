namespace BlossomProduct.Core.ViewModels
{
    public class ProductEditVm : ProductCreateVm
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}

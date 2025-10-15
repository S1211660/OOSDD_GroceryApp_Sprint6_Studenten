using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly GlobalViewModel _global;

        public ObservableCollection<Product> Products { get; set; }

        public ProductViewModel(IProductService productService, GlobalViewModel global)
        {
            _productService = productService;
            _global = global;

            Products = new ObservableCollection<Product>();
            foreach (var p in _productService.GetAll())
                Products.Add(p);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            Products.Clear();
            foreach (var p in _productService.GetAll())
                Products.Add(p);
        }

        [RelayCommand]
        public async Task ShowNewProduct()
        {
            if (_global.Client != null && _global.Client.Role == Role.Admin)
                await Shell.Current.GoToAsync(nameof(NewProductView), true);
        }
    }
}


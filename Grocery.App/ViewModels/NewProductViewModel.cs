using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Globalization;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : ObservableObject
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private int stock = 0;

        [ObservableProperty]
        private DateTime shelfLife = DateTime.Now.AddMonths(1);

        [ObservableProperty]
        private decimal price = 0.00m;

        [ObservableProperty]
        private string priceText = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool hasError = false;

        public NewProductViewModel(IProductService productService)
        {
            _productService = productService;
        }

        [RelayCommand]
        private async Task SaveProduct()
        {
            try
            {
                HasError = false;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Name))
                {
                    ShowError("Productnaam mag niet leeg zijn.");
                    return;
                }

                if (Stock < 0)
                {
                    ShowError("Voorraad kan niet negatief zijn.");
                    return;
                }

                var raw = (PriceText ?? string.Empty).Trim();

                if (!decimal.TryParse(raw.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
                {
                    ShowError("Ongeldige prijs. Gebruik bijvoorbeeld 2,99 of 2.99.");
                    return;
                }
                if (parsed < 0)
                {
                    ShowError("Prijs kan niet negatief zijn.");
                    return;
                }

                Price = decimal.Round(parsed, 2);

                if (ShelfLife.Date < DateTime.Now.Date)
                {
                    ShowError("THT datum mag niet in het verleden liggen.");
                    return;
                }

                Product newProduct = new(0, Name, Stock, DateOnly.FromDateTime(ShelfLife), Price);

                _productService.Add(newProduct);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ShowError($"Fout bij opslaan: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }
    }
}
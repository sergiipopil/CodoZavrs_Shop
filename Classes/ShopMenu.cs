﻿using PhoneNumbers;
using Shop.Classes.account;
using Shop.Classes.Extensions;
using Shop.Classes.forms;
using Shop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskShop.Classes;

namespace Shop.Classes
{
    public class ShopMenu
    {
        public static ProductManager product = new ProductManager();
        public Shop shop = new Shop();
        public ShopManager shopManager = new ShopManager();

        public static Seller seller = new Seller("Tom")
        { Age = 34 };
        public SellerManager sellerManager = new SellerManager(seller, product);

        public ShopRegData shopRegInfo = new()
        {
            DateCreadeted = new DateTime(2023, 5, 1),
            OwnerName = "Codo",
            OwnerSurName = "Zavrs",
            RegNumber = "UA7777777777"
        };

        public ShopHelperData shopHelperData = new ShopHelperData()
        {
            Phone = "+777777",
            WebSite = "https://sim23.ua/",
            Email = "codozavrsShop@gmail.com"
        };

        private CustomerManager customer = new();

        public ShopMenu()
        {
            MainMenu(); 
        }
        private void InitProductList()
        {
            product.ProductList = new List<Product>()
            {
                new Product() { Id = 1, Title = "Сhocolate", Count = 25, Price = 65.3m},
                new Product() { Id = 2, Title = "Milk", Count = 30, Price = 52.5m, Weight=1000 },
                new Product() { Id = 3, Title = "Coffee", Count = 45, Price = 247.8m, Weight=900 },
                new Product() { Id = 4, Title = "Tea", Count = 20, Price = 195, Weight=500 },
                new Product() { Id = 5, Title = "Sugar", Count = 120, Price = 35, Weight=1000 }
            };
        }

        // Код Search Product:
        private List<Product> SearchProducts(string searchTerm)
        {
            return product.ProductList
                .Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private async void ProductList(ShopMenu shopMenu)
        {
            try
            {
                if (shopMenu == null)
                {
                    Console.WriteLine("Error: Shop menu is null.");
                    return;
                }

                string searchTerm;

                while (true)
                {
                    Console.WriteLine("Enter a search term or type 'exit' to quit:");
                    Console.WriteLine("Sw:");

                    searchTerm = Console.ReadLine();

                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        Console.WriteLine("Error: Search term is empty.");
                        continue;
                    }

                    if (searchTerm.ToLower() == "exit")
                    {
                        break;
                    }

                    List<Product> filteredProducts = shopMenu.SearchProducts(searchTerm.ToLower());

                    if (filteredProducts.Count > 0)
                    {
                        filteredProducts = filteredProducts.OrderBy(p => p.Price).ThenBy(p => p.Id).ToList();
                        shopMenu.ShowFilteredProducts(filteredProducts);
                        product.ShowProductsList();
                    }
                    else
                    {
                        Console.WriteLine("No products found.");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ShowFilteredProducts(List<Product> filteredProducts)
        {
            Console.WriteLine("\nFiltered Products:\n");

            foreach (var product in filteredProducts)
            {
                if (product.Count > 0 && product.Price > 0)
                {
                    Console.WriteLine($"Id: {product.Id}, Title: {product.Title}, Count: {product.Count}, PricePerKg: {product.Price}");
                }
                else
                {
                    Console.WriteLine($"Id: {product.Id}, Title: {product.Title}, Price: {product.Price}");
                }

            }

            Console.WriteLine("Full list of products\n\n");

            Console.WriteLine("Available other products:");
        }
        // Кiнець Search Product

        private void MainMenu()
        {
            InitProductList();
            Console.Clear();
            Console.WriteLine($"You are welcome to {shop.Name}\n");
            Console.WriteLine(shopRegInfo.ToString());
            Console.WriteLine($"If you want to contact us:phone - {shopHelperData.Phone}, email - {shopHelperData.Email}, website - {shopHelperData.WebSite}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Main menu:\n\n" +
                "Press 0 - EXIT\n" +
                "Press 1 - Seller Mode\n" +
                "Press 2 - Buyer Mode\n" +
                "Press 3 - Rigst your account\n" +
                "Press 4 - To Loggin into your account\n" +
                "Press 5 - To Search the product\n");
            Console.ResetColor();
            Console.Write("Select menu item:");
            bool isCorrectMode = Enum.TryParse(Console.ReadLine(), out AppMode modeType);

            if (!isCorrectMode && !Enum.IsDefined(typeof(AppMode), modeType))
            {
                MainMenu();
            }
            else
            {
                switch (modeType)
                {
                    case AppMode.Exit:
                        Console.WriteLine("Thanks for visit us!");
                        Environment.Exit(0);
                        break;
                    case AppMode.Seller:
                        Console.Clear();
                        SellerMenu();
                        break;
                    case AppMode.Buyer:
                        Console.Clear();
                        BuyerMenu();
                        break;
                    case AppMode.Registration:
                        RegistrationForm registrationForm = new RegistrationForm();
                        registrationForm.NewRegistrationForm();
                        break;
                    case AppMode.Loggin:
                        LoginForm userLoggin = new LoginForm();
                        userLoggin.TryLogin();
                        break;

                    case AppMode.SearchProduct:
                        ProductList(this);
                        break;

                    default:
                        Console.WriteLine("Please enter your choose");
                        break;
                }
            }
        }
        private void SellerMenu()
        {
            Console.WriteLine("It`s seller mode");
            Console.WriteLine("Seller menu:\n\n" +
                "Press 0 - Return to Main Menu\n" +
                "Press 1 - sold item\n" +
                "Press 2 - Open the shop\n" +
                "Press 3 - Close the shop\n");

            Console.Write("Select menu item:");
            bool isCorrectMode = Enum.TryParse(Console.ReadLine(), out SellerMode sellerModeType);

            if (!isCorrectMode || !Enum.IsDefined(typeof(SellerMode), sellerModeType))
            {
                Console.Clear();
                SellerMenu();
            }
            else
            {
                switch (sellerModeType)
                {
                    case SellerMode.MainMenu:
                        MainMenu();
                        break;

                    case SellerMode.SoldItem:
                        Console.WriteLine("Sold item");
                        Console.WriteLine("Enter the product name, please: ");
                        var productName = Console.ReadLine();

                        Console.WriteLine("Enter the count of the product, please: ");
                        var enteredProductCount = Console.ReadLine();
                        if (int.TryParse(enteredProductCount, out var productCount))
                        {
                            sellerManager.SoldProduct(productName, productCount);
                        }
                        else
                        {
                            Console.WriteLine("Incorrect the count of the product is entered!");
                        }

                        SellerMenu();
                        break;
                    case SellerMode.OpenShop:
                        shopManager.Open(shop);
                        Console.WriteLine("Open store!Press ENTER to continue!");
                        Console.ReadLine();
                        MainMenu();
                        break;
                    case SellerMode.CloseShop:
                        shopManager.Close(shop);      
                        break;
                }
            }
        }
        private void BuyerMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Buyer menu:\n\n" +
                "Press 0 - Return to Main Menu\n" +
                "Press 1 - Get Store Card\n" +
                "Press 2 - Buy item\n" +
                "Press 3 - Return item\n" +
                "Press 4 - Get all items\n" +
                "Press 5 - Get item detais by Id\n" +
                "Press 6 - Get item detais by Title\n" +
                "Press 7 - Get info all items in basket\n" +
                "Press 8 - Get info item in basket by title\n"+
                "Press 9 - Get Shop Status\n");
            Console.ResetColor();
            Console.Write("Select menu item:");
            bool isCorrectMode = Enum.TryParse(Console.ReadLine(), out BuyerMode buyerModeType);

            if (!isCorrectMode && !Enum.IsDefined(typeof(BuyerMode), buyerModeType))
            {
                Console.Clear();
                BuyerMenu();
            }
            else
            {
                switch (buyerModeType)
                {
                    case BuyerMode.MainMenu:
                        MainMenu();
                        break;
                    case BuyerMode.GetStoreCard:
                        customer.GetStoreCard(Customer.storeCard);
                        break;
                    case BuyerMode.BuyItem:
                        customer.BuyProduct(product);
                        break;
                    case BuyerMode.ReturnItem:
                        Console.WriteLine("All products in basket: ");
                        customer.GetBasketItems();
                        customer.DeleteProductFromBasket(product);
                        break;
                    case BuyerMode.ItemDetailsById:
                        Console.Write("Please enter Id of product which you want get details:");
                        bool isCorrectId = int.TryParse(Console.ReadLine(), out int productId);
                        if (isCorrectId)
                        {
                            product.GetProductDetail(productId);
                        }
                        break;
                    case BuyerMode.ItemDetailsByTitle:
                        Console.Write("Please enter Title of product which you want get details:");
                        string title = Console.ReadLine();
                        product.GetProductDetail(title);
                        break;
                    case BuyerMode.GetAllItems:
                        product.ShowProductsList();
                        break;
                    case BuyerMode.GetAllItemsInBasket:
                        customer.GetBasketItems();
                        break;
                    case BuyerMode.GetAllItemsInBaskeByTitle:
                        Console.Write("Please enter Title of product which you want see info:");
                        string itemTitle = Console.ReadLine();
                        customer.GetBasketItems(itemTitle);
                        break;
                    case BuyerMode.GetShopStatus:
                        Console.WriteLine(OpenExtensions.GetStatusMessage(shop));
                        break;
                }
                BuyerMenu();
            }
        }
    }
}

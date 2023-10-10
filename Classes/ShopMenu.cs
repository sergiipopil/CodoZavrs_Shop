﻿using PhoneNumbers;
using Shop.LoginNewTestMark.Forms;

using Shop.Classes.account;
using Shop.Classes.Extensions;
using Shop.Classes.forms;
using Shop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskShop.Classes;
using Shop.Core.Classes;
using Shop.Services;

namespace Shop.Classes
{
    public class ShopMenu
    {
        public static ProductManager product = new ProductManager();
        public Shop shop = new Shop();
        public ShopManager shopManager = new ShopManager();

        public static Seller seller = new Seller("Tom") { Age = 34 };
        Record_Seller RecordSeller1 = new Record_Seller(1, "Bob", new DateTime(2023, 12, 01));

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

        private Customer customer1 = new Customer("Ivan", "Ivanchenko", 3806661177, 5000);
        private CustomerRecord customerRecord = new(new DateTime(2002, 12, 01), 5);

        public ShopMenu()
        {
            InitProductList();
            MainMenu();
        }
        private void InitProductList()
        {
            product.ProductList = new List<Product>()
            {
                new Product() { Id = 1, Title = "Сhocolate", Count = 25, Price = 65.3m, Weight = 1000},
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
                "Press 5 - To Search the product\n" +
                "Press 6 - Popil-abstract\n");
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
                        LoginNewTestMark.Forms.RegistrationForm registrationForm = new LoginNewTestMark.Forms.RegistrationForm();
                        registrationForm.NewRegistrationForm();
                        break;
                    case AppMode.Loggin:
                        LoginNewTestMark.Forms.LoginForm userLoggin = new LoginNewTestMark.Forms.LoginForm();
                        userLoggin.TryLogin();
                        break;

                    case AppMode.SearchProduct:
                        ProductList(this);
                        break;
                    case AppMode.Popil_Abstract:
                        PopilSergii_Abstract();
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
                "Press 1 - Sold item\n" +
                "Press 2 - Change price item\n" +
                "Press 3 - Open the shop\n" +
                "Press 4 - Close the shop\n");

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

                    case SellerMode.ChangePrice:
                        product.ShowProductsList();
                        try
                        {
                            Console.Write("Please enter Id of product which you want change price:");
                            int productId = int.Parse(Console.ReadLine());
                            if (product.ProductList.Any(x => x.Id == productId))
                            {
                                Product selectedProduct = product.GetProduct(productId);
                                product.ShowMainProductInfo(selectedProduct);
                                if (selectedProduct != null)
                                {
                                    Console.Write("Please enter new Price(Format ##,##):");
                                    bool isCorrectPrice = decimal.TryParse(Console.ReadLine().Replace('.', ','),
                                        out decimal newProductPrice);
                                    if (isCorrectPrice && newProductPrice > 0)
                                    {
                                        product.ChangeProductPrice(selectedProduct, newProductPrice);
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Success! Price is changed!");
                                        product.ShowMainProductInfo(selectedProduct);
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("!!! Uncorrect price !!!");
                                        Console.ResetColor();
                                    }
                                }
                            }
                        }
                        catch (FormatException formatException)
                        {
                            LogService.Log(formatException);
                        }
                        catch (OverflowException overflowException)
                        {
                            LogService.Log(overflowException);
                        }
                        catch (Exception ex)
                        {
                            LogService.Log(ex);
                        }
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
                SellerMenu();
            }
        }
        private void BuyerMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Buyer menu:\n\n" +
                "Press 0 - Return to Main Menu\n" +
                "Press 1 - Get customer information\n" +
                "Press 2 - Get Store Card\n" +
                "Press 3 - Buy item\n" +
                "Press 4 - Return item\n" +
                "Press 5 - Get all items\n" +
                "Press 6 - Get item detais by Id\n" +
                "Press 7 - Get item detais by Title\n" +
                "Press 8 - Get item main info by Id\n" +
                "Press 9 - Get info all items in basket\n" +
                "Press 10 - Get info item in basket by title\n" +
                "Press 11 - Get Shop Status\n");

            Console.ResetColor();
            Console.Write("Select menu item:");
            try
            {
                BuyerMode buyerModeType = Enum.Parse<BuyerMode>(Console.ReadLine());
                if (!Enum.IsDefined(typeof(BuyerMode), buyerModeType))
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
                        case BuyerMode.GetCustomerInf:
                            customer1.GetInformationCustomer(customerRecord);
                            break;
                        case BuyerMode.GetStoreCard:
                            customer.GetStoreCard(Customer.storeCard);
                            break;
                        case BuyerMode.BuyItem:
                            customer.BuyProduct(product);
                            break;
                        case BuyerMode.ReturnItem:
                            Console.WriteLine("All products in basket:");
                            customer.GetBasketItems();
                            customer.DeleteProductFromBasket(product);
                            break;
                        case BuyerMode.ItemDetailsById:
                            Console.Write("Please enter Id of product which you want get details:");
                            try
                            {
                                int productIdDetail = int.Parse(Console.ReadLine());
                                product.GetProductDetail(productIdDetail);
                            }
                            //Here save StackTrace
                            catch (Exception ex)
                            {
                                LogService.Log(ex, false);
                                throw;
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
                        case BuyerMode.ItemMainDataById:
                            product.ShowProductsList();
                            Console.Write("Please enter Id of product which you want get details:");
                            try
                            {
                                int productId = int.Parse(Console.ReadLine());

                                if (product.ProductList.Any(x => x.Id == productId))
                                {
                                    product.ShowMainProductInfo(product.GetProduct(productId));
                                }
                            }
                            //Here re-write StackTrace
                            catch (Exception ex)
                            {
                                LogService.Log(ex, false);
                                throw new Exception($"Re-write StackTrace\n\n{ex.Message}");
                            }
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
            catch (ArgumentException argumentException)
            {
                LogService.Log(argumentException);
            }
            catch (OverflowException overflowException)
            {
                LogService.Log(overflowException);
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
            finally
            {
                BuyerMenu();
            }
        }

        private void PopilSergii_Abstract()
        {
            Console.WriteLine("===DEMONSTRATION HOMETASK6 (ABSTRACT)====\n");
            Product simpleProduct = product.GetProduct(1);
            ProductBase productBase = simpleProduct;
            
            Console.WriteLine($"SimpleProduct - Price:{simpleProduct.GetPrice()}");
            Console.WriteLine($"SimpleProduct - Count:{simpleProduct.GetCount()}");

            Console.WriteLine($"ProductBase - Price:{productBase.GetPrice()}");
            Console.WriteLine($"ProductBase - Count:{productBase.GetCount()}\n");

        }
    }
}

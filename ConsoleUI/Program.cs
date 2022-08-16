using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductTest();
            //CategoryTest();
            //OrderTest();
        }

        private static void OrderTest()
        {
            OrderManager orderManager = new OrderManager(new EfOrderDal());
            foreach (var item in orderManager.GetAll())
            {
                Console.WriteLine(item.ShipCity);
            }
        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var item in categoryManager.GetAll().Data)
            {
                Console.WriteLine(item.CategoryId + " " + item.CategoryName);
            }
        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal(),new CategoryManager(new EfCategoryDal()));
            Console.WriteLine("Hello World!");
            var result = productManager.GetProductDetails();
            if (result.Succes==true)
            {
                foreach (var item in result.Data)
                {
                    Console.WriteLine(item.ProductId + " " + item.ProductName + "  " + item.CategoryName + " " + item.UnitsInStock + " " + item.UnitsInStock);

                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            
        }
    }
}

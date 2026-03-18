using ConsoleApp1.CRUD;
using ConsoleApp1.EventExample;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiproTraining;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //CategoryCRUD cc = new CategoryCRUD();
            //Category c = new Category();
            // Add Operation
            //Console.WriteLine("Enter new Category name: ");
            //c.Name = Console.ReadLine();
            //Console.WriteLine("Enter new Id: ");
            //c.Id = Convert.ToInt32(Console.ReadLine());
            //cc.AddCategory(c);

            //Hard Coded Update
            //c.Id = 2;
            //c.Name = "North Litti Chokha";
            //Console.WriteLine(cc.UpdateCategory(c));

            //Delete
            //Console.WriteLine(cc.DeleteCategory(1));

            //Fetch
            //List<Category> clist = cc.CategoriesList();
            //foreach (Category ct in clist)
            //{
            //    Console.WriteLine($"ID : {ct.Id} | Name: {ct.Name}");
            //}

            //For Product

            //ProductCRUD pc = new ProductCRUD();
            //Product p = new Product()
            //{
            //    ProductId = 3,
            //    Name = "Daal Bhat",
            //    CategId = 3
            //};

            //Add

            //Console.WriteLine(pc.AddProduct(p));

            //updateProduct

            //Product prod = pc.UpdateProduct(p);

            //Console.WriteLine($"{prod.ProductId} | {prod.Name} | {prod.CategId}");

            //Delete
            //Console.WriteLine(pc.Delete(3));

            //Getting Data from DB

            //List<Product> products = pc.GetProducts();
            //foreach (Product product in products)
            //{
            //    Console.WriteLine($"{product.ProductId} | {product.Name} | {product.CategId}");
            //}


            //Console.WriteLine("1. Add");
            //Console.WriteLine("2. Update");
            //Console.WriteLine("3. Delete");
            //Console.WriteLine("4. Fetch");
            //Console.WriteLine("5. Exit");
            //Console.WriteLine("--------------------------------------------------------");

            //bool check = true;
            //while (check)
            //{
            //    Console.Write("Please choose your option: ");
            //    int choice = int.Parse(Console.ReadLine());
            //    Product p = new Product();
            //    ProductCRUD pc = new ProductCRUD();
            //    switch (choice)
            //    {
            //        case 1:
            //            Console.Write("Please enter Product Name: ");
            //            p.Name = Console.ReadLine();

            //            Console.Write("Please enter Category Id: ");
            //            p.CategId = int.Parse(Console.ReadLine());

            //            Console.WriteLine(pc.AddProduct(p));
            //            break;

            //        case 2:
            //            Console.Write("Enter Product Id to Update: ");
            //            p.ProductId = int.Parse(Console.ReadLine());

            //            Console.Write("Enter New Product Name: ");
            //            p.Name = Console.ReadLine();

            //            Console.Write("Enter New Category Id: ");
            //            p.CategId = int.Parse(Console.ReadLine());

            //            Product updated = pc.UpdateProduct(p);
            //            Console.WriteLine("Product Updated Successfully!");
            //            break;

            //        case 3:
            //            Console.Write("Enter Product Id to Delete: ");
            //            int deleteId = int.Parse(Console.ReadLine());

            //            Console.WriteLine(pc.Delete(deleteId));
            //            break;

            //        case 4:
            //            List<Product> products = pc.GetProducts();

            //            Console.WriteLine("---- Product List ----");
            //            foreach (Product prod in products)
            //            {
            //                Console.WriteLine($"Id: {prod.ProductId}  Name: {prod.Name}  Category: {prod.CategId}");
            //            }
            //            break;

            //        case 5:
            //            check = false;
            //            Console.WriteLine("Exiting...");
            //            break;

            //        default:
            //            Console.WriteLine("Invalid Choice!");
            //            break;
            //    }

            //}

            //Custom Exception Handling

            //int age = 15;

            //try
            //{
            //    if (age < 18)
            //    {
            //        throw new AgeException("Age must be 18 or above.");
            //    }

            //    Console.WriteLine("Age is Valid.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Custom exception caught: " + ex.Message);
            //}

            //Events

            EventPublishers eP = new EventPublishers();
            EventSubscriber eS = new EventSubscriber();

            eP.myEvent += eS.SubscriberMethod;
            eP.myMethod("Hello from Publisher!");



        }
    }
}

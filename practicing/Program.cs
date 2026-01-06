using Npgsql;
using practicing;


class Program
    {
        static async Task Main(string[] args)
        {
          
            string connString = "Host=localhost;Port=5432;Database=Ecommerce;Username=postgres;Password=1234";


            var repo = new ProductRepository(connString);
            // var allProducts = await repo.GetAllProducts();
            // foreach (var p in allProducts)
            // {
            //     //   Console.WriteLine(item.Id | item.Name | item.Price | item.CategoryName);
            //     Console.WriteLine($"ID: {p.Id} | Name: {p.Name} | Price: {p.Price} | Category: {p.CategoryName}");
            // }
            //
            
            
            
            // Console.WriteLine("Enter Product ID ... ");
            // if (int.TryParse(Console.ReadLine(), out int searchid))
            // {
            //     var product = await repo.GetProductById(searchid);
            //     if (product != null)
            //     {
            //         Console.WriteLine("ID: " + product.Id + " | Name: " + product.Name + " | Price: " + product.Price);
            //     }
            //     else
            //     {
            //         Console.WriteLine("Product not found");
            //     }
            // }
            //
            

            // var newProduct = new ProductDto
            // {
            //     Name = "LabTop Ninja",
            //     Price = 1000.00m,
            // };
            // await repo.AddProduct(newProduct);
            // if (int.TryParse(Console.ReadLine(), out int id) && decimal.TryParse(Console.ReadLine(), out decimal price))
            // { 
            //     await repo.UpdateProduct(id, price);
            //     
            // }

            // if (int.TryParse(Console.ReadLine(), out int id))
            // {
            //     await repo.DeleteProduct(id);
            // }
            //


        }
    }
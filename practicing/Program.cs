using Npgsql;
using practicing;


class Program
    {
        static async Task Main(string[] args)
        {
          
            // string connString = "Host=localhost;Port=5432;Database=Ecommerce;Username=postgres;Password=1234";
            //
            //
            // var repo = new ProductRepository(connString);
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

            
            //--------------
            string connString = "Host=localhost;Port=5432;Database=Ecommerce;Username=postgres;Password=1234";
            var db = new MyDbContext(connString);
            
            await using var conn = db.CreateConnection();
            await conn.OpenAsync();
            
            
            Console.WriteLine("Fetching top 3 expensive products (Skip first 1)...");

            // var products = await db.Products.Query()
            //     .Select("product_id", "name", "price")
            //     .Where("price", 500, ">")
            //     .OrderBy("price", ascending: false) 
            //     .Take(3) 
            //     .Skip(1) 
            //     .ExecuteListAsync(conn, reader => new ProductDto 
            //     {
            //         Id = reader.GetInt32(0),
            //         Name = reader.GetString(1),
            //         Price = reader.GetDecimal(2)
            //     });
            //
            //
            // foreach (var p in products)
            // {
            //     Console.WriteLine($"[ID: {p.Id}] {p.Name} - ${p.Price}");
            // }
            
            
            var qb = db.Products.Query()
                .Select("product_id", "name", "price")
                .Where("price", 500, ">")
                .OrderBy("price", ascending: false)
                .Take(3) 
                .Skip(1);
            
            string finalSql = qb.Build(); 
            Console.WriteLine("--- [Stage 1: Build] ---");
            Console.WriteLine("The SQL generated is: \n" + finalSql);
            //SELECT product_id, name, price
            //FROM products
            //WHERE price > @p0
            //ORDER BY price DESC
            //LIMIT 3 OFFSET 1
        
            
            
            Console.WriteLine("\n--- [Stage 2: Execute] ---");
            var products = await qb.ExecuteListAsync(conn, reader => new ProductDto 
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2)
            });
            
            foreach (var p in products)
            {
                Console.WriteLine($"[ID: {p.Id}] {p.Name} - ${p.Price}");
            }

        }
    }
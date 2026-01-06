# MiniEF - Custom C# ORM for PostgreSQL 🚀

A lightweight, high-performance Mini-ORM built from scratch in C# to interact with PostgreSQL using `Npgsql`. This project demonstrates a deep understanding of ORM architecture, Fluent API design, Reflection, and SQL security.

## ✨ Features

- **Fluent Query Builder**: Build complex SQL queries using a readable, chainable syntax.
- **Full CRUD Support**: Effortlessly Create, Read, Update, and Delete records.
- **Auto-Mapping**: Using C# Reflection to map database rows to DTOs automatically.
- **Pagination & Sorting**: Built-in support for `Take()`, `Skip()`, and `OrderBy()`.
- **SQL Injection Protection**: All queries are parameterized for maximum security.

## 🛠 Project Architecture

The project is divided into 4 main layers:
1. **Models (DTOs)**: Plain C# classes representing database tables.
2. **QueryBuilder**: The engine that converts C# logic into optimized SQL strings.
3. **MyDbSet<T>**: A generic gateway for each table, handling data execution and mapping.
4. **MyDbContext**: The central coordinator that manages database connections and sets.



## 🚀 How to Use

### 1. Initialize the Context
```csharp
var db = new MyDbContext("Your_Connection_String");
2. Query Data (Select)
C#

var products = await db.Products.Query()
    .Select("name", "price")
    .Where("price", 500, ">")
    .OrderBy("price", ascending: false)
    .Take(5)
    .ExecuteListAsync(conn, reader => new ProductDto { ... });
3. Add New Record (Insert)
C#

var newProduct = new ProductDto { Name = "Laptop", Price = 1500 };
await db.Products.AddAsync(newProduct);
4. Update & Delete
C#

// Partial Update
var changes = new Dictionary<string, object> { { "price", 1200 } };
await db.Products.UpdateAsync(id: 1, changes);

// Delete
await db.Products.DeleteAsync(id: 1);
🧠 What I Learned
Implementing Fluent API design patterns.

Leveraging Reflection to build dynamic SQL from objects.

Managing database connections and asynchronous operations in C#.

Designing a scalable architecture similar to Entity Framework Core.

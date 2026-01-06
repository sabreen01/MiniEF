# 🚀 MiniEF: Lightweight C# ORM for PostgreSQL

[![C#](https://img.shields.io/badge/Language-C%23-blue.svg)](https://dotnet.microsoft.com/en-us/languages/csharp)
[![Database](https://img.shields.io/badge/Database-PostgreSQL-336791.svg)](https://www.postgresql.org/)
[![ORM](https://img.shields.io/badge/Type-Mini--ORM-green.svg)]()

A high-performance, custom-built Mini-ORM designed to simplify database interactions in .NET. This project was built from the ground up to explore the inner workings of **Entity Framework Core**, focusing on **Reflection**, **Generic Patterns**, and **Fluent API** design.

---

## 🌟 Key Features

| Feature | Description |
| :--- | :--- |
| **🔗 Fluent API** | Chain methods like `.Select().Where().OrderBy()` for readable queries. |
| **⚡ Full CRUD** | Native support for Create, Read, Update, and Delete operations. |
| **🔮 Auto-Mapping** | Uses **Reflection** to map database records to C# objects automatically. |
| **🛡️ Secure by Design** | Fully parameterized queries to prevent **SQL Injection**. |
| **📑 Pagination** | Easy data paging with built-in `.Take()` and `.Skip()` methods. |

---

## 🏗️ Project Architecture

The engine is built on a modular 4-layer architecture to ensure separation of concerns:

1. **Models (DTOs):** Clean POCO classes representing the data schema.
2. **QueryBuilder:** The core logic engine that translates C# expressions into optimized SQL.
3. **MyDbSet<T>:** A generic repository pattern providing a gateway for table-specific operations.
4. **MyDbContext:** The central manager that coordinates connections and exposes database sets.

---

## 🚀 Quick Start Guide

### 1. Initialize the Context
Connect to your PostgreSQL instance with a simple setup:
```csharp
string connString = "Host=localhost;Database=Ecommerce;Username=postgres;Password=1234";
var db = new MyDbContext(connString);
2. Querying Data (Fluent Select)


var products = await db.Products.Query()
    .Select("name", "price")
    .Where("price", 500, ">")
    .OrderBy("price", ascending: false)
    .Take(5)
    .ExecuteListAsync(conn, reader => new ProductDto 
    {
        Name = reader.GetString(0),
        Price = reader.GetDecimal(1)
    });
3. Data Persistence (Insert, Update, Delete)


// Add New Record
var newProduct = new ProductDto { Name = "Gaming Laptop", Price = 1500 };
await db.Products.AddAsync(newProduct);

// Partial Update
var changes = new Dictionary<string, object> { { "price", 1200 } };
await db.Products.UpdateAsync(id: 1, changes);

// Delete Record
await db.Products.DeleteAsync(id: 1);


## 🧠 Technical Deep Dive: What I Learned


Building MiniEF was a journey through advanced .NET concepts:

Reflection Mastery: Dynamically reading object properties to generate INSERT statements.

Asynchronous Programming: Extensive use of Task, await, and Async DB drivers for non-blocking I/O.

Fluent Interface Design: Implementing method chaining to create a "Natural Language" feel for developers.

Defensive Programming: Handling database connections and disposing resources correctly using await using.

using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TinyNH.DemoStore.Core.Domain;

namespace TinyNH.DemoStore.ProductImporter
{
    /// <summary>
    /// WARNING: Dodgy demo code ahead
    /// A few concerns have been ignored in order to keep things simple:
    /// - no exception handling or logging, all happens in a single transaction etc
    /// </summary>
    public class Importer
    {
        private const string UnknownCategoryCode = "category-unknown";
        private const string UnknownSupplierCode = "supplier-unknown";

        private readonly ISessionFactory sessionFactory;
        private ISession session;

        private readonly Dictionary<string, Supplier> supplierCache = new Dictionary<string, Supplier>();
        private readonly Dictionary<string, Category> categoryCache = new Dictionary<string, Category>();


        public Importer(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Execute()
        {
            var catalogueData = GetCatalogueData();

            using (session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    ImportSuppliers(catalogueData.Suppliers);
                    ImportCategories(catalogueData.Categories);
                    ImportProducts(catalogueData.Products);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private CatalogueDataSource.CatalogueData GetCatalogueData()
        {
            var catalogueData = new CatalogueDataSource().GetCatalogueData();
            // Add data for special-case null objects to use for potential missing / unknown data
            // Adding them here means ensures that it will always be present in db
            catalogueData.Categories.Add(new CatalogueDataSource.CategoryData { Code = UnknownCategoryCode, Name = "Unknown Category" });
            catalogueData.Suppliers.Add(new CatalogueDataSource.SupplierData { Code = UnknownSupplierCode, Name = "Unknown Supplier" });
            return catalogueData;
        }

        private void ImportSuppliers(List<CatalogueDataSource.SupplierData> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("Importing supplier: {0}", item.Code);

                // Update / create object
                var supplier = GetSupplier(item.Code)
                    ?? new Supplier { Code = item.Code };
                supplier.Name = item.Name;

                session.SaveOrUpdate(supplier);

                // Add to cache for use when importing products
                if (!supplierCache.ContainsKey(supplier.Code))
                    supplierCache.Add(supplier.Code, supplier);
            }
        }

        private void ImportCategories(List<CatalogueDataSource.CategoryData> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("Importing category: {0}", item.Code);

                // Update / create object
                var category = GetCategory(item.Code)
                    ?? new Category { Code = item.Code };
                category.Name = item.Name;

                session.SaveOrUpdate(category);

                // Add to cache for use when importing products
                if (!categoryCache.ContainsKey(category.Code))
                    categoryCache.Add(category.Code, category);
            }
        }

        private void ImportProducts(List<CatalogueDataSource.ProductData> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("Importing product: {0}", item.Code);

                var supplier = GetSupplier(item.SupplierCode);
                if (supplier == null)
                {
                    Console.WriteLine("Warning: Could not find supplier with code {0}, using default supplier", item.SupplierCode);
                    supplier = GetSupplier(UnknownSupplierCode);
                }

                var category = GetCategory(item.CategoryCode) ?? GetCategory(UnknownCategoryCode);
                if (category == null)
                {
                    Console.WriteLine("Warning: Code not find category with code {0}, using default category", item.CategoryCode);
                    category = GetCategory(UnknownCategoryCode);
                }

                var product = session.Query<Product>().SingleOrDefault(x => x.Code == item.Code)
                    ?? new Product { Code = item.Code, Description = "" };
                product.Name = item.Name;
                product.Supplier = supplier;
                product.Category = category;
                product.UnitsInStock = item.UnitsInStock;

                session.SaveOrUpdate(product);
            }
        }

        private Supplier GetSupplier(string code)
        {
            Supplier supplier;
            if (!supplierCache.TryGetValue(code, out supplier))
            {
                supplier = session.Query<Supplier>().SingleOrDefault(x => x.Code == code);
                if (supplier != null)
                    supplierCache.Add(code, supplier);
            }
            return supplier;
        }

        private Category GetCategory(string code)
        {
            Category category;
            if (!categoryCache.TryGetValue(code, out category))
            {
                category = session.Query<Category>().SingleOrDefault(x => x.Code == code);

                if (category != null)
                    categoryCache.Add(code, category);
            }
            return category;
        }
    }
}
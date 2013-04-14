using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TinyNH.DemoStore.ProductImporter
{
    public class CatalogueDataSource
    {
        public CatalogueData GetCatalogueData()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\catalogue.json");
            string data = File.ReadAllText(path);
            var catalogueData = JsonConvert.DeserializeObject<CatalogueData>(data);

            return catalogueData;
        }

        public class CatalogueData
        {
            public List<SupplierData> Suppliers { get; set; }
            public List<CategoryData> Categories { get; set; }
            public List<ProductData> Products { get; set; }

        }

        public class SupplierData
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class CategoryData
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class ProductData
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string SupplierCode { get; set; }
            public string CategoryCode { get; set; }
            public int UnitsInStock
            {
                get;
                set;
            }
        }
    }
}
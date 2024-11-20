using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tuyalocalconfiggenerator.Classes
{
    internal class Output
    {
        public class Yaml
        {
            public Yaml()
            {
                Products = new HashSet<Product>();
                SecondaryEntities = new HashSet<SecondaryEntity>();
                PrimaryEntity = new();
            }
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("products")]
            public ICollection<Product> Products { get; set; }

            [JsonPropertyName("primary_entity")]
            public PrimaryEntity PrimaryEntity { get; set; }

            [JsonPropertyName("secondary_entities")]
            public ICollection<SecondaryEntity> SecondaryEntities { get; set; }
        }

        public class PrimaryEntity
        {
            public PrimaryEntity()
            {
                Dps = new HashSet<PrimaryEntityDp>();
            }
            [JsonPropertyName("entity")]
            public string Entity { get; set; }

            [JsonPropertyName("dps")]
            public ICollection<PrimaryEntityDp> Dps { get; set; }
        }

        public partial class PrimaryEntityDp
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("optional")]
            public bool? Optional { get; set; }

            [JsonPropertyName("mapping")]
            public Mapping[] Mapping { get; set; }

            [JsonPropertyName("hidden")]
            public bool? Hidden { get; set; }
        }

        public partial class Mapping
        {
            [JsonPropertyName("dps_val")]
            public string DpsVal { get; set; }

            [JsonPropertyName("value")]
            public string Value { get; set; }
        }

        public partial class Product
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }
        }

        public  class SecondaryEntity
        {
            public SecondaryEntity()
            {
                Dps = new HashSet<SecondaryEntityDp>();
            }
            [JsonPropertyName("entity")]
            public string Entity { get; set; }

            [JsonPropertyName("class")]
            public string Class { get; set; }

            [JsonPropertyName("dps")]
            public ICollection<SecondaryEntityDp> Dps { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("icon")]
            public string Icon { get; set; }

            [JsonPropertyName("category")]
            public string Category { get; set; }
        }

        public partial class SecondaryEntityDp
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("unit")]
            public string Unit { get; set; }

            [JsonPropertyName("class")]
            public string Class { get; set; }

            [JsonPropertyName("optional")]
            public bool? Optional { get; set; }

            [JsonPropertyName("mapping")]
            public Mapping[] Mapping { get; set; }
        }

    }
}

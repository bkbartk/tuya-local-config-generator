using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YamlDotNet.Serialization;
using static tuyalocalconfiggenerator.Classes.Output;

namespace tuyalocalconfiggenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainSettings _MainSettings = new();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _MainSettings;
        }

        private void txtVacuum_Click(object sender, RoutedEventArgs e)
        {
            var operation = new ParameterizedThreadStart(obj => txtVacuum_Thread());
            Thread thread = new Thread(operation);
            thread.Start();
        }
        private async void txtVacuum_Thread()
        {
            try
            {
                var configJson = System.IO.File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/mappings/vacuum.json");
                var config = System.Text.Json.JsonSerializer.Deserialize<Config>(configJson);

                var jsonMain = System.Text.Json.JsonSerializer.Deserialize<JsonMain>(_MainSettings.Input);
                var jsonSub = System.Text.Json.JsonSerializer.Deserialize<JsonSub>(jsonMain.result.model);
                var output = new Classes.Output.Yaml();
                output.Name = "Robot vacuum";
                var product = new Classes.Output.Product();
                product.Id = "Prod Id";
                product.Name = "Robot vacuum";
                output.Products.Add(product);
                foreach (var property in jsonSub.services.First().properties)
                {

                    var code = property.code;
                    var configMapping = config.mappings.FirstOrDefault(m => m.code == code);
                    if (configMapping != null && configMapping.isPrimary)
                    {
                        var dp = new PrimaryEntityDp();
                        dp.Id = property.abilityId;
                        dp.Name = configMapping.name;
                        dp.Type = property.typeSpec.type; //TODO typerename
                        output.PrimaryEntity.Dps.Add(dp);

                    }
                    else if (configMapping != null)
                    {
                        var dp = new SecondaryEntityDp();
                        dp.Id = property.abilityId;
                        dp.Name = "sensor"; ;
                        dp.Type = property.typeSpec.type;
                        var entry = new SecondaryEntity();
                        entry.Entity = "sensor";
                        entry.Name = configMapping.name;
                        entry.Dps.Add(dp);
                        output.SecondaryEntities.Add(entry);
                    }
                    else
                    {
                        var dp = new SecondaryEntityDp();
                        dp.Id = property.abilityId;
                        dp.Name = "sensor"; ;
                        dp.Type = property.typeSpec.type;
                        var entry = new SecondaryEntity();
                        entry.Entity = "sensor";
                        entry.Name = property.code;
                        entry.Dps.Add(dp);
                        output.SecondaryEntities.Add(entry);
                    }
                }

                var serializer = new YamlDotNet.Serialization.SerializerBuilder()
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitEmptyCollections)
                    .Build();

                //var serializer = new YamlDotNet.Serialization.Serializer();
                var yaml = new StringBuilder();

                await using var textWriter = new StringWriter(yaml);

                serializer.Serialize(textWriter, output, output.GetType());
                _MainSettings.Output = yaml.ToString();
            }
            catch (Exception ex)
            {
                //TODO errorhandling
                _MainSettings.Output = ex.ToString();
            }
        }
    }

    public class MainSettings : INotifyPropertyChanged
    {
        public string Input { get; set; }

        private string _output;
        public string Output
        {
            get { return _output; }
            set
            {
                _output = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ConfigMapping
    {
        public string code { get; set; }
        public string name { get; set; }
        public bool isPrimary { get; set; }
    }

    public class Config
    {
        public List<ConfigMapping> mappings { get; set; }
    }




    //main result
    public class JsonMainResult
    {
        public string model { get; set; }
    }

    public class JsonMain
    {
        public JsonMainResult result { get; set; }
        public bool success { get; set; }
        public long t { get; set; }
        public string tid { get; set; }
    }
    //main result

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Extensions
    {
        public string iconName { get; set; }
        public string attribute { get; set; }
        public string scope { get; set; }
    }

    public class Property
    {
        public int abilityId { get; set; }
        public string accessMode { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public Extensions extensions { get; set; }
        public string name { get; set; }
        public TypeSpec typeSpec { get; set; }
    }

    public class JsonSub
    {
        public string modelId { get; set; }
        public List<Service> services { get; set; }
    }

    public class Service
    {
        public List<object> actions { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public List<object> events { get; set; }
        public string name { get; set; }
        public List<Property> properties { get; set; }
    }

    public class TypeSpec
    {
        public string type { get; set; }
        public List<string> range { get; set; }
        public int? max { get; set; }
        public int? min { get; set; }
        public int? scale { get; set; }
        public int? step { get; set; }
        public string unit { get; set; }
        public int? maxlen { get; set; }
        public List<string> label { get; set; }
    }




}
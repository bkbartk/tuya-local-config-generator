using System.IO;
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
                var jsonMain = System.Text.Json.JsonSerializer.Deserialize<JsonMain>(_MainSettings.Input);
                var jsonSub = System.Text.Json.JsonSerializer.Deserialize<JsonSub>(jsonMain.result.model);
                var output = new Classes.Output.Yaml();
                output.Name = "Robot vacuum";
                var product = new Classes.Output.Product();
                product.Id = "Prod Id";
                product.Name = "Robot vacuum";
                output.Products.Add(product);

                var serializer = new YamlDotNet.Serialization.Serializer();
                var yaml = new StringBuilder();

                await using var textWriter = new StringWriter(yaml);

                serializer.Serialize(textWriter, output, output.GetType());
                _MainSettings.Output = yaml.ToString();
            }
            catch (Exception ex) {
                //TODO errorhandling
                _MainSettings.Output = ex.ToString();
            }     
        }

        private void Hotfix_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = _MainSettings.Output;
        }
    }

    public class MainSettings
    {
        public string Input { get; set; }
        public string Output { get; set; }
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
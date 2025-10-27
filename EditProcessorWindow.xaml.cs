using Microsoft.EntityFrameworkCore;
using ProcessorDB.Data;
using ProcessorDB.Models;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProcessorDB
{
    public partial class EditProcessorWindow : Window
    {
        private AppDbContext _context;
        private Processor _processor;

        public EditProcessorWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _processor = new Processor
            {
                TechSpec = new TechSpec(),
                ProductionInfo = new ProductionInfo()
            };
            LoadComboBoxes();
            DataContext = _processor;
        }

        public EditProcessorWindow(Processor processor)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _processor = processor;
            LoadComboBoxes();
            DataContext = _processor;
        }

        private void LoadComboBoxes()
        {
            try
            {
                var manufacturers = _context.Manufacturers?.ToList();
                if (manufacturers != null)
                    ManufacturerComboBox.ItemsSource = manufacturers;

                var countries = _context.Countries?.ToList();
                if (countries != null)
                    CountryComboBox.ItemsSource = countries;

                ReleaseYearComboBox.ItemsSource = Enumerable.Range(2000, 25).ToList();
                TechProcessComboBox.ItemsSource = new[] { "4nm", "5nm", "7nm", "10nm", "14nm", "22nm", "28nm", "45nm" };
                CacheL3ComboBox.ItemsSource = new[] { "1MB", "2MB", "4MB", "6MB", "8MB", "12MB", "16MB", "18MB", "24MB", "30MB", "32MB", "64MB", "96MB" };
                CoresComboBox.ItemsSource = new[] { 2, 4, 6, 8, 12, 16, 24, 32, 64 };
                SlotComboBox.ItemsSource = new[] { "LGA 1151", "AM4", "LGA 1700", "AM5" };
                WarrantyComboBox.ItemsSource = new[] { 12, 24, 36, 48, 60 };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_processor.Id == 0)
                {
                    _context.Processors.Add(_processor);
                }
                else
                {
                    _context.Processors.Update(_processor);
                }
                await _context.SaveChangesAsync();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !double.TryParse(e.Text, out _);
        }
    }
}

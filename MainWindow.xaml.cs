using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ProcessorDB.Data;
using ProcessorDB.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProcessorDB
{
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        private ObservableCollection<Processor> _processors = new ObservableCollection<Processor>();

        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadManufacturersAsync();
            LoadDataAsync();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, (s, e) => AddProcessor_Click(s, e)));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, (s, e) => EditProcessor_Click(s, e)));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (s, e) => DeleteProcessor_Click(s, e)));
        }

        private async Task LoadManufacturersAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
                var manufacturers = await _context.Manufacturers.ToListAsync();
                ManufacturerFilterComboBox.ItemsSource = manufacturers;
                ManufacturerFilterComboBox.DisplayMemberPath = "Name";
                ManufacturerFilterComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки производителей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var processorsWithIncludes = await _context.Processors
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Country)
                    .Include(p => p.TechSpec)
                    .Include(p => p.ProductionInfo)
                    .ToListAsync();
                _processors = new ObservableCollection<Processor>(processorsWithIncludes);
                ProcessorsGrid.ItemsSource = _processors;
                StatusBar.Text = $"Загружено {_processors.Count} процессоров";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusBar.Text = "Ошибка загрузки";
            }
        }

        private async void AddProcessor_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditProcessorWindow();
            if (window.ShowDialog() == true)
            {
                await LoadDataAsync();
            }
        }

        private async void EditProcessor_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessorsGrid.SelectedItem is Processor selectedProcessor)
            {
                var window = new EditProcessorWindow(selectedProcessor);
                if (window.ShowDialog() == true)
                {
                    await LoadDataAsync();
                }
            }
            else
            {
                MessageBox.Show("Выберите процессор для редактирования");
            }
        }

        private async void DeleteProcessor_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessorsGrid.SelectedItem is Processor selectedProcessor)
            {
                if (MessageBox.Show("Удалить процессор?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.Processors.Remove(selectedProcessor);
                        await _context.SaveChangesAsync();
                        await LoadDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите процессор для удаления");
            }
        }

        private async void ManageManufacturers_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditManufacturersWindow(await _context.Manufacturers.ToListAsync());
            if (window.ShowDialog() == true)
            {
                await LoadManufacturersAsync();
                await LoadDataAsync();
            }
        }

        private async void ManageCountries_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditCountriesWindow(await _context.Countries.ToListAsync());
            if (window.ShowDialog() == true)
            {
                await LoadDataAsync();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterByManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var query = _processors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(p => p.Name != null && p.Name.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase));
            }

            if (decimal.TryParse(MinPriceTextBox.Text, out decimal minPrice))
            {
                query = query.Where(p => p.ProductionInfo != null && p.ProductionInfo.Price >= minPrice);
            }
            if (decimal.TryParse(MaxPriceTextBox.Text, out decimal maxPrice))
            {
                query = query.Where(p => p.ProductionInfo != null && p.ProductionInfo.Price <= maxPrice);
            }

            if (ManufacturerFilterComboBox.SelectedItem is Manufacturer selectedMan)
            {
                query = query.Where(p => p.ManufacturerId == selectedMan.Id);
            }

            var filtered = query.ToList();
            ProcessorsGrid.ItemsSource = filtered;
            StatusBar.Text = $"Отфильтровано {filtered.Count} процессоров";
        }

        private void ReportPriceChart_Click(object sender, RoutedEventArgs e)
        {
            var plotModel = new PlotModel { Title = "Цены процессоров" };
            var barSeries = new BarSeries();
            var labels = new List<string>();
            foreach (var p in _processors.Where(p => p.ProductionInfo != null))
            {
                barSeries.Items.Add(new BarItem { Value = (double)p.ProductionInfo!.Price });
                labels.Add(p.Name ?? "");
            }
            plotModel.Series.Add(barSeries);
            plotModel.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom, ItemsSource = labels });

            var chartWindow = new Window { Title = "Отчет: Цены", Width = 600, Height = 400 };
            var plotView = new OxyPlot.Wpf.PlotView { Model = plotModel };
            chartWindow.Content = plotView;
            chartWindow.ShowDialog();
        }

        private void ReportFrequencyChart_Click(object sender, RoutedEventArgs e)
        {
            var plotModel = new PlotModel { Title = "Частоты процессоров" };
            var barSeries = new BarSeries();
            var labels = new List<string>();
            foreach (var p in _processors.Where(p => p.TechSpec != null))
            {
                barSeries.Items.Add(new BarItem { Value = p.TechSpec!.Frequency });
                labels.Add(p.Name ?? "");
            }
            plotModel.Series.Add(barSeries);
            plotModel.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom, ItemsSource = labels });

            var chartWindow = new Window { Title = "Отчет: Частоты", Width = 600, Height = 400 };
            var plotView = new OxyPlot.Wpf.PlotView { Model = plotModel };
            chartWindow.Content = plotView;
            chartWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ProcessorsGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // Контекстное меню уже определено в XAML
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !double.TryParse(e.Text, out _);
        }
    }
}

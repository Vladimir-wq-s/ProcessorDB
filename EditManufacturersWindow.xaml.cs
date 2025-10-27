using ProcessorDB.Data;
using ProcessorDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessorDB
{
    public partial class EditManufacturersWindow : Window
    {
        private AppDbContext _context;
        private List<Manufacturer> _manufacturers;

        public EditManufacturersWindow(List<Manufacturer> manufacturers)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _manufacturers = manufacturers;
            ManufacturersListBox.ItemsSource = _manufacturers;
            ManufacturersListBox.DisplayMemberPath = "Name";
        }

        private void AddManufacturer_Click(object sender, RoutedEventArgs e)
        {
            var newMan = new Manufacturer { Name = "Новый производитель" };
            _manufacturers.Add(newMan);
            ManufacturersListBox.Items.Refresh();
        }

        private void DeleteManufacturer_Click(object sender, RoutedEventArgs e)
        {
            if (ManufacturersListBox.SelectedItem is Manufacturer selected)
            {
                _manufacturers.Remove(selected);
                ManufacturersListBox.Items.Refresh();
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var man in _manufacturers)
            {
                if (man.Id == 0)
                    _context.Manufacturers.Add(man);
                else
                    _context.Manufacturers.Update(man);
            }
            await _context.SaveChangesAsync();
            DialogResult = true;
            Close();
        }
    }
}

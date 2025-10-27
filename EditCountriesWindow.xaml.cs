using ProcessorDB.Data;
using ProcessorDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessorDB
{
    public partial class EditCountriesWindow : Window
    {
        private AppDbContext _context;
        private List<Country> _countries;

        public EditCountriesWindow(List<Country> countries)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _countries = countries;
            CountriesListBox.ItemsSource = _countries;
            CountriesListBox.DisplayMemberPath = "Name";
        }

        private void AddCountry_Click(object sender, RoutedEventArgs e)
        {
            var newCountry = new Country { Name = "Новая страна" };
            _countries.Add(newCountry);
            CountriesListBox.Items.Refresh();
        }

        private void DeleteCountry_Click(object sender, RoutedEventArgs e)
        {
            if (CountriesListBox.SelectedItem is Country selected)
            {
                _countries.Remove(selected);
                CountriesListBox.Items.Refresh();
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            foreach (var country in _countries)
            {
                if (country.Id == 0)
                    _context.Countries.Add(country);
                else
                    _context.Countries.Update(country);
            }
            await _context.SaveChangesAsync();
            DialogResult = true;
            Close();
        }
    }
}

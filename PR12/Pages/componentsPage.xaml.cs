using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR12
{
    /// <summary>
    /// Логика взаимодействия для componentsPage.xaml
    /// </summary>
    /// 
    public partial class componentsPage : Page
    {
        private List<basepart_> allParts;
        private string selectedCategory = "Все";
        private Button activeCategoryButton = null;

        public componentsPage()
        {
            InitializeComponent();
            LoadParts();
            LoadManufacturers();
            LoadCategories();
        }

        private void LoadParts()
        {
            allParts = Core.Context.basepart_.ToList();
            partsList.ItemsSource = allParts;
        }

        private void LoadManufacturers()
        {
            var manufacturers = Core.Context.manufacturer_.ToList();
            manufacturers.Insert(0, new manufacturer_ { id = 0, name = "Производитель: " });

            manufacturerFilter.ItemsSource = manufacturers;
            manufacturerFilter.DisplayMemberPath = "name";
            manufacturerFilter.SelectedIndex = 0;
        }

        private void LoadCategories()
        {
            categoryList.ItemsSource = Core.Context.parttype_.ToList();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterParts();
        }

        private void manufacturerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterParts();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (activeCategoryButton != null)
                activeCategoryButton.Background = activeCategoryButton.Tag.ToString() == "Все"
                    ? (Brush)new BrushConverter().ConvertFrom("#3C507D")
                    : (Brush)new BrushConverter().ConvertFrom("#617891");

            btn.Background = (Brush)new BrushConverter().ConvertFrom("#A2676C");
            activeCategoryButton = btn;
            selectedCategory = btn.Tag.ToString();

            FilterParts();
        }

        private void FilterParts()
        {
            var filtered = allParts;

            if (!string.IsNullOrWhiteSpace(searchBox.Text))
            {
                string text = searchBox.Text.ToLower();
                filtered = filtered.Where(p => p.name.ToLower().Contains(text)).ToList();
            }

            if (manufacturerFilter.SelectedItem is manufacturer_ m && m.id != 0)
                filtered = filtered.Where(p => p.manufacturerid == m.id).ToList();

            if (selectedCategory != "Все")
                filtered = filtered.Where(p => p.parttype_.name == selectedCategory).ToList();

            partsList.ItemsSource = filtered;
        }

        private void SelectPart_Click(object sender, RoutedEventArgs e)
        {
            var part = (sender as Button).DataContext as basepart_;

            if (!allParts.Any(p => p.id == part.id))
            {
                allParts.Add(part);
                MessageBox.Show($"Добавлено: {part.name}");
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new buildSummaryPage(allParts));
        }
    }
}

using PR12.Classes;
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

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для startPage.xaml
    /// </summary>
    public partial class startPage : Page
    {
        private User selectedMaster = null;
        private ServiceType selectedServiceType = null;

        public startPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Мастера (RoleID = 2)
            masterCB.ItemsSource = Core.Context.User.Where(u => u.RoleID == 2).ToList();

            // Типы услуг
            serviceTypeCB.ItemsSource = Core.Context.ServiceType.ToList();

            UpdateUI();
        }

        private void UpdateUI()
        {
            var services = Core.Context.Service.ToList();

            // Фильтр по мастеру
            if (selectedMaster != null)
            {
                var allowedTypeIDs = selectedMaster.MasterServiceType.Select(mst => mst.ServiceTypeID).ToList();
                services = services.Where(s => allowedTypeIDs.Contains(s.ServiceTypeID)).ToList();
            }

            // Фильтр по типу услуги
            if (selectedServiceType != null)
            {
                services = services.Where(s => s.ServiceTypeID == selectedServiceType.ID).ToList();
            }

            // Подготовка для отображения — упрощено
            var displayServices = services.Select(s =>
            {
                // Берем первого мастера, который связан с ServiceType
                var master = s.ServiceType?.MasterServiceType
                                .Select(mst => mst.User)
                                .FirstOrDefault(u => u.RoleID == 2);

                return new
                {
                    s.ID,
                    s.ImagePath,
                    s.Name,
                    s.Price,
                    MasterName = master != null ? master.FullName : "Нет мастера"
                };
            }).ToList();

            servicesList.ItemsSource = displayServices;
        }

        private void MasterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMaster = masterCB.SelectedItem as User;
            UpdateUI();
        }

        private void ServiceTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedServiceType = serviceTypeCB.SelectedItem as ServiceType;
            UpdateUI();
        }

        private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Чтобы записаться, нужно войти в аккаунт.");
            NavigationService.Navigate(new enterPage());
        }

        private void GoToEnterBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new enterPage());
        }

        private void ToAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (User.currentUser != null)
            {
                switch (User.currentUser.RoleID)
                {
                    case 1: NavigationService.Navigate(new accountPage()); break;
                    case 2: NavigationService.Navigate(new masterPage()); break;
                    case 3: NavigationService.Navigate(new managerPage()); break;
                    case 4: NavigationService.Navigate(new adminPage()); break;
                }
            }
        }
    }
}


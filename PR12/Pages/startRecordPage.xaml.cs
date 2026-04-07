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
        private DateTime? selectedDate = null;

        public startPage()
        {
            InitializeComponent();
            LoadData();
            UpdateButtonsByUser();
        }

        private void LoadData()
        {
            var masters = Core.Context.User.Where(u => u.RoleID == 2).ToList();
            masters.Insert(0, new User { ID = 0, FullName = "Без мастера" });
            masterCB.ItemsSource = masters;

            var types = Core.Context.ServiceType.ToList();
            types.Insert(0, new ServiceType { ID = 0, Name = "Без типа" });
            serviceTypeCB.ItemsSource = types;

            UpdateUI();
        }

        private void UpdateButtonsByUser()
        {
            if (User.currentUser == null)
            {
                goToEnterBtn.Visibility = Visibility.Visible;
                toAccountBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                goToEnterBtn.Visibility = Visibility.Collapsed;
                toAccountBtn.Visibility = Visibility.Visible;

                if (User.currentUser.RoleID == 1) toAccountBtn.Content = "← Назад";
                else toAccountBtn.Content = "Аккаунт";
            }
        }

        private void UpdateUI()
        {
            var services = Core.Context.Service.ToList();

            bool filterByMaster = selectedMaster != null && selectedMaster.ID != 0;
            bool filterByType = selectedServiceType != null && selectedServiceType.ID != 0;

            var displayServices = new List<dynamic>();

            foreach (var s in services)
            {
                if (filterByType && s.ServiceTypeID != selectedServiceType.ID) continue;

                if (!filterByMaster)
                {
                    displayServices.Add(new
                    {
                        s.ID,
                        s.ImagePath,
                        s.Name,
                        s.Price,
                        MasterName = "",
                        StartTimeText = "",
                        EndTimeText = "",
                        ActionText = "Подробнее",
                        CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#889CB4"))
                    });
                }
                else
                {
                    var mastersForService = s.ServiceType?.MasterServiceType.Select(mst => mst.User).Where(u => u.RoleID == 2).ToList();
                    if (mastersForService == null || mastersForService.Count == 0) continue;

                    foreach (var master in mastersForService)
                    {
                        if (master.ID != selectedMaster.ID) continue;

                        var slots = Core.Context.Timetable
                            .Where(t => t.MasterID == master.ID &&
                                        (!selectedDate.HasValue ||
                                         (t.StartDateTime.Year == selectedDate.Value.Year &&
                                          t.StartDateTime.Month == selectedDate.Value.Month &&
                                          t.StartDateTime.Day == selectedDate.Value.Day)))
                            .OrderBy(t => t.StartDateTime)
                            .ToList();

                        foreach (var slot in slots)
                        {
                            displayServices.Add(new
                            {
                                s.ID,
                                s.ImagePath,
                                s.Name,
                                s.Price,
                                MasterName = master.FullName,
                                StartTimeText = $"Начало: {slot.StartDateTime:HH:mm}",
                                EndTimeText = $"Конец: {slot.EndDateTime:HH:mm}",
                                ActionText = slot.ISBooked ? "Занято" : "Записаться",
                                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(slot.ISBooked ? "#C25C4C" : "#889CB4")),
                                TimetableID = slot.ID
                            });
                        }
                    }
                }
            }

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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = datePicker.SelectedDate;
            UpdateUI();
        }

        private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            dynamic data = btn.DataContext;

            if (User.currentUser != null && User.currentUser.RoleID == 1)
            {
                if (data.ActionText == "Занято")
                {
                    MessageBox.Show("Эта запись уже занята.");
                }
                else
                {
                    // переход на страницу выбранной записи
                    // NavigationService.Navigate(new chosenRecordPage(data.TimetableID));
                }
            }
            else
            {
                MessageBox.Show("Чтобы записаться, нужно войти в аккаунт.");
                NavigationService.Navigate(new enterPage());
            }
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
                    case 1: NavigationService.Navigate(new clientPage()); break;
                    case 2: NavigationService.Navigate(new masterPage()); break;
                    case 3: NavigationService.Navigate(new managerPage()); break;
                    case 4: NavigationService.Navigate(new adminPage()); break;
                }
            }
        }
    }
}


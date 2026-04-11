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

            bool hasMaster = selectedMaster != null && selectedMaster.ID != 0;
            bool hasType = selectedServiceType != null && selectedServiceType.ID != 0;
            bool hasDate = selectedDate.HasValue;

            bool isLoggedIn = User.currentUser != null;
            bool isClient = isLoggedIn && User.currentUser.RoleID == 1;

            var displayServices = new List<object>();

            if (hasDate)
            {
                selectedDateText.Visibility = Visibility.Visible;
                selectedDateText.Text = $"Дата записи: {selectedDate.Value:dd MMMM yyyy}";
            }
            else
            {
                selectedDateText.Visibility = Visibility.Collapsed;
            }

            foreach (var s in services)
            {
                if (hasType && s.ServiceTypeID != selectedServiceType.ID) continue;

                if (!hasMaster && !hasDate)
                {
                    bool showBtn = !isClient;

                    displayServices.Add(new
                    {
                        ImagePath = s.ImagePath,
                        Name = s.Name,
                        Price = s.Price,
                        MasterName = "",
                        StartTimeText = "",
                        EndTimeText = "",
                        ActionText = "Подробнее",
                        CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#889CB4")),
                        MasterVisibility = Visibility.Collapsed,
                        TimeVisibility = Visibility.Collapsed,
                        ButtonVisibility = showBtn ? Visibility.Visible : Visibility.Collapsed,
                        ServiceID = s.ID,
                        TimetableID = 0
                    });
                }
                else
                {
                    var mastersForService = s.ServiceType?.MasterServiceType?.Select(mst => mst.User)?.Where(u => u.RoleID == 2)?.ToList() ?? new List<User>();
                    foreach (var master in mastersForService)
                    {
                        if (hasMaster && master.ID != selectedMaster.ID) continue;

                        var slotsQuery = Core.Context.Timetable.Where(t => t.MasterID == master.ID);
                        if (hasDate)
                        {
                            slotsQuery = slotsQuery.Where(t =>
                                t.StartDateTime.Year == selectedDate.Value.Year &&
                                t.StartDateTime.Month == selectedDate.Value.Month &&
                                t.StartDateTime.Day == selectedDate.Value.Day);
                        }

                        var slots = slotsQuery.OrderBy(t => t.StartDateTime).ToList();
                        foreach (var slot in slots)
                        {
                            bool isBooked = slot.ISBooked;
                            string action = isBooked ? "Занято" : "Записаться";
                            string color = isBooked ? "#C25C4C" : "#889CB4";

                            displayServices.Add(new
                            {
                                ImagePath = s.ImagePath,
                                Name = s.Name,
                                Price = s.Price,
                                MasterName = master.FullName,
                                StartTimeText = $"Время: {slot.StartDateTime:HH:mm} - {slot.EndDateTime:HH:mm}",
                                EndTimeText = "",
                                ActionText = action,
                                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)),
                                MasterVisibility = Visibility.Visible,
                                TimeVisibility = Visibility.Visible,
                                ButtonVisibility = Visibility.Visible,
                                ServiceID = s.ID,
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
            if (btn == null || btn.DataContext == null)
            {
                MessageBox.Show("Данные записи не найдены");
                return;
            }

            dynamic data = btn.DataContext;

            int timetableId = 0;
            int serviceId = 0;
            string actionText = "";

            if (data.TimetableID != null) timetableId = Convert.ToInt32(data.TimetableID);

            if (data.ServiceID != null) serviceId = Convert.ToInt32(data.ServiceID);

            if (data.ActionText != null) actionText = data.ActionText.ToString();

            bool isLoggedIn = User.currentUser != null;
            bool isClient = isLoggedIn && User.currentUser.RoleID == 1;

            if (!isLoggedIn)
            {
                MessageBox.Show("Чтобы записаться, нужно войти в аккаунт.");
                NavigationService.Navigate(new enterPage());
                return;
            }

            if (!isClient)
            {
                MessageBox.Show("Запись доступна только клиентам.");
                return;
            }

            if (actionText == "Занято")
            {
                MessageBox.Show("Эта запись уже занята.");
            }
            else if (actionText == "Записаться")
            {
                if (timetableId == 0)
                {
                    MessageBox.Show("Ошибка: не удалось получить информацию о записи");
                    return;
                }
                NavigationService.Navigate(new chosenRecordPage(timetableId, serviceId));
            }
            else
            {
                MessageBox.Show("Выберите мастера или дату, чтобы увидеть доступные записи.");
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


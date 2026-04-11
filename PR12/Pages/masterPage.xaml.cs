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
    /// Логика взаимодействия для masterPage.xaml
    /// </summary>
    public partial class masterPage : Page
    {
        public masterPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (User.currentUser != null)
            {
                masterNameTBl.Text = User.currentUser.FullName;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            var displayRecords = new List<object>();

            // Получаем только занятые слоты (где ISBooked == true или есть запись)
            var slots = Core.Context.Timetable
                .Where(t => t.MasterID == User.currentUser.ID && t.ISBooked)
                .OrderBy(t => t.StartDateTime)
                .ToList();

            // Если выбрана дата — фильтруем по ней
            if (datePicker.SelectedDate.HasValue)
            {
                var selected = datePicker.SelectedDate.Value;
                slots = slots.Where(t =>
                    t.StartDateTime.Year == selected.Year &&
                    t.StartDateTime.Month == selected.Month &&
                    t.StartDateTime.Day == selected.Day)
                    .ToList();
            }

            foreach (var slot in slots)
            {
                var record = slot.Record.FirstOrDefault();
                if (record == null) continue;

                var service = record.Service;

                displayRecords.Add(new
                {
                    ImagePath = service?.ImagePath ?? "",
                    ServiceName = service?.Name ?? "Услуга",
                    Price = service?.Price ?? 0,
                    ClientName = record.User?.FullName ?? "—",
                    DateText = $"Дата: {slot.StartDateTime:dd.MM.yyyy}",
                    TimeText = $"Время: {slot.StartDateTime:HH:mm}-{slot.EndDateTime:HH:mm}",
                    CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#889CB4")),
                    TimetableID = slot.ID,
                    ServiceID = record.ServiceID
                });
            }

            recordsList.ItemsSource = displayRecords;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn?.DataContext == null) return;

            dynamic data = btn.DataContext;

            int timetableId = 0;
            int serviceId = 0;

            if (data.TimetableID != null)
                timetableId = Convert.ToInt32(data.TimetableID);

            if (data.ServiceID != null)
                serviceId = Convert.ToInt32(data.ServiceID);

            if (timetableId == 0)
            {
                MessageBox.Show("Ошибка: не удалось получить информацию о записи");
                return;
            }

            NavigationService.Navigate(new chosenRecordPage(timetableId, serviceId));
        }

        private void EditServicesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (User.currentUser == null || User.currentUser.RoleID != 2)
            {
                MessageBox.Show("Функция доступна только мастеру");
                return;
            }

            new editMasterServicesWindow().ShowDialog();  
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            User.currentUser = null;
            NavigationService.Navigate(new enterPage());
        }
    }
}

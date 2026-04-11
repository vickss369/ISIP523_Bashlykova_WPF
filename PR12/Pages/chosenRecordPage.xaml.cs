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
    /// Логика взаимодействия для chosenRecordPage.xaml
    /// </summary>
    public partial class chosenRecordPage : Page
    {
        private int timetableID;
        private int serviceID;
        private int selectedPaymentTypeID = 0;

        private Timetable currentSlot;
        private Service currentService;
        private Record currentRecord;

        public chosenRecordPage(int timetableID, int serviceID)
        {
            InitializeComponent();
            this.timetableID = timetableID;
            this.serviceID = serviceID;
            LoadRecordInfo();
        }

        private void LoadRecordInfo()
        {
            currentSlot = Core.Context.Timetable.FirstOrDefault(t => t.ID == timetableID);
            currentService = Core.Context.Service.FirstOrDefault(s => s.ID == serviceID);
            currentRecord = Core.Context.Record.FirstOrDefault(r => r.TimetableID == timetableID);

            if (currentSlot == null || currentService == null)
            {
                MessageBox.Show("Ошибка загрузки записи");
                NavigationService?.GoBack();
                return;
            }

            var master = Core.Context.User.FirstOrDefault(u => u.ID == currentSlot.MasterID);
            var serviceType = Core.Context.ServiceType.FirstOrDefault(st => st.ID == currentService.ServiceTypeID);

            bool isMaster = User.currentUser != null && User.currentUser.RoleID == 2;

            string userLabel = isMaster ? "Клиент:" : "Мастер:";
            string userName = isMaster? (currentRecord?.User?.FullName ?? "-") : (master?.FullName ?? "-");

            string userPhone = isMaster ? (currentRecord?.User?.PhoneNumber ?? "-") : "";

            DataContext = new
            {
                ServiceName = currentService.Name,
                ServiceTypeName = serviceType?.Name ?? "-",
                UserLabel = userLabel + " " + userName,
                UserPhone = userPhone,
                Price = currentService.Price,
                DateText = $"Дата: {currentSlot.StartDateTime:dd.MM.yyyy}",
                StartTimeText = currentSlot.StartDateTime.ToString("HH:mm"),
                EndTimeText = currentSlot.EndDateTime.ToString("HH:mm"),
                ImagePath = currentService.ImagePath
            };

            if (isMaster)
            {
                clientPanel.Visibility = Visibility.Collapsed;
                phoneTBl.Visibility = Visibility.Visible;
                actionBtn.Content = "Завершить запись";
                actionBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#72B172"));
            }
            else
            {
                clientPanel.Visibility = Visibility.Visible;
                phoneTBl.Visibility = Visibility.Collapsed;
                actionBtn.Content = "Записаться";
                actionBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8BA2C1"));
                actionBtn.IsEnabled = !currentSlot.ISBooked;
            }
        }

        private void paymentTypeRB_Checked(object sender, RoutedEventArgs e)
        {
            if (cardRB.IsChecked == true) selectedPaymentTypeID = 1;
            if (cashRB.IsChecked == true) selectedPaymentTypeID = 2;
            if (sbpRB.IsChecked == true) selectedPaymentTypeID = 3;

            actionBtn.IsEnabled = selectedPaymentTypeID != 0;
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isMaster = User.currentUser != null && User.currentUser.RoleID == 2;
            if (isMaster)
            {
                if (currentRecord == null)
                {
                    MessageBox.Show("Запись не найдена");
                    return;
                }

                currentRecord.RecordStatusID = 2;
                Core.Context.SaveChanges();

                MessageBox.Show("Запись успешно завершена!");
                NavigationService.GoBack();
            }
            else
            {
                if (User.currentUser == null)
                {
                    MessageBox.Show("Войдите в аккаунт");
                    return;
                }

                if (selectedPaymentTypeID == 0)
                {
                    MessageBox.Show("Выберите тип оплаты");
                    return;
                }

                if (currentSlot.ISBooked)
                {
                    MessageBox.Show("Это время уже занято");
                    return;
                }

                Record record = new Record
                {
                    ClientID = User.currentUser.ID,
                    TimetableID = currentSlot.ID,
                    ServiceID = currentService.ID,
                    RecordStatusID = 1,
                    Comment = commentTB.Text,
                    PaymentTypeID = selectedPaymentTypeID
                };

                Core.Context.Record.Add(record);
                currentSlot.ISBooked = true;
                Core.Context.SaveChanges();

                MessageBox.Show("Вы успешно записались!");
                NavigationService.GoBack();
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

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
    /// Логика взаимодействия для adminPage.xaml
    /// </summary>
    public partial class adminPage : Page
    {
        private List<User> allUsers;
        private string currentMode;

        public adminPage()
        {
            InitializeComponent();

            adminNameTBl.Text = User.currentUser.FullName;
            LoadUsersList();
        }

        private void LoadUsersList()
        {
            allUsers = Core.Context.User.ToList();

            var displayUsers = allUsers.Select(u => new
            {
                u.ID,
                u.FullName,
                u.Role,
                u.PhoneNumber,
                u.Login,
                u.Password,
                CardColor = u.IsFreeze ? "#A3A3A3" : "#DDBBA7"
            }).ToList();

            usersList.ItemsSource = displayUsers;
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            User.currentUser = null;
            NavigationService.Navigate(new enterPage());
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            currentMode = "добавление";

            var win = new adminEditWindow(currentMode, 0);
            win.ShowDialog();

            if (win.IsChanged) LoadUsersList();
        }

        private void editUserBtn_Click(object sender, RoutedEventArgs e)
        {
            currentMode = "редактирование";

            var btn = sender as Button;
            if (btn == null) return;

            dynamic user = btn.DataContext;
            int id = user.ID;

            var win = new adminEditWindow(currentMode, id);
            win.ShowDialog();

            if (win.IsChanged) LoadUsersList();
        }

        private void delUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            dynamic user = btn.DataContext;
            int id = user.ID;

            var realUser = Core.Context.User.FirstOrDefault(u => u.ID == id);
            if (realUser != null)
            {
                var result = MessageBox.Show($"Удалить пользователя {realUser.FullName}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Core.Context.User.Remove(realUser);
                    Core.Context.SaveChanges();
                    LoadUsersList();
                }
            }
        }
    }
}

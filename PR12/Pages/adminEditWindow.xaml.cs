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
using System.Windows.Shapes;
using PR12.Classes;
using static System.Net.Mime.MediaTypeNames;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для adminEditWindow.xaml
    /// </summary>
    public partial class adminEditWindow : Window
    {
        private string mode;
        private int id;

        public bool IsChanged { get; private set; } = false;

        public adminEditWindow(string mode, int id)
        {
            InitializeComponent();

            this.mode = mode;
            this.id = id;

            LoadComboBox();
            LoadData();
        }

        private void LoadComboBox()
        {
            roleCB.ItemsSource = Core.Context.Role.ToList();
            roleCB.DisplayMemberPath = "Name";
            roleCB.SelectedValuePath = "ID";
        }

        private void LoadData()
        {
            if (mode == "редактирование")
            {
                titleTBl.Text = "Редактирование пользователя";

                var user = Core.Context.User.FirstOrDefault(u => u.ID == id);
                if (user == null) return;

                usernameTB.Text = user.FullName;
                roleCB.SelectedValue = user.RoleID;
                phoneNumberTB.Text = user.PhoneNumber;
                isFreezeChB.IsChecked = user.IsFreeze;
                loginTB.Text = user.Login;
                passwordTB.Text = user.Password;
            }
            else
            {
                titleTBl.Text = "Добавление пользователя";
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTB.Text) ||
                roleCB.SelectedValue == null ||
                string.IsNullOrWhiteSpace(loginTB.Text) ||
                string.IsNullOrWhiteSpace(passwordTB.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            if (mode == "редактирование")
            {
                var user = Core.Context.User.FirstOrDefault(u => u.ID == id);
                if (user == null) return;

                user.FullName = usernameTB.Text;
                user.RoleID = Convert.ToInt32(roleCB.SelectedValue);
                user.PhoneNumber = phoneNumberTB.Text;
                user.IsFreeze = isFreezeChB.IsChecked == true;
                user.Login = loginTB.Text;
                user.Password = passwordTB.Text;
            }
            else if (mode == "добавление")
            {
                var user = new User
                {
                    FullName = usernameTB.Text,
                    RoleID = Convert.ToInt32(roleCB.SelectedValue),
                    PhoneNumber = phoneNumberTB.Text,
                    IsFreeze = isFreezeChB.IsChecked == true,
                    Login = loginTB.Text,
                    Password = passwordTB.Text
                };

                Core.Context.User.Add(user);
            }

            Core.Context.SaveChanges();
            IsChanged = true;
            Close();
        }
    }
}

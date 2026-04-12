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
using System.Windows.Shapes;
using System.Xml;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для managerEditWindow.xaml
    /// </summary>

    public partial class managerEditWindow : Window
    {
        private string mode;
        private int id;

        private int selectedPaymentTypeID;
        private int selectedServiceID;

        public bool IsChanged { get; private set; } = false;

        public managerEditWindow(string mode, int id)
        {
            InitializeComponent();

            this.mode = mode;
            this.id = id;

            ApplyMode();
            LoadComboBoxes();
            LoadData();
        }

        private void MarkChanged()
        {
            IsChanged = true;
        }

        private int ConvertStatus(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Core.Context.RecordStatus.First().ID;

            text = text.ToLower();

            var status = Core.Context.RecordStatus.ToList().FirstOrDefault(s => s.Name.ToLower() == text);

            return status != null ? status.ID : Core.Context.RecordStatus.First().ID;
        }

        private void LoadComboBoxes()
        {
            if (mode == "services")
            {
                typeCB.ItemsSource = Core.Context.ServiceType.ToList();
                typeCB.DisplayMemberPath = "Name";
                typeCB.SelectedValuePath = "ID";
            }

            if (mode == "products")
            {
                typeCB.ItemsSource = Core.Context.ProductType.ToList();
                typeCB.DisplayMemberPath = "Name";
                typeCB.SelectedValuePath = "ID";

                manufacturerCB.ItemsSource = Core.Context.Manufacturer.ToList();
                manufacturerCB.DisplayMemberPath = "Name";
                manufacturerCB.SelectedValuePath = "ID";
            }
        }

        private void ApplyMode()
        {
            SetAllVisible(false);

            nameTBl.Visibility = nameTB.Visibility = Visibility.Visible;

            if (mode == "services")
            {
                titleTBl.Text = "Услуга";

                priceTBl.Visibility = priceTB.Visibility = Visibility.Visible;
                imageTBl.Visibility = imageTB.Visibility = Visibility.Visible;
                typeTBl.Visibility = typeCB.Visibility = Visibility.Visible;
            }

            else if (mode == "products")
            {
                titleTBl.Text = "Товар";

                priceTBl.Visibility = priceTB.Visibility = Visibility.Visible;
                imageTBl.Visibility = imageTB.Visibility = Visibility.Visible;
                descTBl.Visibility = descTB.Visibility = Visibility.Visible;
                typeTBl.Visibility = typeCB.Visibility = Visibility.Visible;
                manufacturerTBl.Visibility = manufacturerCB.Visibility = Visibility.Visible;
                discountTBl.Visibility = discountTB.Visibility = Visibility.Visible;
                statusTBl.Visibility = statusTB.Visibility = Visibility.Visible;
            }

            else if (mode == "records")
            {
                titleTBl.Text = "Запись";

                LoadAvailableDates();

                nameTBl.Visibility = nameTB.Visibility = Visibility.Visible;
                priceTBl.Visibility = priceTB.Visibility = Visibility.Visible;
                imageTBl.Visibility = imageTB.Visibility = Visibility.Visible;

                clientTBl.Visibility = clientTB.Visibility = Visibility.Visible;
                statusTBl.Visibility = statusTB.Visibility = Visibility.Visible;
                commentTBl.Visibility = commentTB.Visibility = Visibility.Visible;

                dateTBl.Visibility = dateDP.Visibility = Visibility.Visible;
                timeTBl.Visibility = timeCB.Visibility = Visibility.Visible;

                paymentTypeTBl.Visibility =
                    cardRB.Visibility =
                    cashRB.Visibility =
                    sbpRB.Visibility = Visibility.Visible;
            }

            else if (mode == "orders")
            {
                titleTBl.Text = "Заказ";

                nameTBl.Visibility = nameTB.Visibility = Visibility.Visible;
                clientTBl.Visibility = clientTB.Visibility = Visibility.Visible;
                statusTBl.Visibility = statusTB.Visibility = Visibility.Visible;
                takenCB.Visibility = Visibility.Visible;
            }

            else if (mode == "manufacturers")
            {
                titleTBl.Text = "Производитель";
                nameTBl.Visibility = nameTB.Visibility = Visibility.Visible;
            }

            else if (mode == "serviceTypes")
            {
                titleTBl.Text = "Тип услуги";
                nameTBl.Visibility = nameTB.Visibility = Visibility.Visible;
            }

            else if (mode == "productTypes")
            {
                titleTBl.Text = "Тип товара";
                nameTBl.Visibility = nameTB.Visibility = Visibility.Visible;
            }
        }

        private void SetAllVisible(bool visible)
        {
            var v = visible ? Visibility.Visible : Visibility.Collapsed;

            nameTBl.Visibility = nameTB.Visibility = v;
            priceTBl.Visibility = priceTB.Visibility = v;
            imageTBl.Visibility = imageTB.Visibility = v;
            descTBl.Visibility = descTB.Visibility = v;
            typeTBl.Visibility = typeCB.Visibility = v;
            manufacturerTBl.Visibility = manufacturerCB.Visibility = v;
            clientTBl.Visibility = clientTB.Visibility = v;
            statusTBl.Visibility = statusTB.Visibility = v;
            commentTBl.Visibility = commentTB.Visibility = v;
            dateTBl.Visibility = dateDP.Visibility = v;
            timeTBl.Visibility = timeCB.Visibility = v;
            discountTBl.Visibility = discountTB.Visibility = v;

            paymentTypeTBl.Visibility =
                cardRB.Visibility =
                cashRB.Visibility =
                sbpRB.Visibility = v;

            takenCB.Visibility = v;
        }

        private void LoadData()
        {
            if (mode == "products")
            {
                var item = Core.Context.Product.First(x => x.ID == id);

                nameTB.Text = item.Name ?? "";
                priceTB.Text = item.Price.ToString();
                imageTB.Text = item.ImagePath ?? "";
                descTB.Text = item.Description ?? "";

                discountTB.Text = item.Discount?.ToString() ?? "";

                typeCB.SelectedValue = item.ProductTypeID;
                manufacturerCB.SelectedValue = item.ManufacturerID;
            }

            else if (mode == "services")
            {
                var item = Core.Context.Service.First(x => x.ID == id);

                nameTB.Text = item.Name ?? "";
                priceTB.Text = item.Price.ToString();
                imageTB.Text = item.ImagePath ?? "";

                typeCB.SelectedValue = item.ServiceTypeID;
            }
            
            else if (mode == "records")
            {
                var item = Core.Context.Record.First(x => x.ID == id);

                nameTB.Text = item.Service?.Name ?? "";
                priceTB.Text = item.Service != null ? item.Service.Price.ToString() : "";
                imageTB.Text = item.Service?.ImagePath ?? "";

                clientTB.Text = item.User?.FullName ?? "";
                statusTB.Text = item.RecordStatus?.Name ?? "";
                commentTB.Text = item.Comment ?? "";

                dateDP.SelectedDate = item.Timetable?.StartDateTime;

                if (dateDP.SelectedDate != null)
                    LoadTimes(dateDP.SelectedDate.Value);

                timeCB.SelectedValue = item.TimetableID;

                selectedPaymentTypeID = item.PaymentTypeID;
                selectedServiceID = item.ServiceID;

                if (item.PaymentTypeID == 1) cardRB.IsChecked = true;
                if (item.PaymentTypeID == 2) cashRB.IsChecked = true;
                if (item.PaymentTypeID == 3) sbpRB.IsChecked = true;
            }

            else if (mode == "orders")
            {
                var item = Core.Context.Order.First(x => x.ID == id);

                nameTB.Text = $"Заказ #{item.ID}";
                clientTB.Text = item.User?.FullName ?? "";
                statusTB.Text = item.OrderStatus ?? "";
                takenCB.IsChecked = item.IsTaken;
            }

            else if (mode == "manufacturers")
            {
                nameTB.Text = Core.Context.Manufacturer.First(x => x.ID == id).Name;
            }

            else if (mode == "serviceTypes")
            {
                nameTB.Text = Core.Context.ServiceType.First(x => x.ID == id).Name;
            }

            else if (mode == "productTypes")
            {
                nameTB.Text = Core.Context.ProductType.First(x => x.ID == id).Name;
            }
        }

        private void LoadAvailableDates()
        {
            var data = Core.Context.Timetable.ToList();

            var dates = data.Where(t => !t.ISBooked).Select(t => t.StartDateTime.Date).Distinct().ToList();
            if (!dates.Any()) return;

            dateDP.DisplayDateStart = dates.Min();
            dateDP.DisplayDateEnd = dates.Max();
        }

        private void LoadTimes(DateTime date)
        {
            var selected = date.Date;

            var slots = Core.Context.Timetable.ToList().Where(t => t.StartDateTime.Date == selected)
                .Select(t => new
                {
                    ID = t.ID,
                    TimeText =
                        t.StartDateTime.ToString("HH:mm") + " - " +
                        t.EndDateTime.ToString("HH:mm") +
                        (t.ISBooked ? " (занято)" : "")
                }).ToList();

            timeCB.ItemsSource = slots;
            timeCB.DisplayMemberPath = "TimeText";
            timeCB.SelectedValuePath = "ID";
        }

        private void dateDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateDP.SelectedDate != null)
                LoadTimes(dateDP.SelectedDate.Value);
        }

        private void timeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timeCB.SelectedValue == null) return;

            int slotId = (int)timeCB.SelectedValue;

            var slot = Core.Context.Timetable.ToList().FirstOrDefault(t => t.ID == slotId);
            if (slot != null && slot.ISBooked)
            {
                MessageBox.Show("Это время уже занято");
                timeCB.SelectedItem = null;
            }
        }

        private void paymentTypeRB_Checked(object sender, RoutedEventArgs e)
        {
            if (cardRB.IsChecked == true) selectedPaymentTypeID = 1;
            if (cashRB.IsChecked == true) selectedPaymentTypeID = 2;
            if (sbpRB.IsChecked == true) selectedPaymentTypeID = 3;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            MarkChanged();

            if (mode == "records")
            {
                var item = Core.Context.Record.First(x => x.ID == id);

                var user = Core.Context.User.FirstOrDefault(x => x.FullName == clientTB.Text);
                if (user == null) return;

                int timetableID = (int)timeCB.SelectedValue;

                var slot = Core.Context.Timetable.First(t => t.ID == timetableID);
                var oldSlot = Core.Context.Timetable.First(t => t.ID == item.TimetableID);

                if (oldSlot.ID != timetableID)
                    oldSlot.ISBooked = false;

                item.ClientID = user.ID;
                item.TimetableID = timetableID;
                item.Comment = commentTB.Text;
                item.RecordStatusID = ConvertStatus(statusTB.Text);
                item.PaymentTypeID = selectedPaymentTypeID;

                slot.ISBooked = true;
            }

            else if (mode == "products")
            {
                var item = Core.Context.Product.First(x => x.ID == id);

                item.Name = nameTB.Text;
                item.Price = string.IsNullOrWhiteSpace(priceTB.Text)
                    ? 0
                    : double.Parse(priceTB.Text);

                item.ImagePath = imageTB.Text;
                item.Description = descTB.Text;
            }

            else if (mode == "services")
            {
                var item = Core.Context.Service.First(x => x.ID == id);

                item.Name = nameTB.Text;
                item.Price = string.IsNullOrWhiteSpace(priceTB.Text)
                    ? 0
                    : double.Parse(priceTB.Text);

                item.ImagePath = imageTB.Text;
            }

            else if (mode == "orders")
            {
                var item = Core.Context.Order.First(x => x.ID == id);

                item.OrderStatus = statusTB.Text;
                item.IsTaken = takenCB.IsChecked == true;
            }

            else if (mode == "manufacturers")
            {
                Core.Context.Manufacturer.First(x => x.ID == id).Name = nameTB.Text;
            }

            else if (mode == "serviceTypes")
            {
                Core.Context.ServiceType.First(x => x.ID == id).Name = nameTB.Text;
            }

            else if (mode == "productTypes")
            {
                Core.Context.ProductType.First(x => x.ID == id).Name = nameTB.Text;
            }

            Core.Context.SaveChanges();
            Close();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            MarkChanged();

            if (mode == "records")
            {
                var user = Core.Context.User.FirstOrDefault(x => x.FullName == clientTB.Text);
                if (user == null) return;

                int timetableID = (int)timeCB.SelectedValue;

                var slot = Core.Context.Timetable.First(t => t.ID == timetableID);

                Core.Context.Record.Add(new Record
                {
                    ClientID = user.ID,
                    TimetableID = timetableID,
                    ServiceID = selectedServiceID,
                    RecordStatusID = ConvertStatus(statusTB.Text),
                    Comment = commentTB.Text,
                    PaymentTypeID = selectedPaymentTypeID
                });

                slot.ISBooked = true;
            }

            else if (mode == "products")
            {
                Core.Context.Product.Add(new Product
                {
                    Name = nameTB.Text,
                    Price = string.IsNullOrWhiteSpace(priceTB.Text)
                        ? 0
                        : double.Parse(priceTB.Text),
                    ImagePath = imageTB.Text,
                    Description = descTB.Text,
                    ProductTypeID = (int)typeCB.SelectedValue,
                    ManufacturerID = (int)manufacturerCB.SelectedValue
                });
            }

            else if (mode == "services")
            {
                Core.Context.Service.Add(new Service
                {
                    Name = nameTB.Text,
                    Price = string.IsNullOrWhiteSpace(priceTB.Text)
                        ? 0
                        : double.Parse(priceTB.Text),
                    ImagePath = imageTB.Text,
                    ServiceTypeID = (int)typeCB.SelectedValue
                });
            }

            else if (mode == "orders")
            {
                Core.Context.Order.Add(new Order
                {
                    OrderStatus = statusTB.Text,
                    IsTaken = takenCB.IsChecked == true
                });
            }

            else if (mode == "manufacturers")
            {
                Core.Context.Manufacturer.Add(new Manufacturer { Name = nameTB.Text });
            }

            else if (mode == "serviceTypes")
            {
                Core.Context.ServiceType.Add(new ServiceType { Name = nameTB.Text });
            }

            else if (mode == "productTypes")
            {
                Core.Context.ProductType.Add(new ProductType { Name = nameTB.Text });
            }

            Core.Context.SaveChanges();
            Close();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MarkChanged();

            if (mode == "records")
            {
                var item = Core.Context.Record.First(x => x.ID == id);

                var slot = Core.Context.Timetable.First(t => t.ID == item.TimetableID);
                slot.ISBooked = false;

                Core.Context.Record.Remove(item);
            }

            else if (mode == "products")
                Core.Context.Product.Remove(Core.Context.Product.First(x => x.ID == id));

            else if (mode == "services")
                Core.Context.Service.Remove(Core.Context.Service.First(x => x.ID == id));

            else if (mode == "orders")
                Core.Context.Order.Remove(Core.Context.Order.First(x => x.ID == id));

            else if (mode == "manufacturers")
                Core.Context.Manufacturer.Remove(Core.Context.Manufacturer.First(x => x.ID == id));

            else if (mode == "serviceTypes")
                Core.Context.ServiceType.Remove(Core.Context.ServiceType.First(x => x.ID == id));

            else if (mode == "productTypes")
                Core.Context.ProductType.Remove(Core.Context.ProductType.First(x => x.ID == id));

            Core.Context.SaveChanges();
            Close();
        }
    }
}

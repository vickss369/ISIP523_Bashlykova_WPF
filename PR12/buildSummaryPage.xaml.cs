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
    /// Логика взаимодействия для buildSummaryPage.xaml
    /// </summary>
    
    public partial class buildSummaryPage : Page
    {
        private List<basepart_> selectedParts;
        List<string> errors = new List<string>();

        public buildSummaryPage(List<basepart_> parts)
        {
            InitializeComponent();
            saveBtn.IsEnabled = false;
            buildNameTB.IsEnabled = false;
            authorTB.IsEnabled = false;

            selectedParts = parts;

            LoadParts();
            CalculateTotalPrice();
            CheckCompatibility();
        }

        private void LoadParts()
        {
            selectedPartsList.ItemsSource = selectedParts;
        }

        private void CalculateTotalPrice()
        {
            decimal total = selectedParts.Sum(p => p.price);
            totalPriceTBl.Text = $"{total}$";
        }

        private void CheckCompatibility()
        {
            var cpu = selectedParts.FirstOrDefault(p => p.cpu_ != null)?.cpu_;
            var motherboard = selectedParts.FirstOrDefault(p => p.motherboard_ != null)?.motherboard_;
            var cooler = selectedParts.FirstOrDefault(p => p.processorcooler_ != null)?.processorcooler_;
            var ram = selectedParts.FirstOrDefault(p => p.ram_ != null)?.ram_;
            var gpu = selectedParts.FirstOrDefault(p => p.gpu_ != null)?.gpu_;
            var psu = selectedParts.FirstOrDefault(p => p.powersupply_ != null)?.powersupply_;
            var pcCase = selectedParts.FirstOrDefault(p => p.case_ != null)?.case_;

            if (cpu != null && motherboard != null && cpu.socketid != motherboard.socketid)
            {
                errors.Add("! Сокет процессора несовместим с материнской платой");
            }

            if (cooler != null && cpu != null && !cooler.socketprocessorcooler_.Any(s => s.socketid == cpu.socketid))
            {
                errors.Add("! Кулер не поддерживает сокет процессора");
            }

            if (motherboard != null && pcCase != null && !pcCase.boardformfactorcase_.Any(f => f.formfactorid == motherboard.formfactorid))
            {
                errors.Add("! Корпус не поддерживает форм-фактор материнской платы");
            }

            if (ram != null && motherboard != null && ram.memorytypeid != motherboard.memorytypeid)
            {
                errors.Add("! Тип оперативной памяти не поддерживается материнской платой");
            }

            if (gpu != null && psu != null && gpu.recommendpower.HasValue && gpu.recommendpower.Value > psu.power)
            {
                errors.Add("! Недостаточная мощность блока питания");
            }

            compatibilityTBl.Text = errors.Count == 0 ? "Все комплектующие совместимы!" : string.Join("\n", errors);
            if (errors.Count == 0)
            {
                saveBtn.IsEnabled = true;
                buildNameTB.IsEnabled = true;
                authorTB.IsEnabled = true;
            }
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            var part = (sender as Button).DataContext as basepart_;

            if (part == null) return;

            selectedParts.Remove(part);

            selectedPartsList.ItemsSource = null;
            selectedPartsList.ItemsSource = selectedParts;

            CalculateTotalPrice();
            CheckCompatibility();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(buildNameTB.Text) || string.IsNullOrWhiteSpace(authorTB.Text))
            {
                MessageBox.Show("Введите название сборки и автора");
                return;
            }

            var assembly = new assembly_
            {
                name = buildNameTB.Text,
                author = authorTB.Text
            };

            Core.Context.assembly_.Add(assembly);
            Core.Context.SaveChanges();

            foreach (var part in selectedParts)
            {
                Core.Context.partassembly_.Add(new partassembly_
                {
                    assemblyid = assembly.id,
                    partid = part.id
                });
            }

            Core.Context.SaveChanges();

            assembly.partassembly_ = Core.Context.partassembly_.Where(p => p.assemblyid == assembly.id).ToList();

            MessageBox.Show(
                $"Сборка сохранена!\n\n" +
                $"Название: {assembly.name}\n" +
                $"Автор: {assembly.author}\n" +
                $"Количество частей: {assembly.PartsCount}\n" +
                $"Общая цена: {assembly.TotalPrice:C}\n" +
                $"Дата создания: {assembly.CreatedDateFormatted}"
            );

            NavigationService.Navigate(new MainPage());
        }
    }
}

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
        public buildSummaryPage(List<basepart_> parts)
        {
            InitializeComponent();

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
            totalPriceText.Text = $"{total} ₽";
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

            List<string> errors = new List<string>();

            if (cpu != null && motherboard != null && cpu.socketid != motherboard.socketid)
            {
                errors.Add("Сокет процессора несовместим с материнской платой");
            }

            if (cooler != null && cpu != null && !cooler.socketprocessorcooler_.Any(s => s.socketid == cpu.socketid))
            {
                errors.Add("Кулер не поддерживает сокет процессора");
            }

            if (motherboard != null && pcCase != null && !pcCase.boardformfactorcase_.Any(f => f.formfactorid == motherboard.formfactorid))
            {
                errors.Add("Корпус не поддерживает форм-фактор материнской платы");
            }

            if (ram != null && motherboard != null && ram.memorytypeid != motherboard.memorytypeid)
            {
                errors.Add("Тип оперативной памяти не поддерживается материнской платой");
            }

            if (gpu != null && psu != null && gpu.recommendpower.HasValue && gpu.recommendpower.Value > psu.power)
            {
                errors.Add("Недостаточная мощность блока питания");
            }

            compatibilityText.Text = errors.Count == 0 ? "Все комплектующие совместимы!" : string.Join("\n", errors);
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(buildNameBox.Text) ||
                string.IsNullOrWhiteSpace(authorBox.Text))
            {
                MessageBox.Show("Введите название сборки и автора");
                return;
            }

            var assembly = new assembly_
            {
                name = buildNameBox.Text,
                author = authorBox.Text
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

            MessageBox.Show("Сборка сохранена!");
        }
    }
}

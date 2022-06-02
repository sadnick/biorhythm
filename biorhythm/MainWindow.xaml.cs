using LiveCharts;
using LiveCharts.Wpf;
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

namespace biorhythm
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainWindow
    {

        List<string> Labels = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            arbitrary.IsEnabled = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            arbitrary.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<operation> biorhythms = new List<operation>();
            int arbitrarys = 0;
            DateTime birthDate = DateTime.Now;

            const int phys = 23;
            const int emot = 28;
            const int intel = 33;

            try
            {
                birthDate = Convert.ToDateTime(BirthDate.Text);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при получении даты рождения");
            }

            if (arbitrary.IsEnabled == true)
            {
                try
                {
                    arbitrarys = Convert.ToInt32(arbitrary.Text);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при получении данных отcчета");
                }
            }
            else
            {
                try
                {
                    arbitrarys = Convert.ToInt32(cmbCountdown.Text);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при получении данных отчета");
                }
            }
            DateTime dateCountDown = Convert.ToDateTime(countDownTime.Text);

            double maxEm = double.MinValue;
            double maxInt = double.MinValue;
            double maxPhys = double.MinValue;
            double maxSum = double.MinValue;
            string maxEmDate = String.Empty;
            string maxIntDate = String.Empty;
            string maxPhysDate = String.Empty;
            string maxSumDate = String.Empty;


            for (int i = 0; i < arbitrarys; i++)
            {
                var bior = new operation()
                {
                    Date = dateCountDown.AddDays(i).ToShortDateString(),
                    Emotional = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / emot)) * 100, 2),
                    Intellectual = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / intel)) * 100, 2),
                    Physical = Math.Round((Math.Sin(2 * Math.PI * ((dateCountDown - birthDate).Days + i) / phys)) * 100, 2),
                };
                bior.Total = Math.Round(bior.Emotional + bior.Intellectual + bior.Physical, 2);

                biorhythms.Add(bior);
            }
            Dates.ItemsSource = biorhythms;

            foreach (operation bior in biorhythms)
            {
                if (bior.Physical > maxPhys)
                {
                    maxPhys = bior.Physical;
                    maxPhysDate = bior.Date;
                }
                if (bior.Emotional > maxEm)
                {
                    maxEm = bior.Emotional;
                    maxEmDate = bior.Date;
                }
                if (bior.Intellectual > maxInt)
                {
                    maxInt = bior.Intellectual;
                    maxIntDate = bior.Date;
                }
                if (bior.Total > maxSum)
                {
                    maxSum = bior.Total;
                    maxSumDate = bior.Date;
                }
            }


            stat.Items.Clear();
            stat.Items.Add($"Дата рождения - {birthDate.ToShortDateString()}");
            stat.Items.Add($"Длительность прогноза - {arbitrarys}");
            stat.Items.Add($"Период с - {dateCountDown.ToShortDateString()} - {dateCountDown.AddDays(arbitrarys).ToShortDateString()}");
            stat.Items.Add($"Эмоциональный максимум - {maxEm}: {maxEmDate}");
            stat.Items.Add($"Интеллектуальный максимум - {maxInt}: {maxIntDate}");
            stat.Items.Add($"Физический максимум - {maxPhys}: {maxPhysDate}");

            ChartValues<double> PhysicalValues = new ChartValues<double>();
            ChartValues<double> EmotionalValues = new ChartValues<double>();
            ChartValues<double> IntellectualValues = new ChartValues<double>();
            SeriesCollection series = new SeriesCollection();

            foreach (operation bior in biorhythms)
            {
                PhysicalValues.Add(bior.Physical);
                EmotionalValues.Add(bior.Emotional);
                IntellectualValues.Add(bior.Intellectual);
                Labels.Add(bior.Date.ToString());
            }

            series.Add(new LineSeries
            {
                Title = "Физические ритмы",
                Values = PhysicalValues,
            });
            series.Add(new LineSeries
            {
                Title = "Эмоциональные ритмы",
                Values = EmotionalValues
            });
            series.Add(new LineSeries
            {
                Title = "Интеллектуальные ритмы",
                Values = IntellectualValues
            });
            chart.AxisY = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Значения",
                    MinValue = -100,
                    MaxValue = 100,
                }
            };
            chart.Series = series;
            chart.Update();


            if (dateon.IsChecked == true)
            {
                chart.AxisX = new AxesCollection()
                {
                    new Axis()
                    {
                        Title = "Дата",
                        Labels = Labels,
                    }
                };
            }
            else
            {
                chart.AxisX = new AxesCollection()
                {
                    new Axis()
                    {
                        Title = "Дата",
                        MinValue = 1,
                    }
                };
            }
        }
    }
}
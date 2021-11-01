using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace MoulaCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        long Totalresult = 0;
        Historique HistoWindow = new Historique();
        public MainWindow()
        {
            InitializeComponent();

            DatabaseManager databaseManager = new();
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            databaseManager.createTables();
            databaseManager.closeDbConnection();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxBillet_TextChanged(object sender, TextChangedEventArgs e)
        {
            long Result5 = 0;
            long Result10 = 0;
            long Result20 = 0;
            long Result50 = 0;
            long Result100 = 0;
            long Result200 = 0;
            long Result500 = 0;

            if (TextBoxBillet5.Text != "")
            {
                Result5 = long.Parse(TextBoxBillet5.Text) * 5;
            }
            if (TextBoxBillet10.Text != "")
            {
                Result10 = long.Parse(TextBoxBillet10.Text) * 10;
            }
            if (TextBoxBillet20.Text != "")
            {
                Result20 = long.Parse(TextBoxBillet20.Text) * 20;
            }
            if (TextBoxBillet50.Text != "")
            {
                Result50 = long.Parse(TextBoxBillet50.Text) * 50;
            }
            if (TextBoxBillet100.Text != "")
            {
                Result100 = long.Parse(TextBoxBillet100.Text) * 100;
            }
            if (TextBoxBillet200.Text != "")
            {
                Result200 = long.Parse(TextBoxBillet200.Text) * 200;
            }
            if (TextBoxBillet500.Text != "")
            {
                Result500 = long.Parse(TextBoxBillet500.Text) * 500;
            }
            Totalresult = Result5 + Result10 + Result20 + Result50 + Result100 + Result200 + Result500;
            TotalResult.Content = Totalresult.ToString() + " €";
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HistoClicked(object sender, RoutedEventArgs e)
        {
            HistoWindow = new Historique();
            HistoWindow.Owner = this;
            HistoWindow.Show();
        }

        private void SaveButton_Pressed(object sender, RoutedEventArgs e)
        {
            AlloBank alloBank = new();
            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            alloBank.InsertAlloBank(Totalresult.ToString(), dt);
            HistoWindow.UpdateDataGrid();

        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
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
        long Result5 = 0;
        long Result10 = 0;
        long Result20 = 0;
        long Result50 = 0;
        long Result100 = 0;
        long Result200 = 0;
        long Result500 = 0;
        string Monnaie = "0";
        long Totalresult = 0;
        Historique HistoWindow = new Historique();
        public MainWindow()
        {
            InitializeComponent();
            InitNbBilletsAndDB();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxBillet_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxBillet5.Text != "")
            {
                Result5 = long.Parse(TextBoxBillet5.Text) * 5;
            }
            else
            {
                Result5 = 0;
            }
            if (TextBoxBillet10.Text != "")
            {
                Result10 = long.Parse(TextBoxBillet10.Text) * 10;
            }
            else
            {
                Result10 = 0;
            }
            if (TextBoxBillet20.Text != "")
            {
                Result20 = long.Parse(TextBoxBillet20.Text) * 20;
            }
            else
            {
                Result20 = 0;
            }
            if (TextBoxBillet50.Text != "")
            {
                Result50 = long.Parse(TextBoxBillet50.Text) * 50;
            }
            else
            {
                Result50 = 0;
            }
            if (TextBoxBillet100.Text != "")
            {
                Result100 = long.Parse(TextBoxBillet100.Text) * 100;
            }
            else
            {
                Result100 = 0;
            }
            if (TextBoxBillet200.Text != "")
            {
                Result200 = long.Parse(TextBoxBillet200.Text) * 200;
            }
            else
            {
                Result200 = 0;
            }
            if (TextBoxBillet500.Text != "")
            {
                Result500 = long.Parse(TextBoxBillet500.Text) * 500;
            }
            else
            {
                Result500 = 0;
            }
            if (TextBoxMonnaie.Text != "")
            {
                Monnaie = TextBoxMonnaie.Text;
            }
            else
            {
                Monnaie = "0 €";
            }

            Totalresult = Result5 + Result10 + Result20 + Result50 + Result100 + Result200 + Result500;
            TotalResult.Content = "Sortie banque\n" + Totalresult.ToString() + " €";
            long NbBillets = Result5 / 5 + Result10 / 10 + Result20 / 20 + Result50 / 50 + Result100 / 100 + Result200 / 200 + Result500 / 500;
            NbBilletsBtn.Content = "Sauvegarder la sacoche :\n" + NbBillets + " billets";
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
            if (MessageBox.Show("êtes-vous sûr de vouloir faire une sortie banque ?",
                                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AlloBank alloBank = new();
                string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                alloBank.InsertAlloBank(Totalresult.ToString(), dt, long.Parse(TextBoxBillet5.Text), long.Parse(TextBoxBillet10.Text), long.Parse(TextBoxBillet20.Text), long.Parse(TextBoxBillet50.Text), long.Parse(TextBoxBillet100.Text), long.Parse(TextBoxBillet200.Text), long.Parse(TextBoxBillet500.Text));
                HistoWindow.UpdateDataGrid();
            }
        }

        private void InitNbBilletsAndDB ()
        {

            DatabaseManager databaseManager = new();
            databaseManager.createDbFile();
            databaseManager.createDbConnection();
            string select = "SELECT Billet5, Billet10, Billet20, Billet50, Billet100, Billet200, Billet500, Monnaie FROM Encours";
            SQLiteDataReader reader = databaseManager.executeSelectQuery(select);
            long Billet5 = new();
            long Billet10 = new();
            long Billet20 = new();
            long Billet50 = new();
            long Billet100 = new();
            long Billet200 = new();
            long Billet500 = new();
            string Monnaie = "0 €";
            while (reader.Read())
            {
                Billet5 = long.Parse(reader["Billet5"].ToString());
                Billet10 = long.Parse(reader["Billet10"].ToString());
                Billet20 = long.Parse(reader["Billet20"].ToString());
                Billet50 = long.Parse(reader["Billet50"].ToString());
                Billet100 = long.Parse(reader["Billet100"].ToString());
                Billet200 = long.Parse(reader["Billet200"].ToString());
                Billet500 = long.Parse(reader["Billet500"].ToString());
                Monnaie = reader["Monnaie"].ToString();
            }
            
            TextBoxBillet5.Text = Billet5.ToString();
            TextBoxBillet10.Text = Billet10.ToString();
            TextBoxBillet20.Text = Billet20.ToString();
            TextBoxBillet50.Text = Billet50.ToString();
            TextBoxBillet100.Text = Billet100.ToString();
            TextBoxBillet200.Text = Billet200.ToString();
            TextBoxBillet500.Text = Billet500.ToString();
            TextBoxMonnaie.Text = Monnaie + " €";
            reader.Close();
            databaseManager.closeDbConnection();

        }

        private void SaveNbBillets_Pressed(object sender, RoutedEventArgs e)
        {
            AlloBank alloBank = new();
            alloBank.InsertNbBillets(Result5 / 5, Result10 / 10, Result20 / 20, Result50 / 50, Result100 / 100, Result200 / 200, Result500 / 500, Monnaie);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FillEmptyTextBlocks(object sender, RoutedEventArgs e)
        {
            if (TextBoxBillet5.Text == "")
            {
                TextBoxBillet5.Text = "0";
            }
            if (TextBoxBillet10.Text == "")
            {
                TextBoxBillet10.Text = "0";
            }
            if (TextBoxBillet20.Text == "")
            {
                TextBoxBillet20.Text = "0";
            }
            if (TextBoxBillet50.Text == "")
            {
                TextBoxBillet50.Text = "0";
            }
            if (TextBoxBillet100.Text == "")
            {
                TextBoxBillet100.Text = "0";
            }
            if (TextBoxBillet200.Text == "")
            {
                TextBoxBillet200.Text = "0";
            }
            if (TextBoxBillet500.Text == "")
            {
                TextBoxBillet500.Text = "0";
            }
            if (TextBoxMonnaie.Text == "")
            {
                TextBoxMonnaie.Text = "0";
            }
        }
    }
}

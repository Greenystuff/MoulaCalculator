using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MoulaCalc
{
    public partial class Historique : Window
    {

        DatabaseManager dbManager = new();
        private PagingCollectionView pagingCollectionView;
        public Historique()
        {
            InitializeComponent();
            UpdateDataGrid();
            HistoField.SelectedIndex = -1;
        }

        private void Reset_Database(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Voulez vous vraiment effacer la base de données ?",
                                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AlloBank alloBank = new();
                alloBank.DeleteAlloBank();
                ((AreaSeries)StatChart.Series[0]).ItemsSource = null;
                HistoField.ItemsSource = null;
                ((AreaSeries)StatChart.Series[0]).Refresh();
            }
        }

        private void DeleteRows_Cliqued(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < HistoField.Items.Count; i++)
            {
                var item = HistoField.Items[i];
                DataRowView dataRowView = HistoField.Items[i] as DataRowView;
                CheckBox mycheckbox = HistoField.Columns[0].GetCellContent(item) as CheckBox;
                if ((bool)mycheckbox.IsChecked)
                {
                    int Id = Int32.Parse(dataRowView.Row["Id"].ToString());
                    AlloBank alloBank = new();
                    alloBank.DeleteByRowID(Id);
                }
            }
            UpdateDataGrid();
        }

        public void UpdateDataGrid()
        {
            dbManager.CreateDbFile();
            dbManager.CreateDbConnection();
            dbManager.CreateTables();
            SQLiteConnection conn = dbManager.Connection();
            SQLiteCommand command = new("SELECT Total,Date,Id,Billet5,Billet10,Billet20,Billet50,Billet100,Billet200,Billet500 FROM AlloBank ORDER BY Date DESC", conn);
            SQLiteDataAdapter sqlda = new(command);
            DataSet ds = new();
            sqlda.Fill(ds);

            pagingCollectionView = new PagingCollectionView(ds.Tables[0].DefaultView, 12);
            HistoField.ItemsSource = pagingCollectionView;

            List<KeyValuePair<string, long>> list = new();
            for (int i = HistoField.Items.Count - 1; i >= 0; i--)
            {
                DataRowView dataRowView = (DataRowView)HistoField.Items[i];
                list.Add(new KeyValuePair<string, long>(dataRowView.Row["Date"].ToString(), long.Parse(dataRowView.Row["Total"].ToString())));
            }
            
            ((AreaSeries)StatChart.Series[0]).ItemsSource = list;
            ((AreaSeries)StatChart.Series[0]).Refresh();

            dbManager.CloseDbConnection();
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this.pagingCollectionView.MoveToNextPage();
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this.pagingCollectionView.MoveToPreviousPage();
        }

        private void HideDetails(object sender, MouseButtonEventArgs e)
        {
            HistoField.SelectedIndex = -1;
        }

        private void Grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //// Have to do this in the unusual case where the border of the cell gets selected
            //// and causes a crash 'EditItem is not allowed'
            e.Cancel = true;
            HistoField.SelectedIndex = -1;
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class MainWindow
{
    public List<Counter> Counters = new();


    //Constructor
    public MainWindow()
    {
        InitializeComponent();
        RefreshComboBox();
        RefreshTable();
    }

    public CounterValue Data { get; set; }

    //Buttons
    private void cbCounter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbCounter.SelectedItem == null)
        {
            return;
        }

        RefreshTable();
    }

    private void ClickCounterBearbeiten(object sender, RoutedEventArgs e)
    {
        AddNewCounter addNewCounter = new();
        addNewCounter.ShowDialog();
        RefreshComboBox();
        RefreshTable();
    }

    private void EventAlterCounterData(object sender, MouseButtonEventArgs e)
    {
        if (TableDb.SelectedValue != null)
        {
            var secondWindow = new SecondWindow((CounterValue) TableDb.SelectedValue);
            secondWindow.Title = "Zählerstand bearbeiten";
            secondWindow.ShowDialog();
            RefreshComboBox();
            RefreshTable();
        }
    }

    private void BtnZählerstandAnlegen(object sender, RoutedEventArgs e)
    {
        var secondWindow = new SecondWindow(cbCounter.SelectedValue.ToString());
        secondWindow.Title = "Zählerstand Neu Anlegen";
        secondWindow.ShowDialog();
        RefreshComboBox();
        RefreshTable();
    }

    private void BtnLöschenRow(object sender, RoutedEventArgs e)
    {
        using SzContext context = new();
        Data = (CounterValue) TableDb.SelectedValue;

        var removeCounterValues = context.CounterValues.Where(x => x.Id == Data.Id).ToList();

        foreach (var value in removeCounterValues)
        {
            context.CounterValues.Remove(value);
            context.SaveChanges();
        }

        MessageBox.Show("Ihr Datensatz wurde erfolgreich gelöscht.");
        RefreshTable();
    }

    private void ClickCreateUser(object sender, RoutedEventArgs e)
    {
        Verwaltung verwaltung = new();
        verwaltung.Show();
    }

    private void ClickExit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    //Methods for this Class
    public void RefreshTable()
    {
        using SzContext context = new();

        var selectedItem = (Counter) cbCounter.SelectedItem;
        TableDb.ItemsSource = null;
        if (selectedItem.ID != 0)
        {
            TableDb.ItemsSource = context.CounterValues.Include(x => x.Counter).Where(x => x.CounterId == selectedItem.ID).ToList();

        }
        else
        {
            TableDb.ItemsSource = context.CounterValues.Include(x => x.Counter).ToList();

        }
    }

    public void RefreshComboBox()
    {
        Counters.Clear();
        cbCounter.ItemsSource = null;
        cbCounter.ItemsSource = GetCounter();
        cbCounter.DisplayMemberPath = "Name";
        cbCounter.SelectedIndex = 0;
    }

    public List<Counter> GetCounter()
    {
        using SzContext context = new();

        var counterList = context.Counters.Select(x => x).ToList();

        counterList.Add(new Counter
        {
            ID = 0,
            Name = "Select all"
        });

        return counterList.OrderBy(x => x.ID).ToList();
    }
}
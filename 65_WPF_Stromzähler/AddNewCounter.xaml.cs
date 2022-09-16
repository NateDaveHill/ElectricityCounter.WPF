using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class AddNewCounter
{
    //Constructor
    public AddNewCounter()
    {
        InitializeComponent();
        counterTable.ItemsSource = LoadCounterTable();
        FillECounterValue();
    }

    public int? Value { get; set; }

    public List<Counter> Counters { get; set; } = new();

    public Counter Data { get; set; }

    //Buttons
    private void BtnCounterAnlegen(object sender, RoutedEventArgs e)
    {
        CounterHinzufügen counterHinzufügen = new();
        counterHinzufügen.ShowDialog();
        counterTable.ItemsSource = LoadCounterTable();
    }

    private void BtnCounterLöschen(object sender, RoutedEventArgs e) 
    {
        using SzContext context = new SzContext();

        Data = (Counter) counterTable.SelectedValue;

        var counterValue = context.CounterValues.FirstOrDefault(x => x.CounterId == Data.ID);


        if (counterValue == null)
        {
            var counterDelete = context.Counters.FirstOrDefault(x => x.ID == Data.ID);
            context.Counters.Remove(counterDelete);
            context.SaveChanges();
            MessageBox.Show("Ihr Zähler wurde erfolgreich entfernt.");
            counterTable.ItemsSource = LoadCounterTable();
        }
        else
        {
            var result = MessageBox.Show(
                "Wenn Sie einen Zähler löschen werden alle dazugehörigen Zählerständer auch gelöscht. " +
                "\n\n Wollen Sie Fortfahren?", "My App", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Data = (Counter) counterTable.SelectedValue;


                var counterDelete2 = context.Counters.FirstOrDefault(x => x.ID == Data.ID);
                context.Counters.Remove(counterDelete2);

                var counterValueDelete = context.CounterValues.FirstOrDefault(x => x.CounterId == Data.ID);
                if (counterValueDelete == null)
                {
                    MessageBox.Show("Ihr Zähler wurde erfolgreich entfernt.");
                    context.SaveChanges();
                    counterTable.ItemsSource = LoadCounterTable();
                }
                else
                {
                    MessageBox.Show("Ihr Zähler wurde erfolgreich entfernt.");
                    context.CounterValues.Remove(counterValueDelete);
                    context.SaveChanges();
                    counterTable.ItemsSource = LoadCounterTable();
                }
            }
        }
    }


    //Methods for this Class
    public List<Counter> LoadCounterTable()
    {
        using SzContext context = new();

        return context.Counters.Select(x => new Counter
        {
            ID = x.ID,
            Name = x.Name
        }).ToList();
    }

    private int? FillECounterValue()
    {
        using SzContext context = new();

        var counterValueList = context.CounterValues.Select(x => x).ToList();

        foreach (var counterValue in counterValueList)
        {
            Value = counterValue.Id;
        }

        return Value;
    }

    private void Bearbeiten(object sender, MouseButtonEventArgs e)
    {
        if (counterTable.SelectedValue != null)
        {
            var currentSelectedValueFromCounter = (Counter) counterTable.SelectedValue;
            var currentCounterName = currentSelectedValueFromCounter.Name;
            var currentCounterId = currentSelectedValueFromCounter.ID;

            var counterHinzufügen = new CounterHinzufügen(currentCounterName, currentCounterId);
            counterHinzufügen.ShowDialog();
            counterTable.ItemsSource = LoadCounterTable();
        }
    }
}
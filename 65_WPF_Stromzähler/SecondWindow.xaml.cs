using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class SecondWindow
{
    private readonly SzContext context = new();
    private SqlCommand cmd;
    private Counter currentCounter;
    public MainWindow MainWindow = new();

    //Contructor01
    public SecondWindow(string cbSelection = "")
    {
        InitializeComponent();
        GetCounterName();
        context.Database.EnsureCreated();
        calBoxDate.DisplayDate = DateTime.Now;

        var currentCbSelection = MainWindow.cbCounter.SelectedValue;

        foreach (var counter in Counters)
        {
            var rb = new RadioButton
            {
                Content = counter.Name
            };
            rb.Margin = new Thickness(5);
            rb.Checked += (s, e) => { currentCounter = counter; };

            if (currentCounter == null)
            {
                rb.IsChecked = true;
                currentCounter = counter;
            }

            radioButtons.Children.Add(rb);
        }

        foreach (RadioButton item in radioButtons.Children)
        {
            if (cbSelection == item.Content.ToString())
            {
                currentCounter = Counters.FirstOrDefault(x => x.Name == cbSelection);
            }
        }
    }

    //Contructor02
    public SecondWindow(CounterValue data) : this()
    {
        Data = data;
        txtBoxCounter.Value = Data.Value;
        calBoxDate.SelectedDate = Data.Date;


        foreach (RadioButton item in radioButtons.Children)
        {
            if (item.Content.Equals(Data.Counter.Name))
            {
                currentCounter = Counters.FirstOrDefault(x => x.ID == Data.CounterId);
                item.IsChecked = true;
            }
        }
    }

    public List<Counter> Counters { get; set; } = new();
    public CounterValue Data { get; set; }
    public char Letter { get; set; }

    //Buttons
    private void btnSpeichern(object sender, RoutedEventArgs e)
    {
        if (txtBoxCounter.Value % 1 != 0)
        {
            MessageBox.Show("Bitte geben Sie eine Ganzzahl ein.");
            return;
        }

        if (calBoxDate.SelectedDate == null)
        {
            MessageBox.Show("Bitte wählen Sie ein Datum aus.");
            return;
        }

        if (calBoxDate.SelectedDate.Value.Year <= 1754 || calBoxDate.SelectedDate.Value.Year >= 9999)
        {
            MessageBox.Show("Bitte wählen Sie ein gültiges Datum zwischen 01.01.1753 und 31.12.9999 aus.");
            return;
        }

        if (txtBoxCounter.Value == null || txtBoxCounter.Value >= 999999 || txtBoxCounter.Value == 0)
        {
            MessageBox.Show("Bitte geben Sie eine Gültige Zahl ein.");
            return;
        }


        if (Data != null)
        {
            var updateEntityEntry = context.CounterValues.Where(x => x.Id == Data.Id).ToList();
            foreach (var entity in updateEntityEntry)
            {
                entity.Value = (int) txtBoxCounter.Value;
                entity.CounterId = (int) currentCounter.ID;
                entity.Date = (DateTime) calBoxDate.SelectedDate;
                context.CounterValues.Update(entity);
                context.SaveChanges();
            }

            MessageBox.Show("Ihre Daten wurden Erfolgreich geupdated.");
        }
        else
        {
            if (currentCounter != null)
            {
                context.CounterValues.Add(new CounterValue
                {
                    Date = (DateTime) calBoxDate.SelectedDate,
                    CounterId = (int) currentCounter.ID,
                    Value = (int) txtBoxCounter.Value
                });
                context.SaveChanges();

                MessageBox.Show("Ihre Daten wurden Erfolgreich gespeichert.");
            }
            else
            {
                MessageBox.Show("Bitte zuerst einen Stromzähler anlegen");
            }
        }

        Close();
    }


    private void btnAbbrechen(object sender, RoutedEventArgs e)
    {
        Close();
    }

    //Methods for this Class
    public void GetCounterName()
    {
        var counterList = context.Counters.Select(x => x).ToList();

        foreach (var counter in counterList)
        {
            Counters.Add(new Counter
            {
                ID = counter.ID,
                Name = counter.Name
            });
        }
    }
}
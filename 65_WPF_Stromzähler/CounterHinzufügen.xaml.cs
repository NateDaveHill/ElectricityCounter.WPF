using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class CounterHinzufügen
{
    //Constructor
    public CounterHinzufügen()
    {
        InitializeComponent();
        FillCounterList();
    }

    public CounterHinzufügen(string currentCounter, int? currentCounterId) : this()
    {
        newCounterNameHinzufügen.Text = currentCounter;
        CurrentCounterId = currentCounterId;
    }

    public int? CurrentCounterId { get; set; }
    public List<Counter> CounterList { get; set; } = new();

    public SqlCommand Cmd { get; set; }

    //Buttons

    private void BtnAbbrechen(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnSpeichern(object sender, RoutedEventArgs e)
    {
        using SzContext context = new();
        context.Database.EnsureCreated();

        foreach (var counter in CounterList)
        {
            if (newCounterNameHinzufügen.Text == counter.Name)
            {
                MessageBox.Show("Dieser Name ist bereits vergeben, bitte versuchen Sie es erneut.");
                return;
            }

            if (CurrentCounterId == null)
            {
                if (string.IsNullOrEmpty(newCounterNameHinzufügen.Text))
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Namen ein.");
                    return;
                }

                if (newCounterNameHinzufügen.Text.Length > 15)
                {
                    MessageBox.Show("15 Zeichen ist die maximale Zeichenlänge!");
                    return;
                }
            }
        }

        if (CurrentCounterId == null)
        {
            context.Counters.Add(new Counter
            {
                Name = newCounterNameHinzufügen.Text
            });
            
            context.SaveChanges();
            MessageBox.Show("Ihr Zähler wurde erfolgreich hinzugefügt.");
            FillCounterList();
        }
        else
        {
            var entity = context.Counters.FirstOrDefault(x => x.ID == CurrentCounterId);
            entity.Name = newCounterNameHinzufügen.Text;
            context.SaveChanges();
            MessageBox.Show("Ihr Zähler wurde erfolgreich geupdated.");
            FillCounterList();
        }

        Close();
    }

    public void FillCounterList()
    {
        using SzContext context = new();
        context.Database.EnsureCreated();

        var counter = context.Counters.Select(x => x).ToList();
        foreach (var item in counter)
        {
            CounterList.Add(new Counter
            {
                ID = item.ID,
                Name = item.Name
            });
        }
    }

}
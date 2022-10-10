using System.Linq;
using System.Windows;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class BenutzerRegistrierung
{
    private readonly SzContext context = new();
    private readonly ComputeStringToSha256Hash encrypting = new();

    public BenutzerRegistrierung()
    {
        InitializeComponent();
        context.Database.EnsureCreated();

    }

    //Buttons for this Class
    private void BtnSpeichern(object sender, RoutedEventArgs e)
    {
        var encodedPassword = encrypting.ComputeStringToSha256HashMethod(txtUserPasswordConfirm.Password);

        var affectedDataSet = context.UserLogins.FirstOrDefault(x => x.Username == TxtUserNameCreate.Text);

        if (affectedDataSet != null)
        {
            MessageBox.Show("Dieser Benutzer existiert bereits. Bitte geben Sie einen anderen Benutzernamen ein.");
            TxtUserNameCreate.Clear();
            TxtUserPasswordCreate.Clear();
            txtUserPasswordConfirm.Clear();
            return;
        }

        if (TxtUserPasswordCreate.Password != txtUserPasswordConfirm.Password)
        {
            MessageBox.Show("Ihr Passwort stimmt nicht überein. Bitte wiederholen Sie ihre Eingabe.");
            txtUserPasswordConfirm.Clear();
            TxtUserPasswordCreate.Clear();
            return;
        }

        if (affectedDataSet == null)
        {
            context.UserLogins.Add(new UserLogin
            {
                Username = TxtUserNameCreate.Text,
                Password = encodedPassword
            });
            context.SaveChanges();
            MessageBox.Show("Ihr Benutzer wurde angelegt.");
            Close();
        }
    }

    private void BtnAbbrechen(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
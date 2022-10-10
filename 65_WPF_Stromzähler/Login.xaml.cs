using System.Collections.Generic;
using System.Linq;
using System.Windows;
using StromzählerContext;

namespace _65_WPF_Stromzähler;

public partial class Login
{
    private readonly ComputeStringToSha256Hash encrypting = new();
    private readonly MainWindow mainWindow = new();
    private readonly SzContext context = new();

    public List<UserLogin> UserList = new();

    public Login()
    {
        InitializeComponent();

        context.Database.EnsureCreated();

        //TEST USERLOGIN
        //context.UserLogins.Add(new UserLogin
        //{
        //    Username = "login",
        //    Password = encrypting.ComputeStringToSha256HashMethod("login")
        //});
        //context.SaveChanges();
    }

    //Buttons for this Class
    private void BtnAnmelden(object sender, RoutedEventArgs e)
    {
        context.Database.EnsureCreated();

        var encodedPassword = encrypting.ComputeStringToSha256HashMethod(txtUserPassword.Password);

        var userLogin = context.UserLogins.Where(x => x.Username == TxtUserName.Text && x.Password == encodedPassword)
            .ToList();

        if (userLogin.Count() != 0)
        {
            Close();
            mainWindow.Show();
            return;
        }

        TxtUserName.Clear();
        txtUserPassword.Clear();
        MessageBox.Show("Bitte geben Sie einen gültigen Benutzernamen oder ein gültiges Passwort ein.");
    }

    private void BtnAbbrechen(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
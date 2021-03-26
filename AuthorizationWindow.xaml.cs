using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CRM_System_For_Fitness_Club
{
    public partial class AuthorizationWindow : Window
    {
        private readonly Entities e = new Entities();
        private User authUser = new User();
        public AuthorizationWindow()
        {
            InitializeComponent();
        }
        private void ShowError() 
        {
            ErrorLabel.Visibility = Visibility.Visible;
            LoginTextBox.BorderBrush = PasswordTextBox.BorderBrush = Brushes.Red;
            LoginTextBox.BorderThickness = PasswordTextBox.BorderThickness = new Thickness(2, 2, 2, 2);
        }
        //Проверка введенных данных на соответствие (Linq)
        private User CheckInputData(string email, string password)
        {
            authUser = e.Users.Include("Employee")
                .Where(person => person.Employee.Email == email && person.Password == password)
                .Select(person => person)
                .FirstOrDefault();
            return authUser;
        }

        private void AuthorizationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInputData(LoginTextBox.Text, PasswordTextBox.Password) is null)
            {
                ShowError();
            }
            else
            {
                new MainWindow(authUser).ShowDialog();
                Close();
            }
        }
    }
}

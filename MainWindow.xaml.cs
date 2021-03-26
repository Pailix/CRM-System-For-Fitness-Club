using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static CRM_System_For_Fitness_Club.InputChecker;

namespace CRM_System_For_Fitness_Club
{
    public partial class MainWindow : Window
    {
        private readonly User authUser = new User();
        private readonly Entities e = new Entities();
        #region CollectionsDeclaration
        private ObservableCollection<ClientMembershipLinq> clientsCollection = new ObservableCollection<ClientMembershipLinq>();
        private ObservableCollection<EmployeeSalaryLinq> salariesCollection = new ObservableCollection<EmployeeSalaryLinq>();
        private ObservableCollection<MembershipPurchase> purchasesCollection = new ObservableCollection<MembershipPurchase>();
        private ObservableCollection<ExpenseTypeLinq> expenseCollection = new ObservableCollection<ExpenseTypeLinq>();
        private ObservableCollection<EmployeesLinq> employeesCollection = new ObservableCollection<EmployeesLinq>();
        private ObservableCollection<EmployeeSchedule> sheduleCollection = new ObservableCollection<EmployeeSchedule>();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(User value) 
        {
            InitializeComponent();
            authUser = value;
            Authorization();
            #region ItemSources
            ClientsDataGrid.ItemsSource = clientsCollection;
            CreateClientsTable();
            SalariesDataGrid.ItemsSource = salariesCollection;
            CreateSalaryTable();
            MembershipsDataGrid.ItemsSource = purchasesCollection;
            CreatePurchasesTable();
            ExpensesDataGrid.ItemsSource = expenseCollection;
            CreateExpenseTable();
            EmployeesDataGrid.ItemsSource = employeesCollection;
            CreateEmployeeTable();
            CoachingDataGrid.ItemsSource = sheduleCollection;
            CreateShedule();
            #endregion
        }
        private void Authorization()
        {
            MessageBox.Show("Добрый день, " + authUser.Employee.MiddleName + ". " +
                "Вы вошли в систему как " + authUser.AccessType.AccessType1);
            switch (authUser.Access_ID)
            {
                case 1:
                    AdministrationControlsEnable();
                    break;
                case 2:
                    ManagerControlsEnable();
                    break;
                case 3:
                    CoachControlsEnable();
                    break;
            }
        }
        private void AdministrationControlsEnable()
        {
            EmployeeIDTxtBox.Visibility = EmployeeBirthDatePicker.Visibility = EmployeeBirthLabel.Visibility =
            EmployeeClearButton.Visibility = EmployeeFirstNameTxtBox.Visibility = EmployeeMiddleNameTxtBox.Visibility =
            EmployeeLastNameTxtBox.Visibility = PositionComboBox.Visibility = EmployeeEmailTxtBox.Visibility =
            EmployeePhoneTxtBox.Visibility = EmployeeClearButton.Visibility = DeleteEmployeeBtn.Visibility =
            EditEmployeeBtn.Visibility = AddEmployeeBtn.Visibility = Visibility.Visible;
        }
        private void ManagerControlsEnable()
        {
            ClientPhoneTxtBox.Visibility = ClientBirthDateLabel.Visibility = ClientBirthDatePicker.Visibility =
            ClientChangeInfoBtn.Visibility = ClientFirstNameTxtBox.Visibility = ClientIDTxtBox.Visibility =
            ClientLastNameTxtBox.Visibility = ClientMiddleNameTxtBox.Visibility = ClientRegistrationBtn.Visibility =
            ClientTabClearBtn.Visibility = DeleteClientBtn.Visibility = MembershipIDComboBox.Visibility =
            MembershipTypeLabel.Visibility = MemberBuyButton.Visibility = ClientVisitBtn.Visibility =
            SalaryAmountTxtBox.Visibility = SalaryEmployeeIDTxtBox.Visibility = AddSalaryBtn.Visibility =
            ChangeSalaryBtn.Visibility = ChangeSalaryBtn.Visibility = DeleteSalaryBtn.Visibility = PurchaseIDTxtBox.Visibility =
            DeletePurchaseBtn.Visibility = ExpenseCostTxtBox.Visibility = ExpenseIDTxtBox.Visibility = ExpenseTypeComboBox.Visibility =
            AddExpenseBtn.Visibility = DeleteExpenseButton.Visibility = EditExpenseBtn.Visibility = Visibility.Visible;
        }
        private void CoachControlsEnable()
        {
            SelectedCoachIDTxtBox.Visibility = CoachIDTxtBox.Visibility = CoachingEndTimePick.Visibility =
            CoachingStartTimePick.Visibility = AddCoachBtn.Visibility = DeleteCoachBtn.Visibility = Visibility.Visible;
        }
        #region ClientPart
        #region ClientControls
        private void ClientsDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DataGridRow)sender;
            var selectedObject = selectedRow.Item as ClientMembershipLinq;
            ClientIDTxtBox.Text = selectedObject.Client_ID.ToString();
            ClientFirstNameTxtBox.Text = selectedObject.FirstName;
            ClientMiddleNameTxtBox.Text = selectedObject.MiddleName;
            ClientLastNameTxtBox.Text = selectedObject.LastName;
            ClientBirthDatePicker.SelectedDate = DateTime.Parse(selectedObject.BirthDate);
            ClientPhoneTxtBox.Text = selectedObject.PhoneNumber;
        }
        private void ClientTabClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientFirstNameTxtBox.Clear();
            ClientMiddleNameTxtBox.Clear();
            ClientLastNameTxtBox.Clear();
            ClientPhoneTxtBox.Clear();
            ClientIDTxtBox.Clear();
        }
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientSearch();
        }
        private void ClientChangeInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientInfoChange();
            CreateClientsTable();
        }
        private void ClientRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            AddNewClient();
            CreateClientsTable();
        }
        private void DeleteClientBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveClient();
            CreateClientsTable();
        }
        private void ClientVisitBtn_Click(object sender, RoutedEventArgs e)
        {
            DecreaseNumberOfVisits();
            CreateClientsTable();
        }
        private void MemberBuyBtn_Click(object sender, RoutedEventArgs e)
        {
            ClientBuyMembership();
            CreateClientsTable();
        }
        #endregion
        #region ClientQueries
        private void CreateClientsTable()
        {
            var clientsQuery = e.Clients.Join(e.ClientMemberships
                .Where(p => p.IsActive == true),
                client => client.Client_ID,
                membership => membership.Client_ID,
                (client, membership) => new ClientMembershipLinq
                {
                    Client_ID = client.Client_ID,
                    FirstName = client.FirstName,
                    MiddleName = client.MiddleName,
                    LastName = client.LastName,
                    BirthDate = client.BirthDate.ToString(),
                    PhoneNumber = client.PhoneNumber,
                    RemainingVisits = (byte)membership.RemainingVisits
                }).ToArray();
            FillClientsCollection(clientsQuery);
        }
        private void FillClientsCollection(ClientMembershipLinq[] dataToFill)
        {
            clientsCollection.Clear();
            foreach (var obj in dataToFill)
            {
                clientsCollection.Add(obj);
            }
        }
        private void ClientSearch()
        {
            var resultArray = e.Clients.Join(e.ClientMemberships
                .Where(p => p.IsActive == true && p.Client.FirstName.Contains(SearchTextBox.Text)),
                client => client.Client_ID,
                membership => membership.Client_ID,
                (client, membership) => new ClientMembershipLinq
                {
                    Client_ID = client.Client_ID,
                    FirstName = client.FirstName,
                    MiddleName = client.MiddleName,
                    LastName = client.LastName,
                    BirthDate = client.BirthDate.ToString(),
                    PhoneNumber = client.PhoneNumber,
                    RemainingVisits = (byte)membership.RemainingVisits
                }).ToArray();
            FillClientsCollection(resultArray);
        }
        private void ClientInfoChange()
        {
            if (IsTextStringCorrect(ClientFirstNameTxtBox.Text) && IsTextStringCorrect(ClientMiddleNameTxtBox.Text)
                && IsTextStringCorrect(ClientLastNameTxtBox.Text) && IsPhoneCorrect(ClientPhoneTxtBox)
                && IsAdult(ClientBirthDatePicker.SelectedDate) && ClientIDTxtBox.Text.Length > 0)
            {
                var clientToChange = e.Clients
                    .Where(client => client.Client_ID.ToString() == ClientIDTxtBox.Text)
                    .FirstOrDefault();
                clientToChange.FirstName = ClientFirstNameTxtBox.Text;
                clientToChange.MiddleName = ClientMiddleNameTxtBox.Text;
                clientToChange.LastName = ClientLastNameTxtBox.Text;
                clientToChange.PhoneNumber = ClientPhoneTxtBox.Text;
                clientToChange.BirthDate = ClientBirthDatePicker.SelectedDate;
                e.SaveChanges();
                MessageBox.Show("Данные клиента изменены.");
            }
            else
            {
                MessageBox.Show("Данные на изменение введены некорректно!");
            }
        }
        private void AddNewClient()
        {
            if (IsTextStringCorrect(ClientFirstNameTxtBox.Text) && IsTextStringCorrect(ClientMiddleNameTxtBox.Text)
                && IsTextStringCorrect(ClientLastNameTxtBox.Text) && IsPhoneCorrect(ClientPhoneTxtBox)
                && IsAdult(ClientBirthDatePicker.SelectedDate))
            {
                var client = new Client
                {
                    Client_ID = Guid.NewGuid(),
                    FirstName = ClientFirstNameTxtBox.Text,
                    MiddleName = ClientMiddleNameTxtBox.Text,
                    LastName = ClientLastNameTxtBox.Text,
                    PhoneNumber = ClientPhoneTxtBox.Text,
                    BirthDate = ClientBirthDatePicker.SelectedDate
                };
                ClientMembership clientMembership = new ClientMembership
                {
                    Client_ID = client.Client_ID,
                    ClientMembership_ID = Guid.NewGuid(),
                    Membership_ID = MembershipIDComboBox.SelectedIndex + 1
                };
                clientMembership.RemainingVisits = e.MembershipClassifications
                    .Where(type => type.Membership_ID == clientMembership.Membership_ID)
                    .Select(p => p.NumberOfVisits).First();
                clientMembership.PurchaseDate = DateTime.Now.Date;
                clientMembership.IsActive = true;
                e.Clients.Add(client);
                e.ClientMemberships.Add(clientMembership);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные для регистрации клиента введены неверно!");
            }
        }
        private void RemoveClient()
        {
            if (ClientIDTxtBox.Text.Length > 0)
            {
                Client delClient = e.Clients
                    .FirstOrDefault(p => p.Client_ID.ToString() == ClientIDTxtBox.Text);
                e.Clients.Remove(delClient);
                e.SaveChanges();
                MessageBox.Show("Успешно удалено!");
            }
            else
            {
                MessageBox.Show("Клиент не выбран!");
            }
        }
        private void DecreaseNumberOfVisits()
        {
            if (ClientIDTxtBox.Text.Length > 0)
            {
                ClientMembership clientDecrease = e.ClientMemberships
                    .FirstOrDefault(p => p.Client_ID.ToString() == ClientIDTxtBox.Text && p.IsActive == true);
                clientDecrease.RemainingVisits--;
                e.SaveChanges();
                MessageBox.Show("Посещение отмечено!");
            }
            else
            {
                MessageBox.Show("Клиент не выбран!");
            }
        }
        private void ClientBuyMembership()
        {
            if (ClientIDTxtBox.Text.Length > 0)
            {
                ClientMembership clientBought = e.ClientMemberships
                    .FirstOrDefault(p => p.Client_ID.ToString() == ClientIDTxtBox.Text && p.IsActive == true);
                if (clientBought != default)
                {
                    clientBought.IsActive = false;
                    e.SaveChanges();
                    clientBought = new ClientMembership();
                    clientBought.Client_ID = Guid.Parse(ClientIDTxtBox.Text);
                    clientBought.ClientMembership_ID = Guid.NewGuid();
                    clientBought.Membership_ID = MembershipIDComboBox.SelectedIndex + 1;
                    clientBought.RemainingVisits = e.MembershipClassifications
                        .Where(type => type.Membership_ID == clientBought.Membership_ID)
                        .Select(p => p.NumberOfVisits).First();
                    clientBought.PurchaseDate = DateTime.Now.Date;
                    clientBought.IsActive = true;
                    e.ClientMemberships.Add(clientBought);
                    e.SaveChanges();
                    CreateClientsTable();
                    MessageBox.Show("Покупка оформлена!");
                }
            }
            else
            {
                MessageBox.Show("Ошибка при выборе!");
            }
        }
        #endregion
        #endregion
        #region IncomePart
        #region Salaries
        #region SalariesControls
        private void SalaryDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DataGridRow)sender; ;
            var selectedObject = selectedRow.Item as EmployeeSalaryLinq;
            SalaryAmountTxtBox.Text = selectedObject.SalaryAmount.ToString();
            SalaryEmployeeIDTxtBox.Text = selectedObject.EmployeeID.ToString();
            SalaryIDTxtBox.Text = selectedObject.SalaryID.ToString();
        }
        private void AddSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            AddSalary();
            CreateSalaryTable();
        }
        private void ChangeSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            SalaryChange();
            CreateSalaryTable();
        }
        private void DeleteSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            SalaryDelete();
            CreateSalaryTable();
        }

        #endregion
        #region SalariesQueries
        private void CreateSalaryTable()
        {
            var salariesQuery = e.EmployeesSalaries.Join(e.Employees,
                salary => salary.Employee_ID,
                employee => employee.Employee_ID,
                (salary, employee) => new EmployeeSalaryLinq
                {
                    SalaryID = salary.Salary_ID,
                    EmployeeID = employee.Employee_ID,
                    FullName = employee.FirstName + " " + employee.MiddleName + " " + employee.LastName,
                    SalaryDate = salary.SalaryDate.ToString(),
                    SalaryAmount = (decimal)salary.SalaryAmount
                }).ToArray();
            FillSalaryTable(salariesQuery);
        }
        private void FillSalaryTable(EmployeeSalaryLinq[] salariesArray)
        {
            salariesCollection.Clear();
            foreach (var obj in salariesArray)
            {
                salariesCollection.Add(obj);
            }
        }
        private void AddSalary()
        {
            if (SalaryEmployeeIDTxtBox.Text.Length > 0 && IsDecimalCorrect(SalaryAmountTxtBox.Text))
            {
                EmployeesSalary newSalary = new EmployeesSalary
                {
                    Employee_ID = Guid.Parse(SalaryEmployeeIDTxtBox.Text),
                    Salary_ID = Guid.NewGuid(),
                    SalaryDate = DateTime.Now.Date,
                    SalaryAmount = decimal.Parse(SalaryAmountTxtBox.Text)
                };
                e.EmployeesSalaries.Add(newSalary);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Зарплатные данные введены некорректно!");
            }
        }
        private void SalaryChange()
        {
            if (SalaryIDTxtBox.Text.Length > 0 && IsDecimalCorrect(SalaryAmountTxtBox.Text))
            {
                EmployeesSalary changingSalary = new EmployeesSalary();
                changingSalary = e.EmployeesSalaries
                    .Where(salary => salary.Salary_ID.ToString() == SalaryIDTxtBox.Text)
                    .FirstOrDefault();
                changingSalary.SalaryAmount = decimal.Parse(SalaryAmountTxtBox.Text);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные изменены неверно!");
            }
        }
        private void SalaryDelete()
        {
            if (SalaryIDTxtBox.Text.Length > 0)
            {
                EmployeesSalary salaryToDel = e.EmployeesSalaries
                    .Where(salary => salary.Salary_ID.ToString() == SalaryIDTxtBox.Text)
                    .FirstOrDefault();
                e.EmployeesSalaries.Remove(salaryToDel);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Удалить невозможно!");
            }
        }
        #endregion
        #endregion
        #region Sells
        #region SellsControls
        private void PurchaseDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DataGridRow)sender; ;
            var selectedObject = selectedRow.Item as MembershipPurchase;
            PurchaseIDTxtBox.Text = selectedObject.MembershipPurchaseID.ToString();
        }
        private void DeletePurchaseBtn_Click(object sender, RoutedEventArgs e)
        {
            DeletePurchase();
            CreatePurchasesTable();
        }

        #endregion
        #region SellsQueries
        private void CreatePurchasesTable()
        {
            var purchasesQuery = e.ClientMemberships
                .Join(e.MembershipClassifications,
                purchase => purchase.Membership_ID,
                cost => cost.Membership_ID,
                (purchase, cost) => new MembershipPurchase
                {
                    MembershipPurchaseID = purchase.ClientMembership_ID,
                    PurchaseDate = purchase.PurchaseDate.ToString(),
                    PurchaseAmount = (decimal)cost.MembershipCost,
                }).ToArray();
            FillPurchasesTable(purchasesQuery);
        }
        private void FillPurchasesTable(MembershipPurchase[] purchases)
        {
            purchasesCollection.Clear();
            foreach (var obj in purchases)
            {
                purchasesCollection.Add(obj);
            }
        }
        private void DeletePurchase()
        {
            if (PurchaseIDTxtBox.Text.Length > 0)
            {
                var purchaseToDelete = e.ClientMemberships
                    .Where(purchase => purchase.ClientMembership_ID.ToString() == PurchaseIDTxtBox.Text)
                    .FirstOrDefault();
                e.ClientMemberships.Remove(purchaseToDelete);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Никакая из покупок не выбрана!");
            }
        }
        #endregion
        #endregion
        #region Expenses
        #region ExpensesControls
        private void ExpenseDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DataGridRow)sender; ;
            var selectedObject = selectedRow.Item as ExpenseTypeLinq;
            ExpenseIDTxtBox.Text = selectedObject.ExpenseID.ToString();
            ExpenseCostTxtBox.Text = selectedObject.ExpenseCost.ToString();
            ExpenseTypeComboBox.SelectedIndex = selectedObject.TypeID - 1;
        }
        private void AddExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            AddExpense();
            CreateExpenseTable();
        }
        private void EditExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            EditExpense();
            CreateExpenseTable();
        }
        private void DeleteExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteExpense();
            CreateExpenseTable();
        }
        #endregion
        #region ExpensesQueries
        private void CreateExpenseTable()
        {
            expenseCollection.Clear();
            var expenseQuery = e.Expenses
                .Join(e.ExpensesTypes,
                expense => expense.ExpenseType_ID,
                type => type.ExpenseType_ID,
                (expense, type) => new ExpenseTypeLinq
                {
                    ExpenseID = expense.Expense_ID,
                    ExpenseDate = expense.Date.ToString(),
                    ExpenseCost = (decimal)expense.ExpenseCost,
                    ExpenseType = type.ExpenseTitle,
                    TypeID = type.ExpenseType_ID,
                }).ToArray();
            ExpenceTableFill(expenseQuery);
        }
        private void ExpenceTableFill(ExpenseTypeLinq[] queryArray)
        {
            foreach (var obj in queryArray)
            {
                expenseCollection.Add(obj);
            }
        }
        private void AddExpense()
        {
            if (IsDecimalCorrect(ExpenseCostTxtBox.Text))
            {
                var newExpense = new Expens();
                newExpense.Expense_ID = Guid.NewGuid();
                newExpense.ExpenseType_ID = ExpenseTypeComboBox.SelectedIndex + 1;
                newExpense.ExpenseCost = decimal.Parse(ExpenseCostTxtBox.Text);
                newExpense.Date = DateTime.Now.Date;
                e.Expenses.Add(newExpense);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные введены неверно!");
            }
        }
        private void EditExpense()
        {
            if (IsDecimalCorrect(ExpenseCostTxtBox.Text) && ExpenseIDTxtBox.Text.Length > 0)
            {
                var changingExpense = e.Expenses
                    .Where(expense => expense.Expense_ID.ToString() == ExpenseIDTxtBox.Text)
                    .FirstOrDefault();
                changingExpense.ExpenseCost = decimal.Parse(ExpenseCostTxtBox.Text);
                changingExpense.ExpenseType_ID = ExpenseTypeComboBox.SelectedIndex + 1;
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные изменены некорректно");
            }
        }
        private void DeleteExpense()
        {
            if (ExpenseIDTxtBox.Text.Length > 0)
            {
                var expenseToDelete = e.Expenses
                    .Where(expense => expense.Expense_ID.ToString() == ExpenseIDTxtBox.Text)
                    .FirstOrDefault();
                e.Expenses.Remove(expenseToDelete);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("ID для удаления не выбран!");
            }
        }
        #endregion
        #endregion
        #region ReportPart
        private void GenerateIncome()
        {
            switch (PeriodComboBox.SelectedIndex)
            {
                case 0:
                    CountIncome(DateTime.Today.AddDays(-7));
                    break;
                case 1:
                    CountIncome(DateTime.Today.AddMonths(-1));
                    break;
                case 2:
                    CountIncome(DateTime.Today.AddYears(-1));
                    break;
                case 3:
                    CountIncome(DateTime.MinValue);
                    break;
                default:
                    MessageBox.Show("Неверный период");
                    break;
            }
        }
        private void CountIncome(DateTime selectedPeriod)
        {
            var salaries = e.EmployeesSalaries
                .Where(salary => salary.SalaryDate > selectedPeriod)
                .Sum(salary => salary.SalaryAmount);
            var sales = purchasesCollection.
                Where(sale => DateTime.Parse(sale.PurchaseDate) > selectedPeriod)
                .Sum(sale => sale.PurchaseAmount);
            var expenses = e.Expenses
                .Where(expense => expense.Date > selectedPeriod)
                .Sum(expense => expense.ExpenseCost);
            IncomeTextBlock.Text = (sales - expenses - salaries).ToString();
        }
        private IQueryable<ReportElement> CreateReport()
        {
            var firstQuerry = e.ClientMemberships
                .Join(e.MembershipClassifications,
                cl => cl.Membership_ID,
                mem => mem.Membership_ID,
                (cl, mem) => new ReportElement
                {
                    Date = cl.PurchaseDate.ToString(),
                    Amount = mem.MembershipCost.ToString(),
                    Title = "продажи"
                });
            var secondQuerry = from salaries in e.EmployeesSalaries
                               select new ReportElement
                               {
                                   Date = salaries.SalaryDate.ToString(),
                                   Amount = salaries.SalaryAmount.ToString(),
                                   Title = "зарплата"
                               };
            var thirdQuerry = e.Expenses
                .Join(e.ExpensesTypes,
                expense => expense.ExpenseType_ID,
                type => type.ExpenseType_ID,
                (expense, type) => new ReportElement
                {
                    Date = expense.Date.ToString(),
                    Amount = expense.ExpenseCost.ToString(),
                    Title = type.ExpenseTitle,
                });
            return firstQuerry.Union(secondQuerry.Union(thirdQuerry));
        }
        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            ExcelExport.ExportIntoExcel(CreateReport());
        }
        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenerateIncome();
        }
        #endregion
        #endregion
        #region EmployeePart
        #region EmployeeControls
        private void EmployeesDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DataGridRow)sender; ;
            var selectedObject = selectedRow.Item as EmployeesLinq;
            EmployeePhoneTxtBox.Text = selectedObject.EmployeePhone;
            EmployeeEmailTxtBox.Text = selectedObject.EmployeeEmail;
            EmployeeFirstNameTxtBox.Text = selectedObject.EmployeeFirstName;
            EmployeeMiddleNameTxtBox.Text = selectedObject.EmployeeMiddleName;
            EmployeeLastNameTxtBox.Text = selectedObject.EmployeeLastName;
            EmployeeIDTxtBox.Text = selectedObject.EmployeeID.ToString();
            EmployeeBirthDatePicker.SelectedDate = DateTime.Parse(selectedObject.EmployeeBirthDate);
            PositionComboBox.SelectedIndex = selectedObject.PositionID - 1;
        }
        private void EmployeeClearButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeePhoneTxtBox.Clear();
            EmployeeEmailTxtBox.Clear();
            EmployeeFirstNameTxtBox.Clear();
            EmployeeMiddleNameTxtBox.Clear();
            EmployeeLastNameTxtBox.Clear();
            EmployeeIDTxtBox.Clear();
        }
        private void AddEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee();
            CreateEmployeeTable();
        }
        private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            EditEmployee();
            CreateEmployeeTable();
        }
        private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteEmployee();
            CreateEmployeeTable();
        }
        #endregion
        #region EmployeeQueries
        private void CreateEmployeeTable()
        {
            employeesCollection.Clear();
            var employeesQuery = e.Employees
                .Join(e.Positions,
                employee => employee.Position_ID,
                position => position.Position_ID,
                (employee, position) => new EmployeesLinq
                {
                    EmployeeID = employee.Employee_ID,
                    EmployeeFirstName = employee.FirstName,
                    EmployeeMiddleName = employee.MiddleName,
                    EmployeeLastName = employee.LastName,
                    EmployeeBirthDate = employee.BirthDate.ToString(),
                    EmployeeEmail = employee.Email,
                    EmployeePhone = employee.PhoneNumber,
                    PositionID = position.Position_ID,
                    Position = position.PositionTItle,
                }).ToArray();
            FillEmployeeTable(employeesQuery);
        }
        private void FillEmployeeTable(EmployeesLinq[] queryArray)
        {
            foreach (var obj in queryArray)
            {
                employeesCollection.Add(obj);
            }
        }
        private void AddEmployee()
        {
            if (IsTextStringCorrect(EmployeeFirstNameTxtBox.Text) && IsTextStringCorrect(EmployeeMiddleNameTxtBox.Text)
                && IsTextStringCorrect(EmployeeLastNameTxtBox.Text) && EmployeePhoneTxtBox.IsMaskFull
                && EmployeeEmailTxtBox.Text.Length > 0 && IsAdult(EmployeeBirthDatePicker.SelectedDate))
            {
                var newEmployee = new Employee
                {
                    Employee_ID = Guid.NewGuid(),
                    FirstName = EmployeeFirstNameTxtBox.Text,
                    MiddleName = EmployeeMiddleNameTxtBox.Text,
                    LastName = EmployeeLastNameTxtBox.Text,
                    PhoneNumber = EmployeePhoneTxtBox.Text,
                    Position_ID = PositionComboBox.SelectedIndex + 1,
                    Email = EmployeeEmailTxtBox.Text,
                    BirthDate = EmployeeBirthDatePicker.SelectedDate
                };
                e.Employees.Add(newEmployee);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные о сотруднике введены неверно!");
            }
        }
        private void EditEmployee()
        {
            if (EmployeeIDTxtBox.Text.Length > 0 && IsTextStringCorrect(EmployeeFirstNameTxtBox.Text)
                && IsTextStringCorrect(EmployeeMiddleNameTxtBox.Text)
                && IsTextStringCorrect(EmployeeLastNameTxtBox.Text) && EmployeePhoneTxtBox.IsMaskFull
                && EmployeeEmailTxtBox.Text.Length > 0 && IsAdult(EmployeeBirthDatePicker.SelectedDate))
            {
                var changingEmployee = e.Employees
                    .Where(emp => emp.Employee_ID.ToString() == EmployeeIDTxtBox.Text)
                    .FirstOrDefault();
                changingEmployee.FirstName = EmployeeFirstNameTxtBox.Text;
                changingEmployee.MiddleName = EmployeeMiddleNameTxtBox.Text;
                changingEmployee.LastName = EmployeeLastNameTxtBox.Text;
                changingEmployee.PhoneNumber = EmployeePhoneTxtBox.Text;
                changingEmployee.Position_ID = PositionComboBox.SelectedIndex + 1;
                changingEmployee.Email = EmployeeEmailTxtBox.Text;
                changingEmployee.BirthDate = EmployeeBirthDatePicker.SelectedDate;
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Данные изменены неверно!");
            }
        }
        private void DeleteEmployee()
        {
            if (EmployeeIDTxtBox.Text.Length > 0)
            {
                var empToDelete = e.Employees
                    .Where(employee => employee.Employee_ID.ToString() == EmployeeIDTxtBox.Text)
                    .FirstOrDefault();
                e.Employees.Remove(empToDelete);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Сотрудник не выбран!");
            }
        }
        #endregion
        #endregion
        #region SchedulePart
        #region ScheduleControls
        private void CoachingDataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = (DataGridRow)sender; ;
            var selectedObject = selectedRow.Item as EmployeeSchedule;
            SelectedCoachIDTxtBox.Text = selectedObject.CoachingID.ToString();
            CoachIDTxtBox.Text = selectedObject.CoachID.ToString();
        }
        private void AddCoachBtn_Click(object sender, RoutedEventArgs e)
        {
            AddCoach();
            CreateShedule();
        }
        private void DeleteCoachBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteCoach();
            CreateShedule();
        }
        #endregion
        #region ScheduleQueries
        private void CreateShedule()
        {
            sheduleCollection.Clear();
            var shedule = e.CoachingSchedules
                .Where(date => DbFunctions.TruncateTime(date.CoachingDate) >= DbFunctions.TruncateTime(DateTime.Now))
                .Join(e.Employees,
                train => train.Employee_ID,
                coach => coach.Employee_ID,
                (train, coach) => new EmployeeSchedule
                {
                    CoachingID = train.Shedule_ID,
                    CoachID = coach.Employee_ID,
                    CoachFullName = coach.FirstName + " " + coach.MiddleName + " " + coach.LastName,
                    CoachingDate = train.CoachingDate.ToString(),
                    CoachingStart = train.CoachingStart.ToString(),
                    CoachingEnd = train.CoachingEnd.ToString()
                }).ToArray();
            FillShedule(shedule);
        }
        private void FillShedule(EmployeeSchedule[] queryArray)
        {
            foreach (var obj in queryArray)
            {
                sheduleCollection.Add(obj);
            }
        }
        private void AddCoach()
        {
            if (IsTimePickedRight(CoachingEndTimePick.Value.Value) && IsTimePickedRight(CoachingStartTimePick.Value.Value)
                && IsDateInFuture(CoachingDate.SelectedDate.Value))
            {
                var training = new CoachingSchedule
                {
                    Shedule_ID = Guid.NewGuid(),
                    Employee_ID = (Guid)authUser.Employee_ID,
                    CoachingDate = CoachingDate.SelectedDate,
                    CoachingStart = CoachingStartTimePick.TimeInterval,
                    CoachingEnd = CoachingEndTimePick.TimeInterval
                };
                e.CoachingSchedules.Add(training);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении тренировки!");
            }
        }
        private void DeleteCoach()
        {
            if (CoachIDTxtBox.Text.Length > 0 && SelectedCoachIDTxtBox.Text.Length > 0
                && CoachIDTxtBox.Text == authUser.Employee_ID.ToString())
            {
                var j = e.CoachingSchedules
                    .Where(train => train.Shedule_ID.ToString() == SelectedCoachIDTxtBox.Text)
                    .FirstOrDefault();
                e.CoachingSchedules.Remove(j);
                e.SaveChanges();
            }
            else
            {
                MessageBox.Show("Вы можете удалить только свою тренировку!");
            }
        }
        #endregion

        #endregion

        private void ExixButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new AuthorizationWindow().ShowDialog();
        }
    }
}

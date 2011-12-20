using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using FileSyncGui.Local;
using FileSyncGui.Ref;

namespace FileSyncGui {
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window, INotifyPropertyChanged {

		private MainWindow parentWindow;

		public event PropertyChangedEventHandler PropertyChanged;

		public FileSyncConnection Ref;

        private Boolean creatingAccount = false;
		public Boolean CreatingAccount {
			get { return creatingAccount; }
			set {
				if (creatingAccount == value)
					return;
				creatingAccount = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("CreatingAccount"));
			}
		}

		private Boolean resettingPassword = false;
		public Boolean ResettingPassword {
			get { return resettingPassword; }
			set {
				if (resettingPassword == value)
					return;
				resettingPassword = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ResettingPassword"));
			}
		}

		private Boolean rememberPassword = false;
		public Boolean RememberPassword {
			get { return rememberPassword; }
			set {
				if (rememberPassword == value)
					return;
				rememberPassword = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("RememberPassword"));
			}
		}

		private Boolean enteredLoginAndPass = false;
		public Boolean EnteredLoginAndPass {
			get { return enteredLoginAndPass; }
			set {
				if (enteredLoginAndPass == value)
					return;
				enteredLoginAndPass = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("EnteredLoginAndPass"));

				checkIfAllEntered();
			}
		}

		private Boolean enteredMachineName = false;
		public Boolean EnteredMachineName {
			get { return enteredMachineName; }
			set {
				if (enteredMachineName == value)
					return;
				enteredMachineName = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("EnteredMachineName"));

				checkIfAllEntered();
			}
		}

		private Boolean enteredAllRequired = false;
		public Boolean EnteredAllRequired {
			get { return enteredAllRequired; }
			set {
				if (enteredAllRequired == value)
					return;
				enteredAllRequired = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("EnteredAllRequired"));
			}
		}

		public LoginWindow() {
			this.DataContext = this;

			CreatingAccount = false;
			ResettingPassword = false;
			RememberPassword = false; // I should load it from settings
			EnteredLoginAndPass = false;
			EnteredMachineName = false;
			checkIfAllEntered();

			this.Ref = new FileSyncConnection();

			InitializeComponent();
		}

		public LoginWindow(MainWindow mainWindow)
			: this() {
			parentWindow = mainWindow;
		}

		private string getLogin() {
			return this.UserLogin.Text;
		}

		private void checkIfAllEntered() {
			EnteredAllRequired = (EnteredLoginAndPass && EnteredMachineName);
		}

		private Credentials getCredentials() {
			var cr = new Credentials();
			cr.Login = this.getLogin();
			cr.Password = Security.ComputeHash(this.UserPassword.Password);
			return cr;
		}

		private UserContents getUser() {
			UserContents u = new UserContents();
			u.Login = this.getLogin();
			u.Password = Security.ComputeHash(this.UserPassword.Password);
			u.Name = this.UserFullName.Text;
			u.Email = this.UserEmail.Text;
			return u;
		}

		private MachineContents getMachine() {
			MachineContents m = new MachineContents();
			m.Name = this.MachineName.Text;
			m.Description = this.MachineDescription.Text;
			return m;
		}

		private void buttonLogin_Click(object sender, RoutedEventArgs e) {
			Credentials c = getCredentials();

			try {
				Ref.Login(c);

				parentWindow.credentials = c;
				//MessageBox.Show("User logged in.");
				this.DialogResult = true;
				this.Close();
			} catch (ActionException ex) {
				new SystemMessage(ex).ShowDialog();
			}
		}

		private void buttonCreate_Click(object sender, RoutedEventArgs e) {
			CreatingAccount = true;
		}

		private void buttonCreateSubmit_Click(object sender, RoutedEventArgs e) {
			Credentials c = this.getCredentials();
			MachineContents m = this.getMachine();

			try {
				Ref.AddUser(c, this.getUser());

				this.DialogResult = true;
				//MessageBox.Show("User was created!");

				Ref.Login(c);
				Ref.AddMachine(c, m);

				parentWindow.credentials = c;
				Ref.GetDirList(c, m);
				//parentWindow.machine = new MachineContents(c, id, false, false, true);
				Ref.GetLocalDirList(m);
				//MachineActions.GetContets(c, id);

				//MessageBox.Show("Machine was created!");
				this.Close();
			} catch (ActionException ex) {
				new SystemMessage(ex).ShowDialog();
			}
		}

		private void buttonCreateCancel_Click(object sender, RoutedEventArgs e) {
			CreatingAccount = false;
		}

		private void MachineName_TextChanged(object sender, TextChangedEventArgs e) {
			EnteredMachineName = MachineName.Text.Length > 0;
		}

		private void UserLogin_TextChanged(object sender, TextChangedEventArgs e) {
			EnteredLoginAndPass = UserLogin.Text.Length > 0 && UserPassword.Password.Length > 0;
		}

		private void UserPassword_PasswordChanged(object sender, RoutedEventArgs e) {
			EnteredLoginAndPass = UserLogin.Text.Length > 0 && UserPassword.Password.Length > 0;
		}

		private void buttonLoginHelp_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show(this, "Q: Why login button is disabled?"
				+ "\nA: Because no login or password was entered."
				+ "\n\nQ: What this program does?"
				+ "\nA: FileSync is a file backup/synchronization/exchange application. "
				+ "\n\nQ: I forgot my password, what can I do?"
				+ "\nA: Please contact the administrator of the server."
				+ "\n\nQ: I enter the correct login and password, but I can't log in. What can I do?"
				+ "\nA: To solve the problem, double-check your Internet connection, "
				+ "and then contact FileSync server administrator."
				+ "\n\nQ: Why this dialog window looks so awful?"
				+ "\nA: Please contact programmers from Microsoft, who copied Win2k dialog graphics into WPF.",
				"FileSync: Logging in FAQ");
		}

		private void buttonCreateHelp_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show(this, "Q: Why create button is disabled?"
				+ "\nA: Because some of the required fields are empty."
				+ "\n\nQ: What is the 'machine name'?"
				+ "\nA: Whatever you call your PC, laptop or netbook that you currently use. "
				+ "It can be anything (for example 'my pc') as long as it is meaningful to you."
				+ "\n\nQ: Why should I enter my e-mail?"
				+ "\nA: If you lose your password, you will be able to recover because password reset is done via e-mail."
				+ "\n\nQ: Why should I enter current machine description?"
				+ "\nA: If you want to sync files between many computers as one user, it may be useful to enter "
				+ "some additional info about for example: what type of content it contains in general, "
				+ "how often it needs to be synced etc.",
				"FileSync: Creating account FAQ");
		}

	}
}

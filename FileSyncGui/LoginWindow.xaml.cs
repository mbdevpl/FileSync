using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using FileSyncGui.GuiAbstracts;
using FileSyncGui.GuiObjects;
using FileSyncGui.GuiActions;
//using FileSyncGui.FileSyncServiceReference;

namespace FileSyncGui {
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window, INotifyPropertyChanged {

		private MainWindow parentWindow;

		public event PropertyChangedEventHandler PropertyChanged;

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
			return new Credentials(getLogin(), SafetyActions.ComputeHash(this.UserPassword.Password));
		}

		private UserIdentity getUserIdentity() {
			return new UserIdentity(this.getCredentials(), this.UserFullName.Text, this.UserEmail.Text);
		}

		private MachineIdentity getMachineIdentity() {
			return new MachineIdentity(this.MachineName.Text, this.MachineDescription.Text);
		}

		private void buttonLogin_Click(object sender, RoutedEventArgs e) {
			Credentials c = getCredentials();

			try {
				if (c.LogIn().WasSuccessful) {
					parentWindow.credentials = c;

					//MessageBox.Show("User logged in.");

					this.DialogResult = true;
					this.Close();
				}
			} catch (ActionException ex) {
				new SystemMessage(ex).ShowDialog();
				//MessageBox.Show(ex.Message, ex.Title, MessageBoxButton.OK, 
				//	MessageBoxImage.Exclamation);
			}
		}

		private void buttonCreate_Click(object sender, RoutedEventArgs e) {
			CreatingAccount = true;
			
			//using (ServiceReference1.Service1Client cl = new ServiceReference1.Service1Client()) {
			//    var data = cl.GetData(124);
			//    MessageBox.Show(String.Format("Data was:{0}", data));

			//    MessageBox.Show(cl.add(153, 37).ToString());
			//}

			//new FileSyncServiceReference.Service1Client().A
		}

		private void buttonCreateSubmit_Click(object sender, RoutedEventArgs e) {
			Credentials c = this.getCredentials();
			MachineIdentity id = this.getMachineIdentity();

			try {
				if (this.getUserIdentity().AddToDatabase().WasSuccessful) {
					//MessageBox.Show("User was created!");
					this.DialogResult = true;

					if (c.LogIn().WasSuccessful && id.AddToDatabase(c).WasSuccessful) {
						parentWindow.credentials = c;
						parentWindow.machine = new MachineContents(c, id, false, false, true);
						//MachineActions.GetContets(c, id);

						//MessageBox.Show("Machine was created!");

						this.Close();
					}
				}
			} catch (ActionException ex) {
				MessageBox.Show(ex.Message, ex.Title);

				//if (parentWindow.credentials != null) {
				//    this.DialogResult = true;
				//    this.Close();
				//}
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

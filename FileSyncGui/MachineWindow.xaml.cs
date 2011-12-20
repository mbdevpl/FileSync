using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using FileSyncGui.Local;
using FileSyncGui.Ref;

namespace FileSyncGui {
	/// <summary>
	/// Interaction logic for MachineWindow.xaml
	/// </summary>
	public partial class MachineWindow : Window, INotifyPropertyChanged {

		private MainWindow parentWindow;

		private Credentials credentials;

		private bool creatingMachine = false;
		public bool CreatingMachine {
			get { return creatingMachine; }
			set {
				if (creatingMachine == value)
					return;
				creatingMachine = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("CreatingMachine"));
			}
		}

		private bool enteredRequiredData = false;
		public bool EnteredRequiredData {
			get { return enteredRequiredData; }
			set {
				if (enteredRequiredData == value)
					return;
				enteredRequiredData = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("EnteredRequiredData"));
			}
		}

		private bool machineIsSelected = false;
		public bool MachineIsSelected {
			get { return machineIsSelected; }
			set {
				if (machineIsSelected == value)
					return;
				machineIsSelected = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("MachineIsSelected"));
			}
		}

		private int selectedMachineIndex = -1;
		public int SelectedMachineIndex {
			get { return selectedMachineIndex; }
			set {
				if (selectedMachineIndex == value)
					return;
				selectedMachineIndex = value;
				MachineIsSelected = value != -1;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("SelectedMachineIndex"));
			}
		}

		private ObservableCollection<MachineIdentity> machines;
		public ObservableCollection<MachineIdentity> Machines {
			get { return machines; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public MachineWindow() {
			MachineIsSelected = false;
			SelectedMachineIndex = -1;
			machines = new ObservableCollection<MachineIdentity>();

			InitializeComponent();
		}

		public MachineWindow(MainWindow mw)
			: this() {
			this.parentWindow = mw;
			this.credentials = parentWindow.credentials;

			RefreshMachinesList();
		}

		private void RefreshMachinesList() {
			try {
				List<MachineIdentity> mList = null;//UserActions.GetContents(this.credentials).Machines;
				machines.Clear();
				foreach (MachineIdentity m in mList) machines.Add(m);

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Machines"));
			} catch (ActionException ex) {
				MessageBox.Show(ex.Message, ex.Title);
			}
		}

		private void viewDirs_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			SelectedMachineIndex = viewDirs.SelectedIndex;
		}

		private void NewMachineData_Changed(object sender, TextChangedEventArgs e) {
			EnteredRequiredData = (NewMachineName.Text.Length > 0);
		}

		private void buttonCreate_Click(object sender, RoutedEventArgs e) {
			CreatingMachine = true;
		}

		private void buttonSelect_Click(object sender, RoutedEventArgs e) {
			try {
				parentWindow.machine = new MachineContents();
				//credentials,
				//	machines[SelectedMachineIndex], true, false, true);
				//MachineActions.GetContets(credentials, machines[SelectedMachineIndex]);
				this.DialogResult = true;
				this.Close();
			} catch (ActionException ex) {
				MessageBox.Show(ex.Message, ex.Title);
			}
		}

		private void buttonCancel_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
			this.Close();
		}

		private void buttonCreateSubmit_Click(object sender, RoutedEventArgs e) {
			try {
				new MachineIdentity();
				//this.NewMachineName.Text, this.NewMachineDesc.Text)
				//	.AddToDatabase(credentials);

				//MachineActions.Create(credentials,
				//        new MachineIdentity(this.NewMachineName.Text, this.NewMachineDesc.Text));
			} catch (ActionException ex) {
				MessageBox.Show(ex.Message, ex.Title);
			}

			RefreshMachinesList();
		}

		private void buttonCreateCancel_Click(object sender, RoutedEventArgs e) {
			CreatingMachine = false;
		}

		//private void buttonRefresh_Click(object sender, RoutedEventArgs e) {
		//    if (PropertyChanged != null) {
		//        MessageBox.Show("machines.Count=" + machines.Count.ToString());
		//        PropertyChanged(this, new PropertyChangedEventArgs("Machines"));
		//        MessageBox.Show("Machines.Count=" + Machines.Count.ToString());
		//        MessageBox.Show("viewDirs.Items.Count=" + viewDirs.Items.Count.ToString());
		//    }
		//}

		private void buttonHelp_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show(this, "Q: Why select button is disabled?"
				+ "\nA: Because none of the machines was selected."
				+ "\n\nQ: The list is empty, so how can I select a machine?"
				+ "\nA: Please create a machine first, then select it."
				+ "\n\nQ: The machine I've created earlier is lost, what can I do?"
				+ "\nA: Unfortunately, there is nothing constructive you can do.",
				"FileSync: Selecting machine FAQ");
		}

		private void buttonCreateHelp_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show(this, "Q: Why create button is disabled?"
				+ "\nA: Because machine name was not entered."
				+ "\n\nQ: I create the machine but it is not showing up on the list, what can I do?"
				+ "\nA: Please log out and log in again, the machine should be there.",
				"FileSync: Creating machine FAQ");
		}

	}
}

using System.ComponentModel;
using System.Windows;

using FileSyncGui.GuiAbstracts;
using System;
using System.Text;

namespace FileSyncGui {

	/// <summary>
	/// Interaction logic for SystemMessage.xaml
	/// </summary>
	public partial class SystemMessage : Window, INotifyPropertyChanged {

		private string windowTitle;
		public string WindowTitle {
			get { return windowTitle; }
			set {
				if (windowTitle == value)
					return;
				windowTitle = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("WindowTitle"));
			}
		}

		private string messageTitle;
		public string MessageTitle {
			get { return messageTitle; }
			set {
				if (messageTitle == value)
					return;
				messageTitle = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("MessageTitle"));
			}
		}

		private string messageText;
		public string MessageText {
			get { return messageText; }
			set {
				if (messageText == value)
					return;
				messageText = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("MessageText"));
			}
		}

		private MemeType messageImage;
		public MemeType MessageImage {
			get { return messageImage; }
			set {
				if (messageImage == value)
					return;
				messageImage = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("MessageImage"));
			}
		}

		private bool toggleImage;
		public bool ToggleImage {
			get { return toggleImage; }
			set {
				if (toggleImage == value)
					return;
				toggleImage = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ToggleImage"));
			}
		}

		private bool toggleOk;
		public bool ToggleOk {
			get { return toggleOk; }
			set {
				if (toggleOk == value)
					return;
				toggleOk = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ToggleOk"));
			}
		}

		private bool toggleCancel;
		public bool ToggleCancel {
			get { return toggleCancel; }
			set {
				if (toggleCancel == value)
					return;
				toggleCancel = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ToggleCancel"));
			}
		}

		private bool toggleHelp;
		public bool ToggleHelp {
			get { return toggleHelp; }
			set {
				if (toggleHelp == value)
					return;
				toggleHelp = value;

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ToggleHelp"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public SystemMessage() {
			var text = new StringBuilder("This is a very long example text. ");
			for (int i = 0; i < 3; i++)
				text.Append(text);

			SetContent("FileSync: System message", "This is an example title.",
				text.ToString(), MemeType.AreYouFuckingKiddingMe);
			SetButtons();

			InitializeComponent();
		}

		public SystemMessage(string windowTitle, string messageTitle, string messageText,
				MemeType? memeType = null, bool toggleOk = true, bool toggleCancel = false,
				bool toggleHelp = false) {
			SetContent(windowTitle, messageTitle, messageText, memeType);
			SetButtons(toggleOk, toggleCancel, toggleHelp);

			InitializeComponent();
		}

		public SystemMessage(ActionException ex, bool toggleOk = true, bool toggleCancel = false,
				bool toggleHelp = false)
			: this("FileSync: System message", ex.Title, ex.Message, ex.Image, toggleOk,
				toggleCancel, toggleHelp) {
			//nothing needed here
		}

		public SystemMessage(ActionResult actionResult, bool toggleOk = true,
				bool toggleCancel = false, bool toggleHelp = false)
			: this("Achievement GET!", actionResult.Title, actionResult.Desc, MemeType.FuckYea,
				toggleOk, toggleCancel, toggleHelp) {
			//nothing needed here
		}

		private void Ok_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = true;
			this.Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
			this.Close();
		}

		private void Help_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
			this.Close();
		}

		private void SetContent(string windowTitle, string messageTitle, string messageText,
				MemeType? memeType) {
			WindowTitle = windowTitle;
			MessageTitle = messageTitle;
			MessageText = messageText;
			if (memeType == null) {
				ToggleImage = false;
				MessageImage = MemeType.FuckYea;
			} else {
				ToggleImage = true;
				MessageImage = (MemeType)memeType;
			}
		}

		private void SetButtons(bool toggleOk = true, bool toggleCancel = false,
				bool toggleHelp = false) {
			ToggleOk = true;
			ToggleCancel = false;
			ToggleHelp = false;
		}

	}
}

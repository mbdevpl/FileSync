using System;
using System.Text;

namespace FileSyncGui.Local {

	/// <summary>
	/// Defines the exception thrown by methods from GuiActions namespace.
	/// </summary>
	public class ActionException : Exception {

		private static bool SHOW_EXCEPTION_DETAILS = true;

		private ActionType type;
		/// <summary>
		/// Type of action performed (it caused the construction of this exception).
		/// </summary>
		public ActionType Type {
			get { return type; }
		}

		private string title;
		/// <summary>
		/// Recommended title for the message box when informing about this exception.
		/// </summary>
		public string Title {
			get { return title; }
		}

		private MemeType image;
		/// <summary>
		/// Image shown beside the message.
		/// </summary>
		public MemeType Image {
			get { return image; }
		}

		/// <summary>
		/// Creates new action exception.
		/// </summary>
		/// <param name="message">the message, as in ordinary exception</param>
		/// <param name="actionType">type of the action that caused the exception</param>
		/// <param name="innerException">exception that was catched to throw this one</param>
		public ActionException(string message, ActionType actionType, MemeType memeType
				= MemeType.AreYouFuckingKiddingMe, Exception innerException = null)
			: base(SHOW_EXCEPTION_DETAILS ? GetExtendedMessage(message, innerException) :
				GetTypicalMessage(message, innerException), innerException) {
			SetInitialValues(message, actionType, memeType);
		}

		private void SetInitialValues(string message, ActionType actionType, MemeType memeType) {
			if (memeType.Equals(MemeType.FuckYea)) {
				this.title = actionType.ToString() + " action was successful.";
			} else {
				this.title = actionType.ToString() + " action caused an error.";
			}
			this.type = actionType;
			this.image = memeType;
		}

		private static string GetTypicalMessage(string message, Exception e = null) {
			if (e == null)
				return message;

			return message + " Reason was: " + e.Message + ".";
		}

		private static string GetExtendedMessage(string message, Exception e = null) {

			if (e == null)
				return message;

			var str = new StringBuilder(GetTypicalMessage(message, e));

			str.Append("\n\nSource was: ").Append(e.Source);
			str.Append("\n\nAdditional data: ").Append(e.Data);
			str.Append("\n\nStack trace:\n").Append(e.StackTrace);

			return str.ToString();
		}

	}
}

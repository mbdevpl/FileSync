using System.Runtime.Serialization;

namespace FileSyncObjects {

	/// <summary>
	/// Stores result of the action that happened because user activity in the GUI.
	/// </summary>
	[DataContract]
	public class ActionResult {

		private string description;
		[DataMember]
		public string Description {
			get { return description; }
			set { description = value; }
		}

		private bool wasSuccessful;
		[DataMember]
		public bool WasSuccessful {
			get { return WasSuccessful; }
			set { WasSuccessful = value; }
		}

		/// <summary>
		/// Creates new result of some acition.
		/// </summary>
		/// <param name="actionTitle"></param>
		/// <param name="actionDescription"></param>
		/// <param name="actionType">type of the action, value from ActionType enum</param>
		/// <param name="wasSuccessful">indicates whether the action completed successfully</param>
		/// <param name="resultContext"></param>
		/// <param name="errorContext"></param>
		public ActionResult(string description, bool wasSuccessful = true) {
			this.description = description;
			this.wasSuccessful = wasSuccessful;
		}

	}
}

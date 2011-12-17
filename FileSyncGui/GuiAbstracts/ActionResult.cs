using System;

namespace FileSyncGui.GuiAbstracts {

	/// <summary>
	/// Stores result of the action that happened because user activity in the GUI.
	/// </summary>
	public class ActionResult {

		private string title;
		public string Title {
			get { return title; }
		}

		private string desc;
		public string Desc {
			get { return desc; }
		}

        private ActionType type;
		public ActionType Type {
			get { return type; }
		}

		private ActionResult context;
		public ActionResult Context {
			get { return context; }
		}

		private bool success;
		protected bool Success {
			get { return success; }
			set { success = value; }
		}
		public bool WasSuccessful {
			get { return success; }
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
		public ActionResult(string actionTitle, string actionDescription, ActionType actionType,
			bool wasSuccessful = true, ActionResult resultContext = null,
			ActionException errorContext = null) {
			title = actionTitle;
			desc = actionDescription;
			type = actionType;
			success = wasSuccessful;
			context = resultContext;
		}

		/// <summary>
		/// Creates an action result from an action exception.
		/// </summary>
		/// <param name="ex">exception to be converted to the result</param>
		public ActionResult(ActionException ex)
			: this(ex.Title, ex.Message, ex.Type, ex.Image.Equals(MemeType.FuckYea), null, ex) {
			//nothing needed here
		}

	}
}

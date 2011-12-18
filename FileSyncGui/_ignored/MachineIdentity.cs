using System;
using System.Text;
using System.Windows;

using FileSyncGui.GuiAbstracts;
using FileSyncLib;

namespace FileSyncGui.GuiObjects {

	/// <summary>
	/// Defines the identity of a single machine.
	/// </summary>
	public class MachineIdentity : DependencyObject {

		public static readonly DependencyProperty NameProperty
			= DependencyProperty.Register("Name", typeof(string),
			typeof(MachineIdentity), new UIPropertyMetadata(null));
		public string Name {
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		public static readonly DependencyProperty DescriptionProperty
			= DependencyProperty.Register("Description", typeof(string),
			typeof(MachineIdentity), new UIPropertyMetadata(null));
		public string Description {
			get { return (string)GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		public MachineIdentity(string name, string description = null) {
			Name = name;
			Description = description;
		}

		public MachineIdentity(MachineIdentity mid)
			: this(mid.Name, mid.Description) {
			//nothing needed here
		}

		public MachineIdentity(MachineModel m)
			: this(m.Name, m.Description) {
			//nothing needed here
		}

		public MachineModel ToLib() {
			return new MachineModel(Name, Description);
		}

		public ActionResult AddToDatabase(Credentials cr) {
			try {
				if (cr == null)
					throw new ArgumentNullException("cr", "user credentials must be provided");

				MachManipulator.AddMachine(cr.ToLib(), this.ToLib());

			} catch (Exception ex) {
				throw new ActionException("Failed to create a new machine.", ActionType.Machine,
					MemeType.Fuuuuu, ex);
			}

			return new ActionResult("Machine created.",
				"Your new machine was added to the database.", ActionType.Machine);
		}

		public ActionResult UpdateInDatabase(Credentials cr, MachineIdentity newMid) {
			try {
				if (cr == null)
					throw new ArgumentNullException("cr", "user credentials must be provided");

				MachManipulator.ChangeMachineDetails(cr.ToLib(), newMid.ToLib(), this.ToLib());
			} catch (Exception ex) {
				throw new ActionException("Error while updating machine details.",
					ActionType.Machine, MemeType.Fuuuuu, ex);
			}

			return new ActionResult("Machine updated.", 
				"Current machine details were saved in the database.", ActionType.Machine);
		}

		public ActionResult DeleteFromDatabase(Credentials cr) {
			throw new ActionException("Deletion of machines is not properly handled.", 
				ActionType.Machine);
		}

		protected StringBuilder GetArguments() {
			return new StringBuilder("Name=").Append(Name).Append(",Description=")
				.Append(Description);
		}

		override public string ToString() {
			return new StringBuilder("[").Append(GetArguments()).Append("]").ToString();
		}

	}

}

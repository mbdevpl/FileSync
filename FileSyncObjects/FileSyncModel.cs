using System;

using FileSyncObjects;

namespace FileSyncObjects {

	/// <summary>
	/// Example implementation of the interface.
	/// </summary>
	class FileSyncModel : IFileSyncModel {

		#region User

		public void AddUser(Credentials c, UserContents u) {
			throw new Exception("not implemented");
		}

		public void Login(Credentials c) {
			throw new Exception("not implemented");
		}

		public UserContents GetUser(Credentials c) {
			throw new Exception("not implemented");
		}

		public void GetMachineList(Credentials c, UserContents u) {
			throw new Exception("not implemented");
		}

		#endregion

		#region Machine

		public void AddMachine(Credentials c, MachineContents m) {
			throw new Exception("not implemented");
		}

		public void ChangeMachineDetails(Credentials c, MachineContents newM, 
				MachineContents oldM) {
			throw new Exception("not implemented");
		}

		public void GetDirList(Credentials c, MachineContents m) {
			throw new Exception("not implemented");
		}

		#endregion

		#region Directory

		public void AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			throw new Exception("not implemented");
		}

		public void GetFileList(Credentials c, MachineContents m, DirectoryContents d) {
			throw new Exception("not implemented");
		}

		#endregion

		#region File

		public void AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			throw new Exception("not implemented");
		}

		public void GetFileContent(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			throw new Exception("not implemented");
		}

		#endregion

	}

}

using System;

namespace FileSyncObjects {

	/// <summary>
	/// Example (i.e. empty) implementation of the interface. Extending it provides a convinient way 
	/// of compiling code not belonging to the use cases.
	/// </summary>
	class FileSyncModel : IFileSyncModel {

		#region Tests

		public bool TestWCF() {
			throw new NotImplementedException();
		}

		public bool TestEF() {
			throw new NotImplementedException();
		}

		#endregion

		#region User

		public bool AddUser(UserContents u) {
			throw new NotImplementedException();
		}

		public bool Login(Credentials c) {
			throw new NotImplementedException();
		}

		public UserIdentity GetUser(Credentials c) {
			throw new NotImplementedException();
		}

		public UserContents GetUserWithMachines(Credentials c) {
			throw new NotImplementedException();
		}

		public bool DelUser(Credentials c) {
			throw new NotImplementedException();
		}

		#endregion

		#region Machine

		public bool AddMachine(Credentials c, MachineContents m) {
			throw new NotImplementedException();
		}

		public bool ChangeMachineDetails(Credentials c, MachineContents newM, MachineContents oldM) {
			throw new NotImplementedException();
		}

		public MachineContents GetMachineWithDirs(Credentials c, MachineIdentity mid) {
			throw new NotImplementedException();
		}

		public bool DelMachine(Credentials c, MachineIdentity mid) {
			throw new NotImplementedException();
		}

		#endregion

		#region Directory

		public bool AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			throw new NotImplementedException();
		}

		public DirectoryContents GetDirectoryWithFiles(Credentials c, MachineContents m, DirectoryIdentity d) {
			throw new NotImplementedException();
		}

		public bool DelDirectory(Credentials c, MachineIdentity mid, DirectoryIdentity did) {
			throw new NotImplementedException();
		}

		#endregion

		#region File

		public bool AddFile(Credentials c, MachineContents m, DirectoryContents d, FileContents f) {
			throw new NotImplementedException();
		}

		public FileContents GetFileWithContent(Credentials c, MachineContents m, DirectoryContents d, FileIdentity f) {
			throw new NotImplementedException();
		}

		public bool DelFile(Credentials c, MachineIdentity mid, DirectoryIdentity did, FileIdentity f) {
			throw new NotImplementedException();
		}

		#endregion

	}

}

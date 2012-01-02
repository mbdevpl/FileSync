using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;

namespace FileSyncObjects {

	/// <summary>
	/// Interface of the FileSync application containing all functionalites, which are visible
	/// to the client. Since operations are not session-based, each function requires credentials.
	/// </summary>
	[ServiceContract]
	public interface IFileSyncModel {

		#region Tests
		[OperationContract]
		bool TestWCF();
		[OperationContract]
		bool TestEF();
		#endregion

		#region User
		[OperationContract]
		bool AddUser(UserContents u);
		[OperationContract]
		bool Login(Credentials c);
		[OperationContract]
		UserIdentity GetUser(Credentials c);
		[OperationContract]
		UserContents GetUserWithMachines(Credentials c);
		[OperationContract]
		bool DelUser(Credentials c);
		#endregion

		#region Machine
		[OperationContract]
		bool AddMachine(Credentials c, MachineContents m);
		[OperationContract]
		bool ChangeMachineDetails(Credentials c, MachineContents newM, MachineContents oldM);
		[OperationContract]
		List<DirectoryContents> GetDirList(Credentials c, MachineContents m);
		[OperationContract]
		MachineContents GetMachineWithDirs(Credentials c, MachineIdentity mid);
		[OperationContract]
		bool DelMachine(Credentials c, MachineIdentity mid);
		#endregion

		#region Directory
		[OperationContract]
		bool AddDirectory(Credentials c, MachineContents m, DirectoryContents d);
		[OperationContract]
		DirectoryContents GetDirectoryWithFiles(Credentials c, MachineContents m, DirectoryIdentity d);
		[OperationContract]
		bool DelDirectory(Credentials c, MachineIdentity mid, DirectoryIdentity did);
		#endregion

		#region File
		[OperationContract]
		bool AddFile(Credentials c, MachineContents m, DirectoryContents d, FileContents f);
		[OperationContract]
		FileContents GetFileWithContent(Credentials c, MachineContents m, DirectoryContents d, FileIdentity f);
		[OperationContract]
		bool DelFile(Credentials c, MachineIdentity mid, DirectoryIdentity did, FileIdentity f);
		#endregion

		#region Other
		//none
		#endregion

	}

}

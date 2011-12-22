using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace FileSyncObjects {

	/// <summary>
	/// Interface of the FileSync application containing all functionalites, which are visible
	/// to the client. Since operations are not session-based, each function requires credentials.
	/// </summary>
	[ServiceContract]
	public interface IFileSyncModel {

		#region Tests
		[OperationContract]
		string TestWCF();
		[OperationContract]
		string TestEF();
		#endregion

		#region User
		[OperationContract]
		void AddUser(UserContents u);
		[OperationContract]
		void Login(Credentials c);
		[OperationContract]
		UserContents GetUser(Credentials c);
		[OperationContract]
		void GetMachineList(Credentials c, UserContents u);
		[OperationContract]
		void DelUser(Credentials c);
		#endregion

		#region Machine
		[OperationContract]
		void AddMachine(Credentials c, MachineContents m);
		[OperationContract]
		void ChangeMachineDetails(Credentials c, MachineContents newM, MachineContents oldM);
		[OperationContract]
		void GetDirList(Credentials c, MachineContents m);
		#endregion

		#region Directory
		[OperationContract]
		void AddDirectory(Credentials c, MachineContents m, DirectoryContents d);
		[OperationContract]
		void GetFileList(Credentials c, MachineContents m, DirectoryContents d);
		#endregion

		#region File
		[OperationContract]
		void AddFile(Credentials c, MachineContents m, DirectoryContents d, FileContents f);
		[OperationContract]
		void GetFileContent(Credentials c, MachineContents m, DirectoryContents d, FileContents f);
		#endregion

		#region Other
		//none
		#endregion

	}

}

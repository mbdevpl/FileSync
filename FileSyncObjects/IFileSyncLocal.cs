using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace FileSyncObjects {

	/// <summary>
	/// Defines use cases for a local side of an application. Actions like uploading 
	/// or downloading objects, reading to or writing from a local file system, accessing 
	/// file properties etc. - they are all defined here.
	/// </summary>
	[ServiceContract]
	public interface IFileSyncLocal {

		#region Machine
		[OperationContract]
		bool UploadMachine(IFileSyncModel connection, Credentials c, MachineContents m);
		[OperationContract]
		bool UpdateMachine(IFileSyncModel connection, Credentials c,
			MachineContents newM, MachineContents oldM);
		[OperationContract]
		MachineContents DownloadMachine(IFileSyncModel connection, Credentials c,
			MachineIdentity mid);
		[OperationContract]
		MachineContents ReadMachineContents(MachineContents m, bool addFilesContents = false);
		[OperationContract]
		bool SaveMachine(MachineContents m);
		[OperationContract]
		bool EraseMachine(MachineContents d);
		#endregion

		#region Directory
		[OperationContract]
		bool UploadDirectory(IFileSyncModel connection, Credentials c, MachineIdentity m, 
			DirectoryContents d);
		[OperationContract]
		DirectoryContents DownloadDirectory(IFileSyncModel connection, Credentials c, 
			MachineIdentity m, DirectoryIdentity d);
		[OperationContract]
		DirectoryIdentity ReadDirectoryMetadata(string localPath);
		[OperationContract]
		DirectoryContents ReadDirectoryContents(string localPath, bool addFilesContents = false);
		[OperationContract]
		DirectoryContents ReadDirectoryContents(DirectoryIdentity d, bool addFilesContents = false);
		[OperationContract]
		bool SaveDirectory(DirectoryContents d);
		/// <summary>
		/// Deletes directory (with contents) from the file system.
		/// </summary>
		/// <param name="localPath">full path to the resource</param>
		/// <returns></returns>
		[OperationContract]
		bool EraseDirectory(string localPath);
		/// <summary>
		/// Erases the directory with all contents, deletes the directory itself.
		/// </summary>
		/// <param name="d">directory identity</param>
		/// <returns></returns>
		[OperationContract]
		bool EraseDirectory(DirectoryIdentity d);
		/// <summary>
		/// Erases the directory contents specified in the parameter, and deletes the directory 
		/// if it is empty after deleting the given contents.
		/// </summary>
		/// <param name="d">directory contents</param>
		/// <returns></returns>
		[OperationContract]
		bool EraseDirectory(DirectoryContents d);
		#endregion

		#region File
		[OperationContract]
		bool UploadFile(IFileSyncModel connection, Credentials c, MachineIdentity mid,
			DirectoryIdentity did, FileContents f);
		[OperationContract]
		FileContents DownloadFile(IFileSyncModel connection, Credentials c, MachineIdentity mid,
			DirectoryIdentity did, FileIdentity fid);
		[OperationContract]
		FileIdentity ReadFileMetadata(string localPath);
		[OperationContract]
		FileContents ReadFileContents(string localPath);
		[OperationContract]
		FileContents ReadFileContents(FileIdentity fid, DirectoryIdentity did);
		/// <summary>
		/// Saves the specified file contents under the given path.
		/// </summary>
		/// <param name="f">file contents</param>
		/// <param name="localPath">full path to the resource</param>
		/// <returns></returns>
		[OperationContract]
		bool SaveFile(FileContents f, string localPath);
		/// <summary>
		/// Saves file to the local path of the supplied directory parameter.
		/// </summary>
		/// <param name="f">file contents</param>
		/// <param name="d">directory identity</param>
		/// <returns></returns>
		[OperationContract]
		bool SaveFile(FileContents f, DirectoryIdentity d);
		/// <summary>
		/// Deletes the file under the specified path.
		/// </summary>
		/// <param name="localPath">full path to the resource</param>
		/// <returns></returns>
		[OperationContract]
		bool EraseFile(string localPath);
		#endregion

	}

}

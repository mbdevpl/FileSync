using System;
using System.Collections.Generic;
using System.IO;

using FileSyncObjects;

namespace FileSyncGui {
	public class FileSyncConnection : IFileSyncModel {

		#region Tests

		public bool TestWCF() {
			bool result = false;
			using (var cl = new Ref.FileSyncModelClient()) {
				//result = cl.TestWCF();
			}
			return result;
		}

		public bool TestEF() {
			bool result = false;
			using (var cl = new Ref.FileSyncModelClient()) {
				//result = cl.TestEF();
			}
			return result;
		}

		#endregion

		#region User (connection)

		public void AddUser(UserContents u) {
			try {
				using (var cl = new Ref.FileSyncModelClient()) {
					cl.AddUser(u);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to create new user account.", 
					ActionType.User, ex);
			}
		}

		public void Login(Credentials c) {
			try {
				using (var cl = new Ref.FileSyncModelClient()) {
					cl.Login(c);
				}
			} catch (Exception ex) {
				throw new ActionException("Login process did not end in a very happy manner. "
					+ "In fact, it ended in a very unpleasant way.",
					ActionType.User, ex, MemeType.AreYouFuckingKiddingMe);
			}
		}

		//public void Logout() {
		//    throw new ActionException("Logout is not properly handled...", ActionType.User,
		//        MemeType.Fuuuuu);
		//}

		public UserContents GetUser(Credentials c) {
			try {
				UserContents u;
				using (var cl = new Ref.FileSyncModelClient()) {
					u = cl.GetUser(c);
				}
				return u;
			} catch (Exception ex) {
				throw new ActionException("Unable to get user details for the given credentials.",
					ActionType.User, ex);
			}
		}

		public void GetMachineList(Credentials c, UserContents u) {
			try {
				using (var cl = new Ref.FileSyncModelClient()) {
					cl.GetMachineList(c, u);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to get machines for a given user.",
					ActionType.User, ex);
			}
		}

		//public void UpdateUser(Credentials c, UserContents newU) {
		//    //try {
		//    //    //UserManipulator.Update()
		//    //} catch (Exception ex) {
		//    //    throw new ActionException("Unable to update credentials.", ActionType.User,
		//    //        MemeType.Fuuuuu, ex);
		//    //}
		//    //return new ActionResult("Credentials updated.", "",
		//    //    ActionType.User);
		//    throw new ActionException("Updating credentials is not properly handled...",
		//        ActionType.User, MemeType.Fuuuuu);
		//}

		public void DelUser(Credentials c) {
			try {
				using (var cl = new Ref.FileSyncModelClient()) {
					cl.DelUser(c);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to delete the user account.", 
					ActionType.User, ex);
			}
		}

		#endregion

		#region User (local)

		public UserContents GetUserWithMachines(Credentials c) {
			try {
				var u = this.GetUser(c);
				this.GetMachineList(c, u);
				return u;
			} catch (ActionException ex) {
				throw new ActionException("Error occured while getting user and his machines "
					+ "at the same time.", ex.Type, ex.Image, ex);
			} catch (Exception ex) {
				throw new ActionException("Unable to get list of machines for the user "
					+ "with the given credentials.", ActionType.User, MemeType.Fuuuuu, ex);
			}
		}

		#endregion

		#region Machine (connection)

		public void AddMachine(Credentials c, MachineContents m) {
			try {
				if (c == null)
					throw new ArgumentNullException("cr", "user credentials must be provided");

				using (var cl = new Ref.FileSyncModelClient()) {
					cl.AddMachine(c, m);
				}
			} catch (Exception ex) {
				throw new ActionException("Failed to create a new machine.", ActionType.Machine,
					MemeType.Fuuuuu, ex);
			}
		}

		public void ChangeMachineDetails(Credentials c, MachineContents newM,
				MachineContents oldM) {
			try {
				if (c == null)
					throw new ArgumentNullException("c", "user credentials must be provided");
				if (newM == null)
					throw new ArgumentNullException("newM", "new machine identity must be provided");
				if (oldM == null)
					throw new ArgumentNullException("oldM", "old machine identity must be provided");

				using (var cl = new Ref.FileSyncModelClient()) {
					ChangeMachineDetails(c, newM, oldM);
				}
			} catch (Exception ex) {
				throw new ActionException("Error while updating machine details.",
					ActionType.Machine, MemeType.Fuuuuu, ex);
			}
		}

		public void GetDirList(Credentials c, MachineContents m) {
			try {
				if (c == null)
					throw new ArgumentNullException("c", "user credentials must be provided");
				if (m == null)
					throw new ArgumentNullException("m", "machine identity must be provided");

				using (var cl = new Ref.FileSyncModelClient()) {
					cl.GetDirList(c, m);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to get list of directories belonging "
					+ "to the machine.", ActionType.Machine, MemeType.Fuuuuu, ex);
			}
		}

		#endregion

		#region Machine (local)

		public void GetLocalDirContents(MachineContents m, bool addLocalFilesContents = false,
			bool addLocalFilesMetadata = true) {
			List<DirectoryContents> dirs = m.Directories;

			if (dirs != null && dirs.Count > 0 && addLocalFilesMetadata) {
				foreach (DirectoryContents d in dirs) {
					if (d.LocalPath == null || d.LocalPath.Equals(EmptyLocalPath))
						return;

					string[] filePaths = Directory.GetFiles(d.LocalPath);

					if (filePaths == null || filePaths.Length == 0)
						return;

					if (d.Files == null)
						d.Files = new List<FileContents>();

					foreach (string path in filePaths) {
						FileContents file;
						if (addLocalFilesContents)
							file = GetLocalFileContent(path);
						else
							file = GetLocalFileMetadata(path);
						d.Files.Add(file);
					}
				}
			}
		}

		public void UploadMachine(Credentials c, MachineContents m) {
			try {
				foreach (DirectoryContents d in m.Directories) {
					UploadDirectory(c, m, d);
					// throw new Exception("directory transfer silently failed");
				}
			} catch (ActionException ex) {
				throw new ActionException("Couldn't upload the machine contents.\n\n"
					+ ex.Message, ActionType.Machine, MemeType.Fuuuuu, ex);
			} catch (Exception ex) {
				throw new ActionException("Error while uploading a whole machine.",
					ActionType.Machine, MemeType.Fuuuuu, ex);
			}
		}

		public void DownloadMachine(Credentials c, MachineContents m) {
			GetDirList(c, m);
			foreach (DirectoryContents d in m.Directories) {
				GetFileList(c, m, d);
				foreach (FileContents f in d.Files) {
					GetFileContent(c, m, d, f);
				}
			}
		}

		public void SaveMachineToDisk(MachineContents machine) {
			throw new NotImplementedException();
		}

		#endregion

		#region Directory (connection)

		public void AddDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.AddDirectory(c, m, d);
				}
			} catch (Exception ex) {
				throw new ActionException("Unable to create a new directory in the database.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}
		}

		public void GetFileList(Credentials c, MachineContents m, DirectoryContents d) {

			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.GetFileList(c, m, d);
				}
			} catch (ActionException ex) {
				throw new ActionException("Unable to download directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			} catch (Exception ex) {
				throw new ActionException("Error while downloading directory contents.",
					ActionType.Directory, MemeType.Fuuuuu, ex);
			}
		}

		#endregion

		#region Directory (local)

		public void UploadDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			//add dir if it does not exist
			AddDirectory(c, m, d);

			foreach (FileContents f in d.Files) {
				try {
					FileContents fUp = null;
					if (f.Size == 0)
						fUp = GetLocalFileContent(f, d.LocalPath + "\\" + f.Name);
					else
						fUp = f;

					UploadFile(c, m, d, f);
				} catch (ActionException ex) {
					throw new ActionException("Couldn't upload the directory contents.\n\n"
						+ ex.Message, ActionType.Directory, MemeType.Fuuuuu, ex);
				} catch (Exception ex) {
					throw new ActionException("Error while uploading a directory.",
						ActionType.Directory, MemeType.Fuuuuu, ex);
				}
			}
		}

		internal void DownloadDirectory(Credentials c, MachineContents m, DirectoryContents d) {
			throw new NotImplementedException();
		}

		internal bool SaveDirectoryToDisk(DirectoryContents d) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function does not assume that the files list is initialized. If it is then 
		/// elements are added to the end. If some elements were added, the list is sorted.
		/// This method performs no cleanup of duplicates in case of incorrect of consequtive use.
		/// </summary>
		public void GetLocalFileList(DirectoryContents d) {
			if (d.LocalPath == null || d.LocalPath.Equals(EmptyLocalPath))
				return;

			string[] filePaths = Directory.GetFiles(d.LocalPath);

			if (filePaths == null || filePaths.Length == 0)
				return;

			if (d.Files == null)
				d.Files = new List<FileContents>();

			foreach (string path in filePaths) {
				FileContents fc = GetLocalFileContent(path);
				d.Files.Add(fc);
			}


		}

		/// <summary>
		/// Removes the duplicate files inside this directory. It performes basing on 
		/// the rule, which states that file names must not repeat.
		/// </summary>
		public void RemoveDuplicateFiles(DirectoryContents d) {
			// Possible second rule example (not introduced):
			// If hash of file.txt equals to hash of file.bmp, both are preserved.
			// If, on the other hand, hash of file.txt equals to hash of file2.txt the later file 
			// is preserved.

			if (d.Files == null || d.Files.Count <= 1)
				return;

			List<FileContents> sortedFiles = new List<FileContents>(d.Files);
			sortedFiles.Sort(FileComparison);

			for (int i = 0; i < sortedFiles.Count - 1; i++) {
				if (sortedFiles[i].Name.Equals(sortedFiles[i + 1].Name)) {
					sortedFiles.RemoveAt(i);
					i--;
				}
			}

			d.Files = sortedFiles;
		}

		private static int FileComparison(FileContents x, FileContents y) {
			int nameComparison = x.Name.CompareTo(y.Name);
			if (nameComparison != 0)
				return nameComparison;

			int modifiedComparison = x.Modified.CompareTo(y.Modified);
			if (modifiedComparison != 0)
				return modifiedComparison;

			int sizeComparison = x.Size.CompareTo(y.Size);
			if (sizeComparison != 0)
				return sizeComparison * (-1);

			int hashComparison = x.Hash.CompareTo(y.Hash);
			if (hashComparison != 0)
				return hashComparison;

			return 0;
		}

		#endregion

		#region File (connection)

		public void AddFile(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.AddFile(c, m, d, f);
				}
			} catch (Exception ex) {
				throw new ActionException("Error occurred while file was uploaded.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}
		}

		public void GetFileContent(Credentials c, MachineContents m, DirectoryContents d,
				FileContents f) {
			try {
				using (FileSyncModelClient cl = new FileSyncModelClient()) {
					cl.GetFileContent(c, m, d, f);
				}
			} catch (Exception ex) {
				throw new ActionException("Error while downloading file contents from database.",
					ActionType.File, MemeType.Fuuuuu, ex);
			}
		}
		#endregion

		#region File (local)

		private FileContents GetLocalFileMetadata(string filePath) {
			if (filePath == null)
				throw new ActionException("No file path was provided.", ActionType.File,
					MemeType.Fuuuuu);

			int lastSlash = filePath.LastIndexOf('\\') + 1;

			string dirPath = filePath.Substring(0, lastSlash);
			string fileName = filePath.Substring(lastSlash);

			if (dirPath.Length == 0 || fileName.Length == 0
					|| filePath.Equals(EmptyLocalPath))
				throw new ActionException("Unable to get file metadata from an ivalid path: '"
					+ filePath + "'.", ActionType.File, MemeType.Fuuuuu);

			FileContents f = new FileContents(fileName,System.IO.File.GetLastWriteTime(filePath));
			f.Type = FileType.PlainText;
			return f;
		}

		public FileContents GetLocalFileContent(string path) {
			return GetLocalFileContent(GetLocalFileMetadata(path), path);
		}

		public FileContents GetLocalFileContent(FileContents f, string filePath) {
			if (f == null)
				throw new ActionException("No initial file identity was given.", ActionType.File,
					MemeType.AreYouFuckingKiddingMe);

			if (filePath == null)
				throw new ActionException("No file path was provided.", ActionType.File,
					MemeType.Fuuuuu);

			int lastSlash = filePath.LastIndexOf('\\') + 1;
			string dirPath = filePath.Substring(0, lastSlash);
			string fileName = filePath.Substring(lastSlash);

			if (dirPath.Length == 0 || fileName.Length == 0
					|| filePath.Equals(EmptyLocalPath))
				throw new ActionException("Unable to get file contents from an ivalid path: '"
					+ filePath + "'.", ActionType.File, MemeType.Fuuuuu);

			string contents = ReadLocalFile(filePath);

			f.Contents = contents;
			f.Size = contents.Length;
			f.Hash = Security.ComputeHash(contents);
			return f;
		}

		private string ReadLocalFile(string filePath) {
			string contents = String.Empty;
			try {
				StreamReader reader = new StreamReader(filePath);
				contents = reader.ReadToEnd();
				reader.Close();
			} catch (Exception ex) {
				throw new ActionException("Error while reading file data from disk. "
					+ "File path was: '" + filePath + "'.", ActionType.File, ex);
			}
			return contents;
		}

		public void UploadFile(Credentials c, MachineContents m, DirectoryContents d,
			FileContents f) {
			AddFile(c, m, d, f);
		}

		#endregion

	}
}

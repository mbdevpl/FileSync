using System;
using System.Collections.Generic;
using System.IO;

using FileSyncObjects;

namespace FileSyncGui {

	/// <summary>
	/// Defines local use cases
	/// </summary>
	public class FileSyncLocal : IFileSyncLocal {

		/// <summary>
		/// Used in directory, and file operations.
		/// </summary>
		internal static string EmptyLocalPath = "\\";

		/// <summary>
		/// Removes the duplicate files inside this directory. It performes basing on 
		/// the rule, which states that file names must not repeat.
		/// </summary>
		private void RemoveDuplicateFiles(DirectoryContents d) {
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

		#region Machine (local)

		public bool UploadMachine(IFileSyncModel connection, Credentials c, MachineContents m) {
			try {
				foreach (DirectoryContents d in m.Directories) {
					if (!UploadDirectory(connection, c, m, d))
						return false;
				}
				return true;
			} catch (ActionException ex) {
				throw new ActionException("Couldn't upload the machine contents."
					, ActionType.Machine, ex);
			} catch (Exception ex) {
				throw new ActionException("Error while uploading a whole machine.",
					ActionType.Machine, ex);
			}
		}

		public bool UpdateMachine(IFileSyncModel connection, Credentials c, MachineContents newM,
				MachineContents oldM) {
			throw new NotImplementedException();
		}

		public MachineContents DownloadMachine(IFileSyncModel connection, Credentials c,
				MachineIdentity mid) {
			var m = connection.GetMachineWithDirs(c, mid);

			if (m == null)
				return null;

			List<DirectoryContents> directories = new List<DirectoryContents>();
			foreach (DirectoryIdentity did in m.Directories) {
				var d = connection.GetDirectoryWithFiles(c, m, did);
				List<FileContents> files = new List<FileContents>();
				foreach (FileIdentity fid in d.Files) {
					var f = connection.GetFileWithContent(c, m, d, fid);
					files.Add(f);
				}
				d.Files = files;
				directories.Add(d);
			}
			m.Directories = directories;
			return m;
		}

		public MachineContents ReadMachineContents(MachineContents m,
				bool addFilesContents = false) {
			if (m.Directories == null || m.Directories.Count == 0)
				return m;

			foreach (DirectoryContents d in m.Directories) {
				if (d.LocalPath == null || d.LocalPath.Equals(EmptyLocalPath))
					return null;

				string[] filePaths = Directory.GetFiles(d.LocalPath);

				if (filePaths == null || filePaths.Length == 0)
					return null;

				if (d.Files == null)
					d.Files = new List<FileContents>();

				foreach (string path in filePaths) {
					FileContents file;
					if (addFilesContents)
						file = ReadFileContents(path);
					else
						file = new FileContents(ReadFileMetadata(path));
					d.Files.Add(file);
				}

				RemoveDuplicateFiles(d);
			}
			return m;
		}

		public bool SaveMachine(MachineContents m) {
			throw new NotImplementedException();
		}

		public bool EraseMachine(MachineContents d) {
			throw new NotImplementedException();
		}

		#endregion

		#region Directory (local)

		public bool UploadDirectory(IFileSyncModel connection, Credentials c, MachineIdentity m,
				DirectoryContents d) {

			//add dir if it does not exist
			if (!connection.AddDirectory(c, new MachineContents(m), d))
				return false;

			foreach (FileContents f in d.Files) {
				try {
					FileContents fUp = null;
					if (f.Size == 0)
						fUp = ReadFileContents(f, d);
					else
						fUp = f;

					if (!UploadFile(connection, c, m, d, f))
						return false;
				} catch (ActionException ex) {
					throw new ActionException("Couldn't upload the directory contents.",
						ActionType.Directory, ex);
				} catch (Exception ex) {
					throw new ActionException("Error while uploading a directory.",
						ActionType.Directory, ex);
				}
			}
			return true;
		}

		public DirectoryContents DownloadDirectory(IFileSyncModel connection, Credentials c,
				MachineIdentity m, DirectoryIdentity d) {
			throw new NotImplementedException();
		}

		public DirectoryIdentity ReadDirectoryMetadata(string localPath) {
			int lastSlash = localPath.LastIndexOf('\\') + 1;
			bool endsWithSlash = (lastSlash >= 0 && lastSlash == localPath.Length);
			
			if (endsWithSlash) {
				localPath = localPath.Substring(0, localPath.Length);
				lastSlash = localPath.LastIndexOf('\\') + 1;
			}
			
			string dirPath = localPath.Substring(0, lastSlash);
			string dirName = localPath.Substring(lastSlash);

			if (dirPath.Length == 0 || dirName.Length == 0
					|| localPath.Equals(EmptyLocalPath))
				throw new ActionException("Unable to get directory metadata from an ivalid path: '"
					+ localPath + "'.", ActionType.File);

			return new DirectoryIdentity(dirName, localPath);
		}

		public DirectoryContents ReadDirectoryContents(string localPath,
				bool addFilesContents = false) {
			return this.ReadDirectoryContents(ReadDirectoryMetadata(localPath), addFilesContents);
		}

		public DirectoryContents ReadDirectoryContents(DirectoryIdentity did,
				bool addFilesContents = false) {
			if (did.LocalPath == null || did.LocalPath.Equals(EmptyLocalPath))
				return null;

			string[] filePaths = Directory.GetFiles(did.LocalPath);

			DirectoryContents d = new DirectoryContents(did, new List<FileContents>());

			if (filePaths == null || filePaths.Length == 0)
				return null;

			foreach (string path in filePaths) {
				FileContents fc = ReadFileContents(path);
				d.Files.Add(fc);
			}
			return d;
		}

		public bool SaveDirectory(DirectoryContents d) {
			string localPath = d.LocalPath;
			throw new NotImplementedException();
		}

		public bool EraseDirectory(string localPath) {
			throw new NotImplementedException();
		}

		public bool EraseDirectory(DirectoryIdentity d) {
			throw new NotImplementedException();
		}

		public bool EraseDirectory(DirectoryContents d) {
			throw new NotImplementedException();
		}

		#endregion

		#region File (local)

		public bool UploadFile(IFileSyncModel connection, Credentials c, MachineIdentity m,
				DirectoryIdentity d, FileContents f) {
			return connection.AddFile(c, new MachineContents(m), new DirectoryContents(d), f);
		}

		public FileContents DownloadFile(IFileSyncModel connection, Credentials c,
				MachineIdentity m, DirectoryIdentity d, FileIdentity f) {
			throw new NotImplementedException();
		}

		public FileIdentity ReadFileMetadata(string localPath) {
			if (localPath == null)
				throw new ActionException("No file path was provided.", ActionType.File);

			int lastSlash = localPath.LastIndexOf('\\') + 1;
			string dirPath = localPath.Substring(0, lastSlash);
			string fileName = localPath.Substring(lastSlash);

			if (dirPath.Length == 0 || fileName.Length == 0
					|| localPath.Equals(EmptyLocalPath))
				throw new ActionException("Unable to get file metadata from an ivalid path: '"
					+ localPath + "'.", ActionType.File, MemeType.Fuuuuu);

			FileIdentity f = new FileIdentity(fileName, System.IO.File.GetLastWriteTime(localPath));
			return f;
		}

		public FileContents ReadFileContents(string localPath) {
			if (localPath == null)
				throw new ActionException("No file path was provided.", ActionType.File,
					MemeType.Fuuuuu);

			FileIdentity fid = ReadFileMetadata(localPath);

			string contents = String.Empty;
			try {
				StreamReader reader = new StreamReader(localPath);
				contents = reader.ReadToEnd();
				reader.Close();
			} catch (Exception ex) {
				throw new ActionException("Error while reading file data from disk. "
					+ "File path was: '" + localPath + "'.", ActionType.File, ex);
			}

			FileContents f = new FileContents(fid, contents);
			return f;
		}

		public FileContents ReadFileContents(FileIdentity fid, DirectoryIdentity did) {
			return this.ReadFileContents(did.LocalPath + "\\" + fid.Name);
		}

		public bool SaveFile(FileContents f, string localPath) {
			throw new NotImplementedException();
		}

		public bool SaveFile(FileContents f, DirectoryIdentity d) {
			throw new NotImplementedException();
		}

		public bool EraseFile(string localPath) {
			throw new NotImplementedException();
		}

		#endregion

	}
}

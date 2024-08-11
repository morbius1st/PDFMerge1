// Solution:     PDFMerge1
// Project:       AndyFavsAndHistory
// File:             FileListKey.cs
// Created:      2021-01-31 (8:30 AM)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AndyFavsAndHistory.FavHistoryMgr
{
	public enum FileListKeyTypes
	{
		CLASSF_FILE,
		SAMPLE_FILE
	}


	[DataContract(Namespace = "")]
	public class FileListKey : IComparer<FileListKey>, IEquatable<FileListKey>
	{
		private static string[] typeCodes = new [] {"CLSF", "SMPL" };
		private static FileListKey Default = new FileListKey();

		[DataMember(Order = 1)]
		public string ProjectNumber { get; private set; }

		[DataMember(Order = 2)]
		public string TypeCode { get; private set; }

		[DataMember(Order = 3)]
		public string UniqueIndex { get; private set; }

		[IgnoreDataMember]
		public static int uniqueIndex { get; set; }

		private FileListKey()
		{
			ProjectNumber = "";
			TypeCode = "";
			UniqueIndex = "";
		}

		public FileListKey(string projectNumber, FileListKeyTypes typeCode)
		{
			string idx = (uniqueIndex++ ).ToString();

			if (projectNumber == null || projectNumber.Length != 7)
				throw new InvalidEnumArgumentException("FileList Key");

			ProjectNumber = projectNumber;
			TypeCode = typeCodes[(int) typeCode];
			UniqueIndex = idx.PadLeft(7, '.');
		}

		public bool Equals(FileListKey keyTest)
		{
			return this.ToString().Equals(keyTest?.ToString() ?? "");
		}

		public int Compare(FileListKey key, FileListKey keyTest)
		{
			return key.ToString().CompareTo(keyTest?.ToString() ?? "");
		}

		public override bool Equals(object obj)
		{
			return this.Equals((FileListKey) obj ?? Default);
		}

		public override string ToString()
		{
			return ProjectNumber + TypeCode + UniqueIndex;
		}
	}
}
#region using directives

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using StoreAndRead.Annotations;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/9/2021 9:27:51 PM

namespace StoreAndRead2.FavHistoryAdmin
{
	[DataContract(Namespace = "")]
	public class FhAdministrator<T> : INotifyPropertyChanged, 
		IEnumerable<ObservableDictionary<FhKey, T>>
		where T : AFavAndHistValue
	{

	#region private fields

		[DataMember]
		private ObservableDictionary<FhKey, T> fileList;

		private FilePathType pathType;

	#endregion

	#region ctor

		public FhAdministrator()
		{
			fileList = new ObservableDictionary<FhKey, T>();
		}

	#endregion

	#region public properties

		// [DataMember]
		// public ObservableDictionary<FhKey, T> FileList
		// {
		// 	get => fileList;
		// 	set
		// 	{
		// 		fileList = value;
		// 		OnPropertyChange();
		// 	}
		// }

		[IgnoreDataMember]
		public FilePathType PathType
		{
			get => pathType;
			set
			{ 
				pathType = value;
				OnPropertyChange();
			}
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Add(FhKey key, T value)
		{
			fileList.Add(key, value);
		}

		public void Clear()
		{
			fileList.Clear();
		}

		public bool ContainsKey(FhKey key)
		{
			return fileList.ContainsKey(key);
		}

		public bool ContainsValue(T value)
		{
			return fileList.ContainsValue(value);
		}

		public int Count => fileList.Count;

		public KeyValuePair<FhKey, T> this[int idx]
		{
			get => fileList[ idx];
			set => fileList[ idx] = value;
		}

		public bool Remove(FhKey key)
		{
			return fileList.Remove(key);
		}

		public void RemoveAt(int index)
		{
			fileList.RemoveAt(index);
		}

		public bool TryGetValue(FhKey key, out T value)
		{
			return fileList.TryGetValue(key, out value);
		}

		IEnumerator<ObservableDictionary<FhKey, T>> IEnumerable<ObservableDictionary<FhKey, T>>.GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new System.NotImplementedException();
		}


	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is FhAdministrator";
		}


		#endregion
	}
}
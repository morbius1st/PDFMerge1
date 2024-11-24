// Solution:     PDFMerge1
// Project:       ClassifierEditor
// File:             PageProperties.cs
// Created:      2024-11-22 (6:22 AM)

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UtilityLibrary;

namespace AndyShared.MergeSupport
{
	[DataContract(Namespace = "")]
	public class PageProperties : INotifyPropertyChanged, ICloneable
	{
		private bool? provideAnnotation = null;
		private bool? provideEmail = null;
		private bool? provideDisclaimer = null;
		private bool? provideAuthor = null;
		private bool? provideBanner = null;
		private bool? provideXrefLinks = null;
		private string pageFormat = null;
		
		private PageProperties pagePropertiesBasis;

		[DataMember(Order = 10)]
		public string PageFormat
		{
			get
			{
				if (pageFormat.IsVoid()) return pagePropertiesBasis?.pageFormat ?? "ST_30x42";
				return pageFormat;
			}
			set
			{
				if (value == pageFormat) return;
				pageFormat = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 20)]
		public bool ProvideXrefLinks
		{
			get
			{
				if (!provideXrefLinks.HasValue) 
					return pagePropertiesBasis.provideXrefLinks.HasValue ? pagePropertiesBasis.provideXrefLinks.Value : false;

				return provideXrefLinks.Value; 
			}
			set
			{
				if (value == provideXrefLinks) return;
				provideXrefLinks = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 30)]
		public bool ProvideBanner
		{
			get
			{
				if (!provideBanner.HasValue) 
					return pagePropertiesBasis.provideBanner.HasValue ? pagePropertiesBasis.provideBanner.Value : false;

				return provideBanner.Value;
			}
			set
			{
				if (value == provideBanner) return;
				provideBanner = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 40)]
		public bool ProvideAuthor
		{
			get
			{
				if (!provideAuthor.HasValue) 
					return pagePropertiesBasis.provideAuthor.HasValue ? pagePropertiesBasis.provideAuthor.Value : false;

				return provideAuthor.Value;
			}
			set
			{
				if (value == provideAuthor) return;
				provideAuthor = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 50)]
		public bool ProvideDisclaimer
		{
			get
			{
				if (!provideDisclaimer.HasValue) 
					return pagePropertiesBasis.provideDisclaimer.HasValue ? pagePropertiesBasis.provideDisclaimer.Value : false;

				return provideDisclaimer.Value;
			}
			set
			{
				if (value == provideDisclaimer) return;
				provideDisclaimer = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 60)]
		public bool ProvideEmail
		{
			get
			{
				if (!provideEmail.HasValue) 
					return pagePropertiesBasis.provideEmail.HasValue ? pagePropertiesBasis.provideEmail.Value : false;

				return provideEmail.Value;
			}
			set
			{
				if (value == provideEmail) return;
				provideEmail = value;
				OnPropertyChanged();
			}
		}
		
		[DataMember(Order = 70)]
		public bool ProvideAnnotation
		{
			get
			{
				if (!provideAnnotation.HasValue) 
					return pagePropertiesBasis.provideAnnotation.HasValue ? pagePropertiesBasis.provideAnnotation.Value : false;

				return provideAnnotation.Value;
			}
			set
			{
				if (value == provideAnnotation) return;
				provideAnnotation = value;
				OnPropertyChanged();
			}
		}

		public PageProperties PagePropertiesBasis
		{
			get => pagePropertiesBasis;
			set
			{
				if (Equals(value, pagePropertiesBasis)) return;
				pagePropertiesBasis = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[DebuggerStepThrough]
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public object Clone()
		{
			PageProperties p = new PageProperties();

			p.pageFormat = pageFormat;
			p.provideXrefLinks = provideXrefLinks;
			p.provideAuthor = provideAuthor;
			p.provideBanner = provideBanner;
			p.provideDisclaimer = provideDisclaimer;
			p.provideAnnotation = provideAnnotation;
			p.provideEmail = provideEmail;

			return p;
		}
	}
}
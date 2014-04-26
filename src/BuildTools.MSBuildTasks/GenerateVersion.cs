//
// Copyright (c) 2008-2012 Wouter Demuynck
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Globalization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTools.MSBuildTasks
{
	/// <summary>
	/// Generates a <see cref="Version"/> numbers using a specified build and revision numbering
	/// strategy.
	/// </summary>
	public class GenerateVersion : Task
	{
		/// <summary>
		/// Executes the task.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the task was executed successfully; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Execute()
		{
			BuildNumberType buildType = (BuildNumberType)Enum.Parse(typeof(BuildNumberType), BuildType, true);
			RevisionNumberType revisionType = (RevisionNumberType)Enum.Parse(typeof(RevisionNumberType), RevisionType, true);
			DateTime startingDate = DateTime.Parse(StartingDate, CultureInfo.InvariantCulture);

			Version version = new Version(Major, Minor);
			if (string.IsNullOrWhiteSpace(VersionFile))
			{
				Log.LogMessage(MessageImportance.Low, "Creating version without using a version file.");
				version = VersionNumberGenerator.GenerateVersion(new Version(Major, Minor), buildType, revisionType, startingDate);
			}
			else
			{
				version = VersionNumberGenerator.GenerateVersion(VersionFile, Major, Minor, buildType, revisionType, startingDate);
			}

			Version = version.ToString();
			Major = version.Major;
			Minor = version.Minor;
			Build = version.Build;
			Revision = version.Revision;
			Log.LogMessage("Version number generated: {0}", version);
			return true;
		}

		/// <summary>
		/// Gets or sets the file system location of the file used to load and store the previously
		/// generated version number. This property can be empty when using build and revision numbering
		/// algorithms that are not based on the previous version number.
		/// </summary>
		public string VersionFile
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the major version number.
		/// </summary>
		[Output]
		public int Major
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the minor version number.
		/// </summary>
		[Output]
		public int Minor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the generated build number.
		/// </summary>
		[Output]
		public int Build
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the generated revision number.
		/// </summary>
		[Output]
		public int Revision
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a <see cref="string"/> corresponding to one of the <see cref="BuildNumberType"/>
		/// enumeration values used for selecting a build numbering strategy.
		/// </summary>
		[Required]
		public string BuildType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a <see cref="string"/> corresponding to one of the <see cref="RevisionNumberType"/>
		/// enumeration values used for selecting a revision numbering strategy.
		/// </summary>
		[Required]
		public string RevisionType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a <see cref="string"/> representing the starting date and time of the project, as
		/// used by some versioning strategies.
		/// </summary>
		[Required]
		public string StartingDate
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a string representation of the generated version number.
		/// </summary>
		[Output]
		public string Version
		{
			get;
			private set;
		}
	}
}
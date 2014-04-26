﻿//
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
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace BuildTools
{
	/// <summary>
	/// Provides methods for generating <see cref="Version"/> numbers using different algorithms
	/// for determining the build and revision numbers.
	/// </summary>
	public static class VersionNumberGenerator
	{
        private static readonly BuildNumberType[] BuildNumberTypesRequiringVersionFile = new[] { BuildNumberType.Fixed, BuildNumberType.Increment };
        private static readonly RevisionNumberType[] RevisionNumberTypesRequiringVersionFile = new[] { RevisionNumberType.Fixed, RevisionNumberType.Increment, RevisionNumberType.BuildIncrement };

		/// <summary>
		/// Generates a new version number using the specified major and minor version numbers
		/// and calculating the build number and revision number using the specified algorithms.
		/// The current version number loaded from the specified file name is used by some algorithms
		/// to increment the build and revision numbers, and the new version is saved in this file after
		/// generating the new values.
		/// </summary>
		/// <param name="versionFile">The location of the file in which the current version number is stored. This file will be overwritten with the newly generated version number.</param>
		/// <param name="buildType">The <see cref="BuildNumberType"/> specifying the algorithm used to calculate the build number.</param>
		/// <param name="revisionType">The <see cref="RevisionNumberType"/> specifying the algorithm used to calculate the revision number.</param>
		public static Version GenerateVersion(string versionFile, BuildNumberType buildType, RevisionNumberType revisionType)
		{
			return GenerateVersion(versionFile, buildType, revisionType, DateTime.UtcNow);
		}

		/// <summary>
		/// Generates a new version number using the specified major and minor version numbers
		/// and calculating the build number and revision number using the specified algorithms.
		/// The current version number loaded from the specified file name is used by some algorithms
		/// to increment the build and revision numbers, and the new version is saved in this file after
		/// generating the new values.
		/// </summary>
		/// <param name="versionFile">The location of the file in which the current version number is stored. This file will be overwritten with the newly generated version number.</param>
		/// <param name="buildType">The <see cref="BuildNumberType"/> specifying the algorithm used to calculate the build number.</param>
		/// <param name="revisionType">The <see cref="RevisionNumberType"/> specifying the algorithm used to calculate the revision number.</param>
		/// <param name="startingDate">The starting date from which to derive the new version number. This parameter is ignored by some algorithms.</param>
		public static Version GenerateVersion(string versionFile, BuildNumberType buildType, RevisionNumberType revisionType, DateTime startingDate)
        {
            return GenerateVersion(versionFile, 1, 0, buildType, revisionType, startingDate);
		}

        /// <summary>
        /// Generates a new version number using the specified major and minor version numbers
        /// and calculating the build number and revision number using the specified algorithms.
        /// The current version number loaded from the specified file name is used by some algorithms
        /// to increment the build and revision numbers, and the new version is saved in this file after
        /// generating the new values.
        /// </summary>
        /// <param name="versionFile">The location of the file in which the current version number is stored. This file will be overwritten with the newly generated version number.</param>
        /// <param name="majorVersion">The major version for the newly generated version number.</param>
        /// <param name="minorVersion">The minor version for the newly generated version number.</param>
        /// <param name="buildType">The <see cref="BuildNumberType"/> specifying the algorithm used to calculate the build number.</param>
        /// <param name="revisionType">The <see cref="RevisionNumberType"/> specifying the algorithm used to calculate the revision number.</param>
        /// <param name="startingDate">The starting date from which to derive the new version number. This parameter is ignored by some algorithms.</param>
        public static Version GenerateVersion(string versionFile, int majorVersion, int minorVersion, BuildNumberType buildType, RevisionNumberType revisionType, DateTime startingDate)
        {
            Version currentVersion = LoadVersionFromFile(versionFile, majorVersion, minorVersion, buildType, revisionType);
            Version newVersion = GenerateVersion(currentVersion, buildType, revisionType, startingDate);
           
            if (RequiresVersionFile(buildType, revisionType))
            {
                SaveVersionToFile(versionFile, newVersion);
            }
            return newVersion;
        }

        /// <summary>
        /// Generates a new version number using the specified major and minor version numbers
        /// and calculating the build number and revision number using the specified algorithms.
        /// </summary>
        /// <param name="majorVersion">The major version for the newly generated version number.</param>
        /// <param name="minorVersion">The minor version for the newly generated version number.</param>
        /// <param name="buildType">The <see cref="BuildNumberType"/> specifying the algorithm used to calculate the build number.</param>
        /// <param name="revisionType">The <see cref="RevisionNumberType"/> specifying the algorithm used to calculate the revision number.</param>
        /// <param name="startingDate">The starting date from which to derive the new version number. This parameter may be ignored by some algorithms.</param>
        public static Version GenerateVersion(int majorVersion, int minorVersion, BuildNumberType buildType, RevisionNumberType revisionType, DateTime startingDate)
        {
            return GenerateVersion(new Version(majorVersion, minorVersion), buildType, revisionType, startingDate);
        }

		/// <summary>
		/// Generates a new version number using the specified major and minor version numbers
		/// and calculating the build number and revision number using the specified algorithms.
		/// </summary>
		/// <param name="currentVersion">The current version from which to derive the new version number.</param>
		/// <param name="buildType">The <see cref="BuildNumberType"/> specifying the algorithm used to calculate the build number.</param>
		/// <param name="revisionType">The <see cref="RevisionNumberType"/> specifying the algorithm used to calculate the revision number.</param>
		/// <param name="startingDate">The starting date from which to derive the new version number. This parameter may be ignored by some algorithms.</param>
		public static Version GenerateVersion(Version currentVersion, BuildNumberType buildType, RevisionNumberType revisionType, DateTime startingDate)
		{
			int build = CalculateBuildNumber(startingDate, currentVersion.Build, buildType);
			int revision = CalculateRevisionNumber(startingDate, currentVersion.Revision, revisionType, build != currentVersion.Build);

			// Correct any overflows that might occur with some algorithms.
			while (build > ushort.MaxValue) build -= ushort.MaxValue;
			while (revision > ushort.MaxValue) revision -= ushort.MaxValue;

			// Generate the return value.
			return new Version(currentVersion.Major, currentVersion.Minor, build, revision);
		}

		private static int CalculateBuildNumber(DateTime startingDate, int currentBuild, BuildNumberType buildType)
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;

			switch (buildType)
			{
				case BuildNumberType.Fixed:
					return Math.Max(currentBuild, 0);

				case BuildNumberType.Increment:
					return Math.Max(currentBuild + 1, 0);

				case BuildNumberType.YearMonthDay:
					int years = now.YearsSince(startingDate);
					while (years >= 7) years -= 7;

					return years * 10000 + int.Parse(now.ToString("MMdd"), CultureInfo.InvariantCulture);

				case BuildNumberType.MonthDay:
					return now.MonthsSince(startingDate) * 100 + now.Day;

				case BuildNumberType.BuildDay:
					return now.YearsSince(startingDate) * 1000 + now.DayOfYear;
			}

			throw new InvalidEnumArgumentException("buildType", (int)buildType, typeof(BuildNumberType));
		}

		private static int CalculateRevisionNumber(DateTimeOffset startingDate, int currentRevision, RevisionNumberType revisionType, bool buildChanged)
		{
			switch (revisionType)
			{
				case RevisionNumberType.Fixed:
					return Math.Max(currentRevision, 0);

				case RevisionNumberType.Increment:
					return Math.Max(currentRevision + 1, 0);

				case RevisionNumberType.BuildIncrement:
					return (buildChanged ? 0 : Math.Max(currentRevision + 1, 0));

				case RevisionNumberType.HourMinute:
					return int.Parse(DateTime.UtcNow.ToString("HHmm", CultureInfo.InvariantCulture));

				case RevisionNumberType.DaySecond:
					return (int)(DateTime.UtcNow.TimeOfDay.Seconds / 10d);

				case RevisionNumberType.DayFraction:
					const double Fraction = short.MaxValue / (24d * 3600d);
					return (int)(DateTime.UtcNow.TimeOfDay.Seconds * Fraction);
			}

			throw new InvalidEnumArgumentException("revisionType", (int)revisionType, typeof(RevisionNumberType));
		}

        private static Version LoadVersionFromFile(string versionFile, int majorVersion, int minorVersion, BuildNumberType buildType, RevisionNumberType revisionType)
        {
            if (RequiresVersionFile(buildType, revisionType))
            {
                if (string.IsNullOrWhiteSpace(versionFile)) throw new ArgumentNullException("versionFile");

                if (File.Exists(versionFile))
                {
                    Version version = new Version(File.ReadAllText(versionFile));
                    return new Version(majorVersion, minorVersion, version.Build, version.Revision);
                }
            }

            return new Version(majorVersion, minorVersion);
        }

        private static bool RequiresVersionFile(BuildNumberType buildType, RevisionNumberType revisionType)
        {
            return BuildNumberTypesRequiringVersionFile.Contains(buildType) ||
                RevisionNumberTypesRequiringVersionFile.Contains(revisionType);
        }

		private static void SaveVersionToFile(string path, Version version)
		{
			File.WriteAllText(path, version.ToString());
		}
    }
}

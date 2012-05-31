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
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTools.MSBuildTasks
{
	/// <summary>
	/// Generates a source file containing assembly metadata attributes.
	/// </summary>
	public sealed class GenerateAssemblyInfo : Task
	{
		/// <summary>
		/// Executes the task.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the task was executed successfully; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Execute()
		{
			CodeDomProvider provider = CodeDomProvider.CreateProvider(Language);
			AssemblyInfoGenerator generator = new AssemblyInfoGenerator(CodeDomProvider.CreateProvider(Language));

			if (CLSCompliant)
			{
				Log.LogMessage("Adding CLSCompliantAttribute.");
				generator.AddCLSCompliant(true);
			}

			if (!string.IsNullOrWhiteSpace(AssemblyVersion))
			{
				Log.LogMessage("Adding AssemblyVersionAttribute ({0}).", AssemblyVersion);
				generator.AddAssemblyVersion(new Version(AssemblyVersion));
			}

			if (!string.IsNullOrWhiteSpace(AssemblyFileVersion))
			{
				Log.LogMessage("Adding AssemblyFileVersionAttribute ({0}).", AssemblyFileVersion);
				generator.AddAssemblyFileVersion(new Version(AssemblyFileVersion));
			}

			if (!string.IsNullOrWhiteSpace(AssemblyInformationalVersion))
			{
				Log.LogMessage("Adding AssemblyInformationalVersionAttribute ({0}).", AssemblyInformationalVersion);
				generator.AddAssemblyInformationalVersion(AssemblyInformationalVersion);
			}

			generator.Save(FileName);
			Log.LogMessage("AssemblyInfo file saved to '{0}'.", FileName);

			return true;
		}

		/// <summary>
		/// Gets or sets the name of the language in which the source code is generated.
		/// </summary>
		/// <remarks>
		/// The value of this property is used to create a <see cref="CodeDomProvider"/> instance. For
		/// more information, see the <see cref="CodeDomProvider.CreateProvider(string)"/> method.
		/// </remarks>
		[Required]
		public string Language
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the file name of the generated source code file.
		/// </summary>
		[Required]
		public string FileName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the <see cref="Version"/> string used for the <see cref="AssemblyVersionAttribute"/>
		/// attribute.
		/// </summary>
		public string AssemblyVersion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the <see cref="Version"/> string used for the <see cref="AssemblyFileVersionAttribute"/>
		/// attribute.
		/// </summary>
		public string AssemblyFileVersion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the string used for the <see cref="AssemblyInformationalVersionAttribute"/> attribute.
		/// </summary>
		public string AssemblyInformationalVersion
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not the assembly is marked as CLS-compliant.
		/// </summary>
		public bool CLSCompliant
		{
			get;
			set;
		}
	}
}
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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace BuildTools
{
	/// <summary>
	/// Provides methods for generating assembly information source files.
	/// </summary>
	public sealed class AssemblyInfoGenerator
	{
		private CodeDomProvider provider;
		private CodeCompileUnit unit;

		private CodeAttributeDeclaration assemblyVersionAttribute;
		private CodeAttributeDeclaration assemblyFileVersionAttribute;
		private CodeAttributeDeclaration assemblyInformationalVersionAttribute;
		private CodeAttributeDeclaration clsCompliantAttribute;

		private bool isInitialized;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyInfoGenerator"/> class.
		/// </summary>
		/// <param name="provider">
		/// The <see cref="CodeDomProvider"/> used to generate the source code.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when the <paramref name="provider"/> argument is <see langword="null"/>.
		/// </exception>
		public AssemblyInfoGenerator(CodeDomProvider provider)
		{
			if (provider == null) throw new ArgumentNullException("provider");

			this.provider = provider;
		}

		/// <summary>
		/// Adds the <see cref="CLSCompliantAttribute"/> to the generated assembly information source code.
		/// </summary>
		/// <param name="isCompliant">
		/// <see langword="true"/> if the assembly is marked as CLS-compliant; otherwise, <see langword="false"/>.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the <see cref="CLSCompliantAttribute"/> has already been added by calling this method.
		/// </exception>
		public void AddCLSCompliant(bool isCompliant)
		{
			EnsureInitialized();

			if (clsCompliantAttribute != null)
				throw new InvalidOperationException("The CLSCompliantAttribute declaration has already been added.");

			clsCompliantAttribute = new CodeAttributeDeclaration(
				new CodeTypeReference(typeof(CLSCompliantAttribute)),
				new CodeAttributeArgument(new CodePrimitiveExpression(isCompliant)));

			unit.AssemblyCustomAttributes.Add(clsCompliantAttribute);
		}

		/// <summary>
		/// Adds the <see cref="AssemblyVersionAttribute"/> to the generated assembly information source code.
		/// </summary>
		/// <param name="version">
		/// The <see cref="Version"/> specified by the <see cref="AssemblyVersionAttribute"/>.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the <see cref="AssemblyVersionAttribute"/> has already been added by calling this method.
		/// </exception>
		public void AddAssemblyVersion(Version version)
		{
			EnsureInitialized();

			if (assemblyVersionAttribute != null)
				throw new InvalidOperationException("The AssemblyVersionAttribute declaration has already been added.");

			assemblyVersionAttribute = new CodeAttributeDeclaration(
					new CodeTypeReference(typeof(AssemblyVersionAttribute)),
					new CodeAttributeArgument(new CodePrimitiveExpression(version.ToString())));

			unit.AssemblyCustomAttributes.Add(assemblyVersionAttribute);
		}

		/// <summary>
		/// Adds the <see cref="AssemblyFileVersionAttribute"/> to the generated assembly information source code.
		/// </summary>
		/// <param name="version">
		/// The <see cref="Version"/> specified by the <see cref="AssemblyFileVersionAttribute"/>.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the <see cref="AssemblyFileVersionAttribute"/> has already been added by calling this method.
		/// </exception>
		public void AddAssemblyFileVersion(Version version)
		{
			EnsureInitialized();

			if (assemblyFileVersionAttribute != null)
				throw new InvalidOperationException("The AssemblyFileVersionAttribute declaration has already been added.");

			assemblyFileVersionAttribute = new CodeAttributeDeclaration(
					new CodeTypeReference(typeof(AssemblyFileVersionAttribute)),
					new CodeAttributeArgument(new CodePrimitiveExpression(version.ToString())));

			unit.AssemblyCustomAttributes.Add(assemblyFileVersionAttribute);
		}

		/// <summary>
		/// Adds the <see cref="AssemblyInformationalVersionAttribute"/> to the generated assembly information source code.
		/// </summary>
		/// <param name="version">
		/// The version specified by the <see cref="AssemblyInformationalVersionAttribute"/>.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the <see cref="AssemblyInformationalVersionAttribute"/> has already been added by calling this method.
		/// </exception>
		public void AddAssemblyInformationalVersion(string version)
		{
			EnsureInitialized();

			if (assemblyInformationalVersionAttribute != null)
				throw new InvalidOperationException("The AssemblyInformationalVersionAttribute declaration has already been added.");

			assemblyInformationalVersionAttribute = new CodeAttributeDeclaration(
				new CodeTypeReference(typeof (AssemblyInformationalVersionAttribute)),
				new CodeAttributeArgument(new CodePrimitiveExpression(version)));

			unit.AssemblyCustomAttributes.Add(assemblyInformationalVersionAttribute);
		}

		/// <summary>
		/// Saves the generated assembly information source code to the specified location.
		/// </summary>
		/// <param name="path">
		/// The file system location used to store the generated assembly information source code.
		/// </param>
		public void Save(string path)
		{
			EnsureInitialized();

			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.IndentString = "    ";
			options.BlankLinesBetweenMembers = true;

			using (StreamWriter writer = File.CreateText(path))
				provider.GenerateCodeFromCompileUnit(unit, writer, options);
		}

		private void EnsureInitialized()
		{
			if (!isInitialized)
			{
				// Set up the code compile unit and namespace imports.
				unit = new CodeCompileUnit();
				unit.Namespaces.Add(new CodeNamespace());
				unit.Namespaces[0].Imports.Add(new CodeNamespaceImport("System"));
				unit.Namespaces[0].Imports.Add(new CodeNamespaceImport("System.Reflection"));

				isInitialized = true;
			}
		}
	}
}
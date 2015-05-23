using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BuildTools.Tests
{
    public class AssemblyInfoBuilderTests
    {
        [Fact]
        public void Ctor_WithNullProvider_ThrowsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AssemblyInfoBuilder(null));
        }

        [Fact]
        public void Ctor_WithNonNullProvider_DoesNotThrow()
        {
            new AssemblyInfoBuilder(new CSharpCodeProvider());
        }

        [Fact]
        public void WithCLSCompliant_WithTrue_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithCLSCompliant(true);
            var assemblyInfo = builder.Build();
            
            Assert.Contains("[assembly: System.CLSCompliantAttribute(true)]", assemblyInfo);
        }

        [Fact]
        public void WithCLSCompliant_WithFalse_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithCLSCompliant(false);
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.CLSCompliantAttribute(false)]", assemblyInfo);
        }

        [Fact]
        public void WithCLSCompliant_CanBeCalledMoreThanOnce()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder
                .WithCLSCompliant(true)
                .WithCLSCompliant(false);
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.CLSCompliantAttribute(false)]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyVersion_WithVersion_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithAssemblyVersion(new Version(4, 3, 2, 1));
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyVersionAttribute(\"4.3.2.1\")]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyVersion_CanBeCalledMoreThanOnce()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder
                .WithAssemblyVersion(new Version(4, 3, 2, 1))
                .WithAssemblyVersion(new Version(3, 0, 0, 0));
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyVersionAttribute(\"3.0.0.0\")]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyFileVersion_WithVersion_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithAssemblyFileVersion(new Version(4, 3, 2, 1));
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyFileVersionAttribute(\"4.3.2.1\")]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyFileVersion_CanBeCalledMoreThanOnce()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder
                .WithAssemblyFileVersion(new Version(4, 3, 2, 1))
                .WithAssemblyFileVersion(new Version(1, 2, 3, 4));
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyFileVersionAttribute(\"1.2.3.4\")]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyInformationalVersion_WithVersion_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithAssemblyInformationalVersion("4.3.2.1 Beta 4");
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyInformationalVersionAttribute(\"4.3.2.1 Beta 4\")]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyInformationalVersion_WithSpecialChars_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithAssemblyInformationalVersion("4.3.2.1 code-named \"Maverick\"");
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyInformationalVersionAttribute(\"4.3.2.1 code-named \\\"Maverick\\\"\")]", assemblyInfo);
        }

        [Fact]
        public void WithAssemblyInformationalVersion_CanBeCalledMoreThanOnce()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder
                .WithAssemblyInformationalVersion("4.3.2.1 code-named \"Maverick\"")
                .WithAssemblyInformationalVersion("3.0 code-named \"Maverick\"");
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyInformationalVersionAttribute(\"3.0 code-named \\\"Maverick\\\"\")]", assemblyInfo);
        }
    }
}

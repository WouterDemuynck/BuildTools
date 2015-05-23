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
        public void WithAssemblyVersion_WithVersion_BuildsCorrectAssemblyInfo()
        {
            var builder = new AssemblyInfoBuilder(new CSharpCodeProvider());
            builder.WithAssemblyVersion(new Version(1, 0));
            var assemblyInfo = builder.Build();

            Assert.Contains("[assembly: System.Reflection.AssemblyVersionAttribute(\"1.0\")]", assemblyInfo);
        }
    }
}

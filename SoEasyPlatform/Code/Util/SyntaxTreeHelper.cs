using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoEasyPlatform 
{
    public class SyntaxTreeHelper
    {
        public static Type GetModelTypeByClass(string classString, string typeName)
        {
            var typeFirstChar = typeName.FirstOrDefault() + "";
            if (Regex.IsMatch(typeFirstChar,@"^\d$"))
            {
                throw new Exception("类的首字母不能是数字");
            }
            //Write("Parsing the code into the SyntaxTree");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(classString);

            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                //typeof(AccessViolationException).Assembly.Location,
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
                typeof(SqlSugar.SqlSugarClient).Assembly.Location
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();


            //Write("Adding the following references");
            //foreach (var r in refPaths)
            // Write(r);

            //Write("Compiling ...");
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    //Write("Compilation failed!");
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    string message = "";
                    foreach (Diagnostic diagnostic in failures)
                    {
                        message += diagnostic.GetMessage();
                    }
                    throw new Exception("解析实体类出错，请检查命名" + message +" \r\n "+classString);
                }
                else
                {
                    //Write("Compilation successful! Now instantiating and executing the code ...");
                    ms.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    var type = assembly.GetType("RoslynCompileSampleDemo." + typeName);
                    //Console.WriteLine(type.Name);
                    return type;
                }
            }
        }

        internal static string TemplateString = @"using System;
namespace  RoslynCompileSampleDemo
{

    public class @(Model.ClassName)
    {
@foreach (var item in Model.PropertyGens)
{

    @: public @item.Type @item.PropertyName { get; set; }
}
    }
}
";
    }
}

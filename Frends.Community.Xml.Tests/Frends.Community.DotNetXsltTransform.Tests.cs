using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Frends.Community.DotNetXsltTransform.Tests
{
    [TestFixture]
    public class XsltTests
    {
        private readonly string _testFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\XsltTestFiles");
        readonly TransformInput _input = new TransformInput();

        [Test]
        public void TestXsltWithDotNet()
        {

            _input.Document = GetTestContent(_testFilesPath + "\\Catalog.xml");
            _input.Stylesheet = GetTestContent(_testFilesPath + "\\CatalogMap.xslt");

            var expected = GetTestContent(_testFilesPath + "\\CatalogResult.txt");
            var ret = TransformData.DotNetXsltTransform(_input, null);
            Assert.That(RemoveWhiteSpace(ret.Result), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
        }

        [Test]
        public void TestXsltWithParametersWithDotNet()
        {
            var parameters = new[]
            {
                new TransformParameters {Name = "testParameter", Value = "Testing the Value of the parameter."}
            };

            _input.Document = GetTestContent(_testFilesPath + "\\Catalog.xml");
            _input.Stylesheet = GetTestContent(_testFilesPath + "\\ParameterTestsMap.xslt");

            var expected = GetTestContent(_testFilesPath + "\\ParameterTestResult.xml");
            var ret = TransformData.DotNetXsltTransform(_input, parameters);

            Assert.That(RemoveWhiteSpace(ret.Result), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
        }

        [Test]
        public void TestXsltWithcSharpWithDotNet()
        {
            _input.Document = GetTestContent(_testFilesPath + "\\CSharpTestInput.xml");
            _input.Stylesheet = GetTestContent(_testFilesPath + "\\CSharpTestMap.xsl");

            var expected = GetTestContent(_testFilesPath + "\\CSharpTestResult.xml");
            var ret = TransformData.DotNetXsltTransform(_input, null);
            Assert.That(RemoveWhiteSpace(ret.Result), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
        }

        [TearDown]
        public void Cleanup()
        {
            var files = Directory.GetFiles(_testFilesPath, "temp *");
            files.ToList().ForEach(File.Delete);
        }

        public string GetTestContent(string path)
        {
            return File.ReadAllText(path);
        }
        public static string RemoveWhiteSpace(string str)
        {
            return Regex.Replace(str, @"\s+", string.Empty);
        }
    }
}
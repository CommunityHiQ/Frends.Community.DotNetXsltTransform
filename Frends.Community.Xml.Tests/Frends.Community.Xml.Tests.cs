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
        private readonly string testFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\XsltTestFiles");
        TransformInput input = new TransformInput();
        TransformParameters param = new TransformParameters();

        [Test]
        public void ShouldThrowFormatException()
        {
                input.document = GetTestContent(testFilesPath + "\\Catalog.xml");
                input.stylesheet = GetTestContent(testFilesPath + "\\CatalogTransform.xslt");
            var expected2 = GetTestContent(testFilesPath + "\\TransformedCatalog.txt");

            try
            {
                var result = TransformData.DotNetXsltTransform(input,null);
            }
            catch (FormatException ex)
            {
                Assert.AreEqual("Unsupported input type. The supported types are XmlDocument and String.", ex.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [Test]
        public void TestXsltWithDotNet()
        {
            
            input.document = GetTestContent(testFilesPath + "\\Catalog.xml");
            input.stylesheet = GetTestContent(testFilesPath + "\\CatalogTransform.xslt");

            var expected = GetTestContent(testFilesPath + "\\TransformedCatalog.txt").ToString();
            var ret = TransformData.DotNetXsltTransform(input, null);
            Assert.That(RemoveWhiteSpace(ret.result), Is.EqualTo(RemoveWhiteSpace(expected).ToString()).IgnoreCase);
        }

        [Test]
        public void TestXslt20_WithParametersWithDotNet()
        {
            // This test doesn't really need xslt 2.0 features, insted it just test that xsl parameters are working
            var parameters = new[]
            {
                new TransformParameters {name = "testParameter", value = "Testing the value of the parameter."}
            };

            input.document = GetTestContent(testFilesPath + "\\Catalog.xml");
            input.stylesheet = GetTestContent(testFilesPath + "\\Xslt20ParameterTests.xslt");

            var expected = GetTestContent(testFilesPath + "\\Xslt20ParameterTestResult.xml");
            var ret = TransformData.DotNetXsltTransform(input, parameters);

            Assert.That(RemoveWhiteSpace(ret.result), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
        }
     
        [Test]
        public void TestXslt10_WithcSharpWithDotNet()
        {
            input.document = GetTestContent(testFilesPath + "\\Xslt10TestCSharp_input.xml");
            input.stylesheet = GetTestContent(testFilesPath + "\\Xslt10TestCSharp.xsl");
            var expected = GetTestContent(testFilesPath + "\\Xslt10TestCSharp_transformed.xml");
            var ret = TransformData.DotNetXsltTransform(input, null);
            Assert.That(RemoveWhiteSpace(ret.result), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
        }

        [TearDown]
        public void Cleanup()
        {
            var files = Directory.GetFiles(testFilesPath, "temp *");
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
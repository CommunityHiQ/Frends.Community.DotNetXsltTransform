using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Frends.Community.Xml.Tests
{
    [TestFixture]
    public class XsltTests
    {
        private readonly string testFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\XsltTestFiles");
        Input input = new Input();
        Parameters param = new Parameters();

        [Test]
        public void ShouldThrowFormatException()
        {
                input.Xml = GetTestContent(testFilesPath + "\\Catalog.xml");
                input.Xslt = GetTestContent(testFilesPath + "\\CatalogTransform.xslt");
            var expected2 = GetTestContent(testFilesPath + "\\TransformedCatalog.txt");

            try
            {
                var result = TransformData.XsltTransform(input,null, CancellationToken.None);
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
            
            input.Xml = GetTestContent(testFilesPath + "\\Catalog.xml");
            input.Xslt = GetTestContent(testFilesPath + "\\CatalogTransform.xslt");

            var expected = GetTestContent(testFilesPath + "\\TransformedCatalog.txt").ToString();
            var result = TransformData.XsltTransform(input, null, CancellationToken.None);
            Assert.That(RemoveWhiteSpace(result.Xml), Is.EqualTo(RemoveWhiteSpace(expected).ToString()).IgnoreCase);
        }

        [Test]
        public void TestXslt20_WithParametersWithDotNet()
        {
            // This test doesn't really need xslt 2.0 features, insted it just test that xsl parameters are working
            var parameters = new[]
            {
                new Parameters {Name = "testParameter", Value = "Testing the value of the parameter."}
            };

            input.Xml = GetTestContent(testFilesPath + "\\Catalog.xml");
            input.Xslt = GetTestContent(testFilesPath + "\\Xslt20ParameterTests.xslt");

            var expected = GetTestContent(testFilesPath + "\\Xslt20ParameterTestResult.xml");
            var result = TransformData.XsltTransform(input, parameters, CancellationToken.None);

            Assert.That(RemoveWhiteSpace(result.Xml), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
        }
     
        [Test]
        public void TestXslt10_WithcSharpWithDotNet()
        {
            input.Xml = GetTestContent(testFilesPath + "\\Xslt10TestCSharp_input.xml");
            input.Xslt = GetTestContent(testFilesPath + "\\Xslt10TestCSharp.xsl");
            var expected = GetTestContent(testFilesPath + "\\Xslt10TestCSharp_transformed.xml");
            var result = TransformData.XsltTransform(input, null, CancellationToken.None);
            Assert.That(RemoveWhiteSpace(result.Xml), Is.EqualTo(RemoveWhiteSpace(expected)).IgnoreCase);
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
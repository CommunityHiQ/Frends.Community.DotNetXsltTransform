using Frends.Tasks.Attributes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Xsl;

#pragma warning disable 1591

namespace Frends.Community.DotNetXsltTransform
{
    public class TransformInput
    {
        /// <summary>
        /// Input xml document as XmlDocument or xml string.
        /// </summary>
        [DefaultDisplayType(DisplayType.Text)]
        public string document { get; set; }


        /// <summary>
        /// Xsl style sheet for transform as string.
        /// </summary>
        [DefaultDisplayType(DisplayType.Text)]
        public string stylesheet { get; set; }
    }

    [DisplayName("Xslt parameters")]
    public class TransformParameters 
    {   

        /// <summary>
        /// Xslt parameter name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Xslt parameter value
        /// </summary>
        public string value { get; set; }
    }

    public class TransformResult
    {
        /// <summary>
        /// String that contains output from transformation. It might be xml, csv or something else, depending on transformation
        /// </summary>
        public string result { get; set; }

    }

    public class TransformData
    {
        /// <summary>
        /// Task for xslt transforms using .Net.
        /// This task supports only xslt 1.0 transforms, but it supports C# scripts inside xsl file. If you don't need support for it is propably better to use Xml.Transform task.
        /// </summary>
        /// <param name="input">xml document or string</param>
        /// <param name="XSLTParameters">Array of KeyValuePairs: name and value</param>
        /// <returns>Object{string}</returns>
        public static TransformResult DotNetXsltTransform(TransformInput input, TransformParameters[] XSLTParameters)
        {

            var result = new TransformResult();
            var xmlString = "";

            if (input.document.GetType() == typeof(string))
            {
                xmlString = input.document;
            }
            else if (input.document.GetType() == typeof(XmlDocument))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(input.document);

                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xmlString = stringWriter.GetStringBuilder().ToString();
                }
            }
            else
            {
                throw new FormatException("Unsupported input type. The supported types are XmlDocument and String.");
            }

            result.result = DotNetXsltTransformHelper(xmlString, input.stylesheet, XSLTParameters);
            return result;
        }
        private static String DotNetXsltTransformHelper(string xmlString, string XSLT, TransformParameters[] XSLTParameters)
        {
            using (var stringReader = new StringReader(XSLT))
            {

                var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

                using (XmlReader reader = XmlReader.Create(new StringReader(XSLT)), Inputdocument = XmlReader.Create(new StringReader(xmlString)))
                {
                    XsltSettings xslt_settings = new XsltSettings(true, true);

                    XslCompiledTransform myXslTransform = new XslCompiledTransform();
                    myXslTransform.Load(reader, xslt_settings, new XmlUrlResolver());

                    XsltArgumentList argsList = new XsltArgumentList();
                    if (XSLTParameters != null)

                        XSLTParameters.ToList().ForEach(x => argsList.AddParam(x.name, "", x.value));

                    MemoryStream memoryStream = new MemoryStream();
                    using (XmlWriter xmlTextWriter = XmlWriter.Create(memoryStream, myXslTransform.OutputSettings))
                    {
                        myXslTransform.Transform(Inputdocument, argsList, xmlTextWriter);
                        Encoding UTF8WithoutBom = myXslTransform.OutputSettings.Encoding; // Encoding.UTF8;

                        var output = UTF8WithoutBom.GetString(memoryStream.ToArray());
                        output = output.Replace("\n", Environment.NewLine);
                        output = output.Trim(new char[] { '\uFEFF' }); // removu utf-16 bom
                        return output;
                    }
                }
            }
        }
    }
}


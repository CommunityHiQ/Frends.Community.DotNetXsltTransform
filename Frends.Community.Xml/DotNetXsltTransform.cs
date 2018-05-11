using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;

#pragma warning disable 1591

namespace Frends.Community.DotNetXsltTransform
{
    public class TransformInput
    {
        /// <summary>
        /// Input xml Document as XmlDocument, xml string or file location when using IsFile.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Document { get; set; }

        /// <summary>
        /// Input is file. Enables streaming for big files.
        /// </summary>
        public bool IsFile { get; set; }

        /// <summary>
        /// Xsl style sheet for transform as string.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Stylesheet { get; set; }
    }

    [DisplayName("Xslt parameters")]
    public class TransformParameters
    {
        /// <summary>
        /// Xslt parameter Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Xslt parameter Value
        /// </summary>
        public string Value { get; set; }
    }

    public class TransformResult
    {
        /// <summary>
        /// String that contains output from transformation. It might be xml, csv or something else, depending on transformation
        /// </summary>
        public string Result { get; set; }

    }

    public class TransformData
    {
        /// <summary>
        /// Task for xslt transforms using .Net.
        /// This task supports only xslt 1.0 transforms, but it supports C# scripts inside xsl file. If you don't need support for it is propably better to use Xml.Transform task.
        /// </summary>
        /// <param name="input">xml Document or string</param>
        /// <param name="xsltParameters">Array of KeyValuePairs: Name and Value</param>
        /// <returns>Object{string}</returns>
        public static TransformResult DotNetXsltTransform(TransformInput input, TransformParameters[] xsltParameters)
        {
            var result = new TransformResult();
            string xmlString;
            if (input.IsFile || input.Document.GetType() == typeof(string))
            {
                xmlString = input.Document;
            }
            else if (input.Document.GetType() == typeof(XmlDocument))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(input.Document);

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
                throw new FormatException("Unsupported input type. The supported types are XmlDocument, String or file path.");
            }

            var xslt = new XslCompiledTransform();
            using (var xslReader = XmlReader.Create(new StringReader(input.Stylesheet)))
            {
                xslt.Load(xslReader, new XsltSettings(true, true), new XmlUrlResolver());
            }
            
            result.Result = input.IsFile
                ? DotNetXsltTransformFileHelper(xmlString, xslt, xsltParameters)
                : DotNetXsltTransformHelper(xmlString, xslt, xsltParameters);

            return result;
        }

        private static string DotNetXsltTransformHelper(string xmlString, XslCompiledTransform xslt, IEnumerable<TransformParameters> xsltParameters)
        {
            var argsList = new XsltArgumentList();
            xsltParameters?.ToList().ForEach(x => argsList.AddParam(x.Name, "", x.Value));

            using (var inputDocument = XmlReader.Create(new StringReader(xmlString)))
            using (var memoryStream = new MemoryStream())
            using (var xmlTextWriter = XmlWriter.Create(memoryStream, xslt.OutputSettings))
            {
                xslt.Transform(inputDocument, argsList, xmlTextWriter);
                var utf8WithoutBom = xslt.OutputSettings.Encoding; // Encoding.UTF8;

                var output = utf8WithoutBom.GetString(memoryStream.ToArray());
                output = output.Replace("\n", Environment.NewLine);
                output = output.Trim('\uFEFF'); // remove utf-16 bom
                return output;
            }
        }

        private static string DotNetXsltTransformFileHelper(string xmlLocation, XslCompiledTransform xslt, IEnumerable<TransformParameters> xsltParameters)
        {
            var argsList = new XsltArgumentList();
            xsltParameters?.ToList().ForEach(x => argsList.AddParam(x.Name, "", x.Value));

            using (var inputDocument = XmlReader.Create(new StreamReader(xmlLocation)))
            using (var memoryStream = new MemoryStream())
            using (var xmlTextWriter = XmlWriter.Create(memoryStream, xslt.OutputSettings))
            {
                xslt.Transform(inputDocument, argsList, xmlTextWriter);
                var utf8WithoutBom = xslt.OutputSettings.Encoding; // Encoding.UTF8;

                var output = utf8WithoutBom.GetString(memoryStream.ToArray());
                output = output.Replace("\n", Environment.NewLine);
                output = output.Trim('\uFEFF'); // remove utf-16 bom
                return output;
            }
        }
    }
}
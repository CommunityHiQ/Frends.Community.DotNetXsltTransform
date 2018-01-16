- [Frends.Community.DotNetXsltTransform](#Frends.Community.DotNetXsltTransform)
   - [Installing](#installing)
   - [Building](#building)
   - [Contributing](#contributing)
   - [Documentation](#documentation)
      - [XsltTransform](#xslttransform)
		 - [Input](#input)
		 - [Parameters](#arameters)
		 - [Result](#result)
   - [License](#license)
       
# Frends.Community.DotNetXsltTransform
This repository contais FRENDS4 Community XsltTransform Task

## Installing
You can install the task via FRENDS UI Task view or you can find the nuget package from the following nuget feed
`https://www.myget.org/F/frends/api/v2`

## Building
Ensure that you have `https://www.myget.org/F/frends/api/v2` added to your nuget feeds

Clone a copy of the repo

git clone `https://github.com/CommunityHiQ/Frends.Community.Xml.git`

Restore dependencies

`nuget restore Frends.Community.Xml`

Rebuild the project

Run Tests with nunit3. Tests can be found under

Frends.Community.Xml.Tests\bin\Release\Frends.Community.Xml.Tests.dll

Create a nuget package

`nuget pack nuspec/Frends.Community.Xml.nuspec`

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### DotNetXsltTransform

Community XML task for xslt transforms using .NET parser. This task supports only xslt 1.0 transforms, but it supports C# scripts inside xsl file. If you don't need support for it is propably better to use Xml.Transform task.

#### Input
| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| Document  | string | Input XmlDocument or string. | ```<ROW>```<br>&nbsp;&nbsp;&nbsp;```<NAME>```MR XML```</NAME>```<br>&nbsp;&nbsp;&nbsp;```<ID>```2017```</ID>```<br>```</ROW>```|
| Stylesheet  | string | Xslt transform | <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"><br>	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" /> <br><xsl:param name="ID" /><br><xsl:template match="/"><br>&nbsp;&nbsp;&nbsp;```<person>```<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;```<name>```<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<xsl:value-of select="ROW/NAME" /></name><br><name>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<xsl:value-of select="$ID" /><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;```</name>```<br>&nbsp;&nbsp;&nbsp;```</person>```<br></xsl:template><br></xsl:stylesheet>|


#### Parameters
| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| Name  | string | Xslt parameter name |ID|
| Value| string | Xslt parameter value |ExampleValue123|

#### Result
| Property  | Type  | Description |
|-----------|-------|-------------|
| Result| string  |  Returns: Object{string} |

## License
This project is licensed under the MIT License - see the LICENSE file for details
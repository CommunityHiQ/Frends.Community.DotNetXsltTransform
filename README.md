- [Frends.Community.Xml.XsltTransform](#Frends.Community.Xml.XsltTransform)
   - [Installing](#installing)
   - [Building](#building)
   - [Contributing](#contributing)
   - [Documentation](#documentation)
      - [XsltTransform](#xslttransform)
		 - [Input](#input)
		 - [Options](#options)
		 - [Result](#result)
   - [License](#license)
       
# FRENDS.Community.Excel.ConvertExcelFile
This repository contais FRENDS4 Community XsltTransform Task

## Installing
You can install the task via FRENDS UI Task view or you can find the nuget package from the following nuget feed
'https://www.myget.org/F/frends/api/v2'

## Building
Ensure that you have 'https://www.myget.org/F/frends/api/v2' added to your nuget feeds

Clone a copy of the repo

git clone 'https://github.com/FrendsPlatform/Frends.Web.git'

Restore dependencies

nuget restore frends.web`

Rebuild the project

Run Tests with nunit3. Tests can be found under

Frends.Web.Tests\bin\Release\Frends.Web.Tests.dll

Create a nuget package

nuget pack nuspec/Frends.Community.Xml.nuspec`

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### XsltTransform

Common File task to read list of file paths from given directory.

#### Input
| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| InputXml  | string | Input XmlDocument or string. | <ROW><NAME>Testi Testaa</NAME><ID>123</ID></ROW>	|
| Xslt  | string | Xslt transform | 	|

#### Parameters
| Property  | Type  | Description |
|-----------|-------|-------------|
| Name  | string | Xslt parameter name | 
| Value| string | Xslt parameter value |

#### Result
| Property  | Type  | Description |
|-----------|-------|-------------|
| result| string  |  Returns: Object{string} |

## License
This project is licensed under the MIT License - see the LICENSE file for details
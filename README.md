# ConverterTest
File Conversion Test Project

### To Run The Console Version 
* Open in Visual Studio 2017
* Right click on the ConverterConsole project and click "Set as Startup Project"
* Click "Start"
* You will be prompted for the path of a XML or JSON file to be converted
* The path to the converted file will be returned if successful

### To Run the Web Version
* Open in Visual Studio 2017
* Right click on the ConverterWeb project and click "Set as Startup Project"
* Right click on the index.html in the ConverterWeb project and click "Set as Start Page"
* Click "Start"
* A form will allow you to upload and convert the selected XML or JSON files
* A link to the converted file will be returned if successful

### Technology Used
* Visual Studio 2017 Community
* .NET 4.6.2
* Newtonsoft.Json 10.0.1
  * Used for Seralization/Deserialization of XML and JSON files
* RESTful API
  * Used to handle post and get requests for source and converted files
* Vex Dialogs (http://github.hubspot.com/vex/docs/welcome/)
  * Used for modal alerts
* JavaScript Promises 
* XMLHttpRequest

Azure API Management Trace Viewer
==================

Azure API Management has an awesome feature to view the request as it flows through the system. [Docs here](http://azure.microsoft.com/en-gb/documentation/articles/api-management-howto-api-inspector/)

This is a quick little project to allow you to retreive and review the output of the traces from WPF or log to a file with the console app. 

This includes two versions, a simple WPF app that will display the trace output for a request and a console app that will log the request output and trace to a file.

Console Args:
-u/url "Url of the request to trace."
-s/subscriptionKey "Subscription key to be used for trace"
-f/file "Output folder where traces will be saved."

[Get the binaries here](https://github.com/lawrencegripper/AzureAPIMangementTraceViewer/releases)


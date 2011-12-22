FileSync
========

FileSync is a client-server application written in C# & XAML.

Main use cases:

* file backup
* file sharing
* file synchronization

Used tools:

* Windows 7 Professional x64
* Visual Studio 2010 Ultimate
* SQL Server Management Studio

Used technologies:

* WPF
* WCF
* EF
* some other .NET 4.0 features

Project components
------------------

### FileSyncGui - FileSync client

WPF client application. It connects with WCF service application using settings contained in `App.Config`.

### FileSyncObjects - FileSync library

Definitions of objects which are sent between client and server. Also, definition of the interface that is used in communication between different components. Both GUI and WCF service depend greatly on the library.

### FileSyncWcfService - FileSync server

Service which is launched on the server side. It connects to the database via EF, using connection string located in `Web.Config`.
 
### FileSyncGuiTest, FileSyncObjectsTest, FileSyncWcfServiceTest - FileSync unit tests

Collection use case centered tests, which check the features of various parts of the application.
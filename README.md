# TFT_HexGrid
Generate hexagonal grids with mega-hexes for use in The Fantasy Trip game system by Steve Jackson Games.

This is a Blazor WebAssembly application. Apps that target Web Assembly run locally, inside your browser, and thus do not require an active connection to any server anywhere to do their work.

I picked Blazor mainly because I hadn't ever used it before and also because I have a good deal of experience writing .Net Framework applications and already had some of the hexagonal grid generation code implemented in a WPF application from about 10 years ago.

Blazor is kind of a pain to use. The development and especially the debug experience is very painful and slow compared to other exepriences I've had in Visual Studio (ASP.Net, WPF, Winforms, etc). I've also discovered that the Blazor code is not really as performant for web assembly as I'd first understood. For example, a 48 by 72 hexagon grid takes about 20 seconds to generate on my PC. The same computation running natively completes in milliseconds. The rendering itself is not so bad, but also, still somewhat slower than I'd expected.

I'm looking at moving the tool over to MS Azure Static Web Apps to serve the UI and to Azure Functions to offload the computational stuff. That work is in progress in another repo, but I've still got to figure out if the licensing there will allow me to release it for general consumption if that strategy proves out. The bigger problem I have with that option is that the app will no longer be able to run without a web connection. Maybe a Progressive Web App is the answer? The rabbit-hole thickens...

Eventually (November 2021?) MS plans to address the web assembly performance issues. Time will tell.

This is also the first time I've worked with Scalable Vector Graphics. Compared to messing around with GDI+ and HTML5 canvasses and other appoaches I've used for drawing stuff on a screen, SVG is about as simple as it gets, at least for something like a hexagonal grid. And since it's an XML format, it's incredibly easy to manipulate distinct elements on the screen and still have them render cleanly at any resolution you want. Wrapping the SVG in Blazor components makes all the on-screen rendering "just work.""

Finally, I owe acknowledgement to Amit Patel. His excellent [Red Blob Games](https://www.redblobgames.com/) site contains many examples, explanations and suggestions that I relied upon in writing and organizing the code this app uses to generate the grids.

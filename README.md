# TFT_HexGrid
Generate hexagonal grids with mega-hexes for use in The Fantasy Trip game system by Steve Jackson Games.

As you may be able to tell from the visual style, this is a Blazor WebAssembly application. It runs in standalone mode, meaning it runs locally, inside your browser, and does not require an active connection to any server anywhere to do its work.

I picked Blazor mainly because I hadn't ever used it before and also because I have a good deal of experience writing .Net Framework applications and already had some of the hexagonal grid generation code implemented in a WPF application from about 10 years ago.

I've enjoyed working with Blazor so far, but I'm not sure I entirely understand how it works. I think I probably could have achieved better performance by implementing the individual hexagons and megahexes in the grid as RenderFragments instead of as their own Blazor components, but I wanted to try and see if I could make it work that way. Maybe some Blazor expert will come along and explain to me any error in my ways.

This is also the first time I've worked with Scalable Vector Graphics. Compared to messing around with GDI+ and HTML5 canvasses and other appoaches I've used for drawing stuff on a screen, SVG is about as simple as it gets, at least for something like a hexagonal grid. And since it's an XML format, it's incredibly easy to manipulate distinct elements on the screen and still have them render cleanly at any resolution you want. Wrapping the SVG in Blazor components makes all the on-screen rendering "just work.""

Finally, I owe acknowledgement to Amit Patel. His excellent [Red Blob Games](https://www.redblobgames.com/) site contains many examples, explanations and suggestions that I relied upon in writing and organizing the code this app uses to generate the grids.

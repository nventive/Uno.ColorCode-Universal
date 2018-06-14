# ColorCode-Universal port for Uno

This port allows for [Uno-based apps](http://platform.uno) to use [ColorCode-Universal](https://github.com/nventive/Uno.ColorCode-Universal) on Windows, iOS, Android and WebAssembly.

The following nuget packages are available:
- [Uno.ColorCode.UWP](https://www.nuget.org/packages/Uno.ColorCode.UWP)
- [Uno.ColorCode.Core](https://www.nuget.org/packages/Uno.ColorCode.Core)

# ColorCode-Universal

This is a port of [ColorCode](https://colorcode.codeplex.com/) to .NET Standard. The original Html only formatter has been separated from the Logic, so now it can produce Syntax Highlighted code for any output.

This Project can currently produce HTML, and Render to UWP RichTextBlocks.

## Usage

### HTML

To use ColorCode to create colorised HTML, ensure you have installed the ColorCode.HTML NuGet package, then you can use the following code:

```c#
var csharpstring = "public void Method()\n{\n}";
var formatter = new HtmlFormatter();
var html = formatter.GetHtmlString(csharpstring, Languages.CSharp);
```

This will create the formatting into the HTML, via inline styling. To use CSS to format the HTML instead, use the following:

```c#
var csharpstring = "public void Method()\n{\n}";
var formatter = new HtmlClassFormatter();
var html = formatter.GetHtmlString(csharpstring, Languages.CSharp);
var css = formatter.GetCSSString();
```

You will then have to manually reference the css, from the HTML head.

### UWP

To use ColorCode to render colorized code to a RichTextBlock, then you can use the following code:

```C#
var csharpstring = "public void Method()\n{\n}";
var formatter = new RichTextBlockFormatter();
formatter.FormatRichTextBlock(csharpstring, Languages.CSharp, PresentationBlock);
```

Or you can append onto an existing InlineCollection, with the following code:

```C#
var paragraph = new Paragraph();
var csharpstring = "public void Method()\n{\n}";
var formatter = new RichTextBlockFormatter();
formatter.FormatInlines(csharpstring, Languages.CSharp, paragraph.Inlines);
```

## Determining the Programming Language

To get the Programming Language manually, you can provide the identifier name, with the following code:

```C#
var language = ColorCode.Languages.FindById("java");
```
See [LanguageId.cs](ColorCode.Core/Common/LanguageId.cs) for the list of available Languages to parse.

## Packages

| Package | Description | Supports |
| --- | --- | --- |
| [ColorCode.Core](https://www.nuget.org/packages/ColorCode.Core) | The Core Library, containing the Parser, and the classes to create your own formatter. | .NET Standard 1.4 |
| [ColorCode.HTML](https://www.nuget.org/packages/ColorCode.HTML) | The Library containing the HtmlFormatter, and the HtmlClassFormatter, for rendering Html from the Colorized Code. | .NET Standard 1.4 |
| [ColorCode.UWP](https://www.nuget.org/packages/ColorCode.UWP) | The Library containing the RichTextBlockFormatter, for rendering the Colorized Code to a RichTextBlock. | Tested against 10.0.14393.0 and up. |

## Feedback and Requests

Please use [GitHub issues](https://github.com/WilliamABradley/ColorCode-Universal/issues) for bug reports and feature requests.

## Contributing
Want to help out and add some more parsing support, or add a new Formatter platform? Submit a PR!

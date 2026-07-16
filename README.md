![DML.NET](https://github.com/Moreault/DML.NET/blob/master/dmlnet.png)
# DML.NET
.NET implementation of the Dialog Markup Language.

## Setup

If you're already using AutoInject then you don't need to do anything. Otherwise, call this line when you add services :

```c#
services.AddDml();
```

Not using dependency injection? Unfortunately, there are no other alternatives right now. 

## Getting started

```c#
private readonly IDmlSerializer _dmlSerializer;

public YourClass(IDmlSerializer dmlSerializer)
{
	_dmlSerializer = dmlSerializer;
}

public void YourMethod()
{
	//the dml variable will contain a list of "dml strings" which hold information about pieces of the original string
	var dml = _dmlSerializer.Deserialize("This is some <color red=255>string</color> with <bold>DML</bold> tags.");

	//it would look something like this :
	//dml[0] : Text = "This is some ", Length = 13, Color = null (meaning you should draw it with the default text color), StartIndex = 0, EndIndex = 12, Styles = []
	//dml[1] : Text = "string", Length = 6, Color = { Red = 255, Green = 0, Blue = 0, Alpha = 255 }, Styles = []
	//dml[2] : Text = " with ", Length = 6, Color = null, Styles = []
	//dml[3] : Text = "DML", Length = 3, Color = null, , Styles = [ Bold ]
	//dml[4] = Text = " tags.", Length = 6, Color = null, Styles = []

	//it also supports multiple styles for the same piece of text so it could also have been
	//dml[0] : Text = "something", Length = 9, Color = null, , Styles = [ Bold, Underline, Italic ]

}
```

You can always assume that a null or empty value for properties such as Color, Styles or Keyword mean that this part of text should use default values (or, in the case of Keyword, that the text isn't a keyword at all.)

### Sample project
You can use the sample project provided in the solution to test out your use cases with DML to see if they apply correctly. 

## Supported tags

Currently, DML only supports the following tags : 
* Color (HTML-style hex, RGB(A) attributes or a named color- obviously not several at once)
* Highlight (sometimes also known as background color, its syntax is the same as the color's)
* Bold
* Italic
* Underline
* Strikeout
* Keyword (marks a span as a meaningful term your game can color, style and/or make clickable)

All tags are case-insensitive and does not allow duplicates. In other words; the same string fragment cannot be italic twice nor can it define mutliple colors at once.

Quotes are optional but they must be consistent throughout the string.

```c#
//the following is disallowed because the first color tag uses single quotes but the second one uses double quotes
var dml = "<color red='59'>This</color> is <color blue="95">weird</color>.";
//the following is allowed because DML accepts quoteless attributes and does not require the entire string to be quoteless- only that the quote style is consistent
var dml = "<color red=59>This</color> is not as <color blue="95">weird</color>.";
```

Nested color tags are supported. The inner tag will always take precedence over the outer tags.

```c#
//The whole string is green except for the word "grandma" which is red
var text = "<color green=255>Sebastian eloped with my <color red=255>grandma</color>.</color>"
```

The outer color tag is effectively ignored. In the case of nested text styles, however, they are both used.

```c#
//The word "quite" is bold while "clear" is both bold and italic
var text = "Sebastian's intentions are <bold>quite <italic>clear</italic></bold>."
```

```c#
//Everything between the "highlight" tags should have its background color changed to pinkish purple
//The main color tag (255, 255, 255) sets the highlit text to white
//Sebastian and grandma should apear green
//This all probably isn't very on the eyes
var text = "I cannot emphasize this enough; <highlight red=255 blue=255><color red=255 green=255 blue=255>leaving <color green=255>Sebastian</color> alone with your <color green=255>grandma</color> is a terrible idea</color></highlight>."
```

Of course, the above will only be true if your output even supports bold-italic text. If not, then it would be up to you to decide which one takes precedence.

### Color

A color (and its twin `highlight`, which sets the background) can be expressed three ways :

```c#
//RGB(A) attributes (alpha is optional and defaults to 255)
var text = "<color red=255 green=0 blue=0>red text</color>";

//HTML-style hex code, as the tag's value (the leading # is required)
var text = "<color=#FF0000>red text</color>";

//a named color, as the tag's value
var text = "<color=crimson>red text</color>";
```

These forms are mutually exclusive within a single tag : you can't mix a value with RGBA attributes.

Named colors work exactly like keyword ids : DML treats the name as an opaque string and hands it back to you untouched. It does **not** know what `crimson` looks like- resolving a name into an actual color is your game's responsibility. This lets you write readable DML in your dialog strings without copy-pasting hex codes everywhere.

When a named color is used, the deserialized substring exposes it through `ColorName` (and `HighlightName` for highlights) rather than `Color`/`Highlight`, which stay `null`. Conversely, hex and RGBA colors populate `Color`/`Highlight` and leave the name properties `null`.

```c#
//"danger" is a named color; ColorName = "danger" and Color = null
var dml = _dmlSerializer.Deserialize("This is <color=danger>bad</color>.");
```

The `#` prefix is what tells the two apart : anything starting with `#` is parsed as a hex code (and validated as one), anything else is treated as a name. This is deliberate- it means a word that also happens to be a valid hex string (ex: `facade`, `decade`) is unambiguously a name, never a color. A name must start with a letter and may otherwise contain letters, digits, hyphens and underscores.

You can build these strings with the `Color`/`Highlight` extension methods, which now accept a name (or a hex code) :

```c#
var text = "red text".Color("crimson");   //<color=crimson>red text</color>
var text = "red text".Color("#FF0000");   //<color=#FF0000>red text</color>
```

### Keyword

The keyword tag marks a span of text as a meaningful term- the kind of important word you might see highlighted in dialog and click on to get more information. DML deliberately calls it a "keyword" rather than a "link" because DML is only a spec : it doesn't know (or care) what clicking it does. Coloring the word, making it interactive, showing a tooltip, jumping to a codex entry... that's all up to your game.

A keyword can carry an id which DML treats as an opaque string and hands back to you untouched. It's up to your game to resolve what it means.

```c#
//"house" is a keyword whose id is "123"
var text = "A <keyword=123>house</keyword> on a hill.";
```

The id is what makes keywords useful beyond mere coloring : you can use it to look up the right color or style without hardcoding it in every string, to resolve localized or pluralized variants, to attach a click handler, and so on.

When you don't provide an id, it defaults to the text itself. This is handy when the displayed word is already a good enough key.

```c#
//"house" is a keyword whose id is also "house"
var text = "A <keyword>house</keyword> on a hill.";
```

Each piece of deserialized text exposes a `Keyword` property holding that id (or `null` when the text isn't a keyword.) Keywords are independent from the other tags, so the same span can be a keyword *and* be colored, highlighted or styled at the same time.

```c#
//"golden house" is keyword "42" and is also colored gold
var text = "A <keyword=42><color red=255 green=200 blue=0>golden house</color></keyword> here.";
```

Like every other tag, nested keywords follow the inner-takes-precedence rule : within a nested keyword, the innermost id wins for that span and the outer id resumes afterwards.

More support is coming for animations at a later date once proper standards (tag names, properties, animation types, etc...) have been defined.

## About DML

The language is designed with video games in mind but it could be adapted to other scenarios. The goal of DML is to provide a common standard going forward when it comes to strings of dialog in games.
If you've played more than two video games in your life, you might have noticed things such as part of spoken text being a different color or being animated differently (such as shaking or bulging.)
These are the kind of scenarios that DML covers right out of the box. Tags such as color, bold and underline are all recognized. It also parses unsupported tags in case you have weird, specific needs.
Do feel free to leave feedback if you have special needs that are not covered by DML but you think it should. 
Specifications for version 1 of DML is not yet set in stone and there is still room for new features.

## Should be obvious

DML.NET merely provides you with tools to easily extract the parts of your strings that should be colored, styled or animated.

Your code/engine still needs to interpret this information and render it accordingly.
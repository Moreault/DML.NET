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

You can always assume that a null or empty value for properties such as Color or Styles mean that this part of text should use default values.

## Supported tags

Currently, DML only supports the following tags : 
* Color (both HTML-style hex and RGB- obviously not in the same tag)
* Bold
* Italic
* Underline
* Strikeout

All tags are case-insensitive and does not allow duplicates. In other words; the same string fragment cannot be italic twice nor can it define mutliple colors at once.

Quotes are optional but they must be consistent throughout the string.

```c#
//the following is disallowed because the first color tag uses single quotes but the second one uses double quotes
var dml = "<color red='59'>This</color> is <color blue="95">weird</color>.";
//the following is allowed because DML accepts quoteless attributes and does not require the entire string to be quoteless- only that the quote style is consistent
var dml = "<color red=59>This</color> is not as <color blue="95">weird</color>.";
```

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
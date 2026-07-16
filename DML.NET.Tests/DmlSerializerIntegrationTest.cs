using Microsoft.Extensions.DependencyInjection;

namespace DML.NET.Tests;

/// <summary>
/// End-to-end tests that exercise the real parse -> convert pipeline (no mocks) through the public
/// <see cref="ServiceCollectionExtensions.AddDml"/> registration. This guards the actual AwesomeMarkup
/// parser wiring, which the mock-based <see cref="DmlSerializerTest"/> does not cover.
/// </summary>
[TestClass]
public class DmlSerializerIntegrationTest
{
    private static IDmlSerializer CreateSerializer() =>
        new ServiceCollection().AddDml().BuildServiceProvider().GetRequiredService<IDmlSerializer>();

    [TestMethod]
    public void WhenTextHasNoTags_ReturnSingleUnstyledSubstring()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("Plain text only");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "Plain text only" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasKeywordWithId_UseId()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("A <keyword=123>house</keyword> on a hill");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "A " },
            new() { Text = "house", Keyword = "123" },
            new() { Text = " on a hill" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasKeywordWithoutId_DefaultIdToText()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("A <keyword>house</keyword> on a hill");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "A " },
            new() { Text = "house", Keyword = "house" },
            new() { Text = " on a hill" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenKeywordIsNestedInsideAnotherKeyword_InnermostWinsForTheInnerSpan()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("<keyword=outer>x<keyword=inner>y</keyword>z</keyword>");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "x", Keyword = "outer" },
            new() { Text = "y", Keyword = "inner" },
            new() { Text = "z", Keyword = "outer" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasColorTag_ConvertColor()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("a <color red=255 green=0 blue=0>red</color> b");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "a " },
            new() { Text = "red", Color = new Color(255, 0, 0) },
            new() { Text = " b" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasHexColorTag_ConvertColor()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("a <color=#FF0000>red</color> b");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "a " },
            new() { Text = "red", Color = Color.FromHtml("#FF0000") },
            new() { Text = " b" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasNamedColorTag_SetColorName()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("a <color=crimson>red</color> b");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "a " },
            new() { Text = "red", ColorName = "crimson" },
            new() { Text = " b" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasNamedHighlightTag_SetHighlightName()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("a <highlight=danger>warn</highlight> b");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "a " },
            new() { Text = "warn", HighlightName = "danger" },
            new() { Text = " b" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasHighlightTag_ConvertHighlight()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("a <highlight red=0 green=128 blue=255>blue</highlight> b");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "a " },
            new() { Text = "blue", Highlight = new Color(0, 128, 255) },
            new() { Text = " b" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenTextHasStyleTag_ConvertStyle()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("a <bold>strong</bold> b");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "a " },
            new() { Text = "strong", Styles = new List<TextStyle> { TextStyle.Bold } },
            new() { Text = " b" }
        }.ToDmlString());
    }

    [TestMethod]
    public void WhenKeywordWrapsColorTag_SetBothOnTheInnerSpan()
    {
        //Arrange
        var serializer = CreateSerializer();

        //Act
        var result = serializer.Deserialize("A <keyword=42><color red=255 green=200 blue=0>golden house</color></keyword> here");

        //Assert
        result.Should().BeEquivalentTo(new List<DmlSubstring>
        {
            new() { Text = "A " },
            new() { Text = "golden house", Keyword = "42", Color = new Color(255, 200, 0) },
            new() { Text = " here" }
        }.ToDmlString());
    }
}

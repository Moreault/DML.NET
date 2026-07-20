using Microsoft.Extensions.Options;

namespace DML.NET.Tests.Conversion;

[TestClass]
public class DmlProfanityTagConverterTest
{
    private const string GrawlixSymbols = "!@#$%^&*?";

    private static DmlProfanityTagConverter CreateInstance(DmlOptions? options = null) =>
        new(Options.Create(options ?? new DmlOptions()));

    private static MarkupTag ProfanityTag(params MarkupParameter[] attributes) => new()
    {
        Name = DmlTags.Profanity,
        Kind = TagKind.Opening,
        Attributes = attributes.ToList()
    };

    [TestMethod]
    public void WhenTagIsNull_Throw()
    {
        //Arrange
        var instance = CreateInstance();

        //Act
        Action action = () => instance.Convert(null!, "damn");

        //Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void WhenTagIsNotProfanity_Throw()
    {
        //Arrange
        var instance = CreateInstance();
        var tag = new MarkupTag { Name = DmlTags.Color, Kind = TagKind.Opening };

        //Act
        Action action = () => instance.Convert(tag, "damn");

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotConvertBecauseTagUnsupported, DmlTags.Profanity, DmlTags.Profanity, tag));
    }

    [TestMethod]
    public void WhenNoLevelAttribute_UseOptionsDefault()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { DefaultProfanityLevel = ProfanityLevel.Severe, CleanFallback = CleanFallback.DoNothing });

        //Act
        var result = instance.Convert(ProfanityTag(), "damn");

        //Assert
        result.Level.Should().Be(ProfanityLevel.Severe);
    }

    [TestMethod]
    public void WhenNoLevelAttributeAndOptionsDefaultIsNull_LevelIsNull()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { DefaultProfanityLevel = null, CleanFallback = CleanFallback.DoNothing });

        //Act
        var result = instance.Convert(ProfanityTag(), "damn");

        //Assert
        result.Level.Should().BeNull();
    }

    [TestMethod]
    [DataRow("mild", ProfanityLevel.Mild)]
    [DataRow("strong", ProfanityLevel.Strong)]
    [DataRow("severe", ProfanityLevel.Severe)]
    [DataRow("SEVERE", ProfanityLevel.Severe)]
    [DataRow("Severe", ProfanityLevel.Severe)]
    public void WhenLevelAttributeIsValid_UseIt(string value, ProfanityLevel expected)
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { CleanFallback = CleanFallback.DoNothing });

        //Act
        var result = instance.Convert(ProfanityTag(new MarkupParameter { Name = DmlTags.Level, Value = value }), "damn");

        //Assert
        result.Level.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("meh")]
    [DataRow("9")]
    [DataRow("mildish")]
    public void WhenLevelAttributeIsInvalid_Throw(string value)
    {
        //Arrange
        var instance = CreateInstance();

        //Act
        Action action = () => instance.Convert(ProfanityTag(new MarkupParameter { Name = DmlTags.Level, Value = value }), "damn");

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotConvertBecauseInvalidProfanityLevel, value));
    }

    [TestMethod]
    public void WhenCleanAttributeProvided_UseItVerbatim()
    {
        //Arrange
        var instance = CreateInstance();

        //Act
        var result = instance.Convert(ProfanityTag(new MarkupParameter { Name = DmlTags.Clean, Value = "gosh darn" }), "goddamn");

        //Assert
        result.Clean.Should().Be("gosh darn");
    }

    [TestMethod]
    public void WhenCleanAttributeIsBlank_FallBack()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { CleanFallback = CleanFallback.Asterisks });

        //Act
        var result = instance.Convert(ProfanityTag(new MarkupParameter { Name = DmlTags.Clean, Value = " " }), "damn");

        //Assert
        result.Clean.Should().Be("****");
    }

    [TestMethod]
    public void WhenNoCleanAndFallbackIsDoNothing_CleanIsNull()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { CleanFallback = CleanFallback.DoNothing });

        //Act
        var result = instance.Convert(ProfanityTag(), "damn");

        //Assert
        result.Clean.Should().BeNull();
    }

    [TestMethod]
    public void WhenNoCleanAndFallbackIsAsterisks_MaskWithAsterisksPreservingWhitespace()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { CleanFallback = CleanFallback.Asterisks });

        //Act
        var result = instance.Convert(ProfanityTag(), "damn it");

        //Assert
        result.Clean.Should().Be("**** **");
    }

    [TestMethod]
    public void WhenNoCleanAndFallbackIsGrawlix_MaskIsLengthMatchedSymbolsPreservingWhitespace()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { CleanFallback = CleanFallback.Grawlix });

        //Act
        var result = instance.Convert(ProfanityTag(), "damn it");

        //Assert
        result.Clean!.Should().HaveLength("damn it".Length);
        result.Clean![4].Should().Be(' ');
        result.Clean!.Where(c => !char.IsWhiteSpace(c)).Should().OnlyContain(c => GrawlixSymbols.Contains(c));
    }

    [TestMethod]
    public void WhenNoCleanAndFallbackIsGrawlix_IsDeterministic()
    {
        //Arrange
        var instance = CreateInstance(new DmlOptions { CleanFallback = CleanFallback.Grawlix });

        //Act
        var first = instance.Convert(ProfanityTag(), "damn");
        var second = instance.Convert(ProfanityTag(), "damn");

        //Assert
        first.Clean.Should().Be(second.Clean);
    }
}

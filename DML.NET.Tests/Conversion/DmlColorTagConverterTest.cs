﻿namespace DML.NET.Tests.Conversion;

[TestClass]
public class DmlColorTagConverterTest
{
    [TestClass]
    public class Convert : Tester<DmlColorTagConverter>
    {
        [TestMethod]
        public void WhenTagIsNull_Throw()
        {
            //Arrange
            MarkupTag tag = null!;

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenTagIsNotNamedColorOrHighlight_Throw()
        {
            //Arrange
            var tag = Dummy.Create<MarkupTag>();

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotConvertBecauseTagUnsupported, DmlTags.Color, DmlTags.Highlight, tag));
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagContainsNeitherValueNorColorAttributes_Throw(string colorTag)
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = colorTag,
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' does not appear to hold any valid color information");
        }
        
        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagContainsValueThatIsNotHexCode_Throw(string colorTag)
        {
            //Arrange
            var tag = Dummy.Build<MarkupTag>().With(x => x.Name, colorTag).Create();

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : {tag.Value} is not in a valid hex color format.");
        }
        //TODO add Fixture extension to create random hex color codes
        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagHasBothHexCodeValueAndColorAttributes_Throw(string colorTag)
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = colorTag,
                Value = "#FF3456",
                Attributes = new List<MarkupParameter>
                {
                    new()
                    {
                        Name = DmlTags.Alpha,
                        Value = "125"
                    }
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' has both a hex code and RGBA values but must have only one or the other.");
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsHexCode_ReturnAsRgba(string colorTag)
        {
            //Arrange
            var hex = "#FF0000";

            var tag = new MarkupTag
            {
                Name = colorTag,
                Value = hex,
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().BeEquivalentTo(Color.FromHtml(hex));
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagOnlyHasAttributesAndTheyContainNonNumeric_Throw(string colorTag)
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = Dummy.Create<string>() },
                    new() { Name = DmlTags.Green, Value = Dummy.Create<byte>().ToString() },
                    new() { Name = DmlTags.Blue, Value = Dummy.Create<string>() },
                    new() { Name = DmlTags.Alpha, Value = Dummy.Create<byte>().ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' contains non-numeric values.");
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagOnlyHasAttributesAndTheyContainValuesBelowZero_Throw(string colorTag)
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = Dummy.Create<byte>().ToString() },
                    new() { Name = DmlTags.Green, Value = "-1" },
                    new() { Name = DmlTags.Blue, Value = Dummy.Create<byte>().ToString() },
                    new() { Name = DmlTags.Alpha, Value = Dummy.Create<byte>().ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' values outside the accepted range of 0 and 255.");
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagOnlyHasAttributesAndTheyContainValuesOver255_Throw(string colorTag)
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = Dummy.Create<byte>().ToString() },
                    new() { Name = DmlTags.Green, Value = Dummy.Create<byte>().ToString() },
                    new() { Name = DmlTags.Blue, Value = "256" },
                    new() { Name = DmlTags.Alpha, Value = Dummy.Create<byte>().ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' values outside the accepted range of 0 and 255.");
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsFullRgba_Return(string colorTag)
        {
            //Arrange
            var color = Dummy.Create<Color>();

            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = color.Red.ToString() },
                    new() { Name = DmlTags.Green, Value = color.Green.ToString() },
                    new() { Name = DmlTags.Blue, Value = color.Blue.ToString() },
                    new() { Name = DmlTags.Alpha, Value = color.Alpha.ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(color);
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsRgbOnly_ReturnWithMaxAlpha(string colorTag)
        {
            //Arrange
            var color = new Color(Dummy.Create<byte>(), Dummy.Create<byte>(), Dummy.Create<byte>());

            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = color.Red.ToString() },
                    new() { Name = DmlTags.Green, Value = color.Green.ToString() },
                    new() { Name = DmlTags.Blue, Value = color.Blue.ToString() }
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(color);
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsRedOnly_ReturnRed(string colorTag)
        {
            //Arrange
            var red = Dummy.Create<byte>();

            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = red.ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color(red, (byte)0, (byte)0));
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsGreenOnly_ReturnGreen(string colorTag)
        {
            //Arrange
            var green = Dummy.Create<byte>();

            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Green, Value = green.ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color((byte)0, green, (byte)0));
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsBlueOnly_ReturnBlue(string colorTag)
        {
            //Arrange
            var blue = Dummy.Create<byte>();

            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Blue, Value = blue.ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color((byte)0, (byte)0, blue));
        }

        [TestMethod]
        [DataRow(DmlTags.Color)]
        [DataRow(DmlTags.Highlight)]
        public void WhenTagIsAlphaOnly_ReturnWhite(string colorTag)
        {
            //Arrange
            var alpha = Dummy.Create<byte>();

            var tag = new MarkupTag
            {
                Name = colorTag,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Alpha, Value = alpha.ToString() },
                },
                Kind = Dummy.Create<TagKind>()
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color((byte)0, (byte)0, (byte)0, alpha));
        }
    }
}
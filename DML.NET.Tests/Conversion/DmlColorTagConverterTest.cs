namespace DML.NET.Tests.Conversion;

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
        public void WhenTagIsNotNamedColor_Throw()
        {
            //Arrange
            var tag = Fixture.Create<MarkupTag>();

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : '{tag}' must be named '{DmlTags.Color}'.");
        }

        [TestMethod]
        public void WhenTagContainsNeitherValueNorColorAttributes_Throw()
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = DmlTags.Color
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' does not appear to hold any valid color information");
        }

        [TestMethod]
        public void WhenTagContainsValueThatIsNotHexCode_Throw()
        {
            //Arrange
            var tag = Fixture.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create();

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : {tag.Value} is not in a valid hex color format.");
        }
        //TODO add Fixture extension to create random hex color codes
        [TestMethod]
        public void WhenTagHasBothHexCodeValueAndColorAttributes_Throw()
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Value = "#FF3456",
                Attributes = new List<MarkupParameter>
                {
                    new MarkupParameter
                    {
                        Name = DmlTags.Alpha,
                        Value = "125"
                    }
                }
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' has both a hex code and RGBA values but must have only one or the other.");
        }

        [TestMethod]
        public void WhenTagIsHexCode_ReturnAsRgba()
        {
            //Arrange
            var hex = "#FF0000";

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Value = hex,
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().BeEquivalentTo(Color.FromHtml(hex));
        }

        [TestMethod]
        public void WhenTagOnlyHasAttributesAndTheyContainNonNumeric_Throw()
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = Fixture.Create<string>() },
                    new() { Name = DmlTags.Green, Value = Fixture.Create<byte>().ToString() },
                    new() { Name = DmlTags.Blue, Value = Fixture.Create<string>() },
                    new() { Name = DmlTags.Alpha, Value = Fixture.Create<byte>().ToString() },
                }
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' contains non-numeric values.");
        }

        [TestMethod]
        public void WhenTagOnlyHasAttributesAndTheyContainValuesBelowZero_Throw()
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = Fixture.Create<byte>().ToString() },
                    new() { Name = DmlTags.Green, Value = "-1" },
                    new() { Name = DmlTags.Blue, Value = Fixture.Create<byte>().ToString() },
                    new() { Name = DmlTags.Alpha, Value = Fixture.Create<byte>().ToString() },
                }
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' values outside the accepted range of 0 and 255.");
        }

        [TestMethod]
        public void WhenTagOnlyHasAttributesAndTheyContainValuesOver255_Throw()
        {
            //Arrange
            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = Fixture.Create<byte>().ToString() },
                    new() { Name = DmlTags.Green, Value = Fixture.Create<byte>().ToString() },
                    new() { Name = DmlTags.Blue, Value = "256" },
                    new() { Name = DmlTags.Alpha, Value = Fixture.Create<byte>().ToString() },
                }
            };

            //Act
            Action action = () => Instance.Convert(tag);

            //Assert
            action.Should().Throw<Exception>().WithMessage($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' values outside the accepted range of 0 and 255.");
        }

        [TestMethod]
        public void WhenTagIsFullRgba_Return()
        {
            //Arrange
            var color = Fixture.Create<Color>();

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = color.Red.ToString() },
                    new() { Name = DmlTags.Green, Value = color.Green.ToString() },
                    new() { Name = DmlTags.Blue, Value = color.Blue.ToString() },
                    new() { Name = DmlTags.Alpha, Value = color.Alpha.ToString() },
                }
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(color);
        }

        [TestMethod]
        public void WhenTagIsRgbOnly_ReturnWithMaxAlpha()
        {
            //Arrange
            var color = new Color(Fixture.Create<byte>(), Fixture.Create<byte>(), Fixture.Create<byte>());

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = color.Red.ToString() },
                    new() { Name = DmlTags.Green, Value = color.Green.ToString() },
                    new() { Name = DmlTags.Blue, Value = color.Blue.ToString() }
                }
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(color);
        }

        [TestMethod]
        public void WhenTagIsRedOnly_ReturnRed()
        {
            //Arrange
            var red = Fixture.Create<byte>();

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Red, Value = red.ToString() },
                }
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color(red, (byte)0, (byte)0));
        }

        [TestMethod]
        public void WhenTagIsGreenOnly_ReturnGreen()
        {
            //Arrange
            var green = Fixture.Create<byte>();

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Green, Value = green.ToString() },
                }
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color((byte)0, green, (byte)0));
        }

        [TestMethod]
        public void WhenTagIsBlueOnly_ReturnBlue()
        {
            //Arrange
            var blue = Fixture.Create<byte>();

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Blue, Value = blue.ToString() },
                }
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color((byte)0, (byte)0, blue));
        }

        [TestMethod]
        public void WhenTagIsAlphaOnly_ReturnWhite()
        {
            //Arrange
            var alpha = Fixture.Create<byte>();

            var tag = new MarkupTag
            {
                Name = DmlTags.Color,
                Attributes = new List<MarkupParameter>
                {
                    new() { Name = DmlTags.Alpha, Value = alpha.ToString() },
                }
            };

            //Act
            var result = Instance.Convert(tag);

            //Assert
            result.Should().Be(new Color((byte)0, (byte)0, (byte)0, alpha));
        }
    }
}
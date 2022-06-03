namespace DML.NET.Tests.Conversion;

[TestClass]
public class DmlTextStyleConverterTester
{
    [TestClass]
    public class Convert : Tester<DmlTextStyleConverter>
    {
        [TestMethod]
        public void WhenMetaStringIsNull_Throw()
        {
            //Arrange
            MetaString metaString = null!;

            //Act
            var action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenMetaStringContainsMoreThanOneBold_Throw()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                    new() { Name = DmlTags.Bold },
                }
            };

            //Act
            var action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Bold));
        }

        [TestMethod]
        public void WhenMetaStringContainsMoreThanOneItalic_Throw()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                    new() { Name = DmlTags.Italic },
                    new() { Name = DmlTags.Italic },
                }
            };

            //Act
            var action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Italic));
        }

        [TestMethod]
        public void WhenMetaStringContainsMoreThanOneUnderline_Throw()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                    new() { Name = DmlTags.Italic },
                    new() { Name = DmlTags.Underline },
                    new() { Name = DmlTags.Underline },
                }
            };

            //Act
            var action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Underline));
        }

        [TestMethod]
        public void WhenMetaSTringContainsMoreThanOneStrikeout_Throw()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                    new() { Name = DmlTags.Italic },
                    new() { Name = DmlTags.Underline },
                    new() { Name = DmlTags.Strikeout },
                    new() { Name = DmlTags.Strikeout },
                }
            };

            //Act
            var action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Strikeout));
        }

        [TestMethod]
        public void WhenMetaStringContainsOneBold_ReturnOneBold()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Bold });
        }

        [TestMethod]
        public void WhenMetaStringContainsOneItalic_ReturnOneItalic()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Italic },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Italic });
        }

        [TestMethod]
        public void WhenMetaStringContainsOneUnderline_ReturnOneUnderline()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Underline },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Underline });
        }

        [TestMethod]
        public void WhenMetaStringContainsOneStrikeout_ReturnOneStrikeout()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Strikeout },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Strikeout });
        }

        [TestMethod]
        public void WhenMetaStringContainsBoldAndItalic_ReturnBoldAndItalic()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                    new() { Name = DmlTags.Italic },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Bold, TextStyle.Italic });
        }

        [TestMethod]
        public void WhenMetaStringContainsStrikeoutAndUnderline_ReturnStrikeoutAndUnderline()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Strikeout },
                    new() { Name = DmlTags.Underline },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Strikeout, TextStyle.Underline });
        }

        [TestMethod]
        public void WhenMetaStringContainsAllStyles_ReturnAllStyles()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Bold },
                    new() { Name = DmlTags.Italic },
                    new() { Name = DmlTags.Underline },
                    new() { Name = DmlTags.Strikeout },
                }
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new List<TextStyle> { TextStyle.Bold, TextStyle.Italic, TextStyle.Underline, TextStyle.Strikeout });
        }

        [TestMethod]
        public void WhenMetaStringContainsNoStyle_ReturnEmpty()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = "Something",
            };

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEmpty();
        }
    }
}
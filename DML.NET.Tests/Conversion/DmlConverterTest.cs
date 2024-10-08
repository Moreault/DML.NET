﻿namespace DML.NET.Tests.Conversion;

public abstract class DmlConverterTesterBase : Tester<DmlConverter>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        GetMock<IDmlTextStyleConverter>().Setup(x => x.Convert(It.IsAny<MetaString>())).Returns(new List<TextStyle>());
    }
}

[TestClass]
public class DmlConverterTest
{
    [TestClass]
    public class Convert_Collection : DmlConverterTesterBase
    {
        [TestMethod]
        public void WhenCollectionIsNull_Throw()
        {
            //Arrange
            IReadOnlyList<MetaString> metaStrings = null!;

            //Act
            Action action = () => Instance.Convert(metaStrings);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenOneElementIsNull_Throw()
        {
            //Arrange
            var metaStrings = new List<MetaString>(new[] { null!, Dummy.Create<MetaString>() });

            //Act
            Action action = () => Instance.Convert(metaStrings);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenMetaStringsHaveNoTags_ReturnTextOnly()
        {
            //Arrange
            var metaStrings = new List<MetaString>(Dummy.Build<MetaString>().Omit(x => x.Tags).CreateMany());

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(metaStrings.Select(x => new DmlSubstring
            {
                Text = x.Text
            }).ToDmlString());
        }

        [TestMethod]
        public void WhenMetaStringsHaveNoColorTags_ReturnTextOnly()
        {
            //Arrange
            var metaStrings = new List<MetaString>(Dummy.CreateMany<MetaString>());

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(metaStrings.Select(x => new DmlSubstring
            {
                Text = x.Text
            }).ToDmlString());
        }

        [TestMethod]
        public void WhenMetaStringHasColorTagNestedInsideAnotherColorTag_UseLastColorTag()
        {
            //Arrange
            var metaStrings = new List<MetaString>(Dummy.Build<MetaString>().With(x => x.Tags, new List<MarkupTag>
            {
                new() { Name = DmlTags.Color, Value = "First", Kind = TagKind.Opening },
                new() { Name = DmlTags.Color, Value = "Second", Kind = TagKind.Opening  },
            }).CreateMany());

            var expectedColor = Dummy.Create<Color>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(new MarkupTag { Name = DmlTags.Color, Value = "Second", Kind = TagKind.Opening })).Returns(expectedColor);

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(metaStrings.Select(x => new DmlSubstring
            {
                Text = x.Text,
                Color = expectedColor
            }));
        }

        [TestMethod]
        public void WhenMetaStringHasHighlightTagNestedInsideAnotherHighlightTag_UseLastHighlightTag()
        {
            //Arrange
            var metaStrings = new List<MetaString>(Dummy.Build<MetaString>().With(x => x.Tags, new List<MarkupTag>
            {
                new() { Name = DmlTags.Highlight, Value = "First" , Kind = TagKind.Opening },
                new() { Name = DmlTags.Highlight, Value = "Second", Kind = TagKind.Opening },
            }).CreateMany());

            var expectedHighlight = Dummy.Create<Color>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(new MarkupTag { Name = DmlTags.Highlight, Value = "Second", Kind = TagKind.Opening })).Returns(expectedHighlight);

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(metaStrings.Select(x => new DmlSubstring
            {
                Text = x.Text,
                Highlight = expectedHighlight
            }));
        }

        [TestMethod]
        public void WhenMetaStringsHaveOneColorTag_ConvertColor()
        {
            //Arrange
            var metaStrings = new List<MetaString>(new List<MetaString>
            {
                new()
                {
                    Text = Dummy.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                    }
                },
                new()
                {
                    Text = Dummy.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                    }
                },
                new()
                {
                    Text = Dummy.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                    }
                },
            });

            var colors = Dummy.Create<List<Color>>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[0].Tags.Single())).Returns(colors[0]);
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[1].Tags.Single())).Returns(colors[1]);
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[2].Tags.Single())).Returns(colors[2]);

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = metaStrings[0].Text,
                    Color = colors[0]
                }, new()
                {
                    Text = metaStrings[1].Text,
                    Color = colors[1]
                }, new()
                {
                    Text = metaStrings[2].Text,
                    Color = colors[2]
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenMetaStringsHaveOneHighlightTag_ConvertHighlight()
        {
            //Arrange
            var metaStrings = new List<MetaString>(new List<MetaString>
            {
                new()
                {
                    Text = Dummy.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Highlight).Create()
                    }
                },
                new()
                {
                    Text = Dummy.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Highlight).Create()
                    }
                },
                new()
                {
                    Text = Dummy.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Highlight).Create()
                    }
                },
            });

            var highlights = Dummy.Create<List<Color>>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[0].Tags.Single())).Returns(highlights[0]);
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[1].Tags.Single())).Returns(highlights[1]);
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[2].Tags.Single())).Returns(highlights[2]);

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = metaStrings[0].Text,
                    Highlight = highlights[0]
                }, new()
                {
                    Text = metaStrings[1].Text,
                    Highlight = highlights[1]
                }, new()
                {
                    Text = metaStrings[2].Text,
                    Highlight = highlights[2]
                }
            }.ToDmlString());
        }
    }

    [TestClass]
    public class Convert_Single : DmlConverterTesterBase
    {
        [TestMethod]
        public void WhenMetaStringIsNull_Throw()
        {
            //Arrange
            MetaString metaString = null!;

            //Act
            Action action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereIsNoTag_ReturnTextOnly()
        {
            //Arrange
            var metaString = Dummy.Build<MetaString>().Omit(x => x.Tags).Create();

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new DmlSubstring
            {
                Text = metaString.Text
            });
        }

        [TestMethod]
        public void WhenThereIsNoColorTag_ReturnTextOnly()
        {
            //Arrange
            var metaString = Dummy.Create<MetaString>();

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new DmlSubstring
            {
                Text = metaString.Text
            });
        }

        [TestMethod]
        public void WhenMetaStringHasColorTagNestedInsideAnotherColorTag_UseLastColorTag()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = Dummy.Create<string>(),
                Tags = new List<MarkupTag>
                {
                    new() { Name = DmlTags.Color, Value = "First", Kind = TagKind.Opening },
                    new() { Name = DmlTags.Color, Value = "Second", Kind = TagKind.Opening },
                }
            };

            var expectedColor = Dummy.Create<Color>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(new MarkupTag { Name = DmlTags.Color, Value = "Second", Kind = TagKind.Opening })).Returns(expectedColor);

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new DmlSubstring
            {
                Text = metaString.Text,
                Color = expectedColor
            });
        }

        [TestMethod]
        public void WhenThereIsOneColorTag_ConvertColor()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = Dummy.Create<string>(),
                Tags = new List<MarkupTag>
                {
                    Dummy.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                }
            };

            var color = Dummy.Create<Color>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaString.Tags.Single())).Returns(color);

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new DmlSubstring
            {
                Text = metaString.Text,
                Color = color
            });
        }

        [TestMethod]
        public void Always_ConvertTextStyles()
        {
            //Arrange
            var metaString = Dummy.Create<MetaString>();

            var styles = Dummy.CreateMany<TextStyle>().ToList();
            GetMock<IDmlTextStyleConverter>().Setup(x => x.Convert(metaString)).Returns(styles);

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new DmlSubstring
            {
                Text = metaString.Text,
                Styles = styles
            });
        }
    }
}
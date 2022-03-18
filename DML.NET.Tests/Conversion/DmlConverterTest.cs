namespace DML.NET.Tests.Conversion;

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
            var metaStrings = new List<MetaString>(new[] { null!, Fixture.Create<MetaString>() });

            //Act
            Action action = () => Instance.Convert(metaStrings);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenMetaStringsHaveNoTags_ReturnTextOnly()
        {
            //Arrange
            var metaStrings = new List<MetaString>(Fixture.Build<MetaString>().Without(x => x.Tags).CreateMany());

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
            var metaStrings = new List<MetaString>(Fixture.CreateMany<MetaString>());

            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(metaStrings.Select(x => new DmlSubstring
            {
                Text = x.Text
            }).ToDmlString());
        }

        [TestMethod]
        public void WhenMetaStringsHaveMoreThanOneColorTag_ReturnTextOnly()
        {
            //Arrange
            var metaStrings = new List<MetaString>(Fixture.Build<MetaString>().With(x => x.Tags, new List<MarkupTag>
            {
                new MarkupTag { Name = DmlTags.Color },
                new MarkupTag { Name = DmlTags.Color },
            }).CreateMany());

            //Act
            Action action = () => Instance.Convert(metaStrings);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenMetaStringsHaveOneColorTag_ConvertColor()
        {
            //Arrange
            var metaStrings = new List<MetaString>(new List<MetaString>
            {
                new MetaString
                {
                    Text = Fixture.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Fixture.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                    }
                },
                new MetaString
                {
                    Text = Fixture.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Fixture.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                    }
                },
                new MetaString
                {
                    Text = Fixture.Create<string>(),
                    Tags = new List<MarkupTag>
                    {
                        Fixture.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                    }
                },
            });

            var colors = Fixture.Create<List<Color>>();
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[0].Tags.Single())).Returns(colors[0]);
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[1].Tags.Single())).Returns(colors[1]);
            GetMock<IDmlColorTagConverter>().Setup(x => x.Convert(metaStrings[2].Tags.Single())).Returns(colors[2]);


            //Act
            var result = Instance.Convert(metaStrings);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new DmlSubstring
                {
                    Text = metaStrings[0].Text,
                    Color = colors[0]
                }, new DmlSubstring
                {
                    Text = metaStrings[1].Text,
                    Color = colors[1]
                }, new DmlSubstring
                {
                    Text = metaStrings[2].Text,
                    Color = colors[2]
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
            var metaString = Fixture.Build<MetaString>().Without(x => x.Tags).Create();

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
            var metaString = Fixture.Create<MetaString>();

            //Act
            var result = Instance.Convert(metaString);

            //Assert
            result.Should().BeEquivalentTo(new DmlSubstring
            {
                Text = metaString.Text
            });
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneColorTag_Throw()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = Fixture.Create<string>(),
                Tags = new List<MarkupTag>
                {
                    new MarkupTag { Name = DmlTags.Color },
                    new MarkupTag { Name = DmlTags.Color },
                }
            };

            //Act
            Action action = () => Instance.Convert(metaString);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsOneColorTag_ConvertColor()
        {
            //Arrange
            var metaString = new MetaString
            {
                Text = Fixture.Create<string>(),
                Tags = new List<MarkupTag>
                {
                    Fixture.Build<MarkupTag>().With(x => x.Name, DmlTags.Color).Create()
                }
            };

            var color = Fixture.Create<Color>();
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
            var metaString = Fixture.Create<MetaString>();

            var styles = Fixture.CreateMany<TextStyle>().ToList();
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
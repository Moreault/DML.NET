namespace DML.NET.Tests;

[TestClass]
public class DmlStringTest
{
    //TODO Test
    [TestClass]
    public class Indexer_int : Tester
    {
        //TODO Test
    }

    [TestClass]
    public class Indexer_int_int : Tester
    {
        //TODO Test
    }

    [TestClass]
    public class Substring_Start : Tester
    {
        [TestMethod]
        public void WhenStartingFromMiddle_Return()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(20);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "outskirts of ",
                    Color = dmlString[1].Color
                },
                new()
                {
                    Text = "Behabad",
                    Color = dmlString[2].Color
                }
            }.ToDmlString());
        }
    }

    [TestClass]
    public class Substring_start_length : Tester
    {
        [TestMethod]
        public void WhenStartingIndexIsNegative_Throw()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            var startingIndex = -Dummy.Create<int>();
            var length = Dummy.Create<int>();

            //Act
            Action action = () => dmlString.Substring(startingIndex, length);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenLengthIsNegative_Throw()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            var startingIndex = Dummy.Create<int>();
            var length = -Dummy.Create<int>();

            //Act
            Action action = () => dmlString.Substring(startingIndex, length);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenLengthIsZero_ReturnEmpty()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            var startingIndex = Dummy.Create<int>();
            var length = 0;

            //Act
            var result = dmlString.Substring(startingIndex, length);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBothStartingIndexAndLengthAreWithinFirstSubstring_Return()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(0, 4);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "That",
                    Color = dmlString[0].Color
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenStartingIndexIsWithinFirstSubstringButLengthGoesIntoSecond_ReturnPartsOfBoth()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(5, 24);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "base is ",
                    Color = dmlString[0].Color
                },
                new()
                {
                    Text = "on the outskirts",
                    Color = dmlString[1].Color
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenStartingFromMiddle_Return()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(20, 20);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "outskirts of ",
                    Color = dmlString[1].Color
                },
                new()
                {
                    Text = "Behabad",
                    Color = dmlString[2].Color
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenSlicingWithinASingleSubstring_PreserveEveryProperty()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "You heckin scoundrel",
                    Color = Dummy.Create<Color>(),
                    Highlight = Dummy.Create<Color>(),
                    Keyword = "insult",
                    IsProfanity = true,
                    ProfanityLevel = ProfanityLevel.Mild,
                    Clean = "gosh dang",
                    Styles = new List<TextStyle> { TextStyle.Bold }
                }
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(0, 3);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "You",
                    Color = dmlString[0].Color,
                    Highlight = dmlString[0].Highlight,
                    Keyword = "insult",
                    IsProfanity = true,
                    ProfanityLevel = ProfanityLevel.Mild,
                    Clean = "gosh dang",
                    Styles = new List<TextStyle> { TextStyle.Bold }
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenSlicingAcrossSubstrings_PreserveEveryPropertyOnEachPiece()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "You are a ",
                    Keyword = "subject",
                    Styles = new List<TextStyle> { TextStyle.Italic }
                },
                new()
                {
                    Text = "heck",
                    IsProfanity = true,
                    ProfanityLevel = ProfanityLevel.Severe,
                    Clean = "****"
                },
                new()
                {
                    Text = " off",
                    Color = Dummy.Create<Color>()
                }
            }.ToDmlString();

            //Act : from the middle of the first substring ("You are a "), through all of the second ("heck"), into
            //the third (" off"). Full text is "You are a heck off"; indices 4..15 -> "are a heck o".
            var result = dmlString.Substring(4, 12);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "are a ",
                    Keyword = "subject",
                    Styles = new List<TextStyle> { TextStyle.Italic }
                },
                new()
                {
                    Text = "heck",
                    IsProfanity = true,
                    ProfanityLevel = ProfanityLevel.Severe,
                    Clean = "****"
                },
                new()
                {
                    Text = " o",
                    Color = dmlString[2].Color
                }
            }.ToDmlString());
        }
    }

    [TestClass]
    public class ToString_Method : Tester
    {
        //TODO Test
    }
}
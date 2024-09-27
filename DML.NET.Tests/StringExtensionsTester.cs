namespace DML.NET.Tests;

[TestClass]
public class StringExtensionsTester
{
    [TestClass]
    public class Color_Rgb : Tester
    {
        [TestMethod]
        public void Always_SurroundStringWithColor()
        {
            //Arrange
            var value = Dummy.Create<string>();
            var red = Dummy.Create<byte>();
            var green = Dummy.Create<byte>();
            var blue = Dummy.Create<byte>();

            //Act
            var result = value.Color(red, green, blue);

            //Assert
            result.Should().Be($"<color red={red} green={green} blue={blue} alpha=255>{value}</color>");
        }
    }

    [TestClass]
    public class Color_Struct : Tester
    {
        [TestMethod]
        public void Always_SurroundStringWithColor()
        {
            //Arrange
            var value = Dummy.Create<string>();
            var color = Dummy.Create<Color>();

            //Act
            var result = value.Color(color);

            //Assert
            result.Should().Be($"<color red={color.Red} green={color.Green} blue={color.Blue} alpha={color.Alpha}>{value}</color>");
        }
    }

    [TestClass]
    public class Highlight_Rbg : Tester
    {
        [TestMethod]
        public void Always_SurroundStringWithColor()
        {
            //Arrange
            var value = Dummy.Create<string>();
            var red = Dummy.Create<byte>();
            var green = Dummy.Create<byte>();
            var blue = Dummy.Create<byte>();

            //Act
            var result = value.Highlight(red, green, blue);

            //Assert
            result.Should().Be($"<highlight red={red} green={green} blue={blue} alpha=255>{value}</highlight>");
        }
    }

    [TestClass]
    public class Highlight_Struct : Tester
    {
        [TestMethod]
        public void Always_SurroundStringWithColor()
        {
            //Arrange
            var value = Dummy.Create<string>();
            var color = Dummy.Create<Color>();

            //Act
            var result = value.Highlight(color);

            //Assert
            result.Should().Be($"<highlight red={color.Red} green={color.Green} blue={color.Blue} alpha={color.Alpha}>{value}</highlight>");
        }
    }

    [TestClass]
    public class Style : Tester
    {
        [TestMethod]
        public void WhenStyleIsBold_SurroundWithBold()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Style(TextStyle.Bold);

            //Assert
            result.Should().Be($"<bold>{value}</bold>");
        }

        [TestMethod]
        public void WhenStyleIsItalic_SurroundWithItalic()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Style(TextStyle.Italic);

            //Assert
            result.Should().Be($"<italic>{value}</italic>");
        }

        [TestMethod]
        public void WhenStyleIsStrikeout_SurroundWithStrikeout()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Style(TextStyle.Strikeout);

            //Assert
            result.Should().Be($"<strikeout>{value}</strikeout>");
        }

        [TestMethod]
        public void WhenStyleIsUnderline_SurroundWithUnderline()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Style(TextStyle.Underline);

            //Assert
            result.Should().Be($"<underline>{value}</underline>");
        }

        [TestMethod]
        public void WhenOutOfRange_Throw()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var action = () => value.Style((TextStyle)int.MaxValue);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("style");
        }
    }

    [TestClass]
    public class Bold : Tester
    {
        [TestMethod]
        public void Always_SurroundWithBold()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Bold();

            //Assert
            result.Should().Be($"<bold>{value}</bold>");
        }
    }

    [TestClass]
    public class Italic : Tester
    {
        [TestMethod]
        public void Always_SurroundWithItalic()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Italic();

            //Assert
            result.Should().Be($"<italic>{value}</italic>");
        }
    }

    [TestClass]
    public class Strikeout : Tester
    {
        [TestMethod]
        public void Always_SurroundWithStrikeout()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Strikeout();

            //Assert
            result.Should().Be($"<strikeout>{value}</strikeout>");
        }
    }

    [TestClass]
    public class Underline : Tester
    {
        [TestMethod]
        public void Always_SurroundWithUnderline()
        {
            //Arrange
            var value = Dummy.Create<string>();

            //Act
            var result = value.Underline();

            //Assert
            result.Should().Be($"<underline>{value}</underline>");
        }
    }
}
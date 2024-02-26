namespace CapMan.Tests;

public class Element_should_
{
    [Theory]
    [InlineData('.', Element.Dot)]
    [InlineData('O', Element.PowerPill)]
    [InlineData('│', Element.Vertical)]
    [InlineData('─', Element.Horizontal)]
    [InlineData('+', Element.Corner)]
    [InlineData('╭', Element.TopLeft)]
    [InlineData('╮', Element.TopRight)]
    [InlineData('╰', Element.BottomLeft)]
    [InlineData('╯', Element.BottomRight)]
    public void parse_char_to_element(char ch, Element result)
    {
        ch.ToElement().ShouldBe(result);
    }

    [Theory]
    [InlineData(' ')]
    [InlineData('(')]
    [InlineData('J')]
    public void throw_exception_if_invalid_char(char ch)
    {
            Assert.Throws<ArgumentException>(() =>
        {
            ch.ToElement();
        });
    }
}
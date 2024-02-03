using Shouldly;

namespace CapMan.Tests;

public class ElementTests
{
    [Theory]
    [InlineData('.', Element.Dot)]
    [InlineData('O', Element.PowerPill)]
    [InlineData('#', Element.Wall)]
    public void should_parse_char_to_element(char ch, Element result)
    {
        ch.ToElement().ShouldBe(result);
    }

    [Theory]
    [InlineData(' ')]
    [InlineData('(')]
    [InlineData('J')]
    public void should_throw_exception_if_invalid_char(char ch)
    {
            Assert.Throws<ArgumentException>(() =>
        {
            ch.ToElement();
        });
    }
}
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
    [TestCase("text", new[] { "text" })]
    [TestCase("  text  ", new[] { "text" })]
    [TestCase("   d   ", new[] { "d" })]
    [TestCase("hello world", new[] { "hello", "world" })]
    [TestCase("hello    world", new[] { "hello", "world" })]
    [TestCase("''", new[] { "" })]
    [TestCase("", new string[0])]
    [TestCase("\'", new[] { "" })]
    [TestCase("\' ", new[] { " " })]
    [TestCase("\'\"\'", new[] { "\"" })]
    [TestCase("\"\'\"", new[] { "\'" })]
    [TestCase("\"\\\"text\\\"\"", new[] { "\"text\"" })]
    [TestCase("\'\\\'text\\\'\'", new[] { "\'text\'" })]
    [TestCase("'a", new[] { "a" })]
    [TestCase("'a'", new[] { "a" })]
    [TestCase("\\\"abc\"", new[] { "\\", "abc" })]
    [TestCase("b\\\"a\"", new[] { "b\\", "a" })]
    [TestCase("'a'b", new[] { "a", "b" })]
    [TestCase("'\\\\'", new[] { "\\" })]
    [TestCase("\\\\", new[] { "\\\\" })]
    public static void Test(string input, string[] expectedResult)
	{
		var actualResult = FieldsParserTask.ParseLine(input);
		Assert.AreEqual(expectedResult.Length, actualResult.Count);
		for (int i = 0; i < expectedResult.Length; ++i)
		{
			Assert.AreEqual(expectedResult[i], actualResult[i].Value);
		}
	}

	// Скопируйте сюда метод с тестами из предыдущей задачи.
}

public class FieldsParserTask
{
	// При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
	// Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
	public static List<Token> ParseLine(string line)
	{
		if (line == string.Empty || string.IsNullOrWhiteSpace(line))
		{
			return new List<Token>(1);
		}

		List<Token> result = new();
        for (int i = 0; i < line.Length;)
        {
            if (line[i] == ' ')
            {
                i++;
                continue;
            }

            if (line[i] == '\'' || line[i] == '\"')
            {
                result.Add(ReadQuotedField(line, i));
                i = result[^1].GetIndexNextToToken();
            }
            else
            {
                result.Add(ReadField(line, i));
                i = result[^1].GetIndexNextToToken();
            }
        }
        
		
		return result;
		//return new List<Token> { ReadQuotedField(line, 0) }; // сокращенный синтаксис для инициализации коллекции.
	}
        
	private static Token ReadField(string line, int startIndex)
	{
		StringBuilder field = new StringBuilder();
        int currentIndex = startIndex;
        while (currentIndex != line.Length)
        {
            if (line[currentIndex] != '\'' && line[currentIndex] != '\"' && line[currentIndex] != ' ')
            {
                field.Append(line[currentIndex]);
                currentIndex++;
            }
            else
            {
                break;
            }
        }

		return new Token(field.ToString(), startIndex, currentIndex - startIndex);
	}

	public static Token ReadQuotedField(string line, int startIndex)
	{
		return QuotedFieldTask.ReadQuotedField(line, startIndex);
	}
}
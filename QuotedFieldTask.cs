using NUnit.Framework;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
	[TestCase("''", 0, "", 2)]
    [TestCase("\'", 0, "", 1)]
    [TestCase("'a", 0, "a", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase("\"abc\"", 0, "abc", 5)]
    [TestCase("b \"a'\"", 2, "a'", 4)]
    [TestCase("'a'b", 0, "a", 3)]
    [TestCase("a'b'", 1, "b", 3)]
    [TestCase(@"'a\' b'", 0, "a' b", 7)]
    [TestCase(@"some_text ""QF \"""" other_text", 10, "QF \"", 7)]
    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
	{
		var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
		Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
	}

	// Добавьте свои тесты
}

class QuotedFieldTask
{
	public static Token ReadQuotedField(string line, int startIndex)
	{
        if (line.Length == 1)
        {
            return new Token(string.Empty, startIndex, 1);
        }

        StringBuilder tokenText = new StringBuilder();
        char quote = line[startIndex];
        int tokenLength = 1;
        for (int i = startIndex + 1; i < line.Length;)
        {
            if (line[i] == '\\')
            {
                tokenText.Append(line[i + 1]);
                tokenLength += 2;
                i += 2;
                continue;
            }
            if (line[i] == quote)
            {
                tokenLength++;
                break;
            }

            tokenText.Append(line[i]);
            tokenLength ++;
            i++;
        }
        return new Token(tokenText.ToString(), startIndex, tokenLength);
    }
}
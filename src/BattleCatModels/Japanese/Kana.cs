namespace BattleCatModels.Japanese;

public static class Kana
{
    public static string KatakanaToHiragana(string s)
    {
        return string.Create(s.Length, s, static (buffer, s) =>
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = to(s[i]);
            }
        });

        static char to(char c)
        {
            if (c is >= 'ァ' and <= 'ヶ') return (char)(c - 'ァ' + 'ぁ');
            else return c;
        }
    }
}

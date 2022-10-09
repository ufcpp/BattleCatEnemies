using System.Text;

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

    // 今、ひらがなからローマ字を作って、ユーザー入力のローマ字と比較してる。
    // 逆(ユーザー入力のローマ字をひらがなに変換して、Enemy.Kana と比較)の方が精度高いかも。
    //
    // カバちゃんは kabachan か kabatyan か迷う。
    // ch で登録されてるけど、自分は ty の方使ってた。
    public static string HirakanaToRomaji(ReadOnlySpan<char> s)
    {
        static string? oneChar(char c) => c switch
        {
            'ぁ' => "a", 'あ' => "a", 'ぃ' => "i", 'い' => "i", 'ぅ' => "u", 'う' => "u", 'ぇ' => "e", 'え' => "e", 'ぉ' => "o", 'お' => "o",
            'か' => "ka", 'が' => "ga", 'き' => "ki", 'ぎ' => "gi", 'く' => "ku", 'ぐ' => "gu", 'け' => "ke", 'げ' => "ge", 'こ' => "ko", 'ご' => "go",
            'さ' => "sa", 'ざ' => "za", 'し' => "si", 'じ' => "zi", 'す' => "su", 'ず' => "zu", 'せ' => "se", 'ぜ' => "ze", 'そ' => "so", 'ぞ' => "zo",
            'た' => "ta", 'だ' => "da", 'ち' => "ti", 'ぢ' => "di", 'っ' => "tu", 'つ' => "tu", 'づ' => "du", 'て' => "te", 'で' => "de", 'と' => "to", 'ど' => "do",
            'な' => "na", 'に' => "ni", 'ぬ' => "nu", 'ね' => "ne", 'の' => "no",
            'は' => "ha", 'ば' => "ba", 'ぱ' => "pa",
            'ひ' => "hi", 'び' => "bi", 'ぴ' => "pi",
            'ふ' => "hu", 'ぶ' => "bu", 'ぷ' => "pu",
            'へ' => "he", 'べ' => "be", 'ぺ' => "pe",
            'ほ' => "ho", 'ぼ' => "bo", 'ぽ' => "po",
            'ま' => "ma", 'み' => "mi", 'む' => "mu", 'め' => "me", 'も' => "mo",
            'ゃ' => "ya", 'や' => "ya", 'ゅ' => "yu", 'ゆ' => "yu", 'ょ' => "yo", 'よ' => "yo",
            'ら' => "ra", 'り' => "ri", 'る' => "ru", 'れ' => "re", 'ろ' => "ro",
            'ゎ' => "wa", 'わ' => "wa", 'ゐ' => "wi", 'ゑ' => "we", 'を' => "wo",
            'ん' => "n", 'ゔ' => "vu", 'ゕ' => "ka", 'ゖ' => "ke", 'ー' => "-", '～' => "-",
            _ => null,
        };

        static bool twoChar(ReadOnlySpan<char> s, StringBuilder sb)
        {
            var r = s switch
            {
                "ふぁ" => "fa", "ふぃ" => "fi", "ふぇ" => "fe", "ふぉ" => "fo",
                "てぃ" => "thi", "でぃ" => "dhi", "とぅ" => "twu", "どぅ" => "dwu",
                "うぃ" => "wi", "うぇ" => "we", "うぉ" => "wo", // who の方がいい？
                "ゔぁ" => "va", "ゔぃ" => "vi", "ゔぇ" => "ve", "ゔぉ" => "vo",
                "しゃ" => "sha", "しゅ" => "shu", "しぇ" => "she", "しょ" => "sho",
                "じゃ" => "ja", "じゅ" => "ju", "じぇ" => "je", "じょ" => "jo",
                "ちゃ" => "cha", "ちゅ" => "chu", "ちぇ" => "che", "ちょ" => "cho", // tya の方がいいかも？
                _ => null,
            };

            if (r != null)
            {
                sb.Append(r);
                return true;
            }

            var y = s[1] switch
            {
                'ゃ' => "ya",
                'ゅ' => "yu",
                'ょ' => "yo",
                _ => null,
            };

            if (y == null) { return false; }

            var x = s[0] switch
            {
                'き' => "k",
                'ぎ' => "g",
                'ぢ' => "d",
                'に' => "n",
                'ひ' => "h",
                'び' => "b",
                'ぴ' => "p",
                'み' => "m",
                'り' => "r",
                _ => null,
            };

            if (x == null) { return false; }

            sb.Append(x);
            sb.Append(y);
            return true;
        }

        var sb = new StringBuilder();

        for (int i = 0; i < s.Length; i++)
        {
            if (i + 1 < s.Length)
            {
                if (twoChar(s.Slice(i, 2), sb))
                {
                    i++;
                    continue;
                }
                if (s[i] is 'っ' && oneChar(s[i + 1]) is { } r1)
                {
                    // 1個しか先読みしないとちょっと変になるのがある。
                    // っちゃ,tcha (よっちゃん、タッちゃん、マッチョ)
                    // っじゃ,zja (今使ってる範囲では該当なし)
                    //
                    // tcha の方は自分が普段これで打ってるのよしとしようかと。
                    // zja の方はレア？

                    sb.Append(r1[0]);
                    // ここで sb.Appendd(r1); i++; しちゃうと、「っきょ」みたいな場合に変になる。
                    continue;
                }
                if (s[i] is 'ん' && oneChar(s[i + 1]) is { } r2)
                {
                    if (r2[0] is 'a' or 'i' or 'u' or 'e' or 'o' or 'y' or 'n')
                    {
                        // 母音、半母音、な行の前の「ん」だけ nn に。
                        // ne ね、nne んえ、nnne んね で最大3つ並ぶ。
                        // 「巨神ネコ」に「んね」あり。
                        sb.Append("nn");
                        continue;
                    }
                }
            }

            if (oneChar(s[i]) is { } r) sb.Append(r);
            else sb.Append(char.ToLowerInvariant(s[i]));
        }

        return sb.ToString();
    }
}

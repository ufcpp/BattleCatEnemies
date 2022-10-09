using BattleCatModels;

var enemies = Enemy.LoadFromCsv(await File.ReadAllTextAsync("enemies.txt"));

foreach (var e in enemies)
{
    Console.WriteLine($"{e.Id},{e.Name},{e.Kana},{BattleCatModels.Japanese.Kana.HirakanaToRomaji(e.Kana)}");
}

//var kanaChars = enemies.SelectMany(e => e.Kana).Distinct().Order();
//foreach (var c in kanaChars) Console.WriteLine($"{c} {(int)c:X}");

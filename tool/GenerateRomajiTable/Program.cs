using System.Text;

var s = new StringBuilder();

s.Append("""
    public static (int len, string? kana) Match(ReadOnlySpan<char> span) => span switch
    {
    [] => (0, null),

    """);

foreach (var line in Data.List.Split("\n"))
{
    if (line is { Length: 0 }) continue;

    var span = line.AsSpan();
    var i = span.IndexOf(' ');
    var roma = span[..i];
    var kana = span[(i + 1)..];

    s.Append("[");
    foreach (var c in roma)
    {
        s.Append($"""
            '{c}', 
            """);
    }
    s.Append($"""
        ..] => ({roma.Length}, "{kana}"),

        """);
}

s.Append("""
    _ => (-1, null),
    };
    """);

Console.WriteLine(s.ToString());

internal class Data
{
    public const string List = """
a あ
i い
u う
e え
o お

xa ぁ
xi ぃ
xu ぅ
xe ぇ
xo ぉ

xya ゃ
xyi ぃ
xyu ゅ
xye ぇ
xyo ょ

xka ゕ
xke ゖ
xwa ゎ

la ぁ
li ぃ
lu ぅ
le ぇ
lo ぉ

lya ゃ
lyi ぃ
lyu ゅ
lye ぇ
lyo ょ

lka ゕ
lke ゖ
lwa ゎ

ka か
ki き
ku く
ke け
ko こ

kya きゃ
kyi きぃ
kyu きゅ
kye きぇ
kyo きょ

ga が
gi ぎ
gu ぐ
ge げ
go ご

gya ぎゃ
gyi ぎぃ
gyu ぎゅ
gye ぎぇ
gyo ぎょ

gwa ぐぁ
gwi ぐぃ
gwu ぐぅ
gwe ぐぇ
gwo ぐぉ

ca か
ci し
cu く
ce せ
co こ

cha ちゃ
chi ち
chu ちゅ
che ちぇ
cho ちょ

cya ちゃ
cyi ちぃ
cyu ちゅ
cye ちぇ
cyo ちょ

qa くぁ
qi くぃ
qu く
qe くぇ
qo くぉ

qya くゃ
qyi くぃ
qyu くゅ
qye くぇ
qyo くょ

qwa くぁ
qwi くぃ
qwu くぅ
qwe くぇ
qwo くぉ

sa さ
si し
su す
se せ
so そ

sha しゃ
shi し
shu しゅ
she しぇ
sho しょ

sya しゃ
syi しぃ
syu しゅ
sye しぇ
syo しょ

swa すぁ
swi すぃ
swu すぅ
swe すぇ
swo すぉ

za ざ
zi じ
zu ず
ze ぜ
zo ぞ

zya じゃ
zyi じぃ
zyu じゅ
zye じぇ
zyo じょ

ja じゃ
ji じ
ju じゅ
je じぇ
jo じょ

jya じゃ
jyi じぃ
jyu じゅ
jye じぇ
jyo じょ

ta た
ti ち
tu つ
te て
to と

tsa つぁ
tsi つぃ
tsu つ
tse つぇ
tso つぉ

tha てゃ
thi てぃ
thu てゅ
the てぇ
tho てょ

tya ちゃ
tyi ちぃ
tyu ちゅ
tye ちぇ
tyo ちょ

twa とぁ
twi とぃ
twu とぅ
twe とぇ
two とぉ

da だ
di ぢ
du づ
de で
do ど

dha でゃ
dhi でぃ
dhu でゅ
dhe でぇ
dho でょ

dya ぢゃ
dyi ぢぃ
dyu ぢゅ
dye ぢぇ
dyo ぢょ

dwa どぁ
dwi どぃ
dwu どぅ
dwe どぇ
dwo どぉ

na な
ni に
nu ぬ
ne ね
no の

nya にゃ
nyi にぃ
nyu にゅ
nye にぇ
nyo にょ

ha は
hi ひ
hu ふ
he へ
ho ほ

hya ひゃ
hyi ひぃ
hyu ひゅ
hye ひぇ
hyo ひょ

fa ふぁ
fi ふぃ
fu ふ
fe ふぇ
fo ふぉ

fya ふゃ
fyi ふぃ
fyu ふゅ
fye ふぇ
fyo ふょ

fwa ふぁ
fwi ふぃ
fwu ふぅ
fwe ふぇ
fwo ふぉ

ba ば
bi び
bu ぶ
be べ
bo ぼ

bya びゃ
byi びぃ
byu びゅ
bye びぇ
byo びょ

va ゔぁ
vi ゔぃ
vu ゔ
ve ゔぇ
vo ゔぉ

vya ゔゃ
vyi ゔぃ
vyu ゔゅ
vye ゔぇ
vyo ゔょ

pa ぱ
pi ぴ
pu ぷ
pe ぺ
po ぽ

pya ぴゃ
pyi ぴぃ
pyu ぴゅ
pye ぴぇ
pyo ぴょ

ma ま
mi み
mu む
me め
mo も

mya みゃ
myi みぃ
myu みゅ
mye みぇ
myo みょ

ya や
yi い
yu ゆ
ye いぇ
yo よ

ra ら
ri り
ru る
re れ
ro ろ

rya りゃ
ryi りぃ
ryu りゅ
rye りぇ
ryo りょ

wa わ
wi うぃ
wu う
we うぇ
wo を

wha うぁ
whi うぃ
whu う
whe うぇ
who うぉ

nn ん
xn ん

- ー
""";
}

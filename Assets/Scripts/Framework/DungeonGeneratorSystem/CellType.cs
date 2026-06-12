using Framework.Attributes;

namespace Framework.DungeonGeneratorSystem
{
    public enum CellType
    {
        [ColorValue(0.1176471f, 0.1176471f, 0.1803922f)] EMPTY,
        [ColorValue(0.6509804f, 0.5411765f, 0.7176471f)] NORMAL,
        [ColorValue(0, 1, 1)] START,
        [ColorValue(0, 1, 0)] END,
        [ColorValue(1, 0, 0)] NEGATIVE,
        [ColorValue(1, 1, 0)] POSITIVE,
        [ColorValue(0.6222224f, 0, 1, 0.023529412f)] SECRET,
        [ColorValue(1, 0.56296873f, 0)] BOSS
    }
}
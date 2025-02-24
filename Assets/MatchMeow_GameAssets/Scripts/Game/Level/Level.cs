using Newtonsoft.Json.Linq;

public class Level
{
    private int _id;
    private int _width;
    private int _height;
    private int[][] _map;
    private int _maxValue;

    public int Id => _id;
    public int Width => _width;
    public int Height => _height;
    public int[][] Map => _map;
    public int MaxValue => _maxValue;
    public Level(int id, int width, int height, int[][] map, int maxValue)
    {
        _id = id;
        _width = width;
        _height = height;
        _map = map;
        _maxValue = maxValue;
    }

    public override string ToString()
    {
        return "Id: " + _id +
               ". Width" + _width +
               ", Height: " + _height +
               ", Map" + _map +
               ", MaxValue: " + _maxValue;
    }
}

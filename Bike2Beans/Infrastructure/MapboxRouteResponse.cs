namespace Bike2Beans.Infrastructure;

public class MapboxRouteResponse
{
    public string Code { get; set; } = string.Empty;
    public List<MapboxRoute> Routes { get; set; } = new();
    public List<MapboxWaypoint> Waypoints { get; set; } = new();
}
public class MapboxRoute
{
    public double Distance { get; set; }
    public double Duration { get; set; }
    public MapboxGeometry Geometry { get; set; } = new();
    public List<MapboxLeg> Legs { get; set; } = new();
}
public class MapboxGeometry
{
    public string Type { get; set; } = string.Empty;

    // IMPORTANT:
    // Coordinates are [lng, lat]
    public List<List<double>> Coordinates { get; set; } = new();
}
public class MapboxLeg
{
    public double Distance { get; set; }
    public double Duration { get; set; }
    public List<MapboxStep> Steps { get; set; } = new();
}
public class MapboxWaypoint
{
    public string Name { get; set; } = string.Empty;
    public List<double> Location { get; set; } = new();
}
public class MapboxStep
{
    public double Distance { get; set; }
    public double Duration { get; set; }
    public string Name { get; set; } = string.Empty;

    public MapboxManeuver Maneuver { get; set; } = new();
    public MapboxGeometry Geometry { get; set; } = new();
}

public class MapboxManeuver
{
    public string Instruction { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Modifier { get; set; } // "left", "right", "slight left", etc.
    public List<double> Location { get; set; } = new(); // [lng, lat]
}
using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public struct Shadow
    {
        public float startingSlope;
        public float endingSlope;

        public Shadow(float start, float end)
        {
            startingSlope = start;
            endingSlope = end;
        }
    }

    public class Fov
    {
        const int FOV_MAX = 10;

        private Map _map;

        public Fov(Map map)
        {
            _map = map;
        }

        public void UpdateFov(int cx, int cy)
        {
            _map.ToggleMapVisible(false);

            _map.ToggleTileVisible(cx, cy, true);
            _map.ToggleTileVisited(cx, cy, true);

            for (int sector = 1; sector <= 8; sector++)
            {
                //Shadows for each sector
                List<Shadow> shadows = new List<Shadow>();
                float shadowStart = 0f;
                float shadowEnd = 0f;
                bool prevBlocking = false;

                for (int y = 0; y < FOV_MAX; y++)
                {
                    prevBlocking = false;
                    for (int x = 0; x <= y; x++)
                    {
                        var mapPoint = GetMapPointForLocalFov(sector, cx, cy, x, y);
                        int mapX = mapPoint.X;
                        int mapY = mapPoint.Y;

                        //Check if the translated point is in the map range
                        if (!_map.IsWithinMapRange(mapX, mapY) ||
                            !IsWithinFovRange(x,y))
                        {
                            continue;
                        }

                        var slope = CalculateSlope(0, x, 0, y);
                        //System.Console.WriteLine($"{x} {y} Slope = {slope}");
                        //If the point is inside any existing shadows, then its hidden
                        if (InsideShadow(slope, shadows))
                        {
                            _map.ToggleTileVisible(mapX, mapY, false);
                            //_tiles[mapX, mapY].Visited = false;
                            //_tiles[x, y].UpdateTile(new Character(Glyphs.Fill, Color.Red), TileType.Solid);
                            continue;
                        }
                        //If we reached here then the point is visible 
                        _map.ToggleTileVisible(mapX, mapY, true);
                        _map.ToggleTileVisited(mapX, mapY, true);
                        //Check if its a wall
                        if (_map.GetTileType(mapX, mapY) == TileType.Solid &&
                            _map.GetTileType(mapX, mapY) != TileType.Transparent)
                        {
                            if (!prevBlocking)
                            {
                                //Start new shadow
                                shadowStart = CalculateSlope(0, x, 0, y);
                                prevBlocking = true;
                            }
                        }
                        else //not a wall
                        {
                            //if previous point was blocking (wall) and this is not
                            //then that means this is the end of the wall the shadow end slope should be calculated
                            if (prevBlocking)
                            {
                                shadowEnd = CalculateSlope(0, x + 0.5f, 0, y);
                                Shadow s = new Shadow(shadowStart, shadowEnd);
                                shadows.Add(s);
                            }
                        }
                    }

                    //Check for any open shadows. i.e entire row is a wall
                    if (prevBlocking)
                    {
                        shadowEnd = CalculateSlope(0, y + 0.5f, 0, y);
                        Shadow s = new Shadow(shadowStart, shadowEnd);
                        shadows.Add(s);
                        //System.Console.WriteLine("open shadow");
                    }
                }
            }
        }

        private Vec2Int GetMapPointForLocalFov(int sector, int cx, int cy, int x, int y)
        {
            return sector switch
            {
                1 => new Vec2Int(cx + x, cy - y),
                2 => new Vec2Int(cx + y, cy - x),
                3 => new Vec2Int(cx + y, cy + x),
                4 => new Vec2Int(cx + x, cy + y),
                5 => new Vec2Int(cx - x, cy + y),
                6 => new Vec2Int(cx - y, cy + x),
                7 => new Vec2Int(cx - y, cy - x),
                8 => new Vec2Int(cx - x, cy - y),
                _ => throw new ArgumentOutOfRangeException(nameof(sector))
            };
        }

        private float CalculateSlope(float x1, float x2, float y1, float y2)
        {
            if (x2 - x1 < 0)
            {
                return 0;
            }
            return (x2 - x1) / (y2 - y1);
        }

        private bool IsWithinFovRange(int x, int y)
        {
            return x * x + y * y <= FOV_MAX * FOV_MAX;
        }

        private bool InsideShadow(float slope, List<Shadow> shadows)
        {
            foreach (var s in shadows)
            {
                if (slope >= s.startingSlope && slope <= s.endingSlope)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

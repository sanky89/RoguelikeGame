using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RoguelikeGame
{
    public class MapConsole : Console
    {
        public Vector2 Position => new Vector2(_x+1, _y+1);
        public Vector2 offset = Vector2.Zero;


        public MapConsole( string title, int width, int height, ConsoleLocation location, BorderStyle border, Color borderColor) : 
            base(title, width, height, location, border, borderColor)
        {
           
        }

        public void CheckScrollMap(Direction direction)
        {
            if(direction == Direction.RIGHT && Width - Globals.Map.Player.MapX <= 5)
            {
                ScrollMap(Direction.RIGHT);
            }
            if (direction == Direction.LEFT && offset.X + Globals.Map.Player.MapX <= 5)
            {
                ScrollMap(Direction.LEFT);
            }
            if (direction == Direction.UP && offset.Y + Globals.Map.Player.MapY <= 5)
            {
                ScrollMap(Direction.DOWN);
            }
            if (direction == Direction.DOWN && Height - Globals.Map.Player.MapY <= 5)
            {
                ScrollMap(Direction.UP);
            }
        }

        public void ScrollMap(Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    offset.X++;
                    offset.X = Math.Min(0, (int)offset.X);
                    break;
                case Direction.RIGHT:
                    offset.X--;
                    offset.X = Math.Max(offset.X, (Width - Map.COLS - 1));
                    break;
                case Direction.UP:
                    offset.Y--;
                    offset.Y = Math.Max(offset.Y, (Height - Map.ROWS - 1));
                    break;
                case Direction.DOWN:
                    offset.Y++;
                    offset.Y = Math.Min(0, (int)offset.Y);
                    break;
                default:
                    break;
            }
        }

        public override void Draw()
        {
            int maxWidth = Math.Min(Width-1, Map.COLS);
            int maxHeight = Math.Min(Height-1, Map.ROWS);
            int startX = (int)-offset.X;
            int startY = (int)-offset.Y;
            int endX = Math.Min(maxWidth + startX, Map.COLS);
            int endY = Math.Min(maxHeight + startY, Map.ROWS);
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    var tile = Globals.Map.GetTileAtIndex(x, y);

                    if (Globals.Map.IsPlayerTile(x,y))
                    {
                        Globals.SpriteBatch.Draw(Globals.GlyphsTexture, 
                            (Position + offset + new Vector2(x, y)) * Globals.TILE_SIZE * Globals.SCALE,
                            Globals.Map.Player.SourceRect,
                            Globals.Map.Player.Color,
                            0f,
                            Vector2.Zero,
                            Globals.SCALE,
                            SpriteEffects.None,
                            0);
                    }
                    else if(Globals.Map.IsMonsterTile(x, y) && tile.Visible)
                    {
                        Globals.SpriteBatch.Draw(Globals.GlyphsTexture,
                            (Position + offset + new Vector2(x, y)) * Globals.TILE_SIZE * Globals.SCALE,
                            Globals.Map.Monster.SourceRect,
                            Globals.Map.Monster.Color,
                            0f,
                            Vector2.Zero,
                            Globals.SCALE,
                            SpriteEffects.None,
                            0);
                    }
                    else
                    {
                        if (!tile.Visible && !tile.Visited)
                        {
                            continue;
                        }

                        Globals.SpriteBatch.Draw(Globals.GlyphsTexture,
                            (Position + offset + new Vector2(x,y))  * Globals.TILE_SIZE * Globals.SCALE, tile.SourceRect,
                            tile.DisplayColor,
                            0f,
                            Vector2.Zero,
                            Globals.SCALE,
                            SpriteEffects.None,
                            0);
                    }
                }
            }
            base.Draw();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RoguelikeGame
{
    public class MapConsole : Console
    {
        public Vector2 Position => new Vector2(_x+1, _y+1);
        public Vector2 offset = Vector2.Zero;
        private Texture2D _debugRect;

        public bool ShowDebugOverlay = false;

        public MapConsole( string title, int width, int height, ConsoleLocation location, BorderStyle border, Color borderColor) : 
            base(title, width, height, location, border, borderColor)
        {
            var playerMapPosition = Globals.Map.PlayerMapPosition;
            var midX = Width/2;
            var midY = Height/2;
            if (playerMapPosition.X >= Width)
            { 
                offset.X = playerMapPosition.X - midX;
            }
            if (playerMapPosition.Y >= Height)
            {
                offset.Y = playerMapPosition.Y - midY;
            }

            _debugRect = new Texture2D(Globals.GraphicsDevice, 1, 1);
            _debugRect.SetData(new Color[] { Color.Green });
        }

        public void CheckScrollMap(InputAction inputAction)
        {

            if ((inputAction == InputAction.MOVE_RIGHT ||
                inputAction == InputAction.MOVE_NE ||
                inputAction == InputAction.MOVE_SE) 
                && Width - Globals.Map.Player.MapX + offset.X  <= 5)
            {
                ScrollMap(Direction.RIGHT);
            }
            if ((inputAction == InputAction.MOVE_LEFT ||
                inputAction == InputAction.MOVE_NW ||
                inputAction == InputAction.MOVE_SW) &&
                Globals.Map.Player.MapX - offset.X <= 5)
            {
                ScrollMap(Direction.LEFT);
            }
            if ((inputAction == InputAction.MOVE_UP ||
                inputAction == InputAction.MOVE_NW ||
                inputAction == InputAction.MOVE_NE) &&
                Globals.Map.Player.MapY - offset.Y <= 5)
            {
                ScrollMap(Direction.DOWN);
            }
            if ((inputAction == InputAction.MOVE_DOWN ||
                inputAction == InputAction.MOVE_SW ||
                inputAction == InputAction.MOVE_SE) &&
                Height - Globals.Map.Player.MapY + offset.Y <= 5)
            {
                ScrollMap(Direction.UP);
            }
        }

        public void ScrollMap(Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    offset.X--;
                    offset.X = Math.Max(0, (int)offset.X);
                    break;
                case Direction.RIGHT:
                    offset.X++;
                    offset.X = Math.Min(offset.X, Globals.Map.Cols);
                    break;
                case Direction.UP:
                    offset.Y++;
                    offset.Y = Math.Min(offset.Y, Globals.Map.Rows);
                    break;
                case Direction.DOWN:
                    offset.Y--;
                    offset.Y = Math.Max(0, (int)offset.Y);
                    break;
                default:
                    break;
            }
        }

        public override void Draw()
        {
            int startX = (int)offset.X;
            int startY = (int)offset.Y;
            int endX = startX + Width;
            if(endX > Globals.Map.Cols)
            {
                endX = Globals.Map.Cols;
            }
            int endY = startY + Height;
            if(endY > Globals.Map.Rows)
            {
                endY = Globals.Map.Rows;
            }
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    var tile = Globals.Map.GetTileAtIndex(x, y);
                    if (!tile.Visible && !tile.Visited)
                    {
                        continue;
                    }

                    Globals.SpriteBatch.Draw(Globals.SpriteSheet,
                        (Position - offset + new Vector2(x, y)) * Globals.TILE_SIZE * Scale, 
                        tile.SourceRect,
                        tile.DisplayColor,
                        0f,
                        Vector2.Zero,
                        Scale,
                        SpriteEffects.None,
                        0);

                    if (Globals.Map.IsPlayerTile(x,y))
                    {
                        Globals.SpriteBatch.Draw(Globals.SpriteSheet, 
                            (Position - offset + new Vector2(x, y)) * Globals.TILE_SIZE * Scale,
                            Globals.Map.Player.SourceRect,
                            Globals.Map.Player.Color,
                            0f,
                            Vector2.Zero,
                            Scale,
                            SpriteEffects.None,
                            0);
                    }
                    else if(Globals.Map.IsMonsterTile(x, y, out var m) && tile.Visible)
                    {
                        Globals.SpriteBatch.Draw(Globals.SpriteSheet,
                            (Position - offset + new Vector2(x, y)) * Globals.TILE_SIZE * Scale,
                            m.SourceRect,
                            m.Color,
                            0f,
                            Vector2.Zero,
                            Scale,
                            SpriteEffects.None,
                            0);
                    }
                    else if(Globals.Map.ContainsItem(x,y, out var item) && tile.Visible)
                    {
                        Globals.SpriteBatch.Draw(Globals.SpriteSheet,
                            (Position - offset + new Vector2(x, y)) * Globals.TILE_SIZE * Scale,
                            item.SourceRect,
                            item.Color,
                            0f,
                            Vector2.Zero,
                            Scale,
                            SpriteEffects.None,
                            0);
                    }

                    if(ShowDebugOverlay && Globals.Map.Pathfinder.GetNodeCost(y,x) == 1000)
                    {
                        var location = (Position - offset + new Vector2(x, y)) * Globals.TILE_SIZE;
                        Globals.SpriteBatch.Draw(_debugRect, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(Globals.TILE_SIZE, Globals.TILE_SIZE)), Color.White);
                    }
                }
            }

            base.Draw();
        }
    }
}

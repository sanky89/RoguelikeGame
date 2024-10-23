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
        private Map _map;
        private GameRoot _gameRoot;
        private Texture2D _spriteSheet;

        public bool ShowDebugOverlay = false;

        public MapConsole(GameRoot gameRoot, Texture2D spriteSheet, Texture2D asciiTexture, string title, int width, int height, ConsoleLocation location, BorderStyle border, Color borderColor) : 
            base(gameRoot, asciiTexture, title, width, height, location, border, borderColor)
        {
            _gameRoot = gameRoot;
            _map = gameRoot.Map;
            _spriteSheet = spriteSheet;
            var playerMapPosition = _map.PlayerMapPosition;
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

            //_debugRect = new Texture2D(_gameRoot.GraphicsDevice, 1, 1);
            //_debugRect.SetData(new Color[] { Color.White });
        }

        public void CheckScrollMap(InputAction inputAction)
        {

            if ((inputAction == InputAction.MOVE_RIGHT ||
                inputAction == InputAction.MOVE_NE ||
                inputAction == InputAction.MOVE_SE) 
                && Width - _map.Player.MapX + offset.X  <= 5)
            {
                ScrollMap(Direction.RIGHT);
            }
            if ((inputAction == InputAction.MOVE_LEFT ||
                inputAction == InputAction.MOVE_NW ||
                inputAction == InputAction.MOVE_SW) &&
                _map.Player.MapX - offset.X <= 5)
            {
                ScrollMap(Direction.LEFT);
            }
            if ((inputAction == InputAction.MOVE_UP ||
                inputAction == InputAction.MOVE_NW ||
                inputAction == InputAction.MOVE_NE) &&
                _map.Player.MapY - offset.Y <= 5)
            {
                ScrollMap(Direction.DOWN);
            }
            if ((inputAction == InputAction.MOVE_DOWN ||
                inputAction == InputAction.MOVE_SW ||
                inputAction == InputAction.MOVE_SE) &&
                Height - _map.Player.MapY + offset.Y <= 5)
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
                    offset.X = Math.Min(offset.X, _map.Cols);
                    break;
                case Direction.UP:
                    offset.Y++;
                    offset.Y = Math.Min(offset.Y, _map.Rows);
                    break;
                case Direction.DOWN:
                    offset.Y--;
                    offset.Y = Math.Max(0, (int)offset.Y);
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int startX = (int)offset.X;
            int startY = (int)offset.Y;
            int endX = startX + Width;
            if(endX > _map.Cols)
            {
                endX = _map.Cols;
            }
            int endY = startY + Height;
            if(endY > _map.Rows)
            {
                endY = _map.Rows;
            }
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    var tile = _map.GetTileAtIndex(x, y);
                    if (!tile.Visible && !tile.Visited)
                    {
                        continue;
                    }

                    _gameRoot.SpriteBatch.Draw(_spriteSheet,
                        (Position - offset + new Vector2(x, y)) * GameConstants.TILE_SIZE * Scale, 
                        tile.SourceRect,
                        tile.DisplayColor,
                        0f,
                        Vector2.Zero,
                        Scale,
                        SpriteEffects.None,
                        0);

                    if (_map.IsPlayerTile(x,y))
                    {
                        var facing = _map.Player.Facing;
                        var flip = facing == Direction.RIGHT ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                        spriteBatch.Draw(_spriteSheet, 
                            (Position - offset + new Vector2(x, y)) * GameConstants.TILE_SIZE * Scale,
                            _map.Player.SourceRect,
                            _map.Player.Color,
                            0f,
                            Vector2.Zero,
                            Scale,
                            flip,
                            0);
                    }
                    else if(_map.IsMonsterTile(x, y, out var m) && tile.Visible)
                    {
                        var flip = m.Facing == Direction.RIGHT ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                        spriteBatch.Draw(_spriteSheet,
                            (Position - offset + new Vector2(x, y)) * GameConstants.TILE_SIZE * Scale,
                            m.SourceRect,
                            m.Color,
                            0f,
                            Vector2.Zero,
                            Scale,
                            flip,
                            0);
                    }
                    else if(_map.ContainsItem(x,y, out var item) && tile.Visible)
                    {
                        spriteBatch.Draw(_spriteSheet,
                            (Position - offset + new Vector2(x, y)) * GameConstants.TILE_SIZE * Scale,
                            item.SourceRect,
                            item.Color,
                            0f,
                            Vector2.Zero,
                            Scale,
                            SpriteEffects.None,
                            0);
                    }

                    if(ShowDebugOverlay)
                    {
                        //var location = (Position - offset + new Vector2(x, y)) * Globals.TILE_SIZE;
                        //var color = Globals.Map.Pathfinder.GetNodeCost(y, x) == 1000 ? new Color(Color.Red, 80) : new Color(Color.Green, 80) ;
                        //var color = tile.TileType == TileType.Walkable ? Color.Green : Color.Red;
                        //Globals.SpriteBatch.Draw(_debugRect, new Rectangle(new Point((int)location.X, (int)location.Y), new Point(Globals.TILE_SIZE-1, Globals.TILE_SIZE-1)), color);
                    }
                }
            }

            base.Draw(spriteBatch);
        }
    }
}

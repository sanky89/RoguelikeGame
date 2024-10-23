using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGame
{
    public class StatsConsole : Console
    {
        private Stats _playerStats;
        private int _lineNumber = 0;
        private const int SPACING = 20;

        public StatsConsole(GameRoot gameRoot, Texture2D asciiTexture, string title, int width, int height, ConsoleLocation location, BorderStyle border = BorderStyle.None, Color borderColor = default) : base(gameRoot, asciiTexture, title, width, height, location, border, borderColor)
        {
            _playerStats = _gameRoot.Map.Player.Stats;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawStats(spriteBatch, _playerStats, Color.White);

            _lineNumber += 2;
            for (int i = 0; i < _gameRoot.Map.VisibleMonsters.Count; i++)
            {
                Monster m = _gameRoot.Map.VisibleMonsters[i];
                DrawStats(spriteBatch, m.Stats, Color.Red, m.Name);
            }
            _lineNumber = 0;
            base.Draw(spriteBatch);
        }

        private void DrawStats(SpriteBatch spriteBatch, Stats stats, Color color, string overrideDisplayId = null)
        {
            foreach (var kvp in stats)
            {
                var stat = kvp.Value;
                if (!stat.ShouldDisplayStat())
                {
                    continue;
                }
                var text = $"{overrideDisplayId ?? stat.Name}: {stat.CurrentValue}/{stat.MaxValue}";
                spriteBatch.DrawString(_gameRoot.Font,
                    text,
                    new Vector2((_x+2) * GameConstants.ASCII_SIZE, (_y+2) * GameConstants.ASCII_SIZE + _lineNumber++ * SPACING),
                    color);
            }
        }
    }
}

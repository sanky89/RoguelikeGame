using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGame
{
    public class StatsConsole : Console
    {
        private Stats _playerStats;
        private int _lineNumber = 0;
        private const int SPACING = 20;


        public StatsConsole(string title, int width, int height, ConsoleLocation location, BorderStyle border = BorderStyle.None, Color borderColor = default) : base(title, width, height, location, border, borderColor)
        {
            _playerStats = Globals.Map.Player.Stats;
        }

        public override void Draw()
        {
            base.Draw();
            DrawStats(_playerStats, Color.White);

            _lineNumber += 2;
            for (int i = 0; i < Globals.Map.VisibleMonsters.Count; i++)
            {
                Monster m = Globals.Map.VisibleMonsters[i];
                DrawStats(m.Stats, Color.Red, m.Name);
            }
            _lineNumber = 0;
        }

        private void DrawStats(Stats stats, Color color, string overrideDisplayId = null)
        {
            foreach (var kvp in stats)
            {
                var stat = kvp.Value;
                if (!stat.ShouldDisplayStat())
                {
                    continue;
                }
                var text = $"{overrideDisplayId ?? stat.Name}: {stat.CurrentValue}/{stat.MaxValue}";
                Globals.SpriteBatch.DrawString(Globals.Font,
                    text,
                    new Vector2((_x+2) * Globals.ASCII_SIZE, (_y+2) * Globals.ASCII_SIZE + _lineNumber++ * SPACING),
                    color);
            }
        }
    }
}

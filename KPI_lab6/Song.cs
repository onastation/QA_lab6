namespace KPI_lab6
{
    public class Song
    {
        public string Artist { get; set; }
        public string SongName { get; set; }
        public int Duration { get; set; }

        public override string ToString()
        {
            return $"{Artist} {SongName} {Duration}";
        }

        public override bool Equals(object? other)
        {
            var toCompareWith = other as Song;
            if (toCompareWith == null)
                return false;
            return Artist == toCompareWith.Artist && SongName == toCompareWith.SongName && Duration == toCompareWith.Duration;
        }
    }
}
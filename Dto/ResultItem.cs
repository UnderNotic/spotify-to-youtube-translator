namespace SpotifyToYoutubeTranslator.Dto
{
    public class ResultItem
    {

        public ResultItem(string name, string artist, string album, string ytId)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Url = $"https://www.youtube.com/watch?v={ytId}";
        }

        public string Name { get; }
        public string Artist { get; }
        public string Album { get; }
        public string Url { get; }
    }
}
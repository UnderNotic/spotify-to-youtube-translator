public class YoutubeItems{
    public YoutubeItem[] Items { get; set; }
}

public class YoutubeItem{
    public YoutubeId Id { get; set; }
}

public class YoutubeId{
    public string VideoId { get; set; }
}
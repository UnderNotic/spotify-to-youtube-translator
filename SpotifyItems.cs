public class SpotifyItems
{
    public SpotifyItem[] Items { get; set; }
}


public class SpotifyItem
{
    public SpotifyTrack Track { get; set; }
}

public class SpotifyTrack
{
    public string Name { get; set; }
    public SpotifyAlbum Album { get; set; }
    
    public SpotifyArtist[] Artists { get; set; }
}

public class SpotifyAlbum
{
    public string Name { get; set; }
}

public class SpotifyArtist{
public string Name { get; set; }
}


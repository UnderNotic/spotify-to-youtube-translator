using System;

namespace SpotifyToYoutubeTranslator.Function
{
    public class SpotifyToken
    {
        public string Token { get; }
        public DateTime ExpiryDate { get; }

        public SpotifyToken(string token,  int expiresIn)
        {
            Token = token;
            ExpiryDate = DateTime.UtcNow.Add(TimeSpan.FromSeconds(expiresIn));
        }
    }
}
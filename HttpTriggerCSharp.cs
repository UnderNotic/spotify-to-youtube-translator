using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Linq;
using SpotifyToYoutubeTranslator.Dto;

namespace SpotifyToYoutubeTranslator.Function
{
    public static class HttpTriggerCSharp
    {
        private static HttpClient _httpClient = new HttpClient();
        private static readonly string SPOTIFY_APP_KEY = System.Environment.GetEnvironmentVariable("SPOTIFY_APP_KEY");
        private static readonly string YOUTUBE_APP_KEY = System.Environment.GetEnvironmentVariable("YOUTUBE_APP_KEY");

        [FunctionName("HelloWorld")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string playlist = req.Query["playlist"]; //2GK6j1Wh1NfmIPcFVXC97tX

            SpotifyItems spotifyItems;
            try
            {
                spotifyItems = await GetSpotifyItems(req, playlist);
            }
            catch (HttpRequestException ex)
            {
                return new BadRequestObjectResult($"Cannot get playlist info from spotify - {ex.Message}");
            }

            var resultItems = spotifyItems.Items.Select(async track =>
            {
                var youtubeItems = await GetYoutubeItems(track);

                return new ResultItem(track.Track.Name, track.Track.Artists[0].Name, track.Track.Album.Name, youtubeItems.Items[0].Id.VideoId);
            });

            return (ActionResult)new OkObjectResult(await Task.WhenAll(resultItems));
        }

        private static async Task<YoutubeItems> GetYoutubeItems(SpotifyItem track)
        {
            var youtubeRequest = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/youtube/v3/search?q={track.Track.Artists[0].Name}-{track.Track.Name}&type=video&part=id&maxResults=1&key={YOUTUBE_APP_KEY}");

            var youtubeRequestResponse = await _httpClient.SendAsync(youtubeRequest);
            var youtubeItems = await youtubeRequestResponse.Content.ReadAsAsync<YoutubeItems>();
            return youtubeItems;
        }

        private static async Task<SpotifyItems> GetSpotifyItems(HttpRequest req, string playlist)
        {
            var tokenHeaders = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            tokenRequest.Content = tokenHeaders;
            tokenRequest.Headers.Add("Authorization", SPOTIFY_APP_KEY);

            var tokenRequestResponse = await _httpClient.SendAsync(tokenRequest);

            var tokenRequestBody = await tokenRequestResponse.Content.ReadAsAsync<dynamic>();
            var token = tokenRequestBody["access_token"].ToString();

            var playlistQuery = HttpUtility.ParseQueryString(string.Empty);
            playlistQuery["fields"] = "items(track(name,artists, album(name))), next";

            string playlistQueryString = playlistQuery.ToString();
            var playlistRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/playlists/{playlist}/tracks?{playlistQueryString}");
            playlistRequest.Headers.Add("Authorization", $"Bearer {token}");

            var playlistRequestResponse = await _httpClient.SendAsync(playlistRequest);
            playlistRequestResponse.EnsureSuccessStatusCode();

            var spotifyItems = await playlistRequestResponse.Content.ReadAsAsync<SpotifyItems>();
            return spotifyItems;
        }
    }
}

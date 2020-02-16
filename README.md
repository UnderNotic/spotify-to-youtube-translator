# spotify-to-youtube-translator

Transform spotify playlist to youtube list of links

## QuickStart

- setup .net core (version 3.1 >= required)
- install Azure Functions Core Tools
  https://docs.microsoft.com/pl-pl/azure/azure-functions/functions-run-local
- add to `local.settings.json` api keys: 

```json
    "Values": {
        "AzureWebJobsStorage": "",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "SPOTIFY_APP_KEY": "Basic YourSpotifyAppKey",
        "YOUTUBE_APP_KEY": "YourYoutubeAppKey"
    }
```
- run `func start`
- call http://localhost:7071/api/map?playlist=YourSpotifyPlaylistId

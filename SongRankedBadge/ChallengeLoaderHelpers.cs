using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

internal static class ChallengeLoaderHelpers
{
    public static HashSet<string> LoadAllChallengePlaylists(string directory)
    {
        var hashset = new HashSet<string>();
        if (Directory.Exists(directory))
        {
            foreach (var file in Directory.GetFiles(directory, "*__Challenge Saber*"))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var jObject = JObject.Parse(json);
                    var songs = GetSongs(jObject);
                    if (songs != null)
                    {
                        foreach (var song in songs)
                        {
                            var songHash = song["hash"]?.ToString();
                            if (!string.IsNullOrEmpty(songHash))
                            {
                                hashset.Add(songHash!.ToLower());
                            }
                        }
                    }
                }
                catch
                {
                    // Ignore malformed files
                }
            }
            return hashset;
        }
        return hashset;
    }

    private static JArray? GetSongs(JObject jObject)
    {
        return jObject["songs"] as JArray;
    }
}
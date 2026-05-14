using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

public static class ChallengeLoader
{
    public static HashSet<string> ChallengesHashes = new HashSet<string>();

    public static HashSet<string> LoadAllChallengePlaylists(string directory)
    {
        var hashset = new HashSet<string>();

        if (Directory.Exists(directory))
        {
            foreach (var file in Directory.GetFiles(directory, "*_Challenge Saber.bplist"))
            {
                var json = File.ReadAllText(file);
                var jObject = JObject.Parse(json);

               
                if (jObject["songs"] is JArray songs)
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
        }

        ChallengesHashes = hashset;
        System.Console.WriteLine($"[SongRankedBadge] Loaded {hashset.Count} challenge songs.");
        return hashset;
    }
}
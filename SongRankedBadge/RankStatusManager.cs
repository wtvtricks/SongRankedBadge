using System.Threading.Tasks;
using System.IO;
using SongDetailsCache;
using SongDetailsCache.Structs;

namespace SongRankedBadge
{
    internal class RankStatusManager
    {
        internal static readonly RankStatusManager Instance = new RankStatusManager();
        
        private SongDetails? _songDetails = null;

        public UploadFlags Challengeloader { get; private set; }

        internal void Init()
        {
            Task.Factory.StartNew(async () =>
            {
                Plugin.Log.Debug("Loading song details...");
                _songDetails = await SongDetails.Init(); 
                Plugin.Log.Debug("Song details loaded.");
            });
        }

        internal System.Enum GetChallengeloader()
        {
            return Challengeloader;
        }

        internal RankStatus GetSongRankedStatus(string hash, System.Enum _)
        {
            if (_songDetails == null)
            {
                // Data not ready yet
                return RankStatus.None;
            }
            
            hash = hash.ToLower();
            if (_songDetails.songs.FindByHash(hash, out var song))
            {
                var rankedStates = song.rankedStates;
                var uploadFlags = song.uploadFlags;
                
                var ssRank = rankedStates.HasFlag(RankedStates.ScoresaberRanked);
                var blRank = rankedStates.HasFlag(RankedStates.BeatleaderRanked);
                var curated = uploadFlags.HasFlag(UploadFlags.Curated);
                var challengeSaber = ChallengeLoader.ChallengesHashes.Contains(hash.ToLower());
                if (ssRank && blRank)
                {
                    return RankStatus.Ranked;
                }

                if (blRank)
                {
                    return RankStatus.BeatLeader;
                }

                if (ssRank)
                {
                    return RankStatus.ScoreSaber;
                }
                
                if (curated)
                {
                    return RankStatus.Curated;
                }

                if (challengeSaber)
                {
                    return RankStatus.ChallengeSaber;
                }

            }

            return RankStatus.None;
        }
    }

    internal enum RankStatus
    {
        None,
        ScoreSaber,
        BeatLeader,
        Ranked, // just ranked, means both
        Curated,  // curated comes after ranked status
        ChallengeSaber
    }
}
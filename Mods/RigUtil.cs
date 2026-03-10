using ExitGames.Client.Photon;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaTagScripts;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cryptic.Menu
{
    public class RandomUtilities
    {
        /// <summary>
        /// Generates a Vector3 with each component set to a random value between -range and range.
        /// </summary>
        /// <param name="range">The maximum absolute value for each component of the generated vector.</param>
        /// <returns>A Vector3 with random x, y, and z components within the specified range.</returns>
        public static Vector3 RandomVector3(float range = 1f) =>
            new Vector3(Random.Range(-range, range),
                        Random.Range(-range, range),
                        Random.Range(-range, range));

        /// <summary>
        /// Generates a random Quaternion with each Euler angle component in the range [0, range).
        /// </summary>
        /// <param name="range">The upper bound (exclusive) for each Euler angle in degrees. Defaults to 360.</param>
        /// <returns>A Quaternion with random rotation based on the specified range.</returns>
        public static Quaternion RandomQuaternion(float range = 360f) =>
            Quaternion.Euler(Random.Range(0f, range),
                        Random.Range(0f, range),
                        Random.Range(0f, range));

        /// <summary>
        /// Generates a random color with each RGB component in the range [0, range) and the specified alpha value.
        /// </summary>
        /// <param name="range">The exclusive upper bound for the random RGB component values.</param>
        /// <param name="alpha">The alpha (transparency) value of the generated color.</param>
        /// <returns>A randomly generated Color with the specified range and alpha.</returns>
        public static Color RandomColor(byte range = 255, byte alpha = 255) =>
            new Color32((byte)Random.Range(0, range),
                        (byte)Random.Range(0, range),
                        (byte)Random.Range(0, range),
                        alpha);

        /// <summary>
        /// Generates a random alphanumeric string of the specified length using uppercase letters and digits.
        /// </summary>
        /// <param name="length">The length of the generated string. Defaults to 4.</param>
        /// <returns>A random string consisting of uppercase letters and digits.</returns>
        public static string RandomString(int length = 4)
        {
            string random = "";
            for (int i = 0; i < length; i++)
            {
                int rand = Random.Range(0, 36);
                char c = rand < 26
                    ? (char)('A' + rand)
                    : (char)('0' + (rand - 26));
                random += c;
            }

            return random;
        }
    }
    public class GameModeUtilities
    {
        public static List<NetPlayer> InfectedList()
        {
            List<NetPlayer> infected = new List<NetPlayer>();

            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null)
                return infected;

            switch (GorillaGameManager.instance.GameType())
            {
                case GameModeType.Infection:
                case GameModeType.InfectionCompetitive:
                case GameModeType.SuperInfect:
                case GameModeType.FreezeTag:
                case GameModeType.PropHunt:
                    GorillaTagManager tagManager = (GorillaTagManager)GorillaGameManager.instance;
                    if (tagManager.isCurrentlyTag)
                        infected.Add(tagManager.currentIt);
                    else
                        infected.AddRange(tagManager.currentInfected);
                    break;
                case GameModeType.Ghost:
                case GameModeType.Ambush:
                    GorillaAmbushManager ghostManager = (GorillaAmbushManager)GorillaGameManager.instance;
                    if (ghostManager.isCurrentlyTag)
                        infected.Add(ghostManager.currentIt);
                    else
                        infected.AddRange(ghostManager.currentInfected);
                    break;
                case GameModeType.Paintbrawl:
                    GorillaPaintbrawlManager paintbrawlManager = (GorillaPaintbrawlManager)GorillaGameManager.instance;

                    infected.AddRange(paintbrawlManager.playerLives.Where(element => element.Value <= 0).Select(element => element.Key).ToArray().Select(deadPlayer => PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(deadPlayer)).Select(dummy => (NetPlayer)dummy));

                    if (!infected.Contains(NetworkSystem.Instance.LocalPlayer))
                        infected.Add(NetworkSystem.Instance.LocalPlayer);

                    break;
            }

            return infected;
        }

        public static void AddInfected(NetPlayer plr)
        {
            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null)
                return;

            switch (GorillaGameManager.instance.GameType())
            {
                case GameModeType.Infection:
                case GameModeType.InfectionCompetitive:
                case GameModeType.SuperInfect:
                case GameModeType.FreezeTag:
                case GameModeType.PropHunt:
                    GorillaTagManager tagManager = (GorillaTagManager)GorillaGameManager.instance;
                    if (tagManager.isCurrentlyTag)
                        tagManager.ChangeCurrentIt(plr);
                    else if (!tagManager.currentInfected.Contains(plr))
                        tagManager.AddInfectedPlayer(plr);
                    break;
                case GameModeType.Ghost:
                case GameModeType.Ambush:
                    GorillaAmbushManager ghostManager = (GorillaAmbushManager)GorillaGameManager.instance;
                    if (ghostManager.isCurrentlyTag)
                        ghostManager.ChangeCurrentIt(plr);
                    else if (!ghostManager.currentInfected.Contains(plr))
                        ghostManager.AddInfectedPlayer(plr);
                    break;
                case GameModeType.Paintbrawl:
                    GorillaPaintbrawlManager paintbrawlManager = (GorillaPaintbrawlManager)GorillaGameManager.instance;
                    paintbrawlManager.playerLives[plr.ActorNumber] = 0;

                    break;
            }
        }

        public static void RemoveInfected(NetPlayer plr)
        {
            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null)
                return;

            switch (GorillaGameManager.instance.GameType())
            {
                case GameModeType.Infection:
                case GameModeType.InfectionCompetitive:
                case GameModeType.SuperInfect:
                case GameModeType.FreezeTag:
                case GameModeType.PropHunt:
                    GorillaTagManager tagManager = (GorillaTagManager)GorillaGameManager.instance;
                    switch (tagManager.isCurrentlyTag)
                    {
                        case true when tagManager.currentIt == plr:
                            tagManager.currentIt = null;
                            break;
                        case false when tagManager.currentInfected.Contains(plr):
                            tagManager.currentInfected.Remove(plr);
                            break;
                    }
                    break;
                case GameModeType.Ghost:
                case GameModeType.Ambush:
                    GorillaAmbushManager ghostManager = (GorillaAmbushManager)GorillaGameManager.instance;
                    switch (ghostManager.isCurrentlyTag)
                    {
                        case true when ghostManager.currentIt == plr:
                            ghostManager.currentIt = null;
                            break;
                        case false when ghostManager.currentInfected.Contains(plr):
                            ghostManager.currentInfected.Remove(plr);
                            break;
                    }
                    break;
                case GameModeType.Paintbrawl:
                    GorillaPaintbrawlManager paintbrawlManager = (GorillaPaintbrawlManager)GorillaGameManager.instance;
                    paintbrawlManager.playerLives[plr.ActorNumber] = 3;

                    break;
            }
        }

        public static void AddRock(NetPlayer plr)
        {
            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null)
                return;

            switch (GorillaGameManager.instance.GameType())
            {
                case GameModeType.Infection:
                case GameModeType.InfectionCompetitive:
                case GameModeType.SuperInfect:
                case GameModeType.FreezeTag:
                case GameModeType.PropHunt:
                    GorillaTagManager tagManager = (GorillaTagManager)GorillaGameManager.instance;
                    tagManager.ChangeCurrentIt(plr);

                    break;
                case GameModeType.Ghost:
                case GameModeType.Ambush:
                    GorillaAmbushManager ghostManager = (GorillaAmbushManager)GorillaGameManager.instance;
                    ghostManager.ChangeCurrentIt(plr);

                    break;
                case GameModeType.Paintbrawl:
                    GorillaPaintbrawlManager paintbrawlManager = (GorillaPaintbrawlManager)GorillaGameManager.instance;
                    paintbrawlManager.playerLives[plr.ActorNumber] = 0;

                    break;
            }
        }

        public static void RemoveRock(NetPlayer plr)
        {
            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null)
                return;

            switch (GorillaGameManager.instance.GameType())
            {
                case GameModeType.Infection:
                case GameModeType.InfectionCompetitive:
                case GameModeType.SuperInfect:
                case GameModeType.FreezeTag:
                case GameModeType.PropHunt:
                    GorillaTagManager tagManager = (GorillaTagManager)GorillaGameManager.instance;
                    if (tagManager.currentIt == plr)
                        tagManager.ChangeCurrentIt(null);

                    break;
                case GameModeType.Ghost:
                case GameModeType.Ambush:
                    GorillaAmbushManager ghostManager = (GorillaAmbushManager)GorillaGameManager.instance;
                    if (ghostManager.currentIt == plr)
                        ghostManager.ChangeCurrentIt(null);

                    break;
                case GameModeType.Paintbrawl:
                    GorillaPaintbrawlManager paintbrawlManager = (GorillaPaintbrawlManager)GorillaGameManager.instance;
                    paintbrawlManager.playerLives[plr.ActorNumber] = 3;

                    break;
            }
        }
    }
    public static class PlayerExtensions
    {
        #region NetPlayer
        public static Player GetPlayer(this NetPlayer self) =>
            RigUtil.NetPlayerToPlayer(self);

        public static VRRig VRRig(this NetPlayer self) =>
            RigUtil.GetVRRigFromPlayer(self);

        public static bool InRoom(this NetPlayer self) =>
            NetworkSystem.Instance.AllNetPlayers.Contains(self);

        public static Hashtable GetCustomProperties(this NetPlayer self) =>
            self.GetPlayer().CustomProperties;
        #endregion

        #region Player
        public static VRRig VRRig(this Player self) =>
            RigUtil.GetVRRigFromPlayer(self);

        public static bool InRoom(this Player self) =>
            PhotonNetwork.PlayerList.Contains(self);

        #endregion

        #region GorillaTagger
        public static bool IsGrounded(this GorillaTagger tagger, float maxDistance = 0.15f)
        {
            return
                Physics.Raycast(tagger.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, maxDistance, GTPlayer.Instance.locomotionEnabledLayers);
        }
        #endregion
    }
    public static class VRRigExtensions
    {

        public static bool IsTagged(this VRRig rig)
        {
            if (rig == null) return false;
            List<NetPlayer> infectedPlayers = GameModeUtilities.InfectedList();
            NetPlayer targetPlayer = rig.GetPlayer();

            return infectedPlayers.Contains(targetPlayer);
        }

        public static bool IsSteam(this VRRig rig) =>
            rig.GetPlatform() != "Standalone";

        public static bool IsKIDRestricted(this VRRig rig) =>
            !rig.IsMicEnabled && rig.GetName().ToLower().StartsWith("gorilla");

        public static string GetPlatform(this VRRig rig)
        {
            int suspiciouslySteam = 0;
            int suspiciouslyPC = 0;
            int suspiciouslyQuest = 0;

            if (rig.currentRankedSubTierPC > 0)
                suspiciouslySteam++;
            else if (rig.currentRankedSubTierQuest > 0)
                suspiciouslyQuest++;


            if (suspiciouslySteam > suspiciouslyPC && suspiciouslySteam > suspiciouslyQuest) return "Steam";
            if (suspiciouslyPC > suspiciouslySteam && suspiciouslyPC > suspiciouslyQuest) return "PC";
            if (suspiciouslyQuest > suspiciouslySteam && suspiciouslyQuest > suspiciouslyPC) return "Standalone";

            return "Standalone";
        }

        public static string GetCreationDate(this VRRig rig, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt") =>
            RigUtil.GetCreationDate(rig.Creator.UserId, onTranslated, format);

        public static Color GetColor(this VRRig rig)
        {
            if (rig.bodyRenderer.cosmeticBodyType == GorillaBodyType.Skeleton)
                return Color.green;

            switch (rig.setMatIndex)
            {
                case 1:
                    return Color.red;
                case 2:
                case 11:
                    return new Color32(255, 128, 0, 255);
                case 3:
                case 7:
                    return Color.blue;
                case 12:
                    return Color.green;
                default:
                    return rig.playerColor;
            }
        }

        public static bool Active(this VRRig rig) =>
            rig != null && VRRigCache.ActiveRigs.Contains(rig);

        public static float Distance(this VRRig rig, Vector3 position) =>
            Vector3.Distance(rig.transform.position, position);

        public static float Distance(this VRRig rig, VRRig otherRig) =>
            rig.Distance(otherRig.transform.position);

        public static float Distance(this VRRig rig) =>
            rig.Distance(GorillaTagger.Instance.bodyCollider.transform.position);

        public static VRRig GetClosest(this VRRig rig) =>
            VRRigCache.ActiveRigs.Where(targetRig => targetRig != null && targetRig != rig)
                                         .OrderBy(rig.Distance)
                                         .FirstOrDefault();
        public static readonly Dictionary<VRRig, int> playerPing = new Dictionary<VRRig, int>();
        public static int GetPing(this VRRig rig)
        {
            return playerPing.TryGetValue(rig, out int ping) ? ping : PhotonNetwork.GetPing();
        }

        public static int GetTruePing(this VRRig rig)
        {
            double ping = Math.Abs((rig.velocityHistoryList[0].time - PhotonNetwork.Time) * 1000);
            int safePing = (int)Math.Clamp(Math.Round(ping), 0, int.MaxValue);

            return safePing;
        }

        public static string GetName(this VRRig rig) =>
            RigUtil.GetPlayerFromVRRig(rig)?.NickName ?? "null";

        public static NetPlayer GetPlayer(this VRRig rig) =>
            RigUtil.GetPlayerFromVRRig(rig);

        public static Photon.Realtime.Player GetPhotonPlayer(this VRRig rig) =>
            RigUtil.NetPlayerToPlayer(RigUtil.GetPlayerFromVRRig(rig));

        public static ProjectileWeapon GetSlingshot(this VRRig rig) =>
            rig.projectileWeapon;

        public static float[] GetSpeed(this VRRig rig)
        {
            NetPlayer player = rig.GetPlayer();
            switch (GorillaGameManager.instance.GameType())
            {
                case GameModeType.Infection:
                case GameModeType.InfectionCompetitive:
                case GameModeType.FreezeTag:
                case GameModeType.PropHunt:
                    GorillaTagManager tagManager = (GorillaTagManager)GorillaGameManager.instance;
                    return tagManager.isCurrentlyTag
                        ? player == tagManager.currentIt
                            ? (new[]
                            {
                                tagManager.fastJumpLimit,
                                tagManager.fastJumpMultiplier
                            })
                            : (new[]
                        {
                            tagManager.slowJumpLimit,
                            tagManager.slowJumpMultiplier
                        })
                        : tagManager.currentInfected.Contains(player)
                            ? (new[]
                            {
                                tagManager.InterpolatedInfectedJumpSpeed(tagManager.currentInfected.Count),
                                tagManager.InterpolatedInfectedJumpMultiplier(tagManager.currentInfected.Count)
                            })
                            : (new[]
                        {
                            tagManager.InterpolatedNoobJumpSpeed(tagManager.currentInfected.Count),
                            tagManager.InterpolatedNoobJumpMultiplier(tagManager.currentInfected.Count)
                        });
                default:
                    return new[] { 6.5f, 1.1f };
            }
        }

        public static float GetMaxSpeed(this VRRig rig) =>
            rig.GetSpeed()[0];

        public static float GetSpeedMultiplier(this VRRig rig) =>
            rig.GetSpeed()[1];
    }
    public static class VectorExtensions
    {
        public static float Distance(this Vector3 point, Vector3 to) =>
            Vector3.Distance(point, to);

        public static Vector3 Lerp(this Vector3 a, Vector3 b, float t) =>
            Vector3.Lerp(a, b, t);

        public static Vector3 XyZ(this Vector3 a) =>
            new Vector3(a.x, Mathf.Max(a.y, 0f), a.z);

        public static long Pack(this Vector3 vec) =>
            BitPackUtils.PackWorldPosForNetwork(vec);

        public static Vector3 Random(this Vector3 _, float power = 1) =>
            RandomUtilities.RandomVector3(power);

        public static Vector3 ClampMagnitude(this Vector3 vec, float magnitude) =>
            Vector3.ClampMagnitude(vec, magnitude);

        public static Vector3 ClampSqrMagnitude(this Vector3 vec, float sqrMagnitude)
        {
            float currentSqrMag = vec.sqrMagnitude;

            if (!(currentSqrMag > sqrMagnitude) || !(currentSqrMag > 0f)) return vec;
            float scale = MathF.Sqrt(sqrMagnitude / currentSqrMag);
            vec *= scale;

            return vec;
        }
    }
    internal class RigUtil
    {
        public static VRRig GetVRRigFromPlayer(NetPlayer p) =>
            GorillaGameManager.StaticFindRigForPlayer(p);

        public static NetPlayer GetPlayerFromVRRig(VRRig p) =>
            p.Creator ?? NetworkSystem.Instance.GetPlayer(NetworkSystem.Instance.GetOwningPlayerID(p.rigSerializer.gameObject));

        public static NetPlayer GetPlayerFromID(string id) =>
            PhotonNetwork.PlayerList.FirstOrDefault(player => player.UserId == id);

        public static Player NetPlayerToPlayer(NetPlayer p) =>
            p.GetPlayerRef();

        public static Player GetRandomPlayer(bool includeSelf) =>
            includeSelf ?
            PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)] :
            PhotonNetwork.PlayerListOthers[Random.Range(0, PhotonNetwork.PlayerListOthers.Length)];

        private static VRRig rigTarget;
        private static float rigTargetChange;
        public static VRRig GetTargetPlayer(float targetChangeDelay = 1f)
        {
            if (!(Time.time > rigTargetChange) && rigTarget.Active()) return rigTarget;
            rigTargetChange = Time.time + targetChangeDelay;
            rigTarget = GetRandomVRRig(false);

            return rigTarget;
        }

        public static VRRig GetRandomVRRig(bool includeSelf) =>
            GetVRRigFromPlayer(GetRandomPlayer(includeSelf));

        public static NetworkView GetNetworkViewFromVRRig(VRRig p) =>
            p.netView;

        public static PhotonView GetPhotonViewFromVRRig(VRRig p) =>
            GetNetworkViewFromVRRig(p).GetView;

        public static VRRig GetClosestVRRig() =>
            VRRig.LocalRig.GetClosest();

        public static readonly Dictionary<string, float> waitingForCreationDate = new Dictionary<string, float>();
        public static readonly Dictionary<string, string> creationDateCache = new Dictionary<string, string>();
        public static string GetCreationDate(string input, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt")
        {
            if (creationDateCache.TryGetValue(input, out string date))
                return date;
            if (!waitingForCreationDate.ContainsKey(input))
            {
                waitingForCreationDate[input] = Time.time + 10f;
                GetCreationCoroutine(input, onTranslated, format);
            }
            else
            {
                if (!(Time.time > waitingForCreationDate[input])) return "Loading...";
                waitingForCreationDate[input] = Time.time + 10f;
                GetCreationCoroutine(input, onTranslated, format);
            }

            return "Loading...";
        }

        public static void GetCreationCoroutine(string userId, Action<string> onTranslated = null, string format = "MMMM dd, yyyy h:mm tt")
        {
            if (creationDateCache.TryGetValue(userId, out string date))
            {
                onTranslated?.Invoke(date);
                return;
            }

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = userId }, delegate (GetAccountInfoResult result) // Who designed this
            {
                string creationDate = result.AccountInfo.Created.ToString(format);
                creationDateCache[userId] = creationDate;

                onTranslated?.Invoke(creationDate);
            }, delegate { creationDateCache[userId] = "Error"; onTranslated?.Invoke("Error"); });
        }
    }
}

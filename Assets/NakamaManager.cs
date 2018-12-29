using System.Collections;
using System.Collections.Generic;
using Nakama;
using UnityEngine;
using System;
using Nakama.TinyJson;
using SimpleJSON;
public class NakamaManager : MonoBehaviour {


    private readonly IClient client = new Client("defaultkey", "127.0.0.1", 7350, false);
    ISession session;
    // Use this for initialization
    void Start() {
        Connect();
    }

    // Update is called once per frame
    async public void Connect() {

        var session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
        this.session = session;
        Debug.LogFormat("User id '{0}'", session.UserId);
        Debug.LogFormat("User username '{0}'", session.Username);
        Debug.LogFormat("Session has expired: {0}", session.IsExpired);
        Debug.LogFormat("Session expires at: {0}", session.ExpireTime); // in seconds.


    }

    async public void ListTournaments()
    {

        var categoryStart = 1;
        var categoryEnd = 1;
        var startTime = 1538147711;
        var endTime = 1638147711; // all tournaments from the start time
        var limit = 100; // number to list per page
        var result = await client.ListTournamentsAsync(session, categoryStart, categoryEnd, startTime, endTime, limit, null);
        Debug.LogFormat("recive tornoment info: {0}", result);

        var resultParse = JSON.Parse(result.ToString());

        var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        date = date.AddSeconds(resultParse[5]).ToLocalTime();
        Debug.LogFormat("tornoment expires on: '{0}'", date);

    }
    async public void JoinTournament()
    {
        var id = "score";
        await client.JoinTournamentAsync(session, id);
    }

    async public void WriteTournamentRecord()
    {
            var id = "score";
            var score = 100L;
            // var subscore = 10L;
            // using Nakama.TinyJson
            //var metadata = new Dictionary<string, string>()
            //{
            //  { "weather_conditions", "sunny" },
            //  { "track_name", "Silverstone" }
            //}.ToJson();
            var newrecord = await client.WriteTournamentRecordAsync(session, id, score);
            Debug.Log(newrecord);
    }

    async public void ListTournamentRecords()
    {
        var id = "score";
        var record = await client.ListTournamentRecordsAsync(session, id, null, 100, null);
        Debug.Log(record);
    }
    async public void CreateTournament()
    {
        var payload = "{\"id\": \"score\" ,\"sort_order\": \"desc\" ,\"operator\": \"best\" ,\"operator\": \"best\",\"duration\": 86400,\"reset \": \"0 12 * * *\",\"title\": \"score\",\"description\": \"score table \",\"category\": 1,\"start_time\": 0, \"end_time\": 0, \"max_num_score\": 3 ,\"join_required\": true}";
        var rpcid = "clientrpc.create_tournament";
        var Info = await client.RpcAsync(session, rpcid, payload);
        Debug.LogFormat("Retrieved tornoment info: {0}", Info);

    }

    async public void DeleteTournament()
    {
        var payload = "{\"tournament_id\": \"score\"}";

        var rpcid = "clientrpc.delete_tournament";
        var Info = await client.RpcAsync(session, rpcid, payload);
        Debug.LogFormat("Retrieved tornoment info: {0}", Info);
    }


}




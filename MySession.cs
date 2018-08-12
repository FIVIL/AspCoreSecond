using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace seccond
{
    public class MySession
    {
        public ConcurrentDictionary<Guid, SessionData> Sessions { get; set; }
        public MySession()
        {
            Sessions = new ConcurrentDictionary<Guid, SessionData>();
            Cleare();
        }
        public Guid AddSession(string Data, int Data2, DateTime Time)
        {
            var G = Guid.NewGuid();
            Sessions.TryAdd(G, new SessionData
            {
                Data = Data,
                Data2 = Data2,
                AliveTime = Time
            });
            return G;
        }
        public SessionData ReadSession(Guid G)
        {
            if (Sessions.TryGetValue(G, out var sessionData))
            {
                sessionData.AliveTime = DateTime.Now;
                return sessionData;
            };
            return new SessionData();
        }
        public SessionData RemoveSession(Guid G)
        {
            Sessions.TryRemove(G, out var sessionData);
            return sessionData;
        }
        public void Cleare()
        {
            List<Guid> RemoveList = new List<Guid>();
            var _timer = new Timer((a) =>
            {
                foreach (var item in Sessions)
                {
                    if (item.Value.AliveTime < DateTime.Now + item.Value.Time)
                    { RemoveList.Add(item.Key); }
                }
                foreach (var item in RemoveList)
                {
                    Sessions.TryRemove(item, out var s);
                }
            }, null, TimeSpan.Zero,
           TimeSpan.FromMinutes(5));
        }
    }
    public class SessionData
    {
        public string Data { get; set; }
        public int Data2 { get; set; }
        public DateTime AliveTime { get; set; }
        public TimeSpan Time { get; set; }
    }
}

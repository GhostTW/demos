using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Demo.Core.Lua
{
    public class LuaScriptWorker
    {
        public readonly IDictionary<string, string> LuaScripts = new Dictionary<string, string>();
        private const string ConnectionString = "127.0.0.1:6379";
        private readonly ConnectionMultiplexer _connection;
        private readonly IDictionary<string, LoadedLuaScript> _loadedLuaScripts = new Dictionary<string, LoadedLuaScript>();

        public LuaScriptWorker()
        {
            _connection = ConnectionMultiplexer.Connect(ConnectionString);
        }

        private LoadedLuaScript LoadScript(string scriptName)
        {
            if (_loadedLuaScripts.ContainsKey(scriptName))
                return _loadedLuaScripts[scriptName];
            
            var loadedScript = LuaScript
                .Prepare(LuaScripts[scriptName])
                .Load(_connection.GetServer(ConnectionString));
            
            _loadedLuaScripts.Add(scriptName, loadedScript);

            return loadedScript;
        }

        public async Task<RedisResult> ExecuteLuaScript(
            string scriptName, 
            RedisKey[] redisKey,
            RedisValue[] redisValue = null)
        {
            var loadedScript = _loadedLuaScripts.ContainsKey(scriptName) == false ? LoadScript(scriptName) : _loadedLuaScripts[scriptName];

            return await _connection
                .GetDatabase()
                .ScriptEvaluateAsync(loadedScript.Hash, redisKey, redisValue);
        }
    }
}
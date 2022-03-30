using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace CRUD_ORM.logs
{
    public static class SessionExtension
    {
        public static void SetarNaSessao<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T RecuperarDaSessao<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

    }
}

// File: Helpers/SessionHelper.cs
// Mô tả: Lưu object vào Session

using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CNPMFastFood.Helpers
{
    public static class SessionHelper
    {
        // ================= SAVE OBJECT =================

        public static void SetObject(
            this ISession session,
            string key,
            object value)
        {
            // convert object -> json
            session.SetString(
                key,
                JsonSerializer.Serialize(value));
        }

        // ================= GET OBJECT =================

        public static T GetObject<T>(
            this ISession session,
            string key)
        {
            var value = session.GetString(key);

            // nếu null
            if (value == null)
            {
                return default;
            }

            // convert json -> object
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
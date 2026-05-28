// File: Helpers/SessionHelper.cs
// Mô tả:
// Helper dùng để:
// - Lưu object vào Session
// - Lấy object từ Session
// Session chỉ lưu string nên cần convert object <-> json

using Microsoft.AspNetCore.Http; // Hỗ trợ Session
using System.Text.Json; // Dùng để Serialize và Deserialize JSON

namespace CNPMFastFood.Helpers
{
    // Static class chứa các hàm mở rộng cho Session
    public static class SessionHelper
    {
        // =========================
        // SAVE OBJECT TO SESSION
        // Lưu object vào Session
        // =========================

        public static void SetObject(
            this ISession session, // Session hiện tại
            string key,            // Key dùng để lưu dữ liệu
            object value)          // Object cần lưu
        {
            // Convert object -> JSON string
            var json = JsonSerializer.Serialize(value);

            // Lưu JSON vào Session dưới dạng string
            session.SetString(key, json);
        }

        // =========================
        // GET OBJECT FROM SESSION
        // Lấy object từ Session
        // =========================

        public static T GetObject<T>(
            this ISession session, // Session hiện tại
            string key)            // Key dữ liệu cần lấy
        {
            // Lấy dữ liệu string JSON từ Session
            var value = session.GetString(key);

            // Nếu không tồn tại dữ liệu
            if (value == null)
            {
                // Trả về giá trị mặc định của kiểu T
                // Ví dụ:
                // int -> 0
                // object -> null
                return default;
            }

            // Convert JSON -> object kiểu T
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
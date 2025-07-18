using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Blood_Donation_Website.Utilities.Extensions
{
    /// <summary>
    /// Extension methods cho enum để dễ dàng sử dụng trong Views và Controllers
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Lấy Display Name từ enum value
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Display name hoặc enum name nếu không có Display attribute</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var displayAttribute = field?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Lấy Description từ enum value (nếu có)
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Description hoặc null nếu không có</returns>
        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var displayAttribute = field?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Description;
        }

        /// <summary>
        /// Lấy tất cả enum values với display names
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>Dictionary với key là enum value, value là display name</returns>
        public static Dictionary<T, string> GetAllDisplayNames<T>() where T : Enum
        {
            var result = new Dictionary<T, string>();
            var enumType = typeof(T);
            
            foreach (var field in enumType.GetFields())
            {
                if (field.FieldType == enumType)
                {
                    var enumValue = (T)field.GetValue(null);
                    var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
                    var displayName = displayAttribute?.Name ?? field.Name;
                    result[enumValue] = displayName;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Lấy SelectList từ enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="selectedValue">Giá trị được chọn (tùy chọn)</param>
        /// <param name="includeEmptyOption">Có thêm option trống không</param>
        /// <param name="emptyOptionText">Text cho option trống</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<T>(T selectedValue = default, bool includeEmptyOption = false, string emptyOptionText = "-- Chọn --") where T : Enum
        {
            var items = new List<SelectListItem>();
            
            if (includeEmptyOption)
            {
                items.Add(new SelectListItem { Value = "", Text = emptyOptionText });
            }
            
            var displayNames = GetAllDisplayNames<T>();
            foreach (var item in displayNames)
            {
                items.Add(new SelectListItem
                {
                    Value = item.Key.ToString(),
                    Text = item.Value,
                    Selected = item.Key.Equals(selectedValue)
                });
            }
            
            return new SelectList(items, "Value", "Text", selectedValue?.ToString());
        }

        /// <summary>
        /// Lấy SelectList từ enum (không có giá trị được chọn)
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="includeEmptyOption">Có thêm option trống không</param>
        /// <param name="emptyOptionText">Text cho option trống</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<T>(bool includeEmptyOption = false, string emptyOptionText = "-- Chọn --") where T : Enum
        {
            var items = new List<SelectListItem>();
            
            if (includeEmptyOption)
            {
                items.Add(new SelectListItem { Value = "", Text = emptyOptionText });
            }
            
            var displayNames = GetAllDisplayNames<T>();
            foreach (var item in displayNames)
            {
                items.Add(new SelectListItem
                {
                    Value = item.Key.ToString(),
                    Text = item.Value
                });
            }
            
            return new SelectList(items, "Value", "Text");
        }

        /// <summary>
        /// Lấy CSS class dựa trên enum value (cho styling)
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>CSS class name</returns>
        public static string GetCssClass(this Enum enumValue)
        {
            return enumValue.ToString().ToLower();
        }

        /// <summary>
        /// Lấy Bootstrap badge class dựa trên enum value
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Bootstrap badge class</returns>
        public static string GetBadgeClass(this Enum enumValue)
        {
            return enumValue.ToString().ToLower() switch
            {
                "active" or "confirmed" or "completed" or "eligible" => "badge bg-success",
                "pending" or "registered" or "screening" => "badge bg-warning",
                "cancelled" or "failed" or "ineligible" or "noshow" => "badge bg-danger",
                "draft" or "closed" => "badge bg-secondary",
                "published" or "info" => "badge bg-info",
                "warning" => "badge bg-warning",
                "error" => "badge bg-danger",
                _ => "badge bg-primary"
            };
        }

        /// <summary>
        /// Lấy icon class dựa trên enum value
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>FontAwesome icon class</returns>
        public static string GetIconClass(this Enum enumValue)
        {
            return enumValue.ToString().ToLower() switch
            {
                "active" or "confirmed" => "fas fa-check-circle",
                "pending" or "registered" => "fas fa-clock",
                "cancelled" or "failed" => "fas fa-times-circle",
                "completed" => "fas fa-check-double",
                "screening" => "fas fa-stethoscope",
                "eligible" => "fas fa-heart",
                "ineligible" => "fas fa-ban",
                "noshow" => "fas fa-user-times",
                "draft" => "fas fa-edit",
                "published" => "fas fa-bullhorn",
                "closed" => "fas fa-lock",
                "male" => "fas fa-mars",
                "female" => "fas fa-venus",
                "other" => "fas fa-genderless",
                _ => "fas fa-info-circle"
            };
        }
    }
} 
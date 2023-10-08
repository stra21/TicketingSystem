using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DL.Utilities
{
    public static class HelperFunctions
    {
        public static void UpdateObjectProperties<T>(T target, T source)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Get the properties of the type
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                // Check if the property can be written to
                if (property.CanWrite)
                {
                    // Get the corresponding property from the source object
                    PropertyInfo sourceProperty = typeof(T).GetProperty(property.Name);

                    if (sourceProperty != null)
                    {
                        // Set the value of the target property to match the source property
                        property.SetValue(target, sourceProperty.GetValue(source));
                    }
                }
            }
        }
        public static bool IsImage(this string path)
        {
            string extension = path.Split('.')[path.Split('.').Length - 1];
            return AppSettings.AcceptedImageTypes.Contains(extension.ToLower());
        }
        public static string StringifyPath(this string path)
        {
            string name = path.Split("/")[path.Split("/").Length-1];
            return name;
        }
        public static string GetSpecificClaim(this IEnumerable<Claim> claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.FirstOrDefault(x => x.Type == claimType);

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}

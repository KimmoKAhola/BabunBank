using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DataAccessLibrary.DataServices;

public static class DropDownService
{
    public static List<SelectListItem> GetRoles()
    {
        var roles = Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();

        return roles;
    }

    public static IEnumerable<SelectListItem> GetGenderOptions()
    {
        var genders = Enum.GetValues(typeof(GenderOptions))
            .Cast<GenderOptions>()
            .Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() })
            .ToList();

        return genders;
    }

    public static IEnumerable<SelectListItem> GetCountryOptions()
    {
        var countries = Enum.GetValues(typeof(CountryOptions))
            .Cast<CountryOptions>()
            .Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() })
            .ToList();

        return countries;
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Helpers
{
    public static class EnumHelpers
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<int>().Select(e => new SelectListItem() { Text = Enum.GetName(typeof(T), e), Value = e.ToString() })).ToList();
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>(IEnumerable<T> includedList)
        {
            var enumSelectList = includedList.Distinct().Select(e => new SelectListItem() { Text = Enum.GetName(typeof(T), e), Value = e.ToString() }).ToList();
            return enumSelectList;
        }
    }
}

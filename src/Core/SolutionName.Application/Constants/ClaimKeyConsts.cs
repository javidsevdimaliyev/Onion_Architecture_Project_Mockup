using SolutionName.Application.Attributes;
using System.Reflection;

namespace SolutionName.Application.Constants
{

    public class ClaimKeyConsts
    {
        public const string DisableValidations = nameof(DisableValidations);

        //Auth
        public const string Admin = nameof(Admin);
        public const string Moderator = nameof(Moderator);
        public const string Developer = nameof(Developer);
        public const string CEO = nameof(CEO);

 
        private static List<FieldInfo> ClaimsList()
        {
            //var fields = typeof(ClaimKeys)
            //    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            //    .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
            var fields = typeof(ClaimKeyConsts).GetFields().ToList();
            return fields;
        }

        public static Dictionary<long, ClaimModel> AllClaims()
        {
            var res = new Dictionary<long, ClaimModel>();
            var list = ClaimsList();
            for (var i = 1; i <= list.Count; i++)
            {
                var item = list[i - 1];

                var attr = (ClaimAttribute)item.GetCustomAttribute(typeof(ClaimAttribute));

                res.Add(i, new ClaimModel
                {
                    Id = i,
                    Name = item.GetValue(null).ToString(),
                    Module = attr.Module,
                    Page = attr.Page
                });
            }

            return res;
        }
    }

    public class ClaimModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Page { get; set; }
    }
}

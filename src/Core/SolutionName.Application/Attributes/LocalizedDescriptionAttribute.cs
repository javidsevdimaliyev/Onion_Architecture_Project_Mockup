using System.ComponentModel;
using System.Resources;

namespace SolutionName.Application.Attributes;

public class LocalizedDescriptionAttribute : DescriptionAttribute
{
    private readonly ResourceManager _resource;
    private readonly string _resourceKey;

    public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
    {
        _resource = new ResourceManager(resourceType);
        _resourceKey = resourceKey;
    }

    public override string Description
    {
        get
        {
            var displayName = _resource.GetString(_resourceKey);

            return string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", _resourceKey)
                : displayName;
        }
    }
}
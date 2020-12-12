using LocalizationGlobalization101.Attributes;
using LocalizationGlobalization101.Resources.Enums;

namespace LocalizationGlobalization101.Enums
{
    public enum Gender
    {
        [LocalizedDescription("Gender.Male",typeof(EnumResources))]
        Male = 1000,
        [LocalizedDescription("Gender.Female", typeof(EnumResources))]
        Female = 2000
    }
}

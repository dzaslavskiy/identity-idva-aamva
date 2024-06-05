using Aamva.Models;
using DldvServiceReference;

namespace Aamva.Utils;

public static class SoapUtils
{
    public static readonly Dictionary<string, PersonEyeColorCodeSimpleType> EyeColorMap = new()
    {
        {"black", PersonEyeColorCodeSimpleType.BLK},
        {"blue", PersonEyeColorCodeSimpleType.BLU},
        {"brown", PersonEyeColorCodeSimpleType.BRO},
        {"dichromatic", PersonEyeColorCodeSimpleType.DIC},
        {"green", PersonEyeColorCodeSimpleType.GRN},
        {"gray", PersonEyeColorCodeSimpleType.GRY},
        {"hazel", PersonEyeColorCodeSimpleType.HAZ},
        {"maroon", PersonEyeColorCodeSimpleType.MAR},
        {"pink", PersonEyeColorCodeSimpleType.PNK}
    };

    public static readonly Dictionary<string, DocumentCategoryCodeSimpleType> DocumentCatMap = new()
    {
        {"driver license", DocumentCategoryCodeSimpleType.Item1},
        {"driver permit", DocumentCategoryCodeSimpleType.Item2},
        {"id card", DocumentCategoryCodeSimpleType.Item3}
    };

    public static readonly Dictionary<string, PersonSexCodeSimpleType> SexMap = new()
    {
        {"male", PersonSexCodeSimpleType.Item1},
        {"m", PersonSexCodeSimpleType.Item1},
        {"female", PersonSexCodeSimpleType.Item2},
        {"f", PersonSexCodeSimpleType.Item2}
    };

    public static integer? ToSoapInteger(string? integer)
    {
        if (integer == null)
        {
            return null;
        }
        return new integer { Value = integer };
    }

    public static bool CheckDate(string? date)
    {
        return date == null || DateTime.TryParse(date, out _);
    }

    public static date? ToSoapDate(string? date)
    {
        if (date == null)
        {
            return null;
        }
        return new date { Value = DateTime.Parse(date) };
    }

    public static IdentificationType ToSoapId(string id)
    {
        return new IdentificationType { IdentificationID = new @string { Value = id } };
    }

    public static bool CheckDocCat(string? a)
    {
        return a == null || DocumentCatMap.ContainsKey(a.ToLower());
    }

    public static DocumentCategoryCodeType? ToSoapDocCat(string? a)
    {
        if (a == null)
        {
            return null;
        }

        return new DocumentCategoryCodeType { Value = DocumentCatMap[a.ToLower()] };
    }

    public static bool CheckSex(string? a)
    {
        return a == null || SexMap.ContainsKey(a.ToLower());
    }

    public static PersonSexCodeType? ToSoapSex(string? a)
    {
        if (a == null)
        {
            return null;
        }

        return new PersonSexCodeType { Value = SexMap[a.ToLower()] };
    }

    public static bool CheckEyeColor(string? a)
    {
        return a == null || EyeColorMap.ContainsKey(a.ToLower());
    }

    public static PersonEyeColorCodeType? ToSoapEyeColor(string? a)
    {
        if (a == null)
        {
            return null;
        }

        return new PersonEyeColorCodeType { Value = EyeColorMap[a.ToLower()] };
    }

    public static bool CheckState(string? a)
    {
        return a == null || Enum.TryParse<UsStateCodeSimpleType>(a, true, out _);
    }

    public static AddressType ToSoapAddress(ValidationRequest a)
    {
        return new AddressType
        {
            AddressDeliveryPointText = new TextType[]
            {
                 new TextType { Value = a.AddressLine1 },
                 new TextType { Value = a.AddressLine2 }
            },
            LocationCityName = new ProperNameTextType { Value = a.City },
            LocationPostalCode = new @string { Value = a.Zip },
            LocationStateUsPostalServiceCode = a.State != null ? new UsStateCodeType { Value = Enum.Parse<UsStateCodeSimpleType>(a.State, true) }
                                                               : null
        };
    }

    public static PersonNameType ToSoapName(ValidationRequest a)
    {
        return new PersonNameType
        {
            PersonGivenName = new PersonNameTextType { Value = a.FirstName },
            PersonMiddleName = new PersonNameTextType { Value = a.MiddleName },
            PersonNameSuffixText = new TextType { Value = a.Suffix },
            PersonSurName = new PersonNameTextType { Value = a.LastName }
        };
    }
}

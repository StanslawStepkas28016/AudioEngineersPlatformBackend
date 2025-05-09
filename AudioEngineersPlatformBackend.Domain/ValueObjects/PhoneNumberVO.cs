using PhoneNumbers;

namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public class PhoneNumberVO
{
    private readonly string _number;

    public PhoneNumberVO(string phoneNumber)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            phoneNumberUtil.Parse(phoneNumber, null);
        }
        catch (NumberParseException e)
        {
            throw new ArgumentException(e.Message, nameof(phoneNumber));
        }

        _number = phoneNumber;
    }

    public string GetValidPhoneNumber()
    {
        return _number;
    }
}
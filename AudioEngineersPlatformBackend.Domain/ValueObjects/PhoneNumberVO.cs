using PhoneNumbers;

namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct PhoneNumberVo
{
    private readonly string _number;

    private string Number
    {
        get { return _number; }
        init
        {
            PhoneNumberUtil? phoneNumberUtil = PhoneNumberUtil.GetInstance();

            try
            {
                phoneNumberUtil.Parse(value, null);
            }
            catch (NumberParseException e)
            {
                throw new ArgumentException(e.Message, nameof(value));
            }


            _number = value;
        }
    }

    public PhoneNumberVo(string phoneNumber)
    {
        _number = phoneNumber;
    }

    public string GetValidPhoneNumber()
    {
        return Number;
    }
}
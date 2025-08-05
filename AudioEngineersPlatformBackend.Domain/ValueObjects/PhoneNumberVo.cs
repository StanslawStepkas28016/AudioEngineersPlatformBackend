using PhoneNumbers;

namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct PhoneNumberVo
{
    private readonly string _phoneNumber;

    public string PhoneNumber
    {
        get => _phoneNumber;
        init
        {
            PhoneNumberUtil? phoneNumberUtil = PhoneNumberUtil.GetInstance();

            try
            {
                phoneNumberUtil.Parse(value, null);
            }
            catch (NumberParseException e)
            {
                throw new ArgumentException(e.Message);
            }


            _phoneNumber = value;
        }
    }

    public PhoneNumberVo(string phonePhoneNumber)
    {
        PhoneNumber = phonePhoneNumber;
    }
}
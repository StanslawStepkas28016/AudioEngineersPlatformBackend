using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AudioEngineersPlatformBackend.Domain.ValueObjects;
using PhoneNumbers;
using PhoneNumber = PhoneNumbers.PhoneNumber;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public class User
{
    public Guid IdUser { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Password { get; private set; }

    // References
    public Guid IdRole { get; private set; }
    public virtual Role Role { get; private set; }
    public Guid IdUserLog { get; private set; }
    public virtual UserLog UserLog { get; private set; }

    private User()
    {
    }

    public User(string firstName, string lastName, string email, string phoneNumber, string password,
        Guid idRole, Guid idUserLog)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be empty", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));
        }

        if (new EmailVO(email).GetValidEmail() != email)
        {
            throw new ArgumentException("Invalid email address", nameof(email));
        }

        if (new PhoneNumberVO(phoneNumber).GetValidPhoneNumber() != phoneNumber)
        {
            throw new ArgumentException("Invalid phone number", nameof(phoneNumber));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty", nameof(password));
        }

        if (idRole == Guid.Empty)
        {
            throw new ArgumentException("Role id cannot be empty", nameof(idRole));
        }

        if (idUserLog == Guid.Empty)
        {
            throw new ArgumentException("UserLog id cannot be empty", nameof(idUserLog));
        }

        IdUser = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        IdRole = idRole;
        IdUserLog = idUserLog;
    }
}
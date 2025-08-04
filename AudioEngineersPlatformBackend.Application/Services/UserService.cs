using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Application.Services;

public class UserService : IUserService
{
    public void ResetPhoneNumber()
    {
        // TODO: ResetPhoneNumber endpoint
        /*// Handle phone number change request if provided
        if (!string.IsNullOrWhiteSpace(resetEmailRequest.PhoneNumber))
        {
            // Ensure the right format of the provided phoneNumber (will throw an exception if invalid)
            var newValidPhoneNumber = new PhoneNumberVo(resetEmailRequest.PhoneNumber).GetValidPhoneNumber();

            // Check if the phoneNumber is already in use
            if (await _userRepository.IsPhoneNumberAlreadyTaken(newValidPhoneNumber, cancellationToken))
            {
                throw new Exception($"Provided {nameof(resetEmailRequest.PhoneNumber)} is already taken.");
            }

            // Update the data (will check if provided phoneNumber is different from the old one)
            user.ChangePhoneNumber(newValidPhoneNumber);
        }*/
    }
}
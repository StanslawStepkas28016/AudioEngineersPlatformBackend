using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.User.ChangeData;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISESService _sesService;

    public UserService(IUserRepository userRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork,
        ISESService sesService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _sesService = sesService;
    }

    public async Task<Guid> ChangeData(Guid idUser, ChangeDataRequest changeDataRequest,
        CancellationToken cancellationToken)
    {
        // Check if the user is authorized to edit the advert (either the owner or an administrator)
        if (idUser != _currentUserService.IdUser && !_currentUserService.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified data does not belong to you.");
        }

        // Validate if the user exists
        if (!await _userRepository.DoesUserExistByIdUser(idUser, cancellationToken))
        {
            throw new Exception("User not found.");
        }

        // Fetch the user data
        User user = (await _userRepository.FindUserByIdUser(idUser, cancellationToken))!;

        // Handle phone number change request if provided
        if (!string.IsNullOrWhiteSpace(changeDataRequest.PhoneNumber))
        {
            // Ensure the right format of the provided phoneNumber (will throw an exception if invalid)
            var newValidPhoneNumber = new PhoneNumberVo(changeDataRequest.PhoneNumber).GetValidPhoneNumber();

            // Check if the phoneNumber is already in use
            if (await _userRepository.IsPhoneNumberAlreadyTaken(newValidPhoneNumber, cancellationToken))
            {
                throw new Exception("Provided phone number is already taken");
            }

            // Update the data (will check if provided phoneNumber is different from the old one)
            user.TryChangePhoneNumber(newValidPhoneNumber);
        }

        // Handle email change request if provided
        if (!string.IsNullOrWhiteSpace(changeDataRequest.Email))
        {
            // Ensure the right format of the provided email (will throw an exception if invalid)
            string newValidEmail = new EmailVo(changeDataRequest.Email).GetValidEmail();

            // Check if the email is already in use
            if (await _userRepository.IsEmailAlreadyTaken(newValidEmail, cancellationToken))
            {
                throw new Exception("Email is already taken.");
            }

            // Update the data (will check if provided email is different from the old one)
            user.TryChangeUserEmail(newValidEmail);

            // TODO: Handle mail sending...

            // Fetch the userLog data
            var userLog = (await _userRepository.FindUserLogByIdUser(idUser, cancellationToken))!;
            userLog.GenerateAndSetNewVerificationCode();

            // Send a new verification email
            // await _emailService.TrySendRegisterVerificationEmailAsync(user.Email, user.FirstName, userLog.VerificationCode);
        }

        // Persist all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return idUser;
    }

    public async Task Test()
    {
        await _sesService.TrySendRegisterVerificationEmailAsync("s28016@pjwstk.edu.pl", "Stanisław", "123123");
    }
}
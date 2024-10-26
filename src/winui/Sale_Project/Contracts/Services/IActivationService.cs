namespace Sale_Project.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}

using Mecanillama.API.Customers.Domain.Repositories;
using Mecanillama.API.Mechanics.Domain.Models;
using Mecanillama.API.Mechanics.Domain.Repositories;
using Mecanillama.API.Mechanics.Domain.Services;
using Mecanillama.API.Mechanics.Domain.Services.Communication;
using Mecanillama.API.Security.Domain.Repositories;
using Mecanillama.API.Shared.Domain.Repositories;

namespace Mecanillama.API.Mechanics.Services;

public class MechanicService : IMechanicService
{
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MechanicService(IMechanicRepository mechanicRepository, IUnitOfWork unitOfWork, IUserRepository userRepository) {
        _mechanicRepository = mechanicRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Mechanic>> ListAsync()
    {
        return await _mechanicRepository.ListAsync();
    }
    
    public async Task<MechanicResponse> GetByIdAsync(long id)
    {
        var existingMechanic = await _mechanicRepository.FindByIdAsync(id);

        if (existingMechanic == null)
            return new MechanicResponse("Mechanic not found.");

        return new MechanicResponse(existingMechanic);
    }

    public async Task<MechanicResponse> GetByUserIdAsync(long userId)
    {
        var existingMechanic = await _mechanicRepository.FindByUserIdAsync(userId);

        if (existingMechanic == null)
            return new MechanicResponse("Mechanic not found.");

        return new MechanicResponse(existingMechanic);
    } 

    public async Task<MechanicResponse> SaveAsync(Mechanic mechanic)
    {
        var existingUser = await _userRepository.FindByIdAsync(mechanic.UserId);
        if (existingUser == null)
            return new MechanicResponse("User not found.");
        
        try
        {
            await _mechanicRepository.AddAsync(mechanic);
            await _unitOfWork.CompleteAsync();
            return new MechanicResponse(mechanic);
        }
        catch (Exception e) 
        {
            return new MechanicResponse($"An error occurred while saving the mechanic: {e.Message}");
        }
    }

    public async Task<MechanicResponse> UpdateAsync(int id, Mechanic mechanic)
    {
        var existingMechanic = await _mechanicRepository.FindByIdAsync(id);
        if (existingMechanic == null)
        {
            return new MechanicResponse("Mechanic not found ");
        }
        
        var existingUser = await _userRepository.FindByIdAsync(mechanic.UserId);
        if (existingUser == null)
            return new MechanicResponse("User not found.");

        existingMechanic.Name = mechanic.Name;

        try
        {
            _mechanicRepository.Update(existingMechanic);
            await _unitOfWork.CompleteAsync();

            return new MechanicResponse(existingMechanic);
        }
        catch (Exception e)
        {
            return new MechanicResponse($"An error occurred while updating the mechanic: {e.Message}");
        }
    }

    public async Task<MechanicResponse> DeleteAsync(int id)
    {
        var existingMechanic = await _mechanicRepository.FindByIdAsync(id);

        if (existingMechanic == null)
            return new MechanicResponse("Mechanic not found.");

        try
        {
            _mechanicRepository.Remove(existingMechanic);
            await _unitOfWork.CompleteAsync();

            return new MechanicResponse(existingMechanic);
        }
        catch (Exception e)
        {
            return new MechanicResponse($"An error occurred while deleting the mechanic: {e.Message}");
        }
    }
}
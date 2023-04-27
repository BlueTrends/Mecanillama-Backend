using Mecanillama.API.Appointments.Domain.Models;
using Mecanillama.API.Appointments.Domain.Repositories;
using Mecanillama.API.Appointments.Domain.Services;
using Mecanillama.API.Appointments.Domain.Services.Communication;
using Mecanillama.API.Customers.Domain.Repositories;
using Mecanillama.API.Mechanics.Domain.Repositories;
using Mecanillama.API.Shared.Domain.Repositories;

namespace Mecanillama.API.Appointments.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly ICustomerRepository _customerRepository;
    
    public AppointmentService(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork, IMechanicRepository mechanicRepository, ICustomerRepository customerRepository)
    {
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
        _mechanicRepository = mechanicRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Appointment>> ListAsync()
    {
        return await _appointmentRepository.ListAsync();
    }
    public async Task<AppointmentResponse> SaveAsync(Appointment appointment)
    {
        var existingMechanic = await _mechanicRepository.FindByIdAsync(appointment.MechanicId);
        if (existingMechanic == null)
            return new AppointmentResponse("Mechanic not found");

        var existingCustomer = await _customerRepository.FindByIdAsync(appointment.CustomerId);
        if (existingCustomer == null)
            return new AppointmentResponse("Customer not found");
        
        try
        {
            await _appointmentRepository.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();

            return new AppointmentResponse(appointment);
        }
        catch (Exception e)
        {
            return new AppointmentResponse($"An error occurred while saving the appointment: {e.Message}");
        }
    }

    public async Task<AppointmentResponse> UpdateAsync(long id, Appointment appointment)
    {
        var existingAppointment = await _appointmentRepository.FindByIdAsync(id);
        if (existingAppointment == null)
        {
            return new AppointmentResponse("Appointment not found");
        }
        
        var existingMechanic = await _mechanicRepository.FindByIdAsync(appointment.MechanicId);
        if (existingMechanic == null)
            return new AppointmentResponse("Mechanic not found");

        var existingCustomer = await _customerRepository.FindByIdAsync(appointment.CustomerId);
        if (existingCustomer == null)
            return new AppointmentResponse("Customer not found");
        
        existingAppointment.Date = appointment.Date;
        existingAppointment.Time = appointment.Time;
        existingAppointment.MechanicId = appointment.MechanicId;
        existingAppointment.CustomerId = appointment.CustomerId;

        try
        {
            _appointmentRepository.Update(existingAppointment);
            await _unitOfWork.CompleteAsync();
            return new AppointmentResponse(existingAppointment);
        }
        catch (Exception e)
        {
            return new AppointmentResponse($"An error occurred while updating the appointment: {e.Message}");
        }
    }

    public async Task<AppointmentResponse> DeleteAsync(long id)
    {
        var existingAppointment = await _appointmentRepository.FindByIdAsync(id);
        if (existingAppointment == null)
        {
            return new AppointmentResponse("Appointment not found");
        }
        
        try
        {
            _appointmentRepository.Remove(existingAppointment);
            await _unitOfWork.CompleteAsync();

            return new AppointmentResponse(existingAppointment);
        }
        catch (Exception e)
        {
            return new AppointmentResponse($"An error occurred while deleting the appointment: {e.Message}");
        }
    }
}
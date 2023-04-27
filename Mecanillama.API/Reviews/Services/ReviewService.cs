using Mecanillama.API.Mechanics.Domain.Repositories;
using Mecanillama.API.Reviews.Domain.Models;
using Mecanillama.API.Reviews.Domain.Repositories;
using Mecanillama.API.Reviews.Domain.Services;
using Mecanillama.API.Reviews.Domain.Services.Communication;
using Mecanillama.API.Shared.Domain.Repositories;

namespace Mecanillama.API.Reviews.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IMechanicRepository mechanicRepository) {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _mechanicRepository = mechanicRepository;
    }

    public async Task<IEnumerable<Review>> ListAsync()
    {
        return await _reviewRepository.ListAsync();
    }

    public async Task<ReviewResponse> SaveAsync(Review review)
    {
        var existingMechanic = await _mechanicRepository.FindByIdAsync(review.MechanicId);
        if (existingMechanic == null)
        {
            return new ReviewResponse("Mechanic not found");
        }
        
        try
        {
            await _reviewRepository.AddAsync(review);
            await _unitOfWork.CompleteAsync();
            return new ReviewResponse(review);
        }
        catch (Exception e) 
        {
            return new ReviewResponse($"An error occurred while saving the review: {e.Message}");
        }
    }

    public async Task<ReviewResponse> UpdateAsync(int id, Review review)
    {
        var existingReview = await _reviewRepository.FindByIdAsync(id);
        if (existingReview == null)
        {
            return new ReviewResponse("Review not found ");
        }

        var existingMechanic = await _mechanicRepository.FindByIdAsync(review.MechanicId);
        if (existingMechanic == null)
        {
            return new ReviewResponse("Mechanic not found");
        }

        existingReview.Comment = review.Comment;
        existingReview.Score = review.Score;
        existingReview.MechanicId = review.MechanicId;
        try
        {
            _reviewRepository.Update(existingReview);
            await _unitOfWork.CompleteAsync();

            return new ReviewResponse(existingReview);
        }
        catch (Exception e)
        {
            return new ReviewResponse($"An error occurred while updating the review: {e.Message}");
        }
    }

    public async Task<ReviewResponse> DeleteAsync(int id)
    {
        var existingReview = await _reviewRepository.FindByIdAsync(id);

        if (existingReview == null)
            return new ReviewResponse("Customer not found.");

        try
        {
            _reviewRepository.Remove(existingReview);
            await _unitOfWork.CompleteAsync();

            return new ReviewResponse(existingReview);
        }
        catch (Exception e)
        {
            return new ReviewResponse($"An error occurred while deleting the review: {e.Message}");
        }
    }
}
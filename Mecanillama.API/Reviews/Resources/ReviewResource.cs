
using Mecanillama.API.Mechanics.Resources;

namespace Mecanillama.API.Reviews.Resources;

public class ReviewResource
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public int Score { get; set; }
    public MechanicResource Mechanic { get; set; }
}
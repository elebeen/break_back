namespace Nutria.Domain.Dtos.MedicalCondition;

public class MedicalConditionGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;
}
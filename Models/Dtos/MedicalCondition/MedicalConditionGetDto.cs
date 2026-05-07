namespace break_back.Models.Dtos.MedicalCondition;

public class MedicalConditionGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;
}
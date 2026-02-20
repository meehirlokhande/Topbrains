namespace CampusHire;

public class Applicant
{
    public string ApplicantId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
    public string PreferredLocation { get; set; } = string.Empty;
    public string CoreCompetency { get; set; } = string.Empty;
    public int PassingYear { get; set; }
}

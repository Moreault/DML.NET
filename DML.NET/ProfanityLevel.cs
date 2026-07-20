namespace ToolBX.DML.NET;

/// <summary>
/// Severity of a profanity span. It is up to the consuming application to decide what each level means (ex: which
/// levels to mask based on the player's content settings.) DML does not interpret these values : it only reports them.
/// </summary>
public enum ProfanityLevel
{
    Mild,
    Strong,
    Severe
}

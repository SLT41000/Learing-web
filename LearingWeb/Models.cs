namespace LearingWeb;

/// <summary>
/// Video data model returned by the screen data API.
/// </summary>
public class ScreenData
{
    public string Vid { get; set; } = string.Empty;
    public string Mid { get; set; } = string.Empty;
    public string Vname { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Vlink { get; set; } = string.Empty;
}

/// <summary>
/// Catalog entry — a video in the user's watch history.
/// </summary>
public class CatalogData
{
    public string Vid { get; set; } = string.Empty;
    public string Ontime { get; set; } = string.Empty;
    public string Vname { get; set; } = string.Empty;
}

/// <summary>
/// A member's subject enrollment data.
/// </summary>
public class MemberData
{
    public string Mid { get; set; } = string.Empty;
}

/// <summary>
/// Video data used during login to create watchcheck entries.
/// </summary>
public class VidData
{
    public string Vid { get; set; } = string.Empty;
    public string Mid { get; set; } = string.Empty;
}

/// <summary>
/// Completed video (certificate) data.
/// </summary>
public class CompletedVideo
{
    public string Vname { get; set; } = string.Empty;
}

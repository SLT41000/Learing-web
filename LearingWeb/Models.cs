namespace LearingWeb;

/// <summary>
/// Video screen data — used by Index.cshtml, Watch.cshtml, Program.cs API.
/// </summary>
public class ScreenData
{
    public string Vid { get; set; } = string.Empty;
    public string Mid { get; set; } = string.Empty;
    public string Vname { get; set; } = string.Empty;
    public string Vdesc { get; set; } = string.Empty;
    public string Vlink { get; set; } = string.Empty;
    public string MidName { get; set; } = string.Empty;
}

/// <summary>
/// Subject / topic data — used by Index.cshtml.cs, Signin.cshtml.cs.
/// </summary>
public class SubjectData
{
    public string Cid { get; set; } = string.Empty;
    public string Cname { get; set; } = string.Empty;
    public string Cdsc { get; set; } = string.Empty;
}

/// <summary>
/// Watch history entry — used by Catalog.cshtml.cs.
/// </summary>
public class CatalogData
{
    public string Vid { get; set; } = string.Empty;
    public string Vname { get; set; } = string.Empty;
    public string Vlink { get; set; } = string.Empty;
    public string Mid { get; set; } = string.Empty;
    public string MidName { get; set; } = string.Empty;
    public string Ontime { get; set; } = string.Empty;
    public string Alreadywatch { get; set; } = string.Empty;
}

/// <summary>
/// Completed / certificate video — used by Certificate.cshtml.cs.
/// </summary>
public class CompletedVideo
{
    public string Vid { get; set; } = string.Empty;
    public string Vname { get; set; } = string.Empty;
    public string Vdesc { get; set; } = string.Empty;
    public string Ontime { get; set; } = string.Empty;
    public string MidName { get; set; } = string.Empty;
}

/// <summary>
/// Watch-check record — used by Watch.cshtml.cs to show check-in state.
/// </summary>
public class WatchcheckRecord
{
    public string Vid { get; set; } = string.Empty;
    public string Ontime { get; set; } = string.Empty;
    public string Alreadywatch { get; set; } = string.Empty;
}

/// <summary>
/// Video seed data — used by Program.cs /api/all-videos endpoint.
/// </summary>
public class VideoSeed
{
    public string Vid { get; set; } = string.Empty;
    public string Mid { get; set; } = string.Empty;
    public string Vname { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Vlink { get; set; } = string.Empty;
}

/// <summary>
/// All videos cache, populated during startup and used by login-page watchcheck seeding.
/// </summary>
public static class ProgramVideoCache
{
    public static List<ScreenData> AllVideos { get; private set; } = new();
}

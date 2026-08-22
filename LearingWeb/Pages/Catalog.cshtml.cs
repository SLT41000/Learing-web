using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace LearingWeb.Pages;

public class CatalogModel : PageModel
{
    public bool Loading { get; set; } = true;
    public List<CatalogData> CatalogRows { get; set; } = new();

    public async Task OnGetAsync()
    {
        var aid = HttpContext.Session.GetString("aid");
        if (string.IsNullOrEmpty(aid))
        {
            Loading = false;
            return;
        }

        await Task.Run(() =>
        {
            DbHelper.ReadQuery(
                "SELECT c.vid, c.ontime, v.vname FROM catalog AS c INNER JOIN video AS v ON (c.vid = v.vid) WHERE c.aid = @aid ORDER BY c.ontime DESC",
                reader =>
                {
                    CatalogRows.Add(new CatalogData
                    {
                        Vid = reader.GetString(0),
                        Ontime = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        Vname = reader.IsDBNull(2) ? "" : reader.GetString(2)
                    });
                },
                new SqlParameter("@aid", aid)
            );
        });

        Loading = false;
    }
}

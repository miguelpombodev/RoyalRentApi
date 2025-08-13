namespace RoyalRent.Presentation.Middlewares.Models;

/// <summary>
///
/// </summary>
public class Response
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    public Response(object? data)
    {
        Data = data;
    }

    /// <summary>
    ///
    /// </summary>
    public object? Data { get; set; } = null;
}

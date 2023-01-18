using System.Collections.Generic;
using System.Linq;

namespace SpotifyPlaylistDownloader.Models.Common;

public class Result<T>
{
    public T Content { get; set; }
    public bool IsFailure => Errors.Any();
    public List<Error> Errors { get; set; }

    public Result(T content)
    {
        Errors = new List<Error>();
        Content = content;
    }

    public Result()
    {
        Errors = new List<Error>();
    }

    public Result<T> WithError(string message)
    {
        return new Result<T>
        {
            Errors = new List<Error> {new Error {Message = message}}
        };
    }
}
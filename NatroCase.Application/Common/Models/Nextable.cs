namespace NatroCase.Application.Common.Models;

public class Nextable<T>
{
    public Nextable(bool next, List<T> contents)
    {
        this.Next = next;
        this.Contents = contents;
    }

    public Nextable()
    {
    }

    public bool Next { get; set; }

    public List<T> Contents { get; set; }
}
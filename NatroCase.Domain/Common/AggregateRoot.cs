using System.Text.Json.Serialization;

namespace NatroCase.Domain.Common;

public abstract class AggregateRoot
{
    public Guid Id { get; set; }

    public long CreatedDate { get; set; }

    public long LastModifiedDate { get; set; }

    public long Version { get; set; }

    [JsonIgnore]
    public bool IsModified { get; private set; }

    public void SetAsCreated()
    {
        this.Id = Guid.NewGuid();
        long timestamp = Clock.UtcNow.ToTimestamp();
        this.CreatedDate = timestamp;
        this.LastModifiedDate = timestamp;
        this.IsModified = true;
    }

    public void SetAsModified()
    {
        if (this.IsModified)
            return;
        this.LastModifiedDate = Clock.UtcNow.ToTimestamp();
        this.IsModified = true;
    }
}
namespace SalesRecords.Shared.SharedKernel.Domain;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; }

    protected Entity() { }

    protected Entity(TId id) => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id!);

    public static bool operator ==(Entity<TId> a, Entity<TId> b) => a?.Equals(b) ?? b is null;
    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);
}
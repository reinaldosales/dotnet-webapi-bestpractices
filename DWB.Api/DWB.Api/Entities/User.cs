namespace DWB.Api.Entities;

public class User(
    Guid id,
    string username,
    string password,
    DateTime createdAt,
    DateTime updatedAt,
    DateTime? deletedAt)
{
    public Guid Id { get; private set; } = id;
    public string Username { get; private set; } = username;
    public string Password { get; private set; } = password;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime UpdatedAt { get; private set; } = updatedAt;
    public DateTime? DeletedAt { get; private set; } = deletedAt;

    public static User CreateUser(string username, string password)
        => new(Guid.NewGuid(), username, password, DateTime.Now, DateTime.Now, deletedAt: null);

}

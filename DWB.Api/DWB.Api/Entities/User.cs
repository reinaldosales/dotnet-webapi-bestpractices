namespace DWB.Api.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public User()
    {
        // For EF
    }

    public User(
        Guid id,
        string username,
        string password,
        DateTime createdAt,
        DateTime updatedAt,
        DateTime? deletedAt)
    {
        Id = id;
        Username = username;
        Password = password;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    public static User CreateUser(string username, string password)
        => new(Guid.NewGuid(), username, password, DateTime.Now, DateTime.Now, deletedAt: null);

}

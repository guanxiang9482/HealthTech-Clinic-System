using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class DatabaseManager
{
    private readonly string connectionString;

    public DatabaseManager(string server, string database, string user, string password)
    {
        // Connection string tells MySql connector WHERE and HOW to connect
        connectionString = $"Server={server};Database={database};User ID={user};Password={password};SslMode=None;";
    }

    // Opens and returns a ready-to-use connection
    private MySqlConnection OpenConnection()
    {
        var conn = new MySqlConnection(connectionString);
        conn.Open(); // Physically opens the TCP socket to MySQL server
        return conn;
    }

    // ─────────────────────────────────────────
    // CREATE
    // ─────────────────────────────────────────
    public void CreateUser(string name, string email)
    {
        using var conn = OpenConnection();
        using var cmd = new MySqlCommand(
            "INSERT INTO users (name, email) VALUES (@name, @email)", conn);

        // Parameters prevent SQL injection
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@email", email);

        int rowsAffected = cmd.ExecuteNonQuery();
        Console.WriteLine($"Created: {rowsAffected} row(s) inserted.");
    }

    // ─────────────────────────────────────────
    // READ (all rows)
    // ─────────────────────────────────────────
    public List<(int Id, string Name, string Email)> GetAllUsers()
    {
        var users = new List<(int, string, string)>();

        using var conn = OpenConnection();
        using var cmd = new MySqlCommand("SELECT id, name, email FROM users", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            users.Add((
                reader.GetInt32("id"),
                reader.GetString("name"),
                reader.GetString("email")
            ));
        }

        return users;
    }

    // READ (single row by ID)
    public (int Id, string Name, string Email)? GetUserById(int id)
    {
        using var conn = OpenConnection();
        using var cmd = new MySqlCommand(
            "SELECT id, name, email FROM users WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return (reader.GetInt32("id"), reader.GetString("name"), reader.GetString("email"));
        }

        return null; // Not found
    }

    // ─────────────────────────────────────────
    // UPDATE
    // ─────────────────────────────────────────
    public void UpdateUser(int id, string newName, string newEmail)
    {
        using var conn = OpenConnection();
        using var cmd = new MySqlCommand(
            "UPDATE users SET name = @name, email = @email WHERE id = @id", conn);

        cmd.Parameters.AddWithValue("@name", newName);
        cmd.Parameters.AddWithValue("@email", newEmail);
        cmd.Parameters.AddWithValue("@id", id);

        int rowsAffected = cmd.ExecuteNonQuery();
        Console.WriteLine($"Updated: {rowsAffected} row(s) modified.");
    }

    // ─────────────────────────────────────────
    // DELETE
    // ─────────────────────────────────────────
    public void DeleteUser(int id)
    {
        using var conn = OpenConnection();
        using var cmd = new MySqlCommand(
            "DELETE FROM users WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        int rowsAffected = cmd.ExecuteNonQuery();
        Console.WriteLine($"Deleted: {rowsAffected} row(s) removed.");
    }
}
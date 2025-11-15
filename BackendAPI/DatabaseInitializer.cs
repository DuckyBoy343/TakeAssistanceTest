// File: DatabaseInitializer.cs
using Dapper;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabase(WebApplication app)
    {
        // 1. --- Get services WITHOUT triggering the bad IDbConnection
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // 2. --- Get Connection Strings
        var connString = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Build a connection string to the 'master' DB
        var masterConnBuilder = new SqlConnectionStringBuilder(connString)
        {
            InitialCatalog = "master" // Connect to master DB to create our app's DB
        };
        var masterConnString = masterConnBuilder.ConnectionString;

        var script = await File.ReadAllTextAsync("init.sql");
        var batches = Regex.Split(script, @"^\s*GO\s*$",
                                  RegexOptions.Multiline | RegexOptions.IgnoreCase);

        // 3. --- Connect to 'master' and create the database
        try
        {
            logger.LogInformation("Connecting to master to ensure database exists...");
            await using (var masterDb = new SqlConnection(masterConnString))
            {
                await masterDb.OpenAsync();
                await masterDb.ExecuteAsync(batches[0]); // Run the "CREATE DATABASE" part
            }
            logger.LogInformation("Database 'AttendanceDb' created or already exists.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating database from master.");
            throw; // Fail fast if we can't even create the DB
        }

        // 4. --- Add a RETRY LOOP to connect to the new DB
        // It can take a few seconds for the new DB to be fully available.
        var maxRetries = 5;
        for (int retry = 0; retry < maxRetries; retry++)
        {
            try
            {
                logger.LogInformation($"Connecting to AttendanceDb (Attempt {retry + 1}/{maxRetries})...");

                // Now, connect to the new 'AttendanceDb' to run the rest
                await using (var appDb = new SqlConnection(connString))
                {
                    await appDb.OpenAsync();
                    // Run all other batches (schema + seed)
                    foreach (var batch in batches.Skip(1))
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            await appDb.ExecuteAsync(batch);
                        }
                    }
                }

                logger.LogInformation("Database schema and seed data applied successfully.");
                return; // Success! Exit the method.
            }
            catch (SqlException ex) when (ex.Message.Contains("Cannot open database"))
            {
                // This is the error you're seeing!
                logger.LogWarning($"Database not ready yet. Retrying in 3 seconds... Error: {ex.Message}");
                await Task.Delay(3000); // Wait 3 seconds
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying schema and seed data.");
                throw;
            }
        }

        throw new Exception("Could not connect to the database after retries. The app will now stop.");
    }
}